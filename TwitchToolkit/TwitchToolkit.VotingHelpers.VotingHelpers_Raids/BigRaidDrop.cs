using RimWorld;
using TwitchToolkit.Votes;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Raids;

public class BigRaidDrop : VotingHelper
{
	private float points;

	private PawnsArrivalModeDef arrival = PawnsArrivalModeDefOf.CenterDrop;

	public override bool IsPossible()
	{
		points = StorytellerUtility.DefaultThreatPointsNow(target);
		return RaidHelpers.RaidPossible(points, arrival);
	}

	public override void TryExecute()
	{
		RaidHelpers.Raid(points, arrival);
	}
}
