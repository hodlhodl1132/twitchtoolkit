using RimWorld;
using TwitchToolkit.Votes;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Foreigners;

public class TraderShip : VotingHelper
{
	private IncidentWorker worker;

	private IncidentParms parms;

	public override bool IsPossible()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing erences)
		//IL_000c: Expected O, but got Unknown
		worker = (IncidentWorker)new IncidentWorker_OrbitalTraderArrival();
		worker.def = IncidentDefOf.OrbitalTraderArrival;
		parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, target);
		return worker.CanFireNow(parms);
	}

	public override void TryExecute()
	{
		worker.TryExecute(parms);
	}
}
