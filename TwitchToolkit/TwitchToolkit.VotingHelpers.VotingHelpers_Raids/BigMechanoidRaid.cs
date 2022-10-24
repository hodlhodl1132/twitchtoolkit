using RimWorld;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Raids;

public class BigMechanoidRaid : VotingHelper
{
	private float points;

	private PawnsArrivalModeDef arrival = PawnsArrivalModeDefOf.EdgeDrop;

	private Faction faction = Find.FactionManager.OfMechanoids;

	public override bool IsPossible()
	{
		points = StorytellerUtility.DefaultThreatPointsNow(target);
		return RaidHelpers.RaidPossible(points, arrival, null, faction);
	}

	public override void TryExecute()
	{
		RaidHelpers.Raid(points, arrival, null, faction);
	}
}
