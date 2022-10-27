using System.Collections.Generic;
using System.Linq;
using RimWorld;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Mind;

public class MentalBreakBase : VotingHelper
{
	private Map map;

	public Pawn pawn;

	public MentalBreakIntensity intensity;

	public override bool IsPossible()
	{
		if (target is Map)
		{
            Map reference =  map;
			IIncidentTarget obj = target;
			reference = (Map)(object)((obj is Map) ? obj : null);
			List<Pawn> candidates = (from s in map.mapPawns.FreeColonistsSpawned
				where !s.mindState.mentalStateHandler.InMentalState
				orderby s.mindState.mentalBreaker.CurMood
				select s).Take(3).ToList();
			if (candidates != null && candidates.Count > 0)
			{
				candidates.Shuffle();
				pawn = candidates[0];
			}
		}
		return false;
	}

	public override void TryExecute()
	{
		List<MentalBreakDef> mentalBreakDefs = (from s in DefDatabase<MentalBreakDef>.AllDefs
			where s.intensity == intensity
			select s).ToList();
		mentalBreakDefs.Shuffle();
		mentalBreakDefs[0].Worker.TryStart(pawn, "Upset by chat", true);
	}
}
