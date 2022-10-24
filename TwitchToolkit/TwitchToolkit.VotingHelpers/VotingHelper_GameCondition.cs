using RimWorld;
using TwitchToolkit.Votes;

namespace TwitchToolkit.VotingHelpers;

public class VotingHelper_GameCondition : VotingHelper
{
	public string incidentDefName;

	private IncidentWorker worker;

	private IncidentParms parms;

	public override bool IsPossible()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing erences)
		//IL_000c: Expected O, but got Unknown
		worker = (IncidentWorker)new IncidentWorker_MakeGameCondition();
		worker.def = IncidentDef.Named(incidentDefName);
		parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, target);
		return worker.CanFireNow(parms);
	}

	public override void TryExecute()
	{
		worker.TryExecute(parms);
	}
}
