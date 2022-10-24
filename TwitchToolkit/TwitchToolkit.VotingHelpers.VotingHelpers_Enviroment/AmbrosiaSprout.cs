using RimWorld;
using TwitchToolkit.Votes;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Enviroment;

public class AmbrosiaSprout : VotingHelper
{
	private IncidentWorker worker;

	private IncidentParms parms;

	public override bool IsPossible()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing erences)
		//IL_000c: Expected O, but got Unknown
		worker = (IncidentWorker)new IncidentWorker_AmbrosiaSprout();
		worker.def = IncidentDef.Named("AmbrosiaSprout");
		parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, target);
		return worker.CanFireNow(parms);
	}

	public override void TryExecute()
	{
		worker.TryExecute(parms);
	}
}
