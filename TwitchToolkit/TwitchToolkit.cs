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
using TwitchToolkit.Utilities;

namespace TwitchToolkit
{
    public class TwitchToolkit : Mod
    {
        Timer _timer;
        readonly System.Random _rand;
        public IRCClient _client;
        bool _paused;
        Settings _settings;
        public bool _voteActive = false;
        Event[] _voteEvents;
        private VoteEvent _currentVote;
        private IEnumerable<IncidentDef> _eventsPossible;
        public readonly Dictionary<string, int> _voteAnswers;
        public Commands commands;
        public DateTime StartTime;
        public ChatWindow activeChatWindow = null;

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

            _doomsday = false;

            Ticker.Initialize(this);

            _settings = GetSettings<Settings>();
            commands = new Commands(this);

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

            StartTime = DateTime.Now;
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

        public DateTime _modsCommand = DateTime.MinValue;
        public DateTime _aliveCommand = DateTime.MinValue;
        void _client_OnPrivMsg(string channel, string user, string message)
        {
            if (activeChatWindow != null && !message.StartsWith("!") && user != Settings.Username)
            {
                if ((_voteActive && !int.TryParse(message, out int result)) || !_voteActive)
                {
                    string colorcode = Viewer.GetViewerColorCode(user);
                    activeChatWindow.AddMessage(message, user, colorcode);
                }

            }

            commands.CheckCommand(message, user);

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

                    List<IncidentDef> eventTest = _eventsPossible.ToList();
                    Helper.Log("Playing event lottery, total events " + eventTest.Count());

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
                    Helper.Log("Chose events to vote with " + _eventsPossibleChosen.Count());

                    if (Settings.VotingWindow)
                    {
                        if (!Find.WindowStack.TryRemove(typeof(VoteDuelWindow), true) && _eventsPossibleChosen.Count() == 200)
			            {
				            Find.WindowStack.Add(new VoteDuelWindow(_eventsPossibleChosen, this));
			            }
                        else if (!Find.WindowStack.TryRemove(typeof(VoteWindow), true))
                        {
                            Find.WindowStack.Add(new VoteWindow(_eventsPossibleChosen, this));
                        }
                    }

                    if (Settings.VotingChatMsgs)
                    {
                        _client.SendMessage("TwitchStoriesChatMessageNewVote".Translate() + ": " + "TwitchToolKitVoteInstructions".Translate());
                        for (int i = 0; i < _eventsPossibleChosen.Count(); i++)
                        {
                            Helper.Log("Event " + _eventsPossibleChosen.ToString());
                            string msg = "[" + (i + 1) + "] ";
                            msg += (_eventsPossibleChosen[i].LabelCap);
                            _client.SendMessage(msg);
                        }
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
                    int[] votekeys = CountVotes(_eventsPossibleChosen);

                    System.Random rnd = new System.Random();
                    int evt = 0;
                    int voteCount = 0;
                    for (int i = 0; i < votekeys.Count(); i++)
                    {
                        if (votekeys[i] > votekeys[evt])
                        {
                            evt = i;
                        }
                        else if (votekeys[i] == votekeys[evt] && rnd.Next(0, 2) == 1)
                        {
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
                    Find.WindowStack.TryRemove(typeof(VoteDuelWindow), true);
                    Find.WindowStack.TryRemove(typeof(VoteWindow), true);

                    _voteActive = false;
                    _voteEvents = null;
                    _eventsPossibleChosen = null;
                    _eventsPossible = null;
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

        public int[] CountVotes(IncidentDef[] _eventsPossibleChosen)
        {
            int[] votekeys = new int[_eventsPossibleChosen.Count()];
            foreach (KeyValuePair<string, int> vote in _voteAnswers)
            {
                if (_eventsPossibleChosen.Length < vote.Value)
                {
                    continue;
                }
                votekeys[vote.Value - 1] += 1;
            }
            return votekeys;
        }
    }
}
