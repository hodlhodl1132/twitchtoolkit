using System.Collections.Generic;
using RimWorld;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Weather;

public class WeatherRain : IncidentHelper
{
	private Map target = null;

	private WeatherDef weather = null;

	public override bool IsPossible()
	{
		weather = DefDatabase<WeatherDef>.GetNamed("Rain", true);
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
		//IL_0006: Unknown result type (might be due to invalid IL or missing erences)
		Helper.Weather((TaggedString)(Translator.Translate("TwitchStoriesDescription30")), weather, LetterDefOf.PositiveEvent);
	}
}
