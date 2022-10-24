using System.Collections.Generic;
using RimWorld;
using TwitchToolkit.Store;
using Verse;
using Verse.AI.Group;

namespace TwitchToolkit.IncidentHelpers.Misc;

public class Party : IncidentHelper
{
	private IntVec3 intVec;

	private Pawn pawn = null;

	public override bool IsPossible()
	{
		pawn = GatheringsUtility.FindRandomGatheringOrganizer(Faction.OfPlayer, Helper.AnyPlayerMap, GatheringDefOf.Party);
		if (pawn == null)
		{
			return false;
		}
		if (!RCellFinder.TryFindGatheringSpot(pawn, GatheringDefOf.Party, true, out intVec))
		{
			return false;
		}
		return true;
	}

	public override void TryExecute()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing erences)
		//IL_001f: Unknown result type (might be due to invalid IL or missing erences)
		//IL_002f: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0056: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0061: Unknown result type (might be due to invalid IL or missing erences)
		//IL_006c: Unknown result type (might be due to invalid IL or missing erences)
		GatheringDef gatheringDef = GatheringDefOf.Party;
		LordMaker.MakeNewLord(((Thing)pawn).Faction, (LordJob)new LordJob_Joinable_Party(intVec, pawn, gatheringDef), Helper.AnyPlayerMap, (IEnumerable<Pawn>)null);
		string text = ((Entity)pawn).LabelShort + " is throwing a party! Everyone who goes will gain recreation and social energy, and a lasting positive mood boost.";
		Find.LetterStack.ReceiveLetter((TaggedString)("Party"), (TaggedString)(text), LetterDefOf.PositiveEvent, (LookTargets)(new TargetInfo(intVec, Helper.AnyPlayerMap, false)), (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
	}
}
