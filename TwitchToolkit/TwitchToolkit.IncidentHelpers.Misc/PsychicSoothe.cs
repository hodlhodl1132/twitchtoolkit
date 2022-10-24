using RimWorld;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Misc;

public class PsychicSoothe : IncidentHelper
{
	private IncidentParms parms = null;

	private IncidentWorker worker = null;

	public override bool IsPossible()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing erences)
		//IL_000c: Expected O, but got Unknown
		worker = (IncidentWorker)new IncidentWorker_PsychicSoothe();
		worker.def = IncidentDef.Named("PsychicSoothe");
		Map map = Helper.AnyPlayerMap;
		if (map != null)
		{
			parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, (IIncidentTarget)(object)map);
			return worker.CanFireNow(parms);
		}
		return false;
	}

	public override void TryExecute()
	{
		worker.TryExecute(parms);
	}
}
