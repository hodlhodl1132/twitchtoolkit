using System;
using System.Linq;
using RimWorld;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Misc;

public class ShipPartPsychic : IncidentHelper
{
	private IncidentParms parms = null;

	private IncidentWorker worker = null;

	public override bool IsPossible()
	{
		Type innterType = (from s in typeof(IncidentWorker).Assembly.GetTypes()
			where s.Name == "IncidentWorker_CrashedShipPart"
			select s).First();
		object innerObject = Activator.CreateInstance(innterType);
		worker = (IncidentWorker)((innerObject is IncidentWorker) ? innerObject : null);
		worker.def = IncidentDef.Named("PsychicEmanatorShipPartCrash");
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
