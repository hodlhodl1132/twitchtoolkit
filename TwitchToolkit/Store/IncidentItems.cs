using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using TwitchToolkit.Store;

namespace TwitchToolkit
{
    public class IncidentItems
    {
        public static Event[] defaultEvents = Events.GetEvents();

        public static IncItem[] GenerateDefaultProducts()
        {
            IncItem[] defaultProducts =
            {
                new IncItem(0, 0, "Small raid", "TwitchToolkitSmallraid".Translate(), KarmaType.Bad, 900, 0, 3),
                new IncItem(1, 0, "Medium raid", "TwitchToolkitMediumraid".Translate(), KarmaType.Bad, 1200, 1, 2),
                new IncItem(2, 0, "Big raid", "TwitchToolkitBigraid".Translate(), KarmaType.Bad, 1800, 2, 1),
                new IncItem(3, 0, "Medium raid drop", "TwitchToolkitMediumraiddrop".Translate(), KarmaType.Bad, 2100, 3, 1),
                new IncItem(4, 0, "Big raid drop", "TwitchToolkitBigraiddrop".Translate(), KarmaType.Bad, 2200, 4, 2),
                new IncItem(5, 0, "Big siege", "TwitchToolkitBigsiege".Translate(), KarmaType.Bad, 1900, 5, 1),
                new IncItem(6, 0, "Big mechanoid raid", "TwitchToolkitBigmechanoidraid".Translate(), KarmaType.Bad, 2400, 6, 1),
                new IncItem(7, 0, "Infestation", "TwitchToolkitInfestation".Translate(), KarmaType.Bad, 1600, 7, 1),

                new IncItem(8, 0, "Heart attack", "TwitchToolkitHeartattack".Translate(), KarmaType.Bad, 1100, 8, 1),
                new IncItem(9, 0, "Plague", "TwitchToolkitPlague".Translate(), KarmaType.Bad, 650, 9, 1),
                new IncItem(10, 0, "Flu", "TwitchToolkitFlu".Translate(), KarmaType.Bad, 600, 10, 1),
                new IncItem(11, 0, "Infection", "TwitchToolkitInfection".Translate(), KarmaType.Bad, 300, 11, 1),
                new IncItem(12, 0, "Malaria", "TwitchToolkitMalaria".Translate(), KarmaType.Bad, 550, 12, 1),
                new IncItem(13, 0, "Gut worms", "TwitchToolkitGutworms".Translate(), KarmaType.Bad, 400, 13, 1),
                new IncItem(14, 0, "Muscle parasites", "TwitchToolkitMuscleparasites".Translate(), KarmaType.Bad, 600, 14, 1),

                new IncItem(15, 0, "Plague medium", "TwitchToolkitPlaguemedium".Translate(), KarmaType.Bad, 1100, 15, 1),
                new IncItem(16, 0, "Flu medium", "TwitchToolkitFlumedium".Translate(), KarmaType.Bad, 1100, 16, 1),
                new IncItem(17, 0, "Infection medium", "TwitchToolkitInfectionmedium".Translate(), KarmaType.Bad, 1300, 17, 1),
                new IncItem(18, 0, "Malaria medium", "TwitchToolkitMalariamedium".Translate(), KarmaType.Bad, 1300, 18, 1),
                new IncItem(19, 0, "Gut worms medium", "TwitchToolkitGutwormsmedium".Translate(), KarmaType.Bad, 1300, 19, 1),
                new IncItem(20, 0, "Muscle parasites medium", "TwitchToolkitMuscleparasitesmedium".Translate(), KarmaType.Bad, 1300, 20, 1),

                new IncItem(21, 0, "Plague hard", "TwitchToolkitPlaguehard".Translate(), KarmaType.Bad, 1900, 21, 1),
                new IncItem(22, 0, "Flu hard", "TwitchToolkitFluhard".Translate(), KarmaType.Bad, 1900, 22, 1),
                new IncItem(23, 0, "Infection hard", "TwitchToolkitInfectionhard".Translate(), KarmaType.Bad, 1900, 23, 1),
                new IncItem(24, 0, "Malaria hard", "TwitchToolkitMalariahard".Translate(), KarmaType.Bad, 1900, 24, 1),
                new IncItem(25, 0, "Gut worms hard", "TwitchToolkitGutwormshard".Translate(), KarmaType.Bad, 1900, 25, 1),
                new IncItem(26, 0, "Muscle parasites hard", "TwitchToolkitMuscleparasiteshard".Translate(), KarmaType.Bad, 1900, 26, 1),
                new IncItem(27, 0, "Blindness", "TwitchToolkitBlindness".Translate(), KarmaType.Bad, 1900, 27, 1),

                new IncItem(28, 0, "Clear weather", "TwitchToolkitClearweather".Translate(), KarmaType.Good, 400, 28, 2),
                new IncItem(29, 0, "Rain", "TwitchToolkitRain".Translate(), KarmaType.Good, 350, 29, 2),
                new IncItem(30, 0, "Rainy thunderstorm", "TwitchToolkitRainythunderstorm".Translate(), KarmaType.Neutral, 250, 30, 2),
                new IncItem(31, 0, "Dry thunderstorm", "TwitchToolkitDrythunderstorm".Translate(), KarmaType.Bad, 450, 31, 2),
                new IncItem(32, 0, "Snow gentle", "TwitchToolkitSnowgentle".Translate(), KarmaType.Neutral, 350, 32, 2),
                new IncItem(33, 0, "Snow hard", "TwitchToolkitSnowhard".Translate(), KarmaType.Bad, 450, 33, 2),
                new IncItem(34, 0, "Fog", "TwitchToolkitFog".Translate(), KarmaType.Neutral, 300, 34, 2),
                new IncItem(35, 0, "Foggy rain", "TwitchToolkitFoggyrain".Translate(), KarmaType.Neutral, 300, 35, 2),
                new IncItem(36, 0, "Flash storm", "TwitchToolkitFlashstorm".Translate(), KarmaType.Bad, 300, 36, 2),

                new IncItem(37, 0, "Eclipse", "TwitchToolkitEclipse".Translate(), KarmaType.Bad, 450, 37, 2),
                new IncItem(38, 0, "Aurora", "TwitchToolkitAurora".Translate(), KarmaType.Good, 200, 38, 2),
                new IncItem(39, 0, "Vomit rain", "TwitchToolkitVomitrain".Translate(), KarmaType.Bad, -1, 39, 2),
                new IncItem(40, 0, "Ambrosia sprout", "TwitchToolkitAmbrosiasprout".Translate(), KarmaType.Good, 150, 40, 2),
                new IncItem(41, 0, "Meteorite", "TwitchToolkitMeteorite".Translate(), KarmaType.Neutral, 150, 41, 2),
                new IncItem(42, 0, "Meteorite shower", "TwitchToolkitMeteoriteshower".Translate(), KarmaType.Neutral, 600, 42, 2),

                new IncItem(43, 0, "Blight", "TwitchToolkitBlight".Translate(), KarmaType.Bad, 700, 43, 1),
                new IncItem(44, 0, "Solar flare", "TwitchToolkitSolarflare".Translate(), KarmaType.Bad, 500, 44, 1),
                new IncItem(45, 0, "Volcanic winter", "TwitchToolkitVolcanicwinter".Translate(), KarmaType.Doom, 2600, 45, 1),
                new IncItem(46, 0, "Toxic fallout", "TwitchToolkitToxicfallout".Translate(), KarmaType.Doom, 4200, 46, 1),
                new IncItem(47, 0, "Heat wave", "TwitchToolkitHeatwave".Translate(), KarmaType.Bad, 750, 47, 1),
                new IncItem(48, 0, "Cold snap", "TwitchToolkitColdsnap".Translate(), KarmaType.Bad, 800, 48, 1),
                new IncItem(49, 0, "Tornado", "TwitchToolkitTornado".Translate(), KarmaType.Bad, -1, 49, 1),
                new IncItem(50, 0, "Tornados", "TwitchToolkitTornados".Translate(), KarmaType.Bad, -1, 50, 1),

                new IncItem(51, 0, "Wild human", "TwitchToolkitWildhuman".Translate(), KarmaType.Good, 300, 51, 1),
                new IncItem(52, 0, "Wanderer joins", "TwitchToolkitWandererjoins".Translate(), KarmaType.Good, 450, 52, 2),
                new IncItem(53, 0, "Gender swap", "TwitchToolkitGenderswap".Translate(), KarmaType.Neutral, 150, 53, 2),
                new IncItem(54, 0, "Skill increase", "TwitchToolkitSkillincrease".Translate(), KarmaType.Good, 200, 54, 10),
                new IncItem(55, 0, "Party", "TwitchToolkitParty".Translate(), KarmaType.Good, 400, 55, 2),
                new IncItem(56, 0, "Cannibal", "TwitchToolkitCannibal".Translate(), KarmaType.Bad, 750, 56, 1),
                new IncItem(57, 0, "Luciferium", "TwitchToolkitLuciferium".Translate(), KarmaType.Doom, 6000, 57, 1),

                new IncItem(58, 0, "Mad animal", "TwitchToolkitMadanimal".Translate(), KarmaType.Bad, 600, 58, 2),
                new IncItem(59, 0, "Herd migration", "TwitchToolkitHerdmigration".Translate(), KarmaType.Good, 400, 59, 2),
                new IncItem(60, 0, "Animals wander in", "TwitchToolkitAnimalswanderin".Translate(), KarmaType.Good, 300, 60, 2),
                new IncItem(61, 0, "Rare thrumbos", "TwitchToolkitRarethrumbos".Translate(), KarmaType.Good, 500, 61, 2),
                new IncItem(62, 0, "Farm animals", "TwitchToolkitFarmanimals".Translate(), KarmaType.Good, 200, 62, 2),
                new IncItem(63, 0, "Animal self-tamed", "TwitchToolkitAnimalself".Translate(), KarmaType.Good, 150, 63, 2),
                new IncItem(64, 0, "Yorkshire terriers", "TwitchToolkitYorkshireterriers".Translate(), KarmaType.Good, 300, 64, 2),
                new IncItem(65, 0, "Manhunter pack", "TwitchToolkitManhunterpack".Translate(), KarmaType.Bad, 400, 65, 2),
                new IncItem(66, 0, "Predators", "TwitchToolkitPredators".Translate(), KarmaType.Doom, 1500, 66, 2),

                new IncItem(67, 0, "Inspiration", "TwitchToolkitInspiration".Translate(), KarmaType.Good, 150, 67, 3),
                new IncItem(68, 0, "Psychic wave", "TwitchToolkitPsychicwave".Translate(), KarmaType.Bad, 300, 68, 1),
                new IncItem(69, 0, "Psychic drone", "TwitchToolkitPsychicdrone".Translate(), KarmaType.Bad, 400, 69, 1),
                new IncItem(70, 0, "Psychic soothe", "TwitchToolkitPsychicsoothe".Translate(), KarmaType.Good, 250, 70, 2),
                new IncItem(71, 0, "Minor mental break", "TwitchToolkitMinormentalbreak".Translate(), KarmaType.Bad, 300, 71, 1),
                new IncItem(72, 0, "Major mental break", "TwitchToolkitMajormentalbreak".Translate(), KarmaType.Bad, 900, 72, 1),
                new IncItem(73, 0, "Extreme mental break", "TwitchToolkitExtremementalbreak".Translate(), KarmaType.Doom, 1800, 73, 1),
                new IncItem(74, 0, "Berserk mental break", "TwitchToolkitBerserkmentalbreak".Translate(), KarmaType.Doom, 10000, 74, 1),

                new IncItem(75, 0, "Ship chunk drop", "TwitchToolkitShipchunkdrop".Translate(), KarmaType.Good, 300, 75, 3),
                new IncItem(76, 0, "Cargo pod dropped", "TwitchToolkitCargopoddropped".Translate(), KarmaType.Good, 300, 76, 3),
                new IncItem(77, 0, "Transport pod dropped", "TwitchToolkitTransportpoddropped".Translate(), KarmaType.Good, 600, 77, 1),
                new IncItem(78, 0, "Ship part poison", "TwitchToolkitShippartpoison".Translate(), KarmaType.Bad, 1500, 78, 1),
                new IncItem(79, 0, "Ship part psychic", "TwitchToolkitShippartpsychic".Translate(), KarmaType.Bad, 2200, 79, 1),
                new IncItem(80, 1, "Care package", "TwitchToolkitCarepackage".Translate(), KarmaType.Good, -1, 80, 1),

                new IncItem(81, 0, "Man in black", "TwitchToolkitManinblack".Translate(), KarmaType.Good, 350, 81, 1),
                new IncItem(82, 0, "Refugee chased", "TwitchToolkitRefugeechased".Translate(), KarmaType.Neutral, 650, 82, 1),
                new IncItem(83, 0, "Traveler", "TwitchToolkitTraveler".Translate(), KarmaType.Good, 350, 83, 3),
                new IncItem(84, 0, "Visitor", "TwitchToolkitVisitor".Translate(), KarmaType.Good, 200, 84, 3),
                new IncItem(85, 0, "Trader visiting", "TwitchToolkitTradervisiting".Translate(), KarmaType.Good, 275, 85, 3),
                new IncItem(86, 0, "Trader ship", "TwitchToolkitTradership".Translate(), KarmaType.Good, 350, 86, 2),
                new IncItem(87, 0, "Military Aid", "TwitchToolkitMilitaryAid".Translate(), KarmaType.Good, 700, 87, 2),
                new IncItem(88, 0, "Cargo Pod Frenzy", "TwitchToolkitCargoPodFrenzy".Translate(), KarmaType.Good, 900, 88, 2),
                new IncItem(89, 0, "Big Meteorite Shower", "TwitchToolkitBigMeteoriteShower".Translate(), KarmaType.Neutral, -10, 89, 2),
            };

            return defaultProducts;
        }

        public static IncItem GetIncItem(string abr)
        {
            try
            {
                IncItem product = Settings.incItems.Find(x => x.abr == abr);
                return product;
            }
            catch (InvalidCastException e)
            {
                Helper.Log("Invalid Product");
                Helper.Log($"Source: {e.Source} - Message: {e.Message} - Trace: {e.StackTrace}");
                return null;
            }
        }

        public static void MultiplyIncItemPrices(double multiplier)
        {
            foreach(IncItem product in Settings.incItems)
            {
                product.price = Convert.ToInt32((double)product.price * multiplier);
                if (product.price < 1)
                {
                    product.price = 1;
                }
            }
        }
    }
}

