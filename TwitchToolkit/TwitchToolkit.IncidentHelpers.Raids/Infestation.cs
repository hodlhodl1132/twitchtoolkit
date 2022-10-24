using RimWorld;
using ToolkitCore;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Raids;

public class Infestation : IncidentHelperVariables
{
	public int pointsWager = 0;

	public IIncidentTarget target = null;

	public IncidentWorker worker = null;

	public IncidentParms parms = null;

	private bool separateChannel = false;

	public override Viewer Viewer { get; set; }

	public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
	{
		//IL_00f9: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0103: Expected O, but got Unknown
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
		parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, target);
		parms.points = IncidentHelper_PointsHelper.RollProportionalGamePoints(storeIncident, pointsWager, StorytellerUtility.DefaultThreatPointsNow(target));
		parms.forced = true;
		worker = (IncidentWorker)new IncidentWorker_Infestation();
		worker.def = IncidentDef.Named("Infestation");
		bool canFire = worker.CanFireNow(parms);
		if (!canFire)
		{
			TwitchWrapper.SendChatMessage("@" + viewer.username + " no suitable location for infestation to occur.");
		}
		return canFire;
	}

	public override void TryExecute()
	{
		if (worker.TryExecute(parms))
		{
			Viewer.TakeViewerCoins(pointsWager);
			Viewer.CalculateNewKarma(storeIncident.karmaType, pointsWager);
			VariablesHelpers.SendPurchaseMessage($"Starting infestation raid with {pointsWager} points wagered and {(int)parms.points} raid points purchased by {Viewer.username}");
		}
		else
		{
			TwitchWrapper.SendChatMessage("@" + Viewer.username + " could not generate parms for infestation.");
		}
	}
}
