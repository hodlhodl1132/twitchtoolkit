using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using TwitchToolkit.Store;
using Verse;
using Verse.Sound;

namespace TwitchToolkit.Utilities;

public static class EasterEgg
{
	public static void ExecuteHodlEasterEgg()
	{
		int duration = 180000;
		GameCondition_PsychicEmanation maleSoothe = (GameCondition_PsychicEmanation)GameConditionMaker.MakeCondition(GameConditionDefOf.PsychicSoothe, duration);
		GameCondition_PsychicEmanation femaleSoothe = (GameCondition_PsychicEmanation)GameConditionMaker.MakeCondition(GameConditionDefOf.PsychicSoothe, duration);
		maleSoothe.gender = (Gender)1;
		femaleSoothe.gender = (Gender)2;
		Map map = Find.CurrentMap;
		map.gameConditionManager.RegisterCondition((GameCondition)(object)maleSoothe);
		map.gameConditionManager.RegisterCondition((GameCondition)(object)femaleSoothe);
		string text = "A soothing feeling rushes over your colonists. The thought of hodl will help keep your colonists mood up for the next few days.";
		Find.LetterStack.ReceiveLetter((TaggedString)("Hodl is here"), (TaggedString)(text), LetterDefOf.PositiveEvent, (string)null);
		SoundStarter.PlayOneShotOnCamera(SoundDefOf.OrbitalBeam, map);
	}

	public static void ExecuteSaschahiEasterEgg()
	{
		string text = "Saschahi has visited the colony and delivered a very inspiring speech. Some of your colonists are now feeling inspired!";
		Find.LetterStack.ReceiveLetter((TaggedString)("Saschahi is here"), (TaggedString)(text), LetterDefOf.PositiveEvent, (string)null);
		List<Pawn> list = Helper.AnyPlayerMap.mapPawns.FreeColonistsSpawned.ToList();
		foreach (Pawn pawn in list)
		{
			if (!pawn.Inspired)
			{
				InspirationDef inspirationDef = GenCollection.RandomElementByWeightWithFallback<InspirationDef>(from x in DefDatabase<InspirationDef>.AllDefsListForReading
					where true
					select x, (Func<InspirationDef, float>)((InspirationDef x) => x.Worker.CommonalityFor(pawn)), (InspirationDef)null);
				if (inspirationDef != null)
				{
					pawn.mindState.inspirationHandler.TryStartInspiration(inspirationDef, (string)null, true);
				}
			}
		}
	}

	public static void ExecuteSirRandooEasterEgg()
	{
        List<Item> possibleItems = StoreInventory.items.FindAll((Item x) => x.price > 200 && x.price < 2000);
		Item randomItem = GenCollection.RandomElement<Item>((IEnumerable<Item>)possibleItems);
		string text = "SirRandoo has sent you a rare item! Enjoy!";
		Find.LetterStack.ReceiveLetter((TaggedString)("SirRandoo is here"), (TaggedString)(text), LetterDefOf.PositiveEvent);
		randomItem.PutItemInCargoPod("Here have this! - SirRandoo", 1, "SirRandoo");
	}

    public static void ExecuteNryEasterEgg()
    {
        string text = "Valorous luck.  -nry";
        Find.LetterStack.ReceiveLetter((TaggedString)("New objective: Survive."), (TaggedString)(text), LetterDefOf.PositiveEvent);

        List<Item> possibleItems = StoreInventory.items.FindAll((Item x) => x.price > 100 && x.price < 3000);
        Item randomItem = GenCollection.RandomElement(possibleItems);
        randomItem.PutItemInCargoPod("This might help ;)", Rand.Range(1, 5), "nry_chan");

        IncidentHelpers.Misc.Meteorite meteorite = new IncidentHelpers.Misc.Meteorite() { Viewer = new Viewer("nry_chan") };
        if (meteorite.IsPossible())
        {
            meteorite.TryExecute();
        }
        IncidentHelpers.Misc.Meteorite meteorite2 = new IncidentHelpers.Misc.Meteorite() { Viewer = new Viewer("nry_chan") };
        if (meteorite2.IsPossible())
        {
            meteorite2.TryExecute();
        }

        IncidentHelpers.Misc.Meteorite meteorite3 = new IncidentHelpers.Misc.Meteorite() { Viewer = new Viewer("nry_chan") };
        if (meteorite3.IsPossible())
        {
            meteorite3.TryExecute();
        }

        IncidentHelpers.Misc.Meteorite meteorite4 = new IncidentHelpers.Misc.Meteorite() { Viewer = new Viewer("nry_chan") };
        if (meteorite4.IsPossible())
        {
            meteorite4.TryExecute();
        }

        IncidentHelpers.Hazards.Tornados tornados = new IncidentHelpers.Hazards.Tornados(){Viewer = new Viewer("nry_chan")};
        if (tornados.IsPossible())
        {
            tornados.TryExecute();
        }

        IncidentHelpers.Hazards.Tornados tornados2 = new IncidentHelpers.Hazards.Tornados() { Viewer = new Viewer("nry_chan") };
        if (tornados2.IsPossible())
        {
            tornados2.TryExecute();
        }

        Log.Warning("reached the last portion");

    }

    public static void ExecuteYiskahEasterEgg()
    {
        string text = "Yiskah has visited the colony and gently caressed the head of some colonists. Some of your colonists are now feeling inspired!";
        Find.LetterStack.ReceiveLetter((TaggedString)("Yiskah is here"), (TaggedString)(text), LetterDefOf.PositiveEvent);
        List<Pawn> list = Helper.AnyPlayerMap.mapPawns.FreeColonistsSpawned.ToList();
        foreach (Pawn pawn in list)
        {
            if (!pawn.Inspired)
            {
                InspirationDef inspirationDef = GenCollection.RandomElementByWeightWithFallback<InspirationDef>(from x in DefDatabase<InspirationDef>.AllDefsListForReading
                                                                                                                where true
                                                                                                                select x, (Func<InspirationDef, float>)((InspirationDef x) => x.Worker.CommonalityFor(pawn)), (InspirationDef)null);
                if (inspirationDef != null)
                {
                    pawn.mindState.inspirationHandler.TryStartInspiration(inspirationDef, (string)null, true);
                }
            }
        }
    }

    public static void ExecuteLabratEasterEgg()
    {
        string text = "Labrat visited the colony, some colonist offered Labrat some cheese. Labrat appriciated their kindness and left a present.";
        Find.LetterStack.ReceiveLetter((TaggedString)("Labrat is here"), (TaggedString)(text), LetterDefOf.PositiveEvent, (string)null);
        List<Item> possibleItems = StoreInventory.items.FindAll((Item x) => x.price > 200 && x.price < 2000);
        Item randomItem = GenCollection.RandomElement<Item>((IEnumerable<Item>)possibleItems);
        randomItem.PutItemInCargoPod("*squeak*", Rand.Range(1, 5), "Labrat");
    }
}
