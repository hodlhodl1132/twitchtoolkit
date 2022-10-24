using System.Collections.Generic;
using System.Linq;
using RimWorld;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Colonists;

public class GenderSwap : VotingHelper
{
	private Map map;

	private Pawn pawn;

	public override bool IsPossible()
	{
		IIncidentTarget obj = target;
		Map map;
		if ((map = (Map)(object)((obj is Map) ? obj : null)) != null)
		{
			IIncidentTarget obj2 = target;
			map = (Map)(object)((obj2 is Map) ? obj2 : null);
			List<Pawn> candidates = map.mapPawns.FreeColonistsSpawned.ToList();
			if (candidates != null && candidates.Count > 0)
			{
				pawn = candidates[0];
				return true;
			}
		}
		return false;
	}

	public override void TryExecute()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0013: Invalid comparison between Unknown and I4
		//IL_0019: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0054: Unknown result type (might be due to invalid IL or missing erences)
		//IL_005a: Unknown result type (might be due to invalid IL or missing erences)
		pawn.gender = (Gender)(((int)pawn.gender == 2) ? 1 : 2);
		string text = string.Concat(pawn.Name, " has switched genders and is now ", ((object)(Gender)( pawn.gender)).ToString());
		Find.LetterStack.ReceiveLetter((TaggedString)("GenderSwap"), (TaggedString)(text), LetterDefOf.NeutralEvent, (LookTargets)((Thing)(object)pawn), (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
	}
}
