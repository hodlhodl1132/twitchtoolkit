using System.Collections.Generic;
using RimWorld;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Enviroment;

public class MeteoriteShower : VotingHelper
{
	private Map map;

	public override bool IsPossible()
	{
		if (target is Map)
		{
			 Map erence =  map;
			IIncidentTarget obj = target;
			erence = (Map)(object)((obj is Map) ? obj : null);
			return true;
		}
		return false;
	}

	public override void TryExecute()
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing erences)
		//IL_004f: Unknown result type (might be due to invalid IL or missing erences)
		List<Thing> meteorites = new List<Thing>();
		for (int i = 0; i < 5; i++)
		{
			int nxt = Rand.Range(0, 10);
			meteorites.Add(Helper.MeteoriteSpawn(Helper.AnyPlayerMap, nxt == 0));
		}
		string text = "A meteorite shower is happening near your colony.";
		Find.LetterStack.ReceiveLetter((TaggedString)("Meteors"), (TaggedString)(text), LetterDefOf.NeutralEvent, (LookTargets)(meteorites), (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
	}
}
