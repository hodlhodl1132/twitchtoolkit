using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using TwitchToolkit.IRC;
using UnityEngine;
using Verse;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TwitchToolkit
{
    public class TwitchToolkit : Mod
    {
        Timer _timer;
        readonly System.Random _rand;
        IRCClient _client;
        bool _paused;
        Settings _settings;
        public bool _voteActive = false;
        Event[] _voteEvents;
        private VoteEvent _currentVote;
        private IEnumerable<IncidentDef> _eventsPossible;
        readonly Dictionary<string, int> _voteAnswers;
        int _voteType;
        int resetWarning = 0;

        string _ircHost = "irc.twitch.tv";
        short _ircPort = 443;

        bool _doomsday;
        public bool Doomsday
        {
            get
            {
                return _doomsday;
            }
            set
            {
                if (!_doomsday && value == true && _client != null)
                {
                    _client.SendMessage("TwitchStoriesChatMessageDoomsday".Translate());
                }

                _doomsday = value;
            }
        }

        public TwitchToolkit(ModContentPack content) : base(content)
        {
            _timer = new Timer();
            _rand = new System.Random();
            _paused = false;
            _voteAnswers = new Dictionary<string, int>();
            _voteEvents = null;
            _voteType = 0;

            _doomsday = false;

            Ticker.Initialize(this);

            _settings = GetSettings<Settings>();

            if (Settings.AutoConnect)
            {
                Connect();
            }

            _timer.AutoReset = false;
            _timer.Elapsed += _timer_Elapsed;

            StartTimer();
        }

        public void Reset()
        {
            Events.Reset();
            Helper.Reset();
            _doomsday = false;
            _voteEvents = null;
            _extraWait = 0;
            StartTimer();
        }

        public override string SettingsCategory()
        {
            return "Twitch Toolkit";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Settings.DoSettingsWindowContents(inRect);
        }

        public override void WriteSettings()
        {
            Settings.HideOAuth();
            base.WriteSettings();
        }

        public string[] MessageLog
        {
            get
            {
                if (_client == null)
                {
                    return new string[] { };
                }
                return _client.MessageLog;
            }
        }

        public void Connect()
        {
            _client = new IRCClient(_ircHost, _ircPort, Settings.Username, Settings.OAuth, Settings.Channel.ToLower());
            _client.OnPrivMsg += _client_OnPrivMsg;
            _client.Connect();
        }

        public bool Connected
        {
            get
            {
                if (_client == null)
                {
                    return false;
                }
                return _client.Connected;
            }
        }

        public void Disconnect()
        {
            if (_client == null)
            {
                return;
            }
            _client.Disconnect();
        }

        public void Reconnect()
        {
            if (_client == null)
            {
                Connect();
                return;
            }
            _client.Reconnect();
        }

        void StartTimer()
        {
            _timer.Stop();
            if (_extraWait > 0)
            {
                _timer.Interval = Math.Min(_extraWait, Settings.VoteInterval * 60 * 1000);
                _extraWait = 0;
            }
            else
            {
                _timer.Interval = (_eventsPossibleChosen == null ? Settings.VoteInterval : Settings.VoteTime) * 60 * 1000;
            }

            _timer.Start();
        }

        string _version;
        public string Version
        {
            get
            {
                if (_version != null)
                {
                    return _version;
                }

                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(Verse.Map));
                if (assembly == null)
                {
                    return null;
                }

                System.Diagnostics.FileVersionInfo fileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                if (fileVersion == null)
                {
                    return null;
                }

                return (_version = fileVersion.FileVersion);
            }
        }

        DateTime _modsCommand = DateTime.MinValue;
        DateTime _aliveCommand = DateTime.MinValue;
        void _client_OnPrivMsg(string channel, string user, string message)
        {
            if (Settings.CommandsModsEnabled && message.Contains("!installedmods") && (DateTime.Now - _modsCommand).TotalSeconds >= 10)
            {
                _modsCommand = DateTime.Now;
                string msg = "Version: " + Version + ", Mods: ";
                string[] mods = LoadedModManager.RunningMods.Select((m) => m.Name).ToArray();

                for (int i = 0; i < mods.Length; i++)
                {
                    msg += mods[i] + ", ";

                    if (i == (mods.Length - 1) || msg.Length > 256)
                    {
                        msg = msg.Substring(0, msg.Length - 2);
                        _client.SendMessage(msg);
                        msg = "";
                    }
                }
                return;
            }
            
            //admin commands
            if (user.ToLower() == Settings.Channel.ToLower())
            {
                if (message.StartsWith("!resetviewers"))
                {
                    if (resetWarning == 0)
                    {
                        _client.SendMessage($"@{user} " + "TwitchToolkitResetViewersWarningOne".Translate());
                        resetWarning = 1;
                    }
                    else if (resetWarning == 1)
                    {
                        _client.SendMessage($"@{user} " + "TwitchToolkitResetViewersWarningTwo".Translate());
                        resetWarning = 2;
                    }
                    else if (resetWarning == 2)
                    {
                        Helper.Log("Resetting all viewers data");
                        _client.SendMessage($"@{user} " + "TwitchToolkitResetViewersWarningThree".Translate());
                        Settings.ViewerIds = null;
                        Settings.ViewerCoins = null;
                        Settings.ViewerKarma = null;
                        Settings.listOfViewers = new List<Viewer>();
                        WriteSettings();
                        resetWarning = 0;
                    }
                }

                if (message.StartsWith("!addtoolkitmod"))
                {
                        string[] command = message.Split(' ');

                        if (command.Length < 2)
                        {
                            return;
                        }
                        
                        string mod = command[1].Replace("@", "").ToLower();

                        Helper.Log("Checking if mod");
                        if (Viewer.IsModerator(mod))
                        {
                            _client.SendMessage($"@{user} @{mod} is already a TwitchToolkit Moderator.");
                            return;
                        }

                        Viewer modviewer = Viewer.GetViewer(mod);

                        modviewer.SetAsModerator();
                        _client.SendMessage($"@{user} added @{mod} as TwitchToolkit Moderator.");
                }

                if (message.StartsWith("!removetoolkitmod"))
                {
                        string[] command = message.Split(' ');

                        if (command.Length < 2)
                        {
                            return;
                        }
                        
                        string mod = command[1].Replace("@", "").ToLower();


                        Helper.Log("checking if mod");
                        if (!Viewer.IsModerator(mod))
                        {
                            return;
                        }

                        Viewer modviewer = Viewer.GetViewer(mod);

                        modviewer.RemoveAsModerator();
                        _client.SendMessage($"@{user} removed @{mod} as TwitchToolkit Moderator.");
                }
            }

            //moderator commands
            if (user.ToLower() == Settings.Channel.ToLower() || Viewer.IsModerator(user))
            {
                if (message.StartsWith("!refreshviewers"))
                {
                    WebRequest_BeginGetResponse.Main();
                }

                if (message.StartsWith("!karmaround"))
                {
                    Viewer.AwardViewersCoins();
                }

                if (message.StartsWith("!giveallcoins"))
                {
                    try
                    {
                        string[] command = message.Split(' ');

                        if (command.Length < 2)
                        {
                            return;
                        }

                        int amount;
                        bool isNumeric = int.TryParse(command[1], out amount);

                        if (isNumeric)
                        {
                            foreach(Viewer viewer in Settings.listOfViewers)
                            {
                                viewer.GiveViewerCoins(amount);
                            }
                            _client.SendMessage($"@{user} " + "TwitchToolkitGivingAll".Translate() + $" {amount} " + "TwitchToolkitVote".Translate());
                        }
                    }
                    catch (InvalidCastException e)
                    {
                        Helper.Log("Give All Coins Syntax Error " + e.Message);
                    }
                }

                if (message.StartsWith("!givecoins"))
                {
                    try
                    {
                        string[] command = message.Split(' ');

                        if (command.Length < 3)
                        {
                            return;
                        }
                        
                        string giftee = command[1].Replace("@", "");

                        if (user.ToLower() != Settings.Channel.ToLower() && giftee.ToLower() == user.ToLower())
                        {
                            _client.SendMessage($"@{user} moderators cannot give themselves coins.");
                            return;
                        }

                        int amount;
                        bool isNumeric = int.TryParse(command[2], out amount);
                        if (isNumeric)
                        {
                            Viewer viewer = Viewer.GetViewer(giftee);
                            Helper.Log($"Giving viewer {viewer.username} {amount} coins");
                            viewer.GiveViewerCoins(amount);
                            _client.SendMessage($"@{user} " + "TwitchToolkitGiving".Translate() + ' ' + "TwitchToolkitViewer".Translate() + $" {viewer.username} {amount} " + "TwitchToolkitCoins".Translate()  + " " + "TwitchToolkitBalance".Translate() + $" {viewer.GetViewerCoins()}" + ' ' + "TwitchToolkitCoins".Translate());
                        }
                    }
                    catch (InvalidCastException e)
                    {
                        Helper.Log("Invalid Give Viewer Coins Command " + e.Message);
                    }
                }

                if (message.StartsWith("!checkuser"))
                {
                    try
                    {
                        string[] command = message.Split(' ');

                        if (command.Length < 2)
                        {
                            return;
                        }

                        string target = command[1].Replace("@", "");

                        Viewer viewer = Viewer.GetViewer(target);
                        _client.SendMessage($"@{user} - @{viewer.username} " + "TwitchToolkitCoins".Translate() + $": {viewer.GetViewerCoins()} " + "TwitchToolkitKarma".Translate() + $": { viewer.GetViewerKarma()}%. {Settings.KarmaCmd}");

                    }
                    catch (InvalidCastException e)
                    {
                        Helper.Log("Invalid Check User Command " + e.Message);
                    }
                }

                if (message.StartsWith("!setkarma"))
                {
                    try
                    {
                        string[] command = message.Split(' ');

                        if (command.Length < 3)
                        {
                            return;
                        }

                        string target = command[1].Replace("@", "");
                        int amount;
                        bool isNumeric = int.TryParse(command[2], out amount);
                        if (isNumeric)
                        {
                            Viewer viewer = Viewer.GetViewer(target);
                            viewer.SetViewerKarma(amount);
                            _client.SendMessage($"@{user} " + "TwitchToolkitUpdating".Translate() + $" { viewer.username} " + "TwitchToolkitKarma".Translate() + $" {amount}%");
                        }
                    }
                    catch (InvalidCastException e)
                    {
                        Helper.Log("Invalid Check User Command " + e.Message);
                    }
                }

                if (message.StartsWith("!togglecoins"))
                {
                    if (Settings.EarningCoins)
                    {
                        Settings.EarningCoins = false;
                        _client.SendMessage($"@{user} " + "TwitchToolkitEarningCoinsMessage".Translate() + "TwitchToolkitOff".Translate());
                    }
                    else
                    {
                        Settings.EarningCoins = true;
                        _client.SendMessage($"@{user} " + "TwitchToolkitEarningCoinsMessage".Translate() + "TwitchToolkitOn".Translate());
                    }
                }

                if (message.StartsWith("!togglestore"))
                {
                    if (Settings.StoreOpen)
                    {
                        Settings.StoreOpen = false;
                        _client.SendMessage($"@{user} " + "TwitchToolkitStorePurchasesMessage".Translate() + "TwitchToolkitOn".Translate());
                    }
                    else
                    {
                        Settings.StoreOpen = true;
                        _client.SendMessage($"@{user} " + "TwitchToolkitStorePurchasesMessage".Translate() + "TwitchToolkitOn".Translate());
                    }
                }
            }

            // commands are suppressed when not earning coins
            if (Settings.EarningCoins)
            {    
                if (message.StartsWith(Settings.BalanceCmd))
                {
                    Helper.Log("Trying to find User");
                    Viewer viewer = Viewer.GetViewer(user);
                    _client.SendMessage($"@{viewer.username} " + "TwitchToolkitCoins".Translate() + $": {viewer.GetViewerCoins()} " + "TwitchToolkitKarma".Translate() + $": {viewer.GetViewerKarma()}%. {Settings.KarmaCmd}");
                }

                if (message.StartsWith(Settings.KarmaCmd) && !message.Contains("!karmaround"))
                {
                    Viewer viewer = Viewer.GetViewer(user);
                    _client.SendMessage($"@{viewer.username} " + "TwitchToolkitWhatIsKarma".Translate() +  $" { viewer.GetViewerKarma()}%");
                }

                if (message.StartsWith(Settings.InstructionsCmd))
                {
                    _client.SendMessage($"@{user} " + "TwitchToolkitInstructions".Translate() + $": { Settings.CustomPricingSheetLink}");
                }

                if (message.StartsWith(Settings.PurchaselistCmd))
                {
                    _client.SendMessage($"@{user} " + "TwitchToolkitPurchaseList".Translate() + ": {Settings.CustomPricingSheetLink}");
                }

                if (message.StartsWith(Settings.GiftCmd) && Settings.GiftingCoins)
                {
                    string[] command = message.Split(' ');

                    if (command.Count() < 3)
                    {
                        return;
                    }

                    string target = command[1].Replace("@", "");

                    int amount;
                    bool isNumeric = int.TryParse(command[2], out amount);
                    if (isNumeric)
                    {
                        Viewer giftee = Viewer.GetViewer(target);
                        Viewer gifter = Viewer.GetViewer(user);
                        if (gifter.GetViewerCoins() >= amount)
                        {
                            gifter.TakeViewerCoins(amount);
                            giftee.GiveViewerCoins(amount);
                            _client.SendMessage($"@{giftee.username} " + "TwitchToolkitGiftingCoins".Translate() + $" {amount} " + "TwitchToolkitCoins".Translate() + ' ' + "TwitchToolkitFrom".Translate() + " @" + gifter.username);
                        }
                    }
                }
            }

            if (message.StartsWith(Settings.ModinfoCmd))
            {
                _client.SendMessage($"@{user} " + "TwitchToolkitModInfo".Translate() + " https://discord.gg/qrtg224 !");
            }

            if (Settings.StoreOpen)
            {
                if (message.StartsWith(Settings.BuyeventCmd))
                {
                    if (message.Split(' ')[1] == "carepackage")
                    {
                        return;
                    }

                    Helper.Log("Attempting event purchase");
                    ShopCommand command = new ShopCommand(message, Viewer.GetViewer(user));
                    if (command.errormessage != null)
                    {
                        _client.SendMessage(command.errormessage);
                    }
                    else if (command.successmessage != null)
                    {
                        _client.SendMessage(command.successmessage);
                    }
                }

                if (message.StartsWith(Settings.BuyitemCmd))
                {
                    string[] command = message.Split(' ');

                    if (command.Length < 2)
                    {
                        return;
                    }

                    string item = command[1];
                    command[1] = "carepackage";
                    command[0] = item.ToLower();

                    string newcommand = string.Join(" ", command);
                    Helper.Log("Attemping item purchase " + newcommand);
                    ShopCommand command2 = new ShopCommand(newcommand, Viewer.GetViewer(user));
                    if (command2.errormessage != null)
                    {
                        _client.SendMessage(command2.errormessage);
                    }
                    else if (command2.successmessage != null)
                    {
                        _client.SendMessage(command2.successmessage);
                    }
                }

            }

            if (message.StartsWith(Settings.ModsettingsCmd))
            {
                string minutess = Settings.CoinInterval > 1 ? "s" : "";
                string storeon = Settings.StoreOpen ? "on" : "off";
                string earningcoins = Settings.EarningCoins ? "on" : "off";
                string stats_message = 
                $"Twitch toolkit is rewarding viewers {Settings.CoinAmount} coins every {Settings.CoinInterval} minute{minutess}, " +
                $"the store is {storeon} and coin rewards are {earningcoins}. Karma cap is {Settings.KarmaCap}%";

                _client.SendMessage(stats_message);
            }



            if (Settings.CommandsAliveEnabled && message.StartsWith("!alive", StringComparison.InvariantCultureIgnoreCase) && (DateTime.Now - _aliveCommand).TotalSeconds >= 10)
            {
                _aliveCommand = DateTime.Now;
                _client.SendMessage(
                  "TwitchStoriesChatMessageColonyAlive".Translate() + " " + Current.Game.tickManager.TicksGame.ToReadableRimworldTimeString() +
                  ", " +
                  "TwitchStoriesChatMessageGamePlayed".Translate() + " " + Current.Game.Info.RealPlayTimeInteracting.ToReadableTimeString()
                );
                return;
            }

            if (_eventsPossibleChosen == null)
            {
                return;
            }

            int x = 0;
            for (int i = 0; i < message.Length; i++)
            {
                if (message[i] >= '0' && message[i] <= '9')
                {
                    x *= 10;
                    x += (message[i] - '0');
                }
                else
                {
                    return;
                }
            }

            if (x <= 0 || x > _eventsPossibleChosen.Length)
            {
                return;
            }

            _voteAnswers[user] = x;
        }

        DateTime _lastTick = DateTime.MinValue;
        DateTime _lastEventCheck = DateTime.MinValue;
        DateTime _timerElapsed = DateTime.MinValue;
        double _extraWait = 0;
        private IncidentDef[] _eventsPossibleChosen;
        private int _votingStage = 2;

        public void Tick()
        {
            DateTime tick = DateTime.Now;

            if (_lastTick == DateTime.MinValue)
            {
            }
            else if (_paused == true)
            {
                var wait = (_timerElapsed - _lastTick).TotalMilliseconds;
                _extraWait = Math.Min(Math.Max(wait, 0), Settings.VoteInterval * 60 * 1000);
                _paused = false;
                StartTimer();
            }
            else if ((tick - _lastTick).TotalSeconds > 5)
            {
                _extraWait += (tick - _lastTick).TotalMilliseconds;
            }

            if ((tick - _lastEventCheck).TotalSeconds > 120)
            {
                Helper.CheckInfestation();
                _lastEventCheck = tick;
            }

            _lastTick = tick;
        }

        public void StartVote(VoteEvent next)
        {
            Helper.Log("Attemping vote start");
            _timer.Stop();
            _currentVote = next;
            _eventsPossible = _currentVote.options;
            _votingStage = 0;
            _timer_Elapsed(null, null);
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                Helper.Log("Timer Elapsed");
                _timerElapsed = DateTime.Now;

                if (_paused || _client == null || !Helper.ModActive)
                {
                    return;
                }

                if (_eventsPossible == null)
                {
                    if (Current.Game.tickManager.Paused)
                    {
                        _paused = true;
                        return;
                    }

                    if (_extraWait > 0)
                    {
                        return;
                    }
                }
                Helper.Log("Checking Voting Stage");


                _extraWait = 0;

                if (_votingStage == 0)
                {
                    if (_eventsPossible.Count() <= 0)
                    {
                        return;
                    }
                    _voteActive = true;
                    _voteAnswers.Clear();


                    Helper.Log("Playing event lottery");
                    List<IncidentDef> eventTest = _eventsPossible.ToList();

                    int eventsTotal = eventTest.Count();
                    int eventsNeeded = Settings.VoteOptions;
                    if (eventsTotal < eventsNeeded)
                    {
                        eventsNeeded = eventsTotal;
                    }
                    _eventsPossibleChosen = new IncidentDef[eventsNeeded];
                    System.Random rnd = new System.Random();
                    for (int i = 0; i < eventTest.Count(); i++)
                    {
                        if (eventsNeeded == 0)
                        {
                            break;
                        }

                        int win = rnd.Next(1, 101);
                        double perc = ((double)eventsNeeded / eventsTotal) * 100;
                        if (win <= perc)
                        {
                            _eventsPossibleChosen[eventsNeeded - 1] = eventTest[i];
                            eventsNeeded--;
                        }
                        eventsTotal--;
                    }


                    _client.SendMessage("TwitchStoriesChatMessageNewVote".Translate() + ": " + "TwitchToolKitVoteInstructions".Translate());
                    //_voteType = _rand.Next(0, 2);
                    _voteType = 2;
                    for (int i = 0; i < _eventsPossibleChosen.Count(); i++)
                    {
                        string msg = "[" + (i + 1) + "] ";
                        switch (_voteType)
                        {
                            case 0:
                                msg += ("TwitchStoriesVote" + _voteEvents[i].Type.ToString()).Translate();
                                break;
                            case 1:
                                msg += ("TwitchStoriesVote" + _voteEvents[i].MainCategory.ToString()).Translate();
                                break;
                            case 2:
                                msg += (_eventsPossibleChosen[i].LabelCap);
                                break;
                            default:
                                msg += _voteEvents[i].Description;
                                break;
                        }
                        _client.SendMessage(msg);
                    }
                    _votingStage = 1;
                }
                else if (_votingStage == 1)
                {
                    Helper.Log("Starting Votes Count");
                    if (_eventsPossibleChosen.Length <= 0)
                    {
                        return;
                    }
                    Helper.Log("Counting Votes");
                    int[] votekeys = new int[_eventsPossibleChosen.Count()];
                    foreach (KeyValuePair<string, int> vote in _voteAnswers)
                    {
                        if (_eventsPossibleChosen.Length < vote.Value)
                        {
                            continue;
                        }
                        Helper.Log($"Trying to register vote for {votekeys[vote.Value - 1]}");
                        votekeys[vote.Value - 1] += 1;
                        Helper.Log($"New Count {votekeys[vote.Value - 1]}");
                    }
                    int evt = 0;
                    int voteCount = 0;
                    for (int i = 0; i < votekeys.Count(); i++)
                    {
                        Helper.Log($"{votekeys[i]}:{_eventsPossibleChosen[i]} vs {votekeys[evt]}:{_eventsPossibleChosen[evt]}");
                        if (votekeys[i] > votekeys[evt])
                        {
                            evt = i;
                        }
                        else if (votekeys[i] == votekeys[evt] && _rand.Next(0, 2) == 1)
                        {
                            Helper.Log("Tied, picking random winner");
                            evt = i;
                        }
                        voteCount += votekeys[i];
                    }

                    double wonPercentage = ((double)evt / (double)(voteCount == 0 ? 1 : voteCount));

                    string msg = "TwitchStoriesChatMessageVoteEnd".Translate() + " ";

                    msg += $"{_eventsPossibleChosen[evt].LabelCap}";


                    _client.SendMessage(msg);

                    FiringIncident chosen = new FiringIncident(_eventsPossibleChosen[evt], _currentVote.storytellerComp_CustomStoryTeller, _currentVote.parms);
                    Ticker.FiringIncidents.Enqueue(chosen);

                    Helper.Log("Vote: " + _eventsPossibleChosen[evt].LabelCap + " won");

                    _voteActive = false;
                    _voteEvents = null;
                    _eventsPossibleChosen = null;
                    _votingStage = 2;
                }
            }
            catch (Exception exception)
            {
                Helper.Log("Exception: " + exception.Message + " " + exception.StackTrace);
            }
            finally
            {
                Helper.Log("Starting Timer Back up");
                StartTimer();
            }
        }
    }
}
