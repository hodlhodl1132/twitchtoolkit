using System.Collections.Generic;
using System.Linq;
using RimWorld;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Diseases;

public class Blindness : VotingHelper
{
	private List<Pawn> candidates;

	public override bool IsPossible()
	{
		candidates = target.PlayerPawnsForStoryteller.ToList();
		if (candidates == null || candidates.Count < 1)
		{
			return false;
		}
		return true;
	}

	public override void TryExecute()
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0068: Unknown result type (might be due to invalid IL or missing erences)
		Pawn pawn = candidates[0];
		pawn.health.hediffSet.GetNotMissingParts((BodyPartHeight)0, (BodyPartDepth)0, (BodyPartTagDef)null, (BodyPartRecord)null).FirstOrDefault((BodyPartRecord x) => x.def == BodyPartDefOf.Eye);
		string text = string.Concat(pawn.Name, " has experienced sudden blindness.");
		Find.LetterStack.ReceiveLetter((TaggedString)("Blindness"), (TaggedString)(text), LetterDefOf.NegativeEvent, (LookTargets)((Thing)(object)pawn), (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
	}
}
