using RimWorld;
using TwitchToolkit.Votes;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Raids;

public class Infestation : VotingHelper
{
	private float points;

	private IncidentWorker worker;

	private IncidentParms parms;

	public override bool IsPossible()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing erences)
		//IL_000c: Expected O, but got Unknown
		worker = (IncidentWorker)new IncidentWorker_Infestation();
		worker.def = IncidentDef.Named("Infestation");
		parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, target);
		parms.forced = true;
		points = parms.points;
		return worker.CanFireNow(parms);
	}

	public override void TryExecute()
	{
		worker.TryExecute(parms);
	}
}
