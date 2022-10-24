using RimWorld;
using ToolkitCore;
using TwitchToolkit.Incidents;
using TwitchToolkit.Store;
using Verse;
using IncidentWorker_RaidEnemy = RimWorld.IncidentWorker_RaidEnemy;

namespace TwitchToolkit.IncidentHelpers.Raids;

public class SiegeRaid : IncidentHelperVariables
{
	public int pointsWager = 0;

	public IIncidentTarget target = null;

	public IncidentWorker worker = null;

	public IncidentParms parms = null;

	private bool separateChannel = false;

	public override Viewer Viewer { get; set; }

	public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
	{
		this.separateChannel = separateChannel;
		Viewer = viewer;
		string[] command = message.Split(' ');
		if (command.Length < 3)
		{
			TwitchWrapper.SendChatMessage("@" + viewer.username + " syntax is " + storeIncident.syntax);
			return false;
		}
		if (!VariablesHelpers.PointsWagerIsValid(command[2], viewer,  pointsWager,  storeIncident, separateChannel))
		{
			return false;
		}
		target = (IIncidentTarget)(object)Current.Game.AnyPlayerHomeMap;
		if (target == null)
		{
			return false;
		}
		parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatSmall, target);
		parms.points = IncidentHelper_PointsHelper.RollProportionalGamePoints(storeIncident, pointsWager, parms.points);
		parms.raidStrategy = DefDatabase<RaidStrategyDef>.GetNamed("Siege", true);
		parms.faction = Find.FactionManager.RandomEnemyFaction(false, false, false, (TechLevel)4);
		worker = new IncidentWorker_RaidEnemy();
		worker.def = IncidentDefOf.RaidEnemy;
		return worker.CanFireNow(parms);
	}

	public override void TryExecute()
	{
		if (worker.TryExecute(parms))
		{
			Viewer.TakeViewerCoins(pointsWager);
			Viewer.CalculateNewKarma(storeIncident.karmaType, pointsWager);
			VariablesHelpers.SendPurchaseMessage($"Starting siege raid with {pointsWager} points wagered and {(int)parms.points} raid points purchased by {Viewer.username}");
		}
		else
		{
			TwitchWrapper.SendChatMessage("@" + Viewer.username + " could not generate parms for siege raid.");
		}
	}
}
