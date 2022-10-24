using System;
using System.Linq;
using RimWorld;
using TwitchToolkit.Votes;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Drops;

public class ShipPartPoison : VotingHelper
{
	private IncidentWorker worker;

	private IncidentParms parms;

	public override bool IsPossible()
	{
		Type innterType = (from s in typeof(IncidentWorker).Assembly.GetTypes()
			where s.Name == "IncidentWorker_CrashedShipPart"
			select s).First();
		object innerObject = Activator.CreateInstance(innterType);
		worker = (IncidentWorker)((innerObject is IncidentWorker) ? innerObject : null);
		worker.def = IncidentDef.Named("DefoliatorShipPartCrash");
		parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, target);
		return worker.CanFireNow(parms);
	}

	public override void TryExecute()
	{
		worker.TryExecute(parms);
	}
}
