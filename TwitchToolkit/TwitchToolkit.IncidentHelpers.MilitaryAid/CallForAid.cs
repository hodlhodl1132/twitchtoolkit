using RimWorld;
using TwitchToolkit.Incidents;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.MilitaryAid;

public class CallForAid : IncidentHelper
{
	public override bool IsPossible()
	{
		return true;
	}

	public override void TryExecute()
	{
		Map currentMap = Find.CurrentMap;
		IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.AllyAssistance, (IIncidentTarget)(object)currentMap);
		incidentParms.forced = true;
		incidentParms.target = (IIncidentTarget)(object)currentMap;
		incidentParms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
		incidentParms.raidStrategy = RaidStrategyDefOf.ImmediateAttackFriendly;
		IncidentWorker_CallForAid incident = new IncidentWorker_CallForAid
		{
			def = IncidentDef.Named("RaidFriendly")
		};
		((IncidentWorker)incident).TryExecute(incidentParms);
	}
}
