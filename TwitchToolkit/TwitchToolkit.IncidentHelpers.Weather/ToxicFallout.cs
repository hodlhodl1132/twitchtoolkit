using System.Collections.Generic;
using RimWorld;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Weather;

public class ToxicFallout : IncidentHelper
{
	private IncidentParms parms = null;

	private IncidentWorker worker = null;

	public override bool IsPossible()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing erences)
		//IL_000c: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0027: Expected O, but got Unknown
		worker = (IncidentWorker)new IncidentWorker_MakeGameCondition();
		worker.def = IncidentDefOf.ToxicFallout;
		parms = new IncidentParms();
		List<Map> allMaps = Current.Game.Maps;
		allMaps.Shuffle();
		foreach (Map map in allMaps)
		{
			parms.target = (IIncidentTarget)(object)map;
			if (worker.CanFireNow(parms))
			{
				return true;
			}
		}
		return false;
	}

	public override void TryExecute()
	{
		worker.TryExecute(parms);
	}
}
