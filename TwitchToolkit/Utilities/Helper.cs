using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RimWorld;
using Verse;
using TSIncidents = TwitchToolkit.Incidents;
using System.Text;
using TwitchToolkit.Store;
using TwitchToolkit.Incidents;
using Verse.AI.Group;

namespace TwitchToolkit
{
    public static class Helper
    {
        public static string _state = null;
        public static List<string> playerMessages = new List<string>();

        public static string ReplacePlaceholder(
          string quote,
          string colonist = null,
          string colonists = null,
          string gender = null,
          string stat = null,
          string skill = null,
          string material = null,
          string item = null,
          string animal = null,
          string from = null,
          string to = null,
          string amount = null,
          string mod = null,
          string viewer = null,
          string newbalance = null,
          string karma = null,
          string first = null,
          string second = null,
          string third = null)
        {
        quote = quote.Replace("{colonist}", colonist ?? "");
        quote = quote.Replace("{colonists}", colonists ?? "");
        quote = quote.Replace("{gender}", gender ?? "");
        quote = quote.Replace("{stat}", stat ?? "");
        quote = quote.Replace("{skill}", skill ?? "");
        quote = quote.Replace("{material}", material ?? "");
        quote = quote.Replace("{item}", item ?? "");
        quote = quote.Replace("{animal}", animal ?? "");
        quote = quote.Replace("{from}", from ?? "");
        quote = quote.Replace("{to}", to ?? "");
        quote = quote.Replace("{amount}", amount ?? "");
        quote = quote.Replace("{mod}", mod ?? "");
        quote = quote.Replace("{viewer}", viewer ?? "");
        quote = quote.Replace("{newbalance}", newbalance ?? "");
        quote = quote.Replace("{karma}", karma ?? "");
        quote = quote.Replace("{first}", first ?? "");
        quote = quote.Replace("{second}", second ?? "");
        quote = quote.Replace("{third}", third ?? "");
        return quote;
        }

        public static Map AnyPlayerMap
        {
            get
            {
                if (Current.Game == null || Current.Game.Maps == null)
                    return null;
                else
                {
                    List<Map> maps = Current.Game.Maps.Where(s => s.IsPlayerHome).ToList();
                    maps.Shuffle();

                    return maps[0];
                }
            }
        }

        public static bool ModActive
        {
            get
            {
            return
              Helper.AnyPlayerMap != null &&
              Current.Game.storyteller != null &&
              Current.Game.storyteller.def != null &&
              Current.Game.storyteller.def.defName != null;
            }
        }

        private static string[] defaultColors = { "FF0000", "0000FF", "008000", "008000", "FF7F50", "9ACD32", "FF4500", "2E8B57", "DAA520", "D2691E", "5F9EA0", "1E90FF", "FF69B4", "8A2BE2", "8A2BE2"};

        public static string GetRandomColorCode()
        {
            return defaultColors[Verse.Rand.Range(0, defaultColors.Length - 1)];
        }

        public static void Log(string message)
        {
            Verse.Log.Message("<color=#6441A4>[Toolkit]</color> " + message);
        }

        public static void LogPaste(string message)
        {
            Verse.Log.Message(message);
        }

        public static void Vote(string message, LetterDef type)
        {
            if (message == null)
            {
                message = "";
            }

            Current.Game.letterStack.ReceiveLetter("TwitchStoriesVote".Translate(), message, type);
        }

        public static void Vote(string message, LetterDef type, Thing thing)
        {
            if (message == null)
            {
                message = "";
            }

            Current.Game.letterStack.ReceiveLetter("TwitchStoriesVote".Translate(), message, type, new TargetInfo(thing));
        }

        public static void Vote(string message, LetterDef type, List<Thing> things)
        {
            if (message == null)
            {
                message = "";
            }

            Find.LetterStack.ReceiveLetter("TwitchStoriesVote".Translate(), message, type, new LookTargets(things));
        }

        public static void Vote(string message, LetterDef type, List<Pawn> pawns)
        {
            if (message == null)
            {
                message = "";
            }

            Find.LetterStack.ReceiveLetter("TwitchStoriesVote".Translate(), message, type, new LookTargets(pawns));
        }

        public static void Vote(string message, LetterDef type, IntVec3 vec)
        {
            if (message == null)
            {
                message = "";
            }

            Current.Game.letterStack.ReceiveLetter("TwitchStoriesVote".Translate(), message, type, new TargetInfo(vec, Helper.AnyPlayerMap, false));
        }

        public static void CarePackage(string message, LetterDef type, IntVec3 vec)
        {
            if (message == null)
            {
                message = "";
            }

            Current.Game.letterStack.ReceiveLetter("TwitchToolkitCarePackage".Translate(), message, type, new TargetInfo(vec, Helper.AnyPlayerMap, false));
        }
       
        static IEnumerable<Pawn> GetColonists(float percentColony)
        {
            var pawns = new List<Pawn>();
            foreach (Map map in Current.Game.Maps)
            {
                foreach (Pawn pawn in map.mapPawns.AllPawns)
                {
                    if (!pawn.IsColonist || pawn.Dead || pawn.Destroyed || pawn.Discarded)
                    {
                        continue;
                    }

                    pawns.Add(pawn);
                }
            }

            pawns.Shuffle();
            var i = Math.Max(1, (int)(pawns.Count * percentColony));
            while (i-- > 0)
            {
                yield return pawns[i];
            }
        }

        static IEnumerable<Pawn> GetAnimals(float percentAnimals)
        {
            var pawns = new List<Pawn>();
            foreach (Pawn pawn in Helper.AnyPlayerMap.mapPawns.AllPawns)
            {
                if (!pawn.AnimalOrWildMan() || pawn.IsWildMan() || pawn.Dead || pawn.Destroyed || pawn.Discarded)
                {
                    continue;
                }

                pawns.Add(pawn);
            }

            pawns.Shuffle();
            var i = Math.Max(1, (int)(pawns.Count * percentAnimals));
            while (i-- > 0)
            {
                yield return pawns[i];
            }
        }

        public static bool GetRandomVec3(ThingDef thing, Map map, out IntVec3 vec, int contract = 0)
        {
            return CellFinderLoose.TryFindSkyfallerCell(thing, map, out vec, contract, map.Center, 99999, true, false, false, true, true, false, null);
        }

        #region Weather

        public static void Weather(string quote, WeatherDef weather, LetterDef type)
        {
            Helper.AnyPlayerMap.weatherManager.TransitionTo(weather);
            Vote(quote, type);
        }

        public static void WeatherClear(string quote)
        {
            Weather(quote, WeatherDefOf.Clear, LetterDefOf.NeutralEvent);
        }

        public static void WeatherRain(string quote)
        {
            Weather(quote, WeatherDef.Named("Rain"), LetterDefOf.NeutralEvent);
        }

        public static void WeatherFoggyRain(string quote)
        {
            Weather(quote, WeatherDef.Named("FoggyRain"), LetterDefOf.NeutralEvent);
        }

        public static void WeatherRainyThunderstorm(string quote)
        {
            Weather(quote, WeatherDef.Named("RainyThunderstorm"), LetterDefOf.NeutralEvent);
        }

        public static void WeatherDryThunderstorm(string quote)
        {
            Weather(quote, WeatherDef.Named("DryThunderstorm"), LetterDefOf.NeutralEvent);
        }

        public static void WeatherSnowGentle(string quote)
        {
            Weather(quote, WeatherDef.Named("SnowGentle"), LetterDefOf.NeutralEvent);
        }

        public static void WeatherSnowHard(string quote)
        {
            Weather(quote, WeatherDef.Named("SnowHard"), LetterDefOf.NeutralEvent);
        }

        public static void WeatherFog(string quote)
        {
            Weather(quote, WeatherDef.Named("Fog"), LetterDefOf.NeutralEvent);
        }

        public static void GameCondition(string quote, GameConditionDef condition, int ticks, LetterDef type)
        {
            Helper.AnyPlayerMap.GameConditionManager.RegisterCondition(GameConditionMaker.MakeCondition(condition, ticks));
            Vote(quote, type);
        }

        public static void WeatherFlashstorm(string quote, int ticks = 5 * 60 * 60)
        {
            GameCondition(quote, GameConditionDef.Named("Flashstorm"), ticks, LetterDefOf.NeutralEvent);
        }

        #endregion


        public static bool BlightPossible()
        {
            var incident = new RimWorld.IncidentWorker_CropBlight();
            incident.def = new IncidentDef();
            incident.def.tale = null;
            incident.def.category = new IncidentCategoryDef();
            incident.def.category.tale = null;
            return incident.CanFireNow(new IncidentParms
            {
                target = Helper.AnyPlayerMap
            });
        }

        public static void Blight(string quote = null)
        {
            var incident = new RimWorld.IncidentWorker_CropBlight();
            incident.def = new IncidentDef();
            if (quote != null)
            {
                _state = quote;
                Helper.Log("state set to " + _state);
            }
            incident.def.tale = null;
            incident.def.category = new IncidentCategoryDef();
            incident.def.category.tale = null;
            incident.TryExecute(new IncidentParms
            {
                target = Helper.AnyPlayerMap
            });
        }

        //public static bool RefugeeChasedPossible()
        //{
        //    var incident = new RimWorld.IncidentWorker_RefugeeChased();
        //    incident.def = IncidentDef.Named("RefugeeChased");
        //    return incident.CanFireNow(new IncidentParms
        //    {
        //        target = Helper.AnyPlayerMap
        //    });
        //}

        //public static void RefugeeChased(string quote)
        //{
        //    var incident = new RimWorld.IncidentWorker_RefugeeChased();
        //    incident.def = IncidentDef.Named("RefugeeChased");
        //    if (quote != null)
        //    {
        //        _state = quote;
        //        Helper.Log("state set to " + _state);
        //    }
        //    incident.TryExecute(new IncidentParms
        //    {
        //        target = Helper.AnyPlayerMap
        //    });
        //}

        public static bool AnimalTamePossible()
        {           
            var incident = new RimWorld.IncidentWorker_SelfTame();
            incident.def.tale = null;
            incident.def.category = new IncidentCategoryDef();
            incident.def.category.tale = null;
            return incident.CanFireNow(new IncidentParms
            {
                target = Helper.AnyPlayerMap
            }, true);
        }

        public static void AnimalTame(string quote = null)
        {
            var incident = new RimWorld.IncidentWorker_SelfTame();
            incident.def = new IncidentDef();
            if (quote != null)
            {
                _state = quote;
                Helper.Log("state set to " + _state);
            }
            incident.def.tale = null;
            incident.def.category = new IncidentCategoryDef();
            incident.def.category.tale = null;
            incident.TryExecute(new IncidentParms
            {
                target = Helper.AnyPlayerMap
            });
        }

        public static void VomitRain(string quote, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Vomit(Helper.AnyPlayerMap);
            }

            Vote(quote, LetterDefOf.NeutralEvent);
        }

        public static void Vomit(Map map)
        {
            var vomitDef = ThingDef.Named("DropPodIncoming");
            vomitDef.label = "vomit (incoming)";
            vomitDef.graphicData.texPath = "Things/Filth/PoolSoft";

            var vomit = new Filth();
            vomit.def = ThingDefOf.Filth_Vomit;

            IntVec3 intVec;
            if (!GetRandomVec3(vomitDef, map, out intVec))
            {
                return;
            }

            SkyfallerMaker.SpawnSkyfaller(vomitDef, new List<Thing> { vomit }, intVec, map);
        }

        public static void Meteorite(string quote)
        {
            var incident = new RimWorld.IncidentWorker_MeteoriteImpact();
            incident.def = IncidentDef.Named("MeteoriteImpact");
            if (quote != null)
            {
                _state = quote;
                Helper.Log("state set to " + _state);
            }
            incident.TryExecute(new IncidentParms()
            {
                target = Helper.AnyPlayerMap
            });
        }

        public static void MeteoriteShower(string quote, int count, bool mayHitColony = false)
        {
            List<Thing> meteorites = new List<Thing>();
            for (int i = 0; i < count; i++)
            {
                meteorites.Add(MeteoriteSpawn(Helper.AnyPlayerMap, mayHitColony));
            }

            Vote(quote, LetterDefOf.PositiveEvent, meteorites);
        }

        static bool TryFindMeteoriteCell(out IntVec3 cell, Map map)
        {
            int maxMineables = ThingSetMaker_Meteorite.MineablesCountRange.max;
            return CellFinderLoose.TryFindSkyfallerCell(ThingDefOf.MeteoriteIncoming, map, out cell, 10, default(IntVec3), -1, true, false, false, false, true, true, delegate (IntVec3 x)
            {
                int num = Mathf.CeilToInt(Mathf.Sqrt(maxMineables)) + 2;
                var cellRect = CellRect.CenteredOn(x, num, num);
                int num2 = 0;
                var iterator = cellRect.GetIterator();
                while (!iterator.Done())
                {
                    if (iterator.Current.InBounds(map) && iterator.Current.Standable(map))
                    {
                        num2++;
                    }
                    iterator.MoveNext();
                }
                return num2 >= maxMineables;
            });
        }

        public static Thing MeteoriteSpawn(Map map, bool mayHitColony)
        {
            IntVec3 intVec;
            if (mayHitColony)
            {
                if (!GetRandomVec3(ThingDefOf.MeteoriteIncoming, map, out intVec, 2))
                {
                    return null;
                }
            }
            else if (!TryFindMeteoriteCell(out intVec, map))
            {
                return null;
            }

            var list = ThingSetMakerDefOf.Meteorite.root.Generate();
            SkyfallerMaker.SpawnSkyfaller(ThingDefOf.MeteoriteIncoming, list, intVec, map);

            return list.Last();
        }

        public static void Tornado(string quote, int count = 1)
        {
            Map map = Helper.AnyPlayerMap;

            for (int i = 0; i < count; i++)
            {
                IntVec3 loc;
                if (!GetRandomVec3(ThingDefOf.Tornado, map, out loc, 30))
                {
                    return;
                }
                ThingDef tornado = ThingDef.Named("Tornado");
                GenSpawn.Spawn(tornado, loc, map);
            }

            Vote(quote, LetterDefOf.NegativeEvent);
        }

        public static void IncreaseRandomSkill(string quote)
        {
            foreach (Pawn pawn in GetColonists(0))
            {
                if (pawn.skills.skills.Count <= 0)
                {
                    continue;
                }

                pawn.skills.skills.Shuffle();

                for (int i = 0; i < pawn.skills.skills.Count; i++)
                {
                    SkillRecord skill = pawn.skills.skills[i];
                    if (skill.Level >= 20)
                    {
                        continue;
                    }

                    var before = skill.Level;
                    var amount = Verse.Rand.Range(1, 5);
                    skill.Level += amount;

                    quote = ReplacePlaceholder(quote, colonist: pawn.Name.ToString(), skill: skill.def.skillLabel, from: before.ToString(), to: skill.Level.ToString(), amount: amount.ToString());

                    Vote(quote, LetterDefOf.PositiveEvent, pawn);

                    return;
                }
            }
        }

        public static IntVec3 Rain(ThingDef thingDef, Thing thing)
        {
            IntVec3 intVec;
            if (!GetRandomVec3(thingDef, Helper.AnyPlayerMap, out intVec, 5))
            {
                return intVec;
            }

            Map any = Helper.AnyPlayerMap;
            intVec = DropCellFinder.TradeDropSpot(any);
            TradeUtility.SpawnDropPod(intVec, any, thing);

            return intVec;
        }

        public static IntVec3 Rain(ThingDef thingDef, MinifiedThing thing)
        {
            IntVec3 intVec;
            if (!GetRandomVec3(thingDef, Helper.AnyPlayerMap, out intVec, 5))
            {
                return intVec;
            }

            //SkyfallerMaker.SpawnSkyfaller(thingDef, new List<Thing> { thing }, intVec, Helper.AnyPlayerMap);
            //DropPodUtility.DropThingsNear(intVec, Helper.AnyPlayerMap, new List<Thing> { thing }, 110, false, true, true);
            Map any = Helper.AnyPlayerMap;
            intVec = DropCellFinder.TradeDropSpot(any);
            TradeUtility.SpawnDropPod(intVec, any, thing);

            return intVec;
        }


        public static Encoding LanguageEncoding()
        {
            string lang = Prefs.LangFolderName.ToLower();
            Encoding encoding;
            switch(lang)
            {
                case "deutsch":
                    encoding = System.Text.Encoding.GetEncoding(850);
                    break;
                default:
                    encoding = Encoding.UTF8;
                    break;
            }
            return encoding;
        }
    }
}
