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
            if (Settings.CommandsModsEnabled && message.Contains("!mods") && (DateTime.Now - _modsCommand).TotalSeconds >= 10)
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
            if (user == Settings.Channel.ToLower())
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
                            Viewer.AwardViewersCoins(amount);
                            _client.SendMessage($"@{user} giving all viewers {amount} coins.");
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
                        int amount;
                        bool isNumeric = int.TryParse(command[2], out amount);
                        if (isNumeric)
                        {
                            Viewer viewer = Viewer.GetViewer(giftee);
                            Helper.Log($"Giving viewer {viewer.username} {amount} coins");
                            viewer.GiveViewerCoins(amount);
                            _client.SendMessage($"@{user} giving viewer {viewer.username} {amount} coins. New balance {viewer.GetViewerCoins()} coins.");
                        }
                    }
                    catch (InvalidCastException e)
                    {
                        Helper.Log("Invalid Give Viewer Coins Command " + e.Message);
                    }
                }

                if (message.StartsWith("!resetviewers"))
                {
                    if (resetWarning == 0)
                    {
                        _client.SendMessage($"@{user} this will delete all coins and karma date. Please do !resetviewers again to confirm");
                        resetWarning = 1;
                    }
                    else if (resetWarning == 1)
                    {
                        _client.SendMessage($"@{user} are you absolutely sure you want to delete your viewers? !resetviewers one more");
                        resetWarning = 2;
                    }
                    else if (resetWarning == 2)
                    {
                        Helper.Log("Resetting all viewers data");
                        _client.SendMessage($"@{user} resetting all viewers data.");
                        Settings.ViewerIds = null;
                        Settings.ViewerCoins = null;
                        Settings.ViewerKarma = null;
                        Settings.listOfViewers = new List<Viewer>();
                        WriteSettings();
                        resetWarning = 0;
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
                        _client.SendMessage($"@{user} - @{viewer.username} Coins: {viewer.GetViewerCoins()} Karma: {viewer.GetViewerKarma()}%. !whatiskarma");

                    }
                    catch (InvalidCastException e)
                    {
                        Helper.Log("Invalid Check User Command " + e.Message);
                    }
                }

                if (message.StartsWith("!setuserkarma"))
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
                            _client.SendMessage($"@{user} setting {viewer.username} karma to {amount}%");
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
                        _client.SendMessage($"@{user} earning coins is now toggled off.");
                    }
                    else
                    {
                        Settings.EarningCoins = true;
                        _client.SendMessage($"@{user} earning coins is now toggled on.");
                    }
                }

                if (message.StartsWith("!togglestore"))
                {
                    if (Settings.StoreOpen)
                    {
                        Settings.StoreOpen = false;
                        _client.SendMessage($"@{user} store purchases are now toggled off.");
                    }
                    else
                    {
                        Settings.StoreOpen = true;
                        _client.SendMessage($"@{user} store purchases are now toggled on.");
                    }
                }
            }

            if (message.StartsWith("!balance") || message.StartsWith("!bal") || message.StartsWith("!coins"))
            {
                Helper.Log("Trying to find User");
                Viewer viewer = Viewer.GetViewer(user);
                _client.SendMessage($"@{viewer.username} Coins: {viewer.GetViewerCoins()} Karma: {viewer.GetViewerKarma()}%. !whatiskarma");
            }

            if (message.StartsWith("!whatiskarma") || message.StartsWith("!karma"))
            {
                Viewer viewer = Viewer.GetViewer(user);
                _client.SendMessage($"@{viewer.username} karma is the rate at which you earn coins. Buying bad events lowers karma while good events/items raise your karma. You are currently earning karma at a rate of {viewer.GetViewerKarma()}%");
            }

            if (message.StartsWith("!modinfo"))
            {
                _client.SendMessage($"@{user} TwitchToolkit is a mod written by Twitch.tv/hodlhodl that integrates storytelling decisions into chat votes, awards viewers coins for watching, and those coins can be spent on items/events in game. Use !purchaselist to get more info. Join the discord https://discord.gg/qrtg224!");
            }

            if (message.StartsWith("!purchaselist") || message.StartsWith("!instructions"))
            {
                _client.SendMessage($"@{user} events/items can be purchased in game. Example: '!buyitem skillincrease' or '!buyitem beer 5'. Full list here: https://bit.ly/2tHiyu6");
            }

            if (Settings.StoreOpen)
            {
                if (message.StartsWith("!buyevent"))
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

                if (message.StartsWith("!buyitem"))
                {
                    string[] command = message.Split(' ');
                    string item = command[1];
                    command[1] = "carepackage";
                    command[0] = item.ToLower();

                    if (command.Length < 2)
                    {
                        return;
                    }

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


                    _client.SendMessage("TwitchStoriesChatMessageNewVote".Translate() + ": Say the number of the option you want to vote!");
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
