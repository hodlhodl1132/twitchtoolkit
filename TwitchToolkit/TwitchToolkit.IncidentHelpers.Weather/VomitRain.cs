using System.Collections.Generic;
using RimWorld;
using TwitchToolkit.Incidents;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Weather
{
	public class VomitRain : IncidentHelper
	{
		private Map target = (Map) null;
		private WeatherDef weather = (WeatherDef) null;

		public override bool IsPossible()
		{
			this.weather = DefDatabase<WeatherDef>.GetNamed(nameof (VomitRain));
			List<Map> maps = Current.Game.Maps;
			maps.Shuffle<Map>();
			foreach (Map map in maps)
			{
				if (map.weatherManager.curWeather != this.weather)
				{
					this.target = map;
					return true;
				}
			}
			return false;
		}

		public override void TryExecute()
		{
			IncidentWorker_VomitRain incidentWorkerVomitRain = new IncidentWorker_VomitRain();
			incidentWorkerVomitRain.def = IncidentDef.Named(nameof (VomitRain));
			incidentWorkerVomitRain.TryExecute(StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, (IIncidentTarget) this.target));
		}
	}
}

