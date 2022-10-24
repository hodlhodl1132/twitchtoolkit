using System.Collections.Generic;
using System.Linq;
using RimWorld;
using TwitchToolkit.Votes;
using Verse;
using static Verse.DamageWorker;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Diseases;

public class HeartAttack : VotingHelper
{
	public override bool IsPossible()
	{
		if (target.PlayerPawnsForStoryteller != null)
		{
			return true;
		}
		return false;
	}

	public override void TryExecute()
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0067: Unknown result type (might be due to invalid IL or missing erences)
		List<Pawn> candidates = target.PlayerPawnsForStoryteller.ToList();
		candidates.Shuffle();
		candidates[0].health.AddHediff(HediffDef.Named("HeartAttack"), (BodyPartRecord)null, (DamageInfo?)null, (DamageResult)null);
		string text = string.Concat(candidates[0].Name, " is suffering from a heart attack.");
		Find.LetterStack.ReceiveLetter((TaggedString)("HeartAttack"), (TaggedString)(text), LetterDefOf.NegativeEvent, (LookTargets)((Thing)(object)candidates[0]), (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
	}
}
