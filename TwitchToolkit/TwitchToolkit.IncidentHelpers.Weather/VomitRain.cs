using System.Collections.Generic;
using RimWorld;
using TwitchToolkit.Incidents;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Weather;

public class VomitRain : IncidentHelper
{
	private Map target = null;

	private WeatherDef weather = null;

	public override bool IsPossible()
	{
		weather = DefDatabase<WeatherDef>.GetNamed("VomitRain", true);
		List<Map> allMaps = Current.Game.Maps;
		allMaps.Shuffle();
		foreach (Map map in allMaps)
		{
			if (map.weatherManager.curWeather != weather)
			{
				target = map;
				return true;
			}
		}
		return false;
	}

	public override void TryExecute()
	{
		IncidentWorker worker = (IncidentWorker)(object)new IncidentWorker_VomitRain
		{
			def = IncidentDef.Named("VomitRain")
		};
		IncidentParms parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, (IIncidentTarget)(object)target);
		worker.TryExecute(parms);
	}
}
