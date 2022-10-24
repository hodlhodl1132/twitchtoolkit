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
		//IL_0012: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0018: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0029: Expected O, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0032: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0068: Unknown result type (might be due to invalid IL or missing erences)
		//IL_006f: Unknown result type (might be due to invalid IL or missing erences)
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
		//IL_0011: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0017: Unknown result type (might be due to invalid IL or missing erences)
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
		//IL_0042: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0048: Unknown result type (might be due to invalid IL or missing erences)
		List<Item> possibleItems = StoreInventory.items.FindAll((Item x) => x.price > 200 && x.price < 2000);
		Item randomItem = GenCollection.RandomElement<Item>((IEnumerable<Item>)possibleItems);
		string text = "SirRandoo has sent you a rare item! Enjoy!";
		Find.LetterStack.ReceiveLetter((TaggedString)("SirRandoo is here"), (TaggedString)(text), LetterDefOf.PositiveEvent, (string)null);
		randomItem.PutItemInCargoPod("Here have this! - SirRandoo", 1, "SirRandoo");
	}
}
