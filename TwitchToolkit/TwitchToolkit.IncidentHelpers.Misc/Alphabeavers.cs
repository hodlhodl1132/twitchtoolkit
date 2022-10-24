using RimWorld;
using TwitchToolkit.Incidents;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Misc;

public class Alphabeavers : IncidentHelper
{
	private IncidentParms parms = null;

	private IncidentWorker worker = null;

	public override bool IsPossible()
	{
		worker = (IncidentWorker)(object)new IncidentWorker_Alphabeavers();
		worker.def = IncidentDef.Named("Alphabeavers");
		Map map = Helper.AnyPlayerMap;
		if (map != null)
		{
			parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, (IIncidentTarget)(object)map);
			parms.forced = true;
			return worker.CanFireNow(parms);
		}
		return false;
	}

	public override void TryExecute()
	{
		worker.TryExecute(parms);
	}
}
