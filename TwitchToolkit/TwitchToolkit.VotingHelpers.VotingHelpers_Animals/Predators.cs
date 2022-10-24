using System.Collections.Generic;
using RimWorld;
using TwitchToolkit.Incidents;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Animals;

public class Predators : VotingHelper
{
	private IncidentWorker worker;

	private IncidentParms parms;

	public override bool IsPossible()
	{
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
		parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, target);
		return worker.CanFireNow(parms);
	}

	public override void TryExecute()
	{
		worker.TryExecute(parms);
	}
}
