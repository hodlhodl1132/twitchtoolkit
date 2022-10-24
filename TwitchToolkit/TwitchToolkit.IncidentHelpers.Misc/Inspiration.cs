using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Misc;

public class Inspiration : IncidentHelper
{
	private bool successfulInspiration = false;

	private InspirationDef randomAvailableInspirationDef = null;

	public override bool IsPossible()
	{
		List<Pawn> pawns = Helper.AnyPlayerMap.mapPawns.FreeColonistsSpawned.ToList();
		pawns.Shuffle();
		foreach (Pawn pawn in pawns)
		{
			if (pawn.Inspired)
			{
				continue;
			}
			randomAvailableInspirationDef = GenCollection.RandomElementByWeightWithFallback<InspirationDef>(from x in DefDatabase<InspirationDef>.AllDefsListForReading
				where true
				select x, (Func<InspirationDef, float>)((InspirationDef x) => x.Worker.CommonalityFor(pawn)), (InspirationDef)null);
			if (randomAvailableInspirationDef != null)
			{
				successfulInspiration = pawn.mindState.inspirationHandler.TryStartInspiration(randomAvailableInspirationDef, (string)null, true);
				if (successfulInspiration)
				{
					break;
				}
			}
		}
		return successfulInspiration;
	}

	public override void TryExecute()
	{
	}
}
