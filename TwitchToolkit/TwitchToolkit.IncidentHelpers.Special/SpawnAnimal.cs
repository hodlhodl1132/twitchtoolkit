using System.Collections.Generic;
using System.Linq;
using RimWorld;
using ToolkitCore;
using TwitchToolkit.Incidents;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Special;

public class SpawnAnimal : IncidentHelperVariables
{
	private int pointsWager = 0;

	private IncidentWorker worker = null;

	private IncidentParms parms = null;

	private IIncidentTarget target = null;

	private bool separateChannel = false;

	private PawnKindDef pawnKind = null;

	public override Viewer Viewer { get; set; }

	public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
	{
		this.separateChannel = separateChannel;
		Viewer = viewer;
		string[] command = message.Split(' ');
		if (command.Length < 4)
		{
			TwitchWrapper.SendChatMessage("@" + viewer.username + " syntax is " + storeIncident.syntax);
			return false;
		}
		if (!VariablesHelpers.PointsWagerIsValid(command[3], viewer,  pointsWager,  storeIncident, separateChannel))
		{
			return false;
		}
		string animalKind = command[2].ToLower();
		List<PawnKindDef> allAnimals = (from s in DefDatabase<PawnKindDef>.AllDefs
			where s.RaceProps.Animal && string.Join("", ((Def)s).defName.Split(' ')).ToLower() == animalKind
			select s).ToList();
		if (allAnimals.Count < 1)
		{
			TwitchWrapper.SendChatMessage("@" + viewer.username + " no animal " + animalKind + " found.");
			return false;
		}
		target = (IIncidentTarget)(object)Helper.AnyPlayerMap;
		if (target == null)
		{
			return false;
		}
		float points = IncidentHelper_PointsHelper.RollProportionalGamePoints(storeIncident, pointsWager, StorytellerUtility.DefaultThreatPointsNow(target));
		pawnKind = allAnimals[0];
		int num = ManhunterPackIncidentUtility.GetAnimalsCount(pawnKind, points);
		worker = (IncidentWorker)(object)new IncidentWorker_SpecificAnimalsWanderIn(null, pawnKind, joinColony: true, num, manhunter: false, defaultText: true);
		worker.def = IncidentDef.Named("FarmAnimalsWanderIn");
		float defaultThreatPoints = StorytellerUtility.DefaultSiteThreatPointsNow();
		parms = StorytellerUtility.DefaultParmsNow(worker.def.category, target);
		parms.points = points;
		return worker.CanFireNow(parms);
	}

	public override void TryExecute()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing erences)
		if (worker.TryExecute(parms))
		{
			Viewer.TakeViewerCoins(pointsWager);
			Viewer.CalculateNewKarma(storeIncident.karmaType, pointsWager);
			VariablesHelpers.SendPurchaseMessage($"Spawning animal {((Def)pawnKind).LabelCap} with {pointsWager} coins wagered and {(int)parms.points} animal points purchased by {Viewer.username}");
		}
		else
		{
			TwitchWrapper.SendChatMessage("@" + Viewer.username + " not enough points spent for diseases.");
		}
	}
}
