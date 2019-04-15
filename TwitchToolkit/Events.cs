using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;

namespace TwitchToolkit
{
    public delegate void EventStart(string quote);
    public delegate bool EventPossible();

    public class Quote
    {
        public int[] Ids;
        public string[] Quotes;

        public Quote(int[] ids, string[] quotes)
        {
            Ids = ids;
            Quotes = quotes;
        }
    }

    public class Difficulty
    {
        public int Diff;
        public double Weight = 1.0;
        public double WeightSum;

        public Difficulty(int diff)
        {
            Diff = diff;
        }
    }

    [Flags]
    public enum EventType
    {
        Bad = 1,
        Good = 2,
        Neutral = 4
    };

    [Flags]
    public enum EventCategory
    {
        Animal = 8,
        Colonist = 4,
        Drop = 128,
        Environment = 2,
        Foreigner = 256,
        Disease = 512,
        Hazard = 32,
        Invasion = 1,
        Mind = 64,
        Weather = 16,
    };

    public class Event
    {
        public int Id;
        public EventType Type;
        public EventCategory MainCategory;
        public EventCategory SubCategories;
        public int Difficulty;
        public string Description;
        public string[] Quotes;
        public int Count;
        readonly EventStart _start;
        readonly EventPossible _possible;

        public double Weight = 1.0;
        public double WeightSum;
        public string chatmessage = null;

        public Event(int id, EventType type, EventCategory mainCategory, int difficulty, string description, EventPossible possible, EventStart start)
        {
            Id = id;
            Type = type;
            MainCategory = mainCategory;
            Difficulty = difficulty;
            Description = description;
            Count = 0;
            _possible = possible;
            _start = start;
        }

        public void Reset()
        {
            Weight = 1.0;
        }

        public bool IsPossible()
        {
            try
            {
                if (Weight < 1.0)
                {
                    Weight += 0.1;
                }
                return _possible();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Start(bool simulation = false)
        {
            try
            {
                Weight = 0.1;

                if (!simulation)
                {
                    _start(Events.GetQuote(this.Id, this.chatmessage));
                }
            }
            catch (Exception ex)
            {
                Helper.Log("Exception: " + ex.Message + ex.StackTrace);
            }
        }
    }

    public static class Events
    {
        static Random _rand = new Random();

        static Event[] _events = {
      #region Invasion
      new Event(1,   EventType.Bad,     EventCategory.Invasion,        2, "Small raid",         () => Helper.RaidPossible(100, PawnsArrivalModeDefOf.EdgeWalkIn), (quote) => Helper.Raid(quote, 100, PawnsArrivalModeDefOf.EdgeWalkIn)),
      new Event(2,   EventType.Bad,     EventCategory.Invasion,        3, "Medium raid",        () => Helper.RaidPossible(250, PawnsArrivalModeDefOf.EdgeWalkIn), (quote) => Helper.Raid(quote, 250, PawnsArrivalModeDefOf.EdgeWalkIn)),
      new Event(3,   EventType.Bad,     EventCategory.Invasion,        4, "Big raid",           () => Helper.RaidPossible(500, PawnsArrivalModeDefOf.EdgeWalkIn), (quote) => Helper.Raid(quote, 500, PawnsArrivalModeDefOf.EdgeWalkIn)),
      new Event(4,   EventType.Bad,     EventCategory.Invasion,        4, "Medium raid drop",   () => Helper.RaidPossible(450, PawnsArrivalModeDefOf.EdgeWalkIn), (quote) => Helper.Raid(quote, 450, PawnsArrivalModeDefOf.CenterDrop)),
      new Event(5,   EventType.Bad,     EventCategory.Invasion,        5, "Big raid drop",      () => Helper.RaidPossible(900, PawnsArrivalModeDefOf.EdgeWalkIn), (quote) => Helper.Raid(quote, 900, PawnsArrivalModeDefOf.CenterDrop)),
      new Event(6,   EventType.Bad,     EventCategory.Invasion,        4, "Big siege",          () => Helper.SiegePossible(600, PawnsArrivalModeDefOf.EdgeWalkIn), (quote) => Helper.Siege(quote, 600, PawnsArrivalModeDefOf.EdgeWalkIn)),
      new Event(7,   EventType.Bad,     EventCategory.Invasion,        5, "Big mechanoid raid", () => Helper.MechanoidRaidPossible(1500, PawnsArrivalModeDefOf.EdgeWalkIn), (quote) => Helper.MechanoidRaid(quote, 1500, PawnsArrivalModeDefOf.EdgeWalkIn)),
      new Event(8,   EventType.Bad,     EventCategory.Invasion,        3, "Infestation",        () => Helper.InfestationPossible(), (quote) => Helper.Infestation(quote)),
      #endregion

      #region Disease
      new Event(9,   EventType.Bad,     EventCategory.Disease,    2, "Heart attack",             () => true, (quote) => Helper.DiseaseHeartAttack(quote)),
      new Event(10,  EventType.Bad,     EventCategory.Disease,    2, "Disease plague",           () => true, (quote) => Helper.DiseasePlague(quote, 0f)),
      new Event(11,  EventType.Bad,     EventCategory.Disease,    2, "Disease flu",              () => true, (quote) => Helper.DiseaseFlu(quote, 0f)),
      new Event(12,  EventType.Bad,     EventCategory.Disease,    2, "Disease infection",        () => true, (quote) => Helper.DiseaseInfection(quote, 0f)),
      new Event(13,  EventType.Bad,     EventCategory.Disease,    3, "Disease malaria",          () => true, (quote) => Helper.DiseaseMalaria(quote, 0f)),
      new Event(14,  EventType.Bad,     EventCategory.Disease,    1, "Disease gut worms",        () => true, (quote) => Helper.DiseaseGutWorms(quote, 0f)),
      new Event(15,  EventType.Bad,     EventCategory.Disease,    1, "Disease muscle parasites", () => true, (quote) => Helper.DiseaseMuscleParasites(quote, 0f)),

      new Event(16,  EventType.Bad,     EventCategory.Disease,    3, "Disease plague",           () => true, (quote) => Helper.DiseasePlague(quote, 0.5f)),
      new Event(17,  EventType.Bad,     EventCategory.Disease,    3, "Disease flu",              () => true, (quote) => Helper.DiseaseFlu(quote, 0.5f)),
      new Event(18,  EventType.Bad,     EventCategory.Disease,    3, "Disease infection",        () => true, (quote) => Helper.DiseaseInfection(quote, 0.33f)),
      new Event(19,  EventType.Bad,     EventCategory.Disease,    4, "Disease malaria",          () => true, (quote) => Helper.DiseaseMalaria(quote, 0.5f)),
      new Event(20,  EventType.Bad,     EventCategory.Disease,    2, "Disease gut worms",        () => true, (quote) => Helper.DiseaseGutWorms(quote, 0.5f)),
      new Event(21,  EventType.Bad,     EventCategory.Disease,    2, "Disease muscle parasites", () => true, (quote) => Helper.DiseaseMuscleParasites(quote, 0.5f)),

      new Event(22,  EventType.Bad,     EventCategory.Disease,    4, "Disease plague",           () => true, (quote) => Helper.DiseasePlague(quote, 0.8f)),
      new Event(23,  EventType.Bad,     EventCategory.Disease,    4, "Disease flu",              () => true, (quote) => Helper.DiseaseFlu(quote, 1f)),
      new Event(24,  EventType.Bad,     EventCategory.Disease,    4, "Disease infection",        () => true, (quote) => Helper.DiseaseInfection(quote, 0.66f)),
      new Event(25,  EventType.Bad,     EventCategory.Disease,    5, "Disease malaria",          () => true, (quote) => Helper.DiseaseMalaria(quote, 1f)),
      new Event(26,  EventType.Bad,     EventCategory.Disease,    3, "Disease gut worms",        () => true, (quote) => Helper.DiseaseGutWorms(quote, 1f)),
      new Event(27,  EventType.Bad,     EventCategory.Disease,    3, "Disease muscle parasites", () => true, (quote) => Helper.DiseaseMuscleParasites(quote, 1f)),

      new Event(28,  EventType.Bad,     EventCategory.Disease,    5, "Disease Blindness",        () => true, (quote) => Helper.DiseaseBlindness(quote)),
      #endregion

      #region Weather
      new Event(29,  EventType.Neutral, EventCategory.Weather, 1, "Clear weather",     () => true, (quote) => Helper.WeatherClear(quote)),
      new Event(30,  EventType.Neutral, EventCategory.Weather, 1, "Rain",              () => true, (quote) => Helper.WeatherRain(quote)),
      new Event(31,  EventType.Neutral, EventCategory.Weather, 1, "Rainy thunderstorm",() => true, (quote) => Helper.WeatherRainyThunderstorm(quote)),
      new Event(32,  EventType.Neutral, EventCategory.Weather, 1, "Dry thunderstorm",  () => true, (quote) => Helper.WeatherDryThunderstorm(quote)),
      new Event(33,  EventType.Neutral, EventCategory.Weather, 2, "Snow gentle",       () => true, (quote) => Helper.WeatherSnowGentle(quote)),
      new Event(34,  EventType.Neutral, EventCategory.Weather, 2, "Snow hard",         () => true, (quote) => Helper.WeatherSnowHard(quote)),
      new Event(35,  EventType.Neutral, EventCategory.Weather, 1, "Fog",               () => true, (quote) => Helper.WeatherFog(quote)),
      new Event(36,  EventType.Neutral, EventCategory.Weather, 1, "Foggy rain",        () => true, (quote) => Helper.WeatherFoggyRain(quote)),
      new Event(37,  EventType.Neutral, EventCategory.Weather, 3, "Flashstorm",        () => true, (quote) => Helper.WeatherFlashstorm(quote, 30000)),
      #endregion

      #region Environment
      new Event(38,  EventType.Neutral, EventCategory.Environment, 2, "Eclipse",         () => true, (quote) => Helper.WeatherEclipse(quote, 60000)),
      new Event(39,  EventType.Good,    EventCategory.Environment, 2, "Aurora",          () => Helper.WeatherAuroraPossible(), (quote) => Helper.WeatherAurora(quote)),
      new Event(40,  EventType.Neutral, EventCategory.Environment, 3, "Vomit rain",      () => true, (quote) => Helper.VomitRain(quote, 500)),
      new Event(41,  EventType.Good,    EventCategory.Environment, 1, "Ambrosia sprout", () => Helper.AmbrosiaSproutPossible(), (quote) => Helper.AmbrosiaSprout(quote)),
      new Event(42,  EventType.Neutral, EventCategory.Environment, 2, "Meteorite",       () => true, (quote) => Helper.Meteorite(quote)),
      new Event(43,  EventType.Neutral, EventCategory.Environment, 4, "Meteorite shower",() => true, (quote) => Helper.MeteoriteShower(quote, 5, true)),
      #endregion

      #region Hazard
      new Event(44,  EventType.Bad,     EventCategory.Hazard, 2, "Blight",          () => Helper.BlightPossible(), (quote) => Helper.Blight(quote)),
      new Event(45,  EventType.Bad,     EventCategory.Hazard, 3, "Solar flare",     () => Helper.WeatherSolarFlarePossible(60000), (quote) => Helper.WeatherSolarFlare(quote, 60000)),
      new Event(46,  EventType.Bad,     EventCategory.Hazard, 3, "Volcanic winter", () => Helper.WeatherVolcanicWinterPossible(480000), (quote) => Helper.WeatherVolcanicWinter(quote, 480000)),
      new Event(47,  EventType.Bad,     EventCategory.Hazard, 4, "Toxic fallout",   () => Helper.WeatherToxicFalloutPossible(300000), (quote) => Helper.WeatherToxicFallout(quote, 300000)),
      new Event(48,  EventType.Bad,     EventCategory.Hazard, 2, "Heat wave",         () => Helper.WeatherHeatWavePossible(60000), (quote) => Helper.WeatherHeatWave(quote, 60000)),
      new Event(49,  EventType.Bad,     EventCategory.Hazard, 2, "Cold snap",         () => Helper.WeatherColdSnapPossible(60000), (quote) => Helper.WeatherColdSnap(quote, 60000)),
      new Event(50,  EventType.Bad,     EventCategory.Hazard, 3, "Tornado",           () => true, (quote) => Helper.Tornado(quote, 1)),
      new Event(51,  EventType.Bad,     EventCategory.Hazard, 5, "Tornados",          () => true, (quote) => Helper.Tornado(quote, 10)),
      #endregion

      #region Colonists
      new Event(52,  EventType.Good,    EventCategory.Colonist,    1, "Wild human",               () => Helper.WildHumanPossible(), (quote) => Helper.WildHuman(quote)),
      new Event(53,  EventType.Good,    EventCategory.Colonist,    2, "Wanderer joins",           () => Helper.WandererPossible(), (quote) => Helper.Wanderer(quote)),
      new Event(54,  EventType.Neutral, EventCategory.Colonist,    2, "Gender swap",              () => true, (quote) => Helper.GenderSwap(quote)),
      new Event(55,  EventType.Good,    EventCategory.Colonist,    1, "Skill increase",           () => true, (quote) => Helper.IncreaseRandomSkill(quote)),
      new Event(56,  EventType.Good,    EventCategory.Colonist,    2, "Party",                    () => Helper.PartyPossible(), (quote) => Helper.Party(quote)),
      new Event(89,  EventType.Neutral, EventCategory.Colonist,    3, "Cannibal",                () => true, (quote) => Helper.Cannibal(quote)),
      new Event(57,  EventType.Bad,     EventCategory.Colonist,    5, "Luciferium",                () => true, (quote) => Helper.Luciferium(quote, 1)),
      #endregion

      #region Animal
      new Event(58,  EventType.Bad,     EventCategory.Animal, 1, "Mad animal",          () => Helper.MadAnimalPossible(), (quote) => Helper.MadAnimal(quote)),
      new Event(59,  EventType.Neutral, EventCategory.Animal, 1, "Herd migration",      () => Helper.HerdMigrationPossible(), (quote) => Helper.HerdMigration(quote)),
      new Event(60,  EventType.Neutral, EventCategory.Animal, 2, "Animal wander in",    () => Helper.AnimalsWanderInPossible(), (quote) => Helper.AnimalsWanderIn(quote)),
      new Event(61,  EventType.Neutral, EventCategory.Animal, 1, "Rare thrumbos",       () => Helper.ThrumbosPossible(), (quote) => Helper.Thrumbos(quote)),
      new Event(62,  EventType.Good,    EventCategory.Animal, 3, "Farm animals",        () => Helper.FarmAnimalsPossible(), (quote) => Helper.FarmAnimals(quote)),
      new Event(63,  EventType.Good,    EventCategory.Animal, 1, "Animal self-tamed",   () => true, (quote) => Helper.AnimalTame(quote)),
      new Event(64,  EventType.Neutral, EventCategory.Animal, 2, "Yorkshire terriers",  () => Helper.YorkshireTerrierPossible(), (quote) => Helper.YorkshireTerrier(quote)),
      new Event(65,  EventType.Bad,     EventCategory.Animal, 3, "Manhunter pack",      () => Helper.ManhunterPackPossible(), (quote) => Helper.ManhunterPack(quote)),
      new Event(66,  EventType.Bad,     EventCategory.Animal, 5, "Predators",           () => Helper.PredatorsPossible(), (quote) => Helper.Predators(quote)),
      #endregion

      #region Mind
      new Event(67,  EventType.Good,    EventCategory.Mind, 1, "Insipration",              () => true, (quote) => Helper.Inspiration(quote)),
      new Event(68,  EventType.Bad,     EventCategory.Mind, 2, "Psychic wave",    () => Helper.PsychicWavePossible(), (quote) => Helper.PsychicWave(quote)),
      new Event(69,  EventType.Bad,     EventCategory.Mind, 2, "Psychic drone",          () => Helper.PsychicDronePossible(), (quote) => Helper.PsychicDrone(quote)),
      new Event(70,  EventType.Good,    EventCategory.Mind, 2, "Psychic soothe",         () => Helper.PsychicSoothePossible(), (quote) => Helper.PsychicSoothe(quote)),
      new Event(71,  EventType.Bad,     EventCategory.Mind, 2, "Mental break (minor)",      () => true, (quote) => Helper.MentalBreak(quote, 0)),
      new Event(72,  EventType.Bad,     EventCategory.Mind, 3, "Mental break (major)",      () => true, (quote) => Helper.MentalBreak(quote, 1)),
      new Event(73,  EventType.Bad,     EventCategory.Mind, 4, "Mental break (extreme)",    () => true, (quote) => Helper.MentalBreak(quote, 2)),
      new Event(74,  EventType.Bad,     EventCategory.Mind, 5, "Mental break (berserk)",    () => true, (quote) => Helper.Berserk(quote)),
      #endregion

      #region Drop
      new Event(75,  EventType.Neutral, EventCategory.Drop, 1, "Ship chunk drop",          () => Helper.ShipChunkDropPossible(), (quote) => Helper.ShipChunkDrop(quote)),
      new Event(76,  EventType.Good,    EventCategory.Drop, 2, "Cargo pod dropped",        () => Helper.CargoPodPossible(), (quote) => Helper.CargoPod(quote)),
      new Event(77,  EventType.Good,    EventCategory.Drop, 2, "Transport pod dropped",    () => Helper.TransportPodPossible(), (quote) => Helper.TransportPod(quote)),
      new Event(78,  EventType.Bad,     EventCategory.Drop, 4, "Ship part (poison)", () => Helper.ShipPartPoisonPossible(), (quote) => Helper.ShipPartPoison(quote)),
      new Event(79,  EventType.Bad,     EventCategory.Drop, 4, "Ship part (psychic)",() => Helper.ShipPartPsychicPossible(), (quote) => Helper.ShipPartPsychic(quote)),
      new Event(80,  EventType.Good,    EventCategory.Drop, 3, "Gold",                     () => true, (quote) => Helper.Gold(quote, 100, 200)),
      #endregion

      #region Foreigner
      new Event(81,  EventType.Good,    EventCategory.Foreigner, 1, "Man in black",             () => Helper.ManInBlackPossible(), (quote) => Helper.ManInBlack(quote)),
      new Event(82,  EventType.Neutral, EventCategory.Foreigner, 2, "Refugee chased",           () => Helper.RefugeeChasedPossible(), (quote) => Helper.RefugeeChased(quote)),
      new Event(83,  EventType.Neutral, EventCategory.Foreigner, 1, "Traveler",          () => Helper.TravelerPossible(), (quote) => Helper.Traveler(quote)),
      new Event(84,  EventType.Neutral, EventCategory.Foreigner, 1, "Visitor",           () => Helper.VisitorPossible(), (quote) => Helper.Visitor(quote)),
      new Event(85,  EventType.Neutral, EventCategory.Foreigner, 2, "Trader visiting",   () => Helper.TraderPossible(), (quote) => Helper.Trader(quote)),
      new Event(86,  EventType.Neutral, EventCategory.Foreigner, 2, "Trader ship",       () => Helper.TraderShipPossible(), (quote) => Helper.TraderShip(quote)),
      #endregion
      new Event(87,  EventType.Good, EventCategory.Foreigner, 2, "Military aid",       () => true, (quote) => Helper.MilitaryAid(quote)),
      new Event(88,  EventType.Good,    EventCategory.Drop, 2, "Cargo Pod Frenzy",        () => Helper.CargoPodPossible(), (quote) => Helper.CargoPodFrenzy(quote)),
      new Event(90,  EventType.Neutral, EventCategory.Environment, 4, "Big meteorite shower",() => true, (quote) => Helper.MeteoriteShower(quote, 25, true)),
    };

        public static Event[] GetEvents()
        {
            return _events;
        }

        public static void Start(int id)
        {
            foreach (Event e in _events)
            {
                if (e.Id == id)
                {
                    e.Start();
                    break;
                }
            }
        }

        public static string GetQuote(int id, string chatmsg = null)
        {
            return GetQuote(id.ToString(), chatmsg);
        }

        public static string GetQuote(string id, string chatmsg = null)
        {
            var q = "";

            string s;
            if (("TwitchStoriesDescription" + id).TryTranslate(out s))
            {
                q += s;
                q += "\n\n";
            }


            q += "<b><i>";

            if (chatmsg == null)
            {
                q += s;
            }
            else
            {
                q += chatmsg;
            }

            q += "</i></b>";


            return q;
        }

        public static void Reset()
        {
            foreach (Event evt in _events)
            {
                evt.Reset();
            }

        }

        static Events()
        {
        }
    }
}
