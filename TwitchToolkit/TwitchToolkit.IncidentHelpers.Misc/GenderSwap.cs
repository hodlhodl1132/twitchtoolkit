using System.Collections.Generic;
using System.Linq;
using RimWorld;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Misc;

public class GenderSwap : IncidentHelper
{
	private Pawn pawn = null;

	public override bool IsPossible()
	{
		List<Pawn> allPawns = Helper.AnyPlayerMap.mapPawns.FreeColonistsAndPrisonersSpawned.ToList();
		if (allPawns == null || allPawns.Count < 1)
		{
			return false;
		}
		allPawns.Shuffle();
		pawn = allPawns[0];
		return true;
	}

	public override void TryExecute()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0013: Invalid comparison between Unknown and I4
		//IL_0019: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0023: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0078: Unknown result type (might be due to invalid IL or missing erences)
		//IL_007e: Unknown result type (might be due to invalid IL or missing erences)
		pawn.gender = (Gender)(((int)pawn.gender == 2) ? 1 : 2);
		string letterText = Helper.ReplacePlaceholder((TaggedString) (Translator.Translate("TwitchStoriesDescription54")), ((object)pawn.Name).ToString(), null, ((object)(Gender)(pawn.gender)).ToString());
		Current.Game.letterStack.ReceiveLetter(Translator.Translate("TwitchStoriesVote"), (TaggedString)(letterText), LetterDefOf.NeutralEvent, (string)null);
	}
}
