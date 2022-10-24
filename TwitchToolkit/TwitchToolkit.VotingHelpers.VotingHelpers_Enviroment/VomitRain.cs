using RimWorld;
using TwitchToolkit.Incidents;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Enviroment;

public class VomitRain : VotingHelper
{
	private Map map;

	public override bool IsPossible()
	{
		if (target is Map)
		{
			 Map erence =  map;
			IIncidentTarget obj = target;
			erence = (Map)(object)((obj is Map) ? obj : null);
			return map.IsPlayerHome;
		}
		return false;
	}

	public override void TryExecute()
	{
		IncidentWorker worker = (IncidentWorker)(object)new IncidentWorker_VomitRain
		{
			def = IncidentDef.Named("VomitRain")
		};
		IncidentParms parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, (IIncidentTarget)(object)map);
		worker.TryExecute(parms);
	}
}
