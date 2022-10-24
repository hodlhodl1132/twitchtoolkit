using System.Collections.Generic;
using System.Linq;
using RimWorld;
using ToolkitCore;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Diseases;

public class RandomDisease : IncidentHelperVariables
{
	private int pointsWager = 0;

	private IncidentWorker worker = null;

	private IncidentParms parms = null;

	private IIncidentTarget target = null;

	private bool separateChannel = false;

	public override Viewer Viewer { get; set; }

	public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
	{
		//IL_008a: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0094: Expected O, but got Unknown
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
		worker = (IncidentWorker)new IncidentWorker_DiseaseHuman();
		List<IncidentDef> allDiseases = (from s in DefDatabase<IncidentDef>.AllDefs
			where s.workerClass == typeof(IncidentWorker_DiseaseHuman)
			select s).ToList();
		allDiseases.Shuffle();
		worker.def = allDiseases[0];
		target = (IIncidentTarget)(object)Current.Game.AnyPlayerHomeMap;
		if (target == null)
		{
			return false;
		}
		float defaultThreatPoints = StorytellerUtility.DefaultSiteThreatPointsNow();
		parms = StorytellerUtility.DefaultParmsNow(worker.def.category, target);
		parms.points = IncidentHelper_PointsHelper.RollProportionalGamePoints(storeIncident, pointsWager, StorytellerUtility.DefaultThreatPointsNow(target));
		return worker.CanFireNow(parms);
	}

	public override void TryExecute()
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing erences)
		if (worker.TryExecute(parms))
		{
			Viewer.TakeViewerCoins(pointsWager);
			Viewer.CalculateNewKarma(storeIncident.karmaType, pointsWager);
			VariablesHelpers.SendPurchaseMessage($"Starting {((Def)worker.def).LabelCap} with {pointsWager} points wagered and {(int)parms.points} disease points purchased by {Viewer.username}");
		}
		else
		{
			TwitchWrapper.SendChatMessage("@" + Viewer.username + " not enough points spent for diseases.");
		}
	}
}
