using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;
using static Verse.CellRect;

namespace TwitchToolkit;

public static class Helper
{
    public static string _state = null;

    public static List<string> playerMessages = new List<string>();

    private static string[] defaultColors = new string[15]
    {
        "FF0000", "0000FF", "008000", "008000", "FF7F50", "9ACD32", "FF4500", "2E8B57", "DAA520", "D2691E",
        "5F9EA0", "1E90FF", "FF69B4", "8A2BE2", "8A2BE2"
    };

    public static Map AnyPlayerMap
    {
        get
        {
            if (Current.Game == null || Current.Game.Maps == null)
            {
                return null;
            }
            List<Map> maps = (from s in Current.Game.Maps
                              where s.IsPlayerHome
                              select s).ToList();
            Extensions.Shuffle<Map>(maps);
            return maps.FirstOrDefault();
        }
    }

    public static bool ModActive => AnyPlayerMap != null && Current.Game.storyteller != null && Current.Game.storyteller.def != null && ((Def)Current.Game.storyteller.def).defName != null;

    public static string ReplacePlaceholder(string quote, string colonist = null, string colonists = null, string gender = null, string stat = null, string skill = null, string material = null, string item = null, string animal = null, string from = null, string to = null, string amount = null, string mod = null, string viewer = null, string newbalance = null, string karma = null, string first = null, string second = null, string third = null)
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

    public static string GetRandomColorCode()
    {
        return defaultColors[Rand.Range(0, defaultColors.Length - 1)];
    }

    public static Color HexToColor(string hexString)
    {
        //IL_0078: Unknown result type (might be due to invalid IL or missing references)
        //IL_007d: Unknown result type (might be due to invalid IL or missing references)
        //IL_0081: Unknown result type (might be due to invalid IL or missing references)
        if (hexString.IndexOf('#') != -1)
        {
            hexString = hexString.Replace("#", "");
        }
        int b = 0;
        int r = int.Parse(hexString.Substring(0, 2), NumberStyles.AllowHexSpecifier);
        int g = int.Parse(hexString.Substring(2, 2), NumberStyles.AllowHexSpecifier);
        b = int.Parse(hexString.Substring(4, 2), NumberStyles.AllowHexSpecifier);
        return new Color(0.01f * (float)r, 0.01f * (float)g, 0.01f * (float)b);
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
        //IL_0021: Unknown result type (might be due to invalid IL or missing references)
        //IL_0027: Unknown result type (might be due to invalid IL or missing references)
        if (message == null)
        {
            message = "";
        }
        Current.Game.letterStack.ReceiveLetter(Translator.Translate("TwitchStoriesVote"), (TaggedString)(message), type, (string)null);
    }

    public static void Vote(string message, LetterDef type, Thing thing)
    {
        //IL_0021: Unknown result type (might be due to invalid IL or missing references)
        //IL_0027: Unknown result type (might be due to invalid IL or missing references)
        //IL_002e: Unknown result type (might be due to invalid IL or missing references)
        if (message == null)
        {
            message = "";
        }
        Current.Game.letterStack.ReceiveLetter(Translator.Translate("TwitchStoriesVote"), (TaggedString)(message), type, (LookTargets)(new TargetInfo(thing)), (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
    }

    public static void Vote(string message, LetterDef type, List<Thing> things)
    {
        //IL_001c: Unknown result type (might be due to invalid IL or missing references)
        //IL_0022: Unknown result type (might be due to invalid IL or missing references)
        //IL_0029: Unknown result type (might be due to invalid IL or missing references)
        //IL_0037: Expected O, but got Unknown
        if (message == null)
        {
            message = "";
        }
        Find.LetterStack.ReceiveLetter(Translator.Translate("TwitchStoriesVote"), (TaggedString)(message), type, new LookTargets((IEnumerable<Thing>)things), (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
    }

    public static void Vote(string message, LetterDef type, List<Pawn> pawns)
    {
        //IL_001c: Unknown result type (might be due to invalid IL or missing references)
        //IL_0022: Unknown result type (might be due to invalid IL or missing references)
        //IL_0029: Unknown result type (might be due to invalid IL or missing references)
        //IL_0037: Expected O, but got Unknown
        if (message == null)
        {
            message = "";
        }
        Find.LetterStack.ReceiveLetter(Translator.Translate("TwitchStoriesVote"), (TaggedString)(message), type, new LookTargets((IEnumerable<Pawn>)pawns), (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
    }

    public static void Vote(string message, LetterDef type, IntVec3 vec)
    {
        //IL_0021: Unknown result type (might be due to invalid IL or missing references)
        //IL_0027: Unknown result type (might be due to invalid IL or missing references)
        //IL_002d: Unknown result type (might be due to invalid IL or missing references)
        //IL_0034: Unknown result type (might be due to invalid IL or missing references)
        if (message == null)
        {
            message = "";
        }
        Current.Game.letterStack.ReceiveLetter(Translator.Translate("TwitchStoriesVote"), (TaggedString)(message), type, (LookTargets)(new TargetInfo(vec, AnyPlayerMap, false)), (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
    }

    public static void CarePackage(string message, LetterDef type, IntVec3 vec)
    {
        //IL_0021: Unknown result type (might be due to invalid IL or missing references)
        //IL_0027: Unknown result type (might be due to invalid IL or missing references)
        //IL_002d: Unknown result type (might be due to invalid IL or missing references)
        //IL_0034: Unknown result type (might be due to invalid IL or missing references)
        if (message == null)
        {
            message = "";
        }
        Current.Game.letterStack.ReceiveLetter(Translator.Translate("TwitchToolkitCarePackage"), (TaggedString)(message), type, (LookTargets)(new TargetInfo(vec, AnyPlayerMap, false)), (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
    }

    private static IEnumerable<Pawn> GetColonists(float percentColony)
    {
        List<Pawn> pawns = new List<Pawn>();
        foreach (Map map in Current.Game.Maps)
        {
            foreach (Pawn pawn in map.mapPawns.AllPawns)
            {
                if (pawn.IsColonist && !pawn.Dead && !((Thing)pawn).Destroyed && !((Thing)pawn).Discarded)
                {
                    pawns.Add(pawn);
                }
            }
        }
        Extensions.Shuffle(pawns);
        int i = Math.Max(1, (int)((float)pawns.Count * percentColony));
        while (i-- > 0)
        {
            yield return pawns[i];
        }
    }

    private static IEnumerable<Pawn> GetAnimals(float percentAnimals)
    {
        List<Pawn> pawns = new List<Pawn>();
        foreach (Pawn pawn in AnyPlayerMap.mapPawns.AllPawns)
        {
            if (WildManUtility.AnimalOrWildMan(pawn) && !WildManUtility.IsWildMan(pawn) && !pawn.Dead && !((Thing)pawn).Destroyed && !((Thing)pawn).Discarded)
            {
                pawns.Add(pawn);
            }
        }
        Extensions.Shuffle<Pawn>(pawns);
        int i = Math.Max(1, (int)((float)pawns.Count * percentAnimals));
        while (i-- > 0)
        {
            yield return pawns[i];
        }
    }

    public static bool GetRandomVec3(ThingDef thing, Map map, out IntVec3 vec, int contract = 0)
    {
        //IL_0006: Unknown result type (might be due to invalid IL or missing references)
        return CellFinderLoose.TryFindSkyfallerCell(thing, map, out vec, contract, map.Center, 99999, true, false, false, true, true, false, (Predicate<IntVec3>)null);
    }

    public static void Weather(string quote, WeatherDef weather, LetterDef type)
    {
        AnyPlayerMap.weatherManager.TransitionTo(weather);
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
        AnyPlayerMap.GameConditionManager.RegisterCondition(GameConditionMaker.MakeCondition(condition, ticks));
        Vote(quote, type);
    }

    public static void WeatherFlashstorm(string quote, int ticks = 18000)
    {
        GameCondition(quote, GameConditionDef.Named("Flashstorm"), ticks, LetterDefOf.NeutralEvent);
    }

    public static bool BlightPossible()
    {
        //IL_0001: Unknown result type (might be due to invalid IL or missing references)
        //IL_0007: Expected O, but got Unknown
        //IL_0008: Unknown result type (might be due to invalid IL or missing references)
        //IL_0012: Expected O, but got Unknown
        //IL_0024: Unknown result type (might be due to invalid IL or missing references)
        //IL_002e: Expected O, but got Unknown
        //IL_0040: Unknown result type (might be due to invalid IL or missing references)
        //IL_0045: Unknown result type (might be due to invalid IL or missing references)
        //IL_0055: Expected O, but got Unknown
        IncidentWorker_CropBlight incident = new IncidentWorker_CropBlight();
        ((IncidentWorker)incident).def = new IncidentDef();
        ((IncidentWorker)incident).def.tale = null;
        ((IncidentWorker)incident).def.category = new IncidentCategoryDef();
        ((IncidentWorker)incident).def.category.tale = null;
        return ((IncidentWorker)incident).CanFireNow(new IncidentParms
        {
            target = (IIncidentTarget)(object)AnyPlayerMap
        });
    }

    public static void Blight(string quote = null)
    {
        //IL_0001: Unknown result type (might be due to invalid IL or missing references)
        //IL_0007: Expected O, but got Unknown
        //IL_0008: Unknown result type (might be due to invalid IL or missing references)
        //IL_0012: Expected O, but got Unknown
        //IL_0049: Unknown result type (might be due to invalid IL or missing references)
        //IL_0053: Expected O, but got Unknown
        //IL_0065: Unknown result type (might be due to invalid IL or missing references)
        //IL_006a: Unknown result type (might be due to invalid IL or missing references)
        //IL_007a: Expected O, but got Unknown
        IncidentWorker_CropBlight incident = new IncidentWorker_CropBlight();
        ((IncidentWorker)incident).def = new IncidentDef();
        if (quote != null)
        {
            _state = quote;
            Log("state set to " + _state);
        }
        ((IncidentWorker)incident).def.tale = null;
        ((IncidentWorker)incident).def.category = new IncidentCategoryDef();
        ((IncidentWorker)incident).def.category.tale = null;
        ((IncidentWorker)incident).TryExecute(new IncidentParms
        {
            target = (IIncidentTarget)(object)AnyPlayerMap
        });
    }

    public static bool AnimalTamePossible()
    {
        //IL_0001: Unknown result type (might be due to invalid IL or missing references)
        //IL_0007: Expected O, but got Unknown
        //IL_0019: Unknown result type (might be due to invalid IL or missing references)
        //IL_0023: Expected O, but got Unknown
        //IL_0035: Unknown result type (might be due to invalid IL or missing references)
        //IL_003a: Unknown result type (might be due to invalid IL or missing references)
        //IL_004a: Expected O, but got Unknown
        IncidentWorker_SelfTame incident = new IncidentWorker_SelfTame();
        ((IncidentWorker)incident).def.tale = null;
        ((IncidentWorker)incident).def.category = new IncidentCategoryDef();
        ((IncidentWorker)incident).def.category.tale = null;
        return ((IncidentWorker)incident).CanFireNow(new IncidentParms
        {
            target = (IIncidentTarget)(object)AnyPlayerMap
        });
    }

    public static void AnimalTame(string quote = null)
    {
        //IL_0001: Unknown result type (might be due to invalid IL or missing references)
        //IL_0007: Expected O, but got Unknown
        //IL_0008: Unknown result type (might be due to invalid IL or missing references)
        //IL_0012: Expected O, but got Unknown
        //IL_0049: Unknown result type (might be due to invalid IL or missing references)
        //IL_0053: Expected O, but got Unknown
        //IL_0065: Unknown result type (might be due to invalid IL or missing references)
        //IL_006a: Unknown result type (might be due to invalid IL or missing references)
        //IL_007a: Expected O, but got Unknown
        IncidentWorker_SelfTame incident = new IncidentWorker_SelfTame();
        ((IncidentWorker)incident).def = new IncidentDef();
        if (quote != null)
        {
            _state = quote;
            Log("state set to " + _state);
        }
        ((IncidentWorker)incident).def.tale = null;
        ((IncidentWorker)incident).def.category = new IncidentCategoryDef();
        ((IncidentWorker)incident).def.category.tale = null;
        ((IncidentWorker)incident).TryExecute(new IncidentParms
        {
            target = (IIncidentTarget)(object)AnyPlayerMap
        });
    }

    public static void VomitRain(string quote, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vomit(AnyPlayerMap);
        }
        Vote(quote, LetterDefOf.NeutralEvent);
    }

    public static void Vomit(Map map)
    {
        //IL_0027: Unknown result type (might be due to invalid IL or missing references)
        //IL_002d: Expected O, but got Unknown
        //IL_005a: Unknown result type (might be due to invalid IL or missing references)
        ThingDef vomitDef = ThingDef.Named("DropPodIncoming");
        ((Def)vomitDef).label = "vomit (incoming)";
        vomitDef.graphicData.texPath = "Things/Filth/PoolSoft";
        Filth vomit = new Filth();
        ((Thing)vomit).def = ThingDefOf.Filth_Vomit;
        if (GetRandomVec3(vomitDef, map, out var intVec))
        {
            SkyfallerMaker.SpawnSkyfaller(vomitDef, (IEnumerable<Thing>)new List<Thing> { (Thing)(object)vomit }, intVec, map);
        }
    }

    public static void Meteorite(string quote)
    {
        //IL_0001: Unknown result type (might be due to invalid IL or missing references)
        //IL_0007: Expected O, but got Unknown
        //IL_003d: Unknown result type (might be due to invalid IL or missing references)
        //IL_0042: Unknown result type (might be due to invalid IL or missing references)
        //IL_0052: Expected O, but got Unknown
        IncidentWorker_MeteoriteImpact incident = new IncidentWorker_MeteoriteImpact();
        ((IncidentWorker)incident).def = IncidentDef.Named("MeteoriteImpact");
        if (quote != null)
        {
            _state = quote;
            Log("state set to " + _state);
        }
        ((IncidentWorker)incident).TryExecute(new IncidentParms
        {
            target = (IIncidentTarget)(object)AnyPlayerMap
        });
    }

    public static void MeteoriteShower(string quote, int count, bool mayHitColony = false)
    {
        List<Thing> meteorites = new List<Thing>();
        for (int i = 0; i < count; i++)
        {
            meteorites.Add(MeteoriteSpawn(AnyPlayerMap, mayHitColony));
        }
        Vote(quote, LetterDefOf.PositiveEvent, meteorites);
    }

    private static bool TryFindMeteoriteCell(out IntVec3 cell, Map map)
    {
        //IL_000f: Unknown result type (might be due to invalid IL or missing references)
        //IL_002e: Unknown result type (might be due to invalid IL or missing references)
        //IL_0034: Unknown result type (might be due to invalid IL or missing references)
        int maxMineables = ThingSetMaker_Meteorite.MineablesCountRange.max;
        return CellFinderLoose.TryFindSkyfallerCell(ThingDefOf.MeteoriteIncoming, map, out cell, 10, default(IntVec3), -1, true, false, false, false, true, true, (Predicate<IntVec3>)delegate (IntVec3 x)
        {
            //IL_0015: Unknown result type (might be due to invalid IL or missing references)
            //IL_0018: Unknown result type (might be due to invalid IL or missing references)
            //IL_001d: Unknown result type (might be due to invalid IL or missing references)
            //IL_0022: Unknown result type (might be due to invalid IL or missing references)
            //IL_0027: Unknown result type (might be due to invalid IL or missing references)
            //IL_002d: Unknown result type (might be due to invalid IL or missing references)
            //IL_0041: Unknown result type (might be due to invalid IL or missing references)
            int num = Mathf.CeilToInt(Mathf.Sqrt((float)maxMineables)) + 2;
            CellRect val = CellRect.CenteredOn(x, num, num);
            int num2 = 0;
            CellRectIterator iterator = ((CellRect)( val)).GetIterator();
            while (!((CellRectIterator)(iterator)).Done())
            {
                if (GenGrid.InBounds(((CellRectIterator)(iterator)).Current, map) && GenGrid.Standable(((CellRectIterator)(iterator)).Current, map))
                {
                    num2++;
                }
                ((CellRectIterator)(iterator)).MoveNext();
            }
            return num2 >= maxMineables;
        });
    }

    public static Thing MeteoriteSpawn(Map map, bool mayHitColony)
    {
        //IL_0052: Unknown result type (might be due to invalid IL or missing references)
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
        List<Thing> list = ThingSetMakerDefOf.Meteorite.root.Generate();
        SkyfallerMaker.SpawnSkyfaller(ThingDefOf.MeteoriteIncoming, (IEnumerable<Thing>)list, intVec, map);
        return list.Last();
    }

    public static void Tornado(string quote, int count = 1)
    {
        //IL_0033: Unknown result type (might be due to invalid IL or missing references)
        Map map = AnyPlayerMap;
        for (int i = 0; i < count; i++)
        {
            if (!GetRandomVec3(ThingDefOf.Tornado, map, out var loc, 30))
            {
                return;
            }
            ThingDef tornado = ThingDef.Named("Tornado");
            GenSpawn.Spawn(tornado, loc, map, (WipeMode)0);
        }
        Vote(quote, LetterDefOf.NegativeEvent);
    }

    public static void IncreaseRandomSkill(string quote)
    {
        foreach (Pawn pawn in GetColonists(0f))
        {
            if (pawn.skills.skills.Count <= 0)
            {
                continue;
            }
            Extensions.Shuffle(pawn.skills.skills);
            for (int i = 0; i < pawn.skills.skills.Count; i++)
            {
                SkillRecord skill = pawn.skills.skills[i];
                if (skill.Level < 20)
                {
                    int before = skill.Level;
                    int amount = Rand.Range(1, 5);
                    skill.Level = (skill.Level + amount);
                    quote = ReplacePlaceholder(quote, ((object)pawn.Name).ToString(), null, null, null, skill.def.skillLabel, null, null, null, before.ToString(), skill.Level.ToString(), amount.ToString());
                    Vote(quote, LetterDefOf.PositiveEvent, (Thing)(object)pawn);
                    return;
                }
            }
        }
    }

    public static IntVec3 Rain(ThingDef thingDef, Thing thing)
    {
        if (!GetRandomVec3(thingDef, AnyPlayerMap, out var intVec, 5))
        {
            return intVec;
        }
        Map any = AnyPlayerMap;
        intVec = DropCellFinder.TradeDropSpot(any);
        TradeUtility.SpawnDropPod(intVec, any, thing);
        return intVec;
    }

    public static IntVec3 Rain(ThingDef thingDef, MinifiedThing thing)
    {
        if (!GetRandomVec3(thingDef, AnyPlayerMap, out var intVec, 5))
        {
            return intVec;
        }
        Map any = AnyPlayerMap;
        intVec = DropCellFinder.TradeDropSpot(any);
        TradeUtility.SpawnDropPod(intVec, any, (Thing)(object)thing);
        return intVec;
    }

    public static Encoding LanguageEncoding()
    {
        string lang = Prefs.LangFolderName.ToLower();
        string text = lang;
        if (text == "deutsch")
        {
            return Encoding.GetEncoding(850);
        }
        return Encoding.UTF8;
    }
}
