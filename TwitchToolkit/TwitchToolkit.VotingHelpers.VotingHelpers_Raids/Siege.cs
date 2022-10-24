using RimWorld;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Raids;

public class Siege : VotingHelper
{
	private float points;

	private PawnsArrivalModeDef arrival = PawnsArrivalModeDefOf.EdgeDrop;

	private RaidStrategyDef strategy = DefDatabase<RaidStrategyDef>.GetNamed("Siege", true);

	public override bool IsPossible()
	{
		points = StorytellerUtility.DefaultThreatPointsNow(target);
		return RaidHelpers.RaidPossible(points, arrival, strategy);
	}

	public override void TryExecute()
	{
		RaidHelpers.Raid(points, arrival, strategy, FactionUtility.DefaultFactionFrom(FactionDef.Named("OutlanderRough")));
	}
}
