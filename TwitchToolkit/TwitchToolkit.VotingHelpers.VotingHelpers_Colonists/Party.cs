using System.Collections.Generic;
using RimWorld;
using TwitchToolkit.Votes;
using Verse;
using Verse.AI.Group;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Colonists;

public class Party : VotingHelper
{
	private Map map;

	private Pawn pawn;

	public override bool IsPossible()
	{
		IIncidentTarget obj = target;
		Map map;
		if ((map = (Map)(object)((obj is Map) ? obj : null)) == null)
		{
			return false;
		}
		IIncidentTarget obj2 = target;
		map = (Map)(object)((obj2 is Map) ? obj2 : null);
		pawn = GatheringsUtility.FindRandomGatheringOrganizer(Faction.OfPlayer, Helper.AnyPlayerMap, GatheringDefOf.Party);
		if (pawn == null)
		{
			return false;
		}
		IntVec3 intVec = default(IntVec3);
		if (!RCellFinder.TryFindGatheringSpot(pawn, GatheringDefOf.Party, true, out intVec))
		{
			return false;
		}
		return true;
	}

	public override void TryExecute()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0035: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0045: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing erences)
		//IL_006c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0076: Unknown result type (might be due to invalid IL or missing erences)
		//IL_007e: Unknown result type (might be due to invalid IL or missing erences)
		IntVec3 intVec = default(IntVec3);
		if (RCellFinder.TryFindGatheringSpot(pawn, GatheringDefOf.Party, true, out intVec))
		{
			LordMaker.MakeNewLord(((Thing)pawn).Faction, (LordJob)new LordJob_Joinable_Party(intVec, pawn, GatheringDefOf.Party), Helper.AnyPlayerMap, (IEnumerable<Pawn>)null);
			string text = ((Entity)pawn).LabelShort + " is throwing a party! Everyone who goes will gain recreation and social energy, and a lasting positive mood boost.";
			Find.LetterStack.ReceiveLetter((TaggedString)("Party"), (TaggedString)(text), LetterDefOf.PositiveEvent, (LookTargets)(new TargetInfo(intVec, map, false)), (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
		}
	}
}
