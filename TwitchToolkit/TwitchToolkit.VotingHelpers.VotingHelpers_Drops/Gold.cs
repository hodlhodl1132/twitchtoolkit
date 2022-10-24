using System.Collections.Generic;
using RimWorld;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Drops;

public class Gold : VotingHelper
{
	private Map map;

	public override bool IsPossible()
	{
		if (target is Map)
		{
			 Map erence =  map;
			IIncidentTarget obj = target;
			erence = (Map)(object)((obj is Map) ? obj : null);
			return true;
		}
		return false;
	}

	public override void TryExecute()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0012: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0042: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0053: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0059: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0063: Unknown result type (might be due to invalid IL or missing erences)
		//IL_006b: Unknown result type (might be due to invalid IL or missing erences)
		ThingDef dropPod = ThingDef.Named("DropPodIncoming");
		Thing gold = new Thing();
		gold.def = ThingDefOf.Gold;
		gold.stackCount = Rand.Range(5, 10) * Find.ColonistBar.GetColonistsInOrder().Count;
		IntVec3 vec = Helper.Rain(dropPod, gold);
		string text = "A drop pod full of gold has arrived for your colony.";
		Find.LetterStack.ReceiveLetter((TaggedString)("Gold"), (TaggedString)(text), LetterDefOf.PositiveEvent, (LookTargets)(new TargetInfo(vec, map, false)), (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
	}
}
