using RimWorld;
using TwitchToolkit.Votes;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Diseases;

public abstract class DiseaseBase : VotingHelper
{
	private float points;

	public IncidentWorker worker = (IncidentWorker)new IncidentWorker_DiseaseHuman();

	public IncidentDef disease;

	public IncidentParms parms;

	public override bool IsPossible()
	{
		parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.DiseaseHuman, target);
		points = parms.points;
		return true;
	}

	public override void TryExecute()
	{
		worker.TryExecute(parms);
	}
}
