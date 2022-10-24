using RimWorld;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Animals;

public class MadAnimal : VotingHelper
{
	private Map map;

	private IncidentWorker worker;

	private IncidentParms parms;

	public override bool IsPossible()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0030: Expected O, but got Unknown
		if (target is Map)
		{
			 Map erence =  map;
			IIncidentTarget obj = target;
			erence = (Map)(object)((obj is Map) ? obj : null);
			worker = (IncidentWorker)new IncidentWorker_AnimalInsanitySingle();
			worker.def = IncidentDef.Named("AnimalInsanitySingle");
			parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatSmall, (IIncidentTarget)(object)map);
			return worker.CanFireNow(parms);
		}
		return false;
	}

	public override void TryExecute()
	{
		worker.TryExecute(parms);
	}
}
