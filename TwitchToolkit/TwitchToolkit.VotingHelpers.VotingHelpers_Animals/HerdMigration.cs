using RimWorld;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Animals;

public class HerdMigration : VotingHelper
{
	private Map map;

	private IncidentWorker worker;

	private IncidentParms parms;

	public override bool IsPossible()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing erences)
		//IL_000c: Expected O, but got Unknown
		worker = (IncidentWorker)new IncidentWorker_HerdMigration();
		worker.def = IncidentDef.Named("HerdMigration");
		parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, target);
		return worker.CanFireNow(parms);
	}

	public override void TryExecute()
	{
		worker.TryExecute(parms);
	}
}
