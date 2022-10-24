using RimWorld;
using ToolkitCore;
using TwitchToolkit.Incidents;
using TwitchToolkit.PawnQueue;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Special;

public class VisitColony : IncidentHelperVariables
{
	private IncidentWorker_VisitColony worker;

	private IncidentParms parms;

	private bool separateChannel;

	public override Viewer Viewer { get; set; }

	public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
	{
		this.separateChannel = separateChannel;
		Viewer = viewer;
		string[] command = message.Split(' ');
		GameComponentPawns gameComponent = Current.Game.GetComponent<GameComponentPawns>();
		if (gameComponent.HasUserBeenNamed(viewer.username))
		{
			TwitchWrapper.SendChatMessage("@" + viewer.username + " you are in the colony and cannot visit.");
			return false;
		}
		worker = new IncidentWorker_VisitColony(viewer);
		((IncidentWorker)worker).def = IncidentDef.Named("VisitColony");
		parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, (IIncidentTarget)(object)Helper.AnyPlayerMap);
		return true;
	}

	public override void TryExecute()
	{
		((IncidentWorker)worker).TryExecute(parms);
		Viewer.TakeViewerCoins(storeIncident.cost);
		Viewer.CalculateNewKarma(storeIncident.karmaType, storeIncident.cost);
		VariablesHelpers.SendPurchaseMessage("@" + Viewer.username + " is visiting the colony.");
	}
}
