using System.Collections.Generic;
using RimWorld;
using ToolkitCore;
using TwitchToolkit.Incidents;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Raids;

public class Predators : IncidentHelperVariables
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
			VariablesHelpers.ViewerDidWrongSyntax(viewer.username, storeIncident.syntax);
			return false;
		}
		if (!VariablesHelpers.PointsWagerIsValid(command[2], viewer,  ref pointsWager,  ref storeIncident, separateChannel))
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
		List<string> animals = new List<string> { "Bear_Grizzly", "Bear_Polar", "Rhinoceros", "Elephant", "Megasloth", "Thrumbo" };
		animals.Shuffle();
		ThingDef def = ThingDef.Named(animals[0]);
		float averagePower = 0f;
		if (def != null && def.race != null)
		{
			foreach (Tool t in def.tools)
			{
				averagePower += t.power;
			}
			averagePower /= (float)def.tools.Count;
		}
		float animalCount = 2.5f;
		if (averagePower > 18f)
		{
			animalCount = 2f;
		}
		worker = (IncidentWorker)(object)new IncidentWorker_SpecificAnimalsWanderIn("TwitchStoriesLetterLabelPredators", PawnKindDef.Named(animals[0]), joinColony: false, (int)animalCount, manhunter: true, defaultText: true);
		worker.def = IncidentDef.Named("HerdMigration");
		return worker.CanFireNow(parms);
	}

	public override void TryExecute()
	{
		if (worker.TryExecute(parms))
		{
			Viewer.TakeViewerCoins(pointsWager);
			Viewer.CalculateNewKarma(storeIncident.karmaType, pointsWager);
			VariablesHelpers.SendPurchaseMessage($"Starting predator pack with {pointsWager} points wagered and {(int)parms.points} raid points purchased by {Viewer.username}");
		}
		else
		{
			TwitchWrapper.SendChatMessage("@" + Viewer.username + " could not generate parms for manhunter pack.");
		}
	}
}
