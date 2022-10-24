using RimWorld;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Misc;

public class AnimalTame : IncidentHelper
{
	private IncidentParms parms = null;

	private IncidentWorker worker = null;

	public override bool IsPossible()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing erences)
		//IL_000c: Expected O, but got Unknown
		worker = (IncidentWorker)new IncidentWorker_SelfTame();
		worker.def = IncidentDef.Named("SelfTame");
		Map map = Helper.AnyPlayerMap;
		if (map != null)
		{
			parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, (IIncidentTarget)(object)map);
			bool canFire = worker.CanFireNow(parms);
			Helper.Log("Can fire " + canFire);
			return canFire;
		}
		return false;
	}

	public override void TryExecute()
	{
		worker.TryExecute(parms);
	}
}
