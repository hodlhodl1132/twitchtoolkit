using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Mind;

public class Inspiration : VotingHelper
{
	private Map map;

	private List<Pawn> candidates;

	private bool successfulInspiration = false;

	private InspirationDef randomAvailableInspirationDef = null;

	public override bool IsPossible()
	{
		if (target is Map)
		{
			 Map erence =  map;
			IIncidentTarget obj = target;
			erence = (Map)(object)((obj is Map) ? obj : null);
			candidates = map.mapPawns.FreeColonistsSpawned.ToList();
			return true;
		}
		return false;
	}

	public override void TryExecute()
	{
		//IL_00d7: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing erences)
		foreach (Pawn pawn in candidates)
		{
			if (pawn.Inspired)
			{
				continue;
			}
			randomAvailableInspirationDef = GenCollection.RandomElementByWeight<InspirationDef>(from x in DefDatabase<InspirationDef>.AllDefsListForReading
				where true
				select x, (Func<InspirationDef, float>)((InspirationDef x) => x.Worker.CommonalityFor(pawn)));
			if (randomAvailableInspirationDef != null)
			{
				successfulInspiration = pawn.mindState.inspirationHandler.TryStartInspiration(randomAvailableInspirationDef, (string)null, true);
				if (successfulInspiration)
				{
					string text = (TaggedString)(string.Concat(pawn.Name, " has experienced an inspiration: ") + ((Def)randomAvailableInspirationDef).LabelCap);
					Find.LetterStack.ReceiveLetter((TaggedString)("Inspiration"), (TaggedString)(text), LetterDefOf.PositiveEvent, (LookTargets)((Thing)(object)pawn), (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
					break;
				}
			}
		}
		if (!successfulInspiration)
		{
			Log.Error("No pawn was available for inspiration.");
		}
	}
}
