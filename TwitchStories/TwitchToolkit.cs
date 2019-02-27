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

        int _difficulty;
        bool _difficultySet;
        public void SetDifficulty(int difficulty, bool manual = false)
        {
        if (_difficultySet && !manual)
        {
        return;
        }

        if (manual)
        {
        _difficultySet = true;
        }

        if (difficulty <= 0 || difficulty > 5)
        {
        return;
        }

        if (
          !Doomsday &&
          difficulty > _difficulty &&
          _client != null &&
          !manual &&
          Helper.ModActive)
        {
        _client.SendMessage("TwitchStoriesChatMessageDifficultyIncreased".Translate().Replace("{difficulty}", difficulty.ToString()));
        }

        _difficulty = difficulty;
        }

        public int Difficulty
        {
            get
            {
            if (Doomsday)
            {
            return 5;
            }

            return _difficulty;
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
        _difficulty = 1;

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
        _difficulty = 1;
        _difficultySet = false;
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

        if (message.Contains("!balance"))
        {
            Helper.Log("Trying to find User");
            Viewer viewer = Viewer.GetViewer(user);
            Helper.Log("Viewer found or created");
            Helper.Log($"@{viewer.username} you have {viewer.GetViewerCoins()} coins and are earning a karma bonus of {viewer.GetViewerKarma()}%. !whatiskarma");
            _client.SendMessage($"@{viewer.username} you have {viewer.GetViewerCoins()} coins and are earning a karma bonus of {viewer.GetViewerKarma()}%. !whatiskarma");
        }

        if (message.StartsWith("!getallviewers") && user == Settings.Channel)
        {
            WebRequest_BeginGetResponse.Main();
        }

        if (message.StartsWith("!awardviewerscoins") && user == Settings.Channel)
        {
            Viewer.AwardViewersCoins();
        }

        if (message.StartsWith("!giveviewercoins") && user == Settings.Channel)
        {
            try {
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


        if (message.StartsWith("!buyevent"))
        {
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
            command[0] = item;
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

        if (user == Settings.Username && message.Contains("!Starting Event"))
        {
        if (message.Contains("Small raid"))
        {
        Event evt = new Event(1, EventType.Bad, EventCategory.Invasion, 2, "Small raid", () => Helper.RaidPossible(100, PawnsArrivalModeDefOf.EdgeWalkIn), (quote) => Helper.Raid(quote, 100, PawnsArrivalModeDefOf.EdgeWalkIn));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Medium raid drop"))
        {
        Event evt = new Event(4, EventType.Bad, EventCategory.Invasion, 4, "Medium raid drop", () => Helper.RaidPossible(450, PawnsArrivalModeDefOf.EdgeWalkIn), (quote) => Helper.Raid(quote, 450, PawnsArrivalModeDefOf.CenterDrop));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Medium raid"))
        {
        Event evt = new Event(2, EventType.Bad, EventCategory.Invasion, 3, "Medium raid", () => Helper.RaidPossible(250, PawnsArrivalModeDefOf.EdgeWalkIn), (quote) => Helper.Raid(quote, 250, PawnsArrivalModeDefOf.EdgeWalkIn));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Big raid drop"))
        {
        Event evt = new Event(5, EventType.Bad, EventCategory.Invasion, 5, "Big raid drop", () => Helper.RaidPossible(900, PawnsArrivalModeDefOf.EdgeWalkIn), (quote) => Helper.Raid(quote, 900, PawnsArrivalModeDefOf.CenterDrop));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Big raid"))
        {
        Event evt = new Event(3, EventType.Bad, EventCategory.Invasion, 4, "Big raid", () => Helper.RaidPossible(500, PawnsArrivalModeDefOf.EdgeWalkIn), (quote) => Helper.Raid(quote, 500, PawnsArrivalModeDefOf.EdgeWalkIn));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Big siege"))
        {
        Event evt = new Event(6, EventType.Bad, EventCategory.Invasion, 4, "Big siege", () => Helper.SiegePossible(600, PawnsArrivalModeDefOf.EdgeWalkIn), (quote) => Helper.Siege(quote, 600, PawnsArrivalModeDefOf.EdgeWalkIn));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Big mechanoid raid"))
        {
        Event evt = new Event(7, EventType.Bad, EventCategory.Invasion, 5, "Big mechanoid raid", () => Helper.MechanoidRaidPossible(1500, PawnsArrivalModeDefOf.EdgeWalkIn), (quote) => Helper.MechanoidRaid(quote, 1500, PawnsArrivalModeDefOf.EdgeWalkIn));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Infestation"))
        {
        Event evt = new Event(8, EventType.Bad, EventCategory.Invasion, 3, "Infestation", () => Helper.InfestationPossible(), (quote) => Helper.Infestation(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Disease plague medium"))
        {
        Event evt = new Event(16, EventType.Bad, EventCategory.Disease, 3, "Disease plague", () => true, (quote) => Helper.DiseasePlague(quote, 0.5f));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Disease flu medium"))
        {
        Event evt = new Event(17, EventType.Bad, EventCategory.Disease, 3, "Disease flu", () => true, (quote) => Helper.DiseaseFlu(quote, 0.5f));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Disease infection medium"))
        {
        Event evt = new Event(18, EventType.Bad, EventCategory.Disease, 3, "Disease infection", () => true, (quote) => Helper.DiseaseInfection(quote, 0.33f));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Disease malaria medium"))
        {
        Event evt = new Event(19, EventType.Bad, EventCategory.Disease, 4, "Disease malaria", () => true, (quote) => Helper.DiseaseMalaria(quote, 0.5f));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Disease gut worms medium"))
        {
        Event evt = new Event(20, EventType.Bad, EventCategory.Disease, 2, "Disease gut worms", () => true, (quote) => Helper.DiseaseGutWorms(quote, 0.5f));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Disease muscle parasites medium"))
        {
        Event evt = new Event(21, EventType.Bad, EventCategory.Disease, 2, "Disease muscle parasites", () => true, (quote) => Helper.DiseaseMuscleParasites(quote, 0.5f));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Disease plague hard"))
        {
        Event evt = new Event(22, EventType.Bad, EventCategory.Disease, 4, "Disease plague", () => true, (quote) => Helper.DiseasePlague(quote, 0.8f));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Disease flu hard"))
        {
        Event evt = new Event(23, EventType.Bad, EventCategory.Disease, 4, "Disease flu", () => true, (quote) => Helper.DiseaseFlu(quote, 1f));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Disease infection hard"))
        {
        Event evt = new Event(24, EventType.Bad, EventCategory.Disease, 4, "Disease infection", () => true, (quote) => Helper.DiseaseInfection(quote, 0.66f));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Disease malaria hard"))
        {
        Event evt = new Event(25, EventType.Bad, EventCategory.Disease, 5, "Disease malaria", () => true, (quote) => Helper.DiseaseMalaria(quote, 1f));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Disease gut worms hard"))
        {
        Event evt = new Event(26, EventType.Bad, EventCategory.Disease, 3, "Disease gut worms", () => true, (quote) => Helper.DiseaseGutWorms(quote, 1f));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Disease muscle parasites hard"))
        {
        Event evt = new Event(27, EventType.Bad, EventCategory.Disease, 3, "Disease muscle parasites", () => true, (quote) => Helper.DiseaseMuscleParasites(quote, 1f));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Heart attack"))
        {
        Event evt = new Event(9, EventType.Bad, EventCategory.Disease, 2, "Heart attack", () => true, (quote) => Helper.DiseaseHeartAttack(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Disease muscle parasites"))
        {
        Event evt = new Event(15, EventType.Bad, EventCategory.Disease, 1, "Disease muscle parasites", () => true, (quote) => Helper.DiseaseMuscleParasites(quote, 0f));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Disease plague"))
        {
        Event evt = new Event(10, EventType.Bad, EventCategory.Disease, 2, "Disease plague", () => true, (quote) => Helper.DiseasePlague(quote, 0f));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Disease flu"))
        {
        Event evt = new Event(11, EventType.Bad, EventCategory.Disease, 2, "Disease flu", () => true, (quote) => Helper.DiseaseFlu(quote, 0f));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Disease infection"))
        {
        Event evt = new Event(12, EventType.Bad, EventCategory.Disease, 2, "Disease infection", () => true, (quote) => Helper.DiseaseInfection(quote, 0f));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Disease malaria"))
        {
        Event evt = new Event(13, EventType.Bad, EventCategory.Disease, 3, "Disease malaria", () => true, (quote) => Helper.DiseaseMalaria(quote, 0f));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Disease gut worms"))
        {
        Event evt = new Event(14, EventType.Bad, EventCategory.Disease, 1, "Disease gut worms", () => true, (quote) => Helper.DiseaseGutWorms(quote, 0f));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Disease blindess"))
        {
        Event evt = new Event(28, EventType.Bad, EventCategory.Disease, 5, "Disease Blindness", () => true, (quote) => Helper.DiseaseBlindness(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Clear weather"))
        {
        Event evt = new Event(29, EventType.Neutral, EventCategory.Weather, 1, "Clear weather", () => true, (quote) => Helper.WeatherClear(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Rain"))
        {
        Event evt = new Event(30, EventType.Neutral, EventCategory.Weather, 1, "Rain", () => true, (quote) => Helper.WeatherRain(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Rainy thunderstorm"))
        {
        Event evt = new Event(31, EventType.Neutral, EventCategory.Weather, 1, "Rainy thunderstorm", () => true, (quote) => Helper.WeatherRainyThunderstorm(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Dry thunderstorm"))
        {
        Event evt = new Event(32, EventType.Neutral, EventCategory.Weather, 1, "Dry thunderstorm", () => true, (quote) => Helper.WeatherDryThunderstorm(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Snow gentle"))
        {
        Event evt = new Event(33, EventType.Neutral, EventCategory.Weather, 2, "Snow gentle", () => true, (quote) => Helper.WeatherSnowGentle(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Snow hard"))
        {
        Event evt = new Event(34, EventType.Neutral, EventCategory.Weather, 2, "Snow hard", () => true, (quote) => Helper.WeatherSnowHard(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Foggy rain"))
        {
        Event evt = new Event(36, EventType.Neutral, EventCategory.Weather, 1, "Foggy rain", () => true, (quote) => Helper.WeatherFoggyRain(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Fog"))
        {
        Event evt = new Event(35, EventType.Neutral, EventCategory.Weather, 1, "Fog", () => true, (quote) => Helper.WeatherFog(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Flashstorm"))
        {
        Event evt = new Event(37, EventType.Neutral, EventCategory.Weather, 3, "Flashstorm", () => true, (quote) => Helper.WeatherFlashstorm(quote, 30000));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Eclipse"))
        {
        Event evt = new Event(38, EventType.Neutral, EventCategory.Environment, 2, "Eclipse", () => true, (quote) => Helper.WeatherEclipse(quote, 60000));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Aurora"))
        {
        Event evt = new Event(39, EventType.Good, EventCategory.Environment, 2, "Aurora", () => Helper.WeatherAuroraPossible(), (quote) => Helper.WeatherAurora(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Vomit rain"))
        {
        Event evt = new Event(40, EventType.Neutral, EventCategory.Environment, 3, "Vomit rain", () => true, (quote) => Helper.VomitRain(quote, 500));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Ambrosia sprout"))
        {
        Event evt = new Event(41, EventType.Good, EventCategory.Environment, 1, "Ambrosia sprout", () => true, (quote) => Helper.AmbrosiaSprout(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Meteorite shower"))
        {
        Event evt = new Event(43, EventType.Neutral, EventCategory.Environment, 4, "Meteorite shower", () => true, (quote) => Helper.MeteoriteShower(quote, 5, true));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Meteorite"))
        {
        Event evt = new Event(42, EventType.Neutral, EventCategory.Environment, 2, "Meteorite", () => true, (quote) => Helper.Meteorite(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Blight"))
        {
        Event evt = new Event(44, EventType.Bad, EventCategory.Hazard, 2, "Blight", () => Helper.BlightPossible(), (quote) => Helper.Blight(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Solar flare"))
        {
        Event evt = new Event(45, EventType.Bad, EventCategory.Hazard, 3, "Solar flare", () => Helper.WeatherSolarFlarePossible(60000), (quote) => Helper.WeatherSolarFlare(quote, 60000));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Volcanic winter"))
        {
        Event evt = new Event(46, EventType.Bad, EventCategory.Hazard, 3, "Volcanic winter", () => Helper.WeatherVolcanicWinterPossible(480000), (quote) => Helper.WeatherVolcanicWinter(quote, 480000));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Toxic fallout"))
        {
        Event evt = new Event(47, EventType.Bad, EventCategory.Hazard, 4, "Toxic fallout", () => Helper.WeatherToxicFalloutPossible(300000), (quote) => Helper.WeatherToxicFallout(quote, 300000));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Heat wave"))
        {
        Event evt = new Event(48, EventType.Bad, EventCategory.Hazard, 2, "Heat wave", () => Helper.WeatherHeatWavePossible(60000), (quote) => Helper.WeatherHeatWave(quote, 60000));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Cold snap"))
        {
        Event evt = new Event(49, EventType.Bad, EventCategory.Hazard, 2, "Cold snap", () => Helper.WeatherColdSnapPossible(60000), (quote) => Helper.WeatherColdSnap(quote, 60000));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Tornados"))
        {
        Event evt = new Event(51, EventType.Bad, EventCategory.Hazard, 5, "Tornados", () => true, (quote) => Helper.Tornado(quote, 10));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Tornado"))
        {
        Event evt = new Event(50, EventType.Bad, EventCategory.Hazard, 3, "Tornado", () => true, (quote) => Helper.Tornado(quote, 1));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Wild human"))
        {
        Event evt = new Event(52, EventType.Good, EventCategory.Colonist, 1, "Wild human", () => Helper.WildHumanPossible(), (quote) => Helper.WildHuman(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Wanderer joins"))
        {
        Event evt = new Event(53, EventType.Good, EventCategory.Colonist, 2, "Wanderer joins", () => Helper.WandererPossible(), (quote) => Helper.Wanderer(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Gender swap"))
        {
        Event evt = new Event(54, EventType.Neutral, EventCategory.Colonist, 2, "Gender swap", () => true, (quote) => Helper.GenderSwap(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Skill increase"))
        {
        Event evt = new Event(55, EventType.Good, EventCategory.Colonist, 1, "Skill increase", () => true, (quote) => Helper.IncreaseRandomSkill(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Party"))
        {
        Event evt = new Event(56, EventType.Good, EventCategory.Colonist, 2, "Party", () => Helper.PartyPossible(), (quote) => Helper.Party(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Cannibal"))
        {
        Event evt = new Event(89, EventType.Neutral, EventCategory.Colonist, 3, "Cannibal", () => true, (quote) => Helper.Cannibal(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Luciferium") && message.Contains("Dropping") == false)
        {
        Event evt = new Event(57, EventType.Bad, EventCategory.Colonist, 5, "Lucelse iferium", () => true, (quote) => Helper.Luciferium(quote, 1));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Mad animal"))
        {
        Event evt = new Event(58, EventType.Bad, EventCategory.Animal, 1, "Mad animal", () => Helper.MadAnimalPossible(), (quote) => Helper.MadAnimal(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Herd migration"))
        {
        Event evt = new Event(59, EventType.Neutral, EventCategory.Animal, 1, "Herd migration", () => Helper.HerdMigrationPossible(), (quote) => Helper.HerdMigration(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Animal wander in"))
        {
        Event evt = new Event(60, EventType.Neutral, EventCategory.Animal, 2, "Animal wander in", () => Helper.AnimalsWanderInPossible(), (quote) => Helper.AnimalsWanderIn(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Rare thrumbos"))
        {
        Event evt = new Event(61, EventType.Neutral, EventCategory.Animal, 1, "Rare thrumbos", () => Helper.ThrumbosPossible(), (quote) => Helper.Thrumbos(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Farm animals"))
        {
        Event evt = new Event(62, EventType.Good, EventCategory.Animal, 3, "Farm animals", () => Helper.FarmAnimalsPossible(), (quote) => Helper.FarmAnimals(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Animal self-tamed"))
        {
        Event evt = new Event(63, EventType.Good, EventCategory.Animal, 1, "Animal self-tamed", () => Helper.AnimalTamePossible(), (quote) => Helper.AnimalTame(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Yorkshire terriers"))
        {
        Event evt = new Event(64, EventType.Neutral, EventCategory.Animal, 2, "Yorkshire terriers", () => Helper.YorkshireTerrierPossible(), (quote) => Helper.YorkshireTerrier(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Manhunter pack"))
        {
        Event evt = new Event(65, EventType.Bad, EventCategory.Animal, 3, "Manhunter pack", () => Helper.ManhunterPackPossible(), (quote) => Helper.ManhunterPack(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Predators"))
        {
        Event evt = new Event(66, EventType.Bad, EventCategory.Animal, 5, "Predators", () => Helper.PredatorsPossible(), (quote) => Helper.Predators(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Inspiration"))
        {
        Event evt = new Event(67, EventType.Good, EventCategory.Mind, 1, "Insipration", () => true, (quote) => Helper.Inspiration(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Psychic wave"))
        {
        Event evt = new Event(68, EventType.Bad, EventCategory.Mind, 2, "Psychic wave", () => Helper.PsychicWavePossible(), (quote) => Helper.PsychicWave(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Psychic drone"))
        {
        Event evt = new Event(69, EventType.Bad, EventCategory.Mind, 2, "Psychic drone", () => Helper.PsychicDronePossible(), (quote) => Helper.PsychicDrone(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Psychic soothe"))
        {
        Event evt = new Event(70, EventType.Good, EventCategory.Mind, 2, "Psychic soothe", () => Helper.PsychicSoothePossible(), (quote) => Helper.PsychicSoothe(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Mental break minor"))
        {
        Event evt = new Event(71, EventType.Bad, EventCategory.Mind, 2, "Mental break (minor)", () => true, (quote) => Helper.MentalBreak(quote, 0));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Mental break major"))
        {
        Event evt = new Event(72, EventType.Bad, EventCategory.Mind, 3, "Mental break (major)", () => true, (quote) => Helper.MentalBreak(quote, 1));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Mental break extreme"))
        {
        Event evt = new Event(73, EventType.Bad, EventCategory.Mind, 4, "Mental break (extreme)", () => true, (quote) => Helper.MentalBreak(quote, 2));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Mental break berserk"))
        {
        Event evt = new Event(74, EventType.Bad, EventCategory.Mind, 5, "Mental break (berserk)", () => true, (quote) => Helper.Berserk(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Ship chunk drop"))
        {
        Event evt = new Event(75, EventType.Neutral, EventCategory.Drop, 1, "Ship chunk drop", () => Helper.ShipChunkDropPossible(), (quote) => Helper.ShipChunkDrop(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Cargo pod dropped"))
        {
        Event evt = new Event(76, EventType.Good, EventCategory.Drop, 2, "Cargo pod dropped", () => Helper.CargoPodPossible(), (quote) => Helper.CargoPod(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Transport pod dropped"))
        {
        Event evt = new Event(77, EventType.Good, EventCategory.Drop, 2, "Transport pod dropped", () => Helper.TransportPodPossible(), (quote) => Helper.TransportPod(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Ship part poison"))
        {
        Event evt = new Event(78, EventType.Bad, EventCategory.Drop, 4, "Ship part (poison)", () => Helper.ShipPartPoisonPossible(), (quote) => Helper.ShipPartPoison(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Ship part psychic"))
        {
        Event evt = new Event(79, EventType.Bad, EventCategory.Drop, 4, "Ship part (psychic)", () => Helper.ShipPartPsychicPossible(), (quote) => Helper.ShipPartPsychic(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Gold") && message.Contains("Dropping") == false)
        {
        Event evt = new Event(80, EventType.Good, EventCategory.Drop, 3, "Gold", () => true, (quote) => Helper.Gold(quote, 10, 20));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Dropping"))
        {
        int amount = Int32.Parse(message.Split(' ')[3]);
        string item = message.Split(' ')[4];
        Event evt = new Event(80, EventType.Good, EventCategory.Drop, 3, "Gold", () => true, (quote) => Helper.CargoDropItem(quote, amount, item));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Man in black"))
        {
        Event evt = new Event(81, EventType.Good, EventCategory.Foreigner, 1, "Man in black", () => Helper.ManInBlackPossible(), (quote) => Helper.ManInBlack(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Refugee chased"))
        {
        Event evt = new Event(82, EventType.Neutral, EventCategory.Foreigner, 2, "Refugee chased", () => Helper.RefugeeChasedPossible(), (quote) => Helper.RefugeeChased(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Traveler"))
        {
        Event evt = new Event(83, EventType.Neutral, EventCategory.Foreigner, 1, "Traveler", () => Helper.TravelerPossible(), (quote) => Helper.Traveler(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Visitor"))
        {
        Event evt = new Event(84, EventType.Neutral, EventCategory.Foreigner, 1, "Visitor", () => Helper.VisitorPossible(), (quote) => Helper.Visitor(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Trader visiting"))
        {
        Event evt = new Event(85, EventType.Neutral, EventCategory.Foreigner, 2, "Trader visiting", () => Helper.TraderPossible(), (quote) => Helper.Trader(quote));
        Ticker.Events.Enqueue(evt);
        }
        else if (message.Contains("Trader ship"))
        {
        Event evt = new Event(86, EventType.Neutral, EventCategory.Foreigner, 2, "Trader ship", () => Helper.TraderShipPossible(), (quote) => Helper.TraderShip(quote));
        Ticker.Events.Enqueue(evt);
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

        if(_eventsPossible == null)
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
        //Current.Game.AnyPlayerHomeMap.PlayerWealthForStoryteller;
        //Current.Game.tickManager.TicksGame;

        //_voteEvents = Events.GetEvents(Settings.VoteOptions, _voteWinningEvent, Doomsday ? Difficulty : 1, Difficulty);
        _voteAnswers.Clear();


        Helper.Log("Playing event lottery");
        List<IncidentDef> eventTest = _eventsPossible.ToList();

        int eventsTotal = eventTest.Count();
        int eventsNeeded = 5;
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
            if(win <= perc)
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
        if (_voteType == 0)
        {
        //msg += ("TwitchStoriesVote" + evt.Type.ToString()).Translate() + ", " + "TwitchStoriesChatMessageVoteCategory".Translate().Replace("{category}", ("TwitchStoriesVote" + evt.MainCategory.ToString()).Translate()) + ".";
        }
        else
        {
        msg += $"{_eventsPossibleChosen[evt].LabelCap}";
        }
        // msg += " (" + wonPercentage.ToString("P0") + " - " + "TwitchStoriesChatMessageVoteDifficulty".Translate().Replace("{difficulty}", evt.Difficulty.ToString()) + ")";
        _client.SendMessage(msg);

        //_voteWinningEvent = evt;
        //Ticker.Events.Enqueue(evt);
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
