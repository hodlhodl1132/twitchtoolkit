using System;
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
        if (!parms.forced)
        {
            if (!CanFireNow(parms))
            {
                return false;
            }
        }
		Map map = (Map)parms.target;
		return RCellFinder.TryFindRandomPawnEntryCell(out var intVec, map, CellFinder.EdgeRoadChance_Animal);
	}

	protected override bool TryExecuteWorker(IncidentParms parms)
	{
        Map map = (Map)parms.target;
		PawnKindDef alphabeaver = PawnKindDefOf.Alphabeaver;
        if (!RCellFinder.TryFindRandomPawnEntryCell(out var intVec, map, CellFinder.EdgeRoadChance_Animal, false, (Predicate<IntVec3>)null))
		{
			return false;
		}
		int freeColonistsCount = map.mapPawns.FreeColonistsCount;
		FloatRange countPerColonistRange = CountPerColonistRange;
		float randomInRange = countPerColonistRange.RandomInRange;
		float f = (float)freeColonistsCount * randomInRange;
		int num = Mathf.Clamp(GenMath.RoundRandom(f), 1, 10);
		for (int i = 0; i < num; i++)
		{
			IntVec3 loc = CellFinder.RandomClosewalkCellNear(intVec, map, 10);
			Pawn newThing = PawnGenerator.GeneratePawn(alphabeaver);
			Pawn pawn = (Pawn)GenSpawn.Spawn(newThing, loc, map);
			(pawn.needs.food).CurLevelPercentage = (1f);
		}
		Find.LetterStack.ReceiveLetter(Translator.Translate("LetterLabelBeaversArrived"), Translator.Translate("BeaversArrived"), LetterDefOf.ThreatSmall, (LookTargets)(new TargetInfo(intVec, map, false)));
		return true;
	}
}
