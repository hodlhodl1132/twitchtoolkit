using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Incidents;

public class IncidentWorker_Alphabeavers : IncidentWorker
{
	private static readonly FloatRange CountPerColonistRange = new FloatRange(1f, 1.5f);

	private const int MinCount = 1;

	private const int MaxCount = 10;

	protected override bool CanFireNowSub(IncidentParms parms)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0020: Expected O, but got Unknown
		if (!((IncidentWorker)this).CanFireNow(parms))
		{
			return false;
		}
		Map map = (Map)parms.target;
		IntVec3 intVec = default(IntVec3);
		return RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, (Predicate<IntVec3>)null);
	}

	protected override bool TryExecuteWorker(IncidentParms parms)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing erences)
		//IL_000d: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0045: Unknown result type (might be due to invalid IL or missing erences)
		//IL_006e: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0073: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0078: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0085: Unknown result type (might be due to invalid IL or missing erences)
		//IL_008e: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0095: Expected O, but got Unknown
		//IL_00c9: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing erences)
		Map map = (Map)parms.target;
		PawnKindDef alphabeaver = PawnKindDefOf.Alphabeaver;
		IntVec3 intVec = default(IntVec3);
		if (!RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, (Predicate<IntVec3>)null))
		{
			return false;
		}
		int freeColonistsCount = map.mapPawns.FreeColonistsCount;
		FloatRange countPerColonistRange = CountPerColonistRange;
		float randomInRange = ((FloatRange)( countPerColonistRange)).RandomInRange;
		float f = (float)freeColonistsCount * randomInRange;
		int num = Mathf.Clamp(GenMath.RoundRandom(f), 1, 10);
		for (int i = 0; i < num; i++)
		{
			IntVec3 loc = CellFinder.RandomClosewalkCellNear(intVec, map, 10, (Predicate<IntVec3>)null);
			Pawn newThing = PawnGenerator.GeneratePawn(alphabeaver, (Faction)null);
			Pawn pawn = (Pawn)GenSpawn.Spawn((Thing)(object)newThing, loc, map, (WipeMode)0);
			((Need)pawn.needs.food).CurLevelPercentage = (1f);
		}
		Find.LetterStack.ReceiveLetter(Translator.Translate("LetterLabelBeaversArrived"), Translator.Translate("BeaversArrived"), LetterDefOf.ThreatSmall, (LookTargets)(new TargetInfo(intVec, map, false)), (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
		return true;
	}
}
