using RimWorld;
using System.Collections.Generic;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Weather
{
    public class WeatherClear : IncidentHelper
    {
        public override bool IsPossible()
        {
            weather = DefDatabase<WeatherDef>.GetNamed("Clear");
            List<Map> allMaps = Current.Game.Maps;
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
            Helper.Weather("TwitchStoriesDescription29".Translate(), weather, LetterDefOf.PositiveEvent);
        }

        private Map target = null;
        private WeatherDef weather = null;
    }

    public class WeatherRain : IncidentHelper
    {
        public override bool IsPossible()
        {
            weather = DefDatabase<WeatherDef>.GetNamed("Rain");
            List<Map> allMaps = Current.Game.Maps;
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
            Helper.Weather("TwitchStoriesDescription30".Translate(), weather, LetterDefOf.PositiveEvent);
        }

        private Map target = null;
        private WeatherDef weather = null;
    }

    public class FoggyRain : IncidentHelper
    {
        public override bool IsPossible()
        {
            weather = DefDatabase<WeatherDef>.GetNamed("FoggyRain");
            List<Map> allMaps = Current.Game.Maps;
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
            Helper.Weather("TwitchStoriesDescription36".Translate(), weather, LetterDefOf.PositiveEvent);
        }

        private Map target = null;
        private WeatherDef weather = null;
    }

    public class RainyThunderStorm : IncidentHelper
    {
        public override bool IsPossible()
        {
            weather = DefDatabase<WeatherDef>.GetNamed("RainyThunderstorm");
            List<Map> allMaps = Current.Game.Maps;
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
            Helper.Weather("TwitchStoriesDescription31".Translate(), weather, LetterDefOf.PositiveEvent);
        }

        private Map target = null;
        private WeatherDef weather = null;
    }

    public class DryThunderStorm : IncidentHelper
    {
        public override bool IsPossible()
        {
            weather = DefDatabase<WeatherDef>.GetNamed("DryThunderstorm");
            List<Map> allMaps = Current.Game.Maps;
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
            Helper.Weather("TwitchStoriesDescription32".Translate(), weather, LetterDefOf.PositiveEvent);
        }

        private Map target = null;
        private WeatherDef weather = null;
    }

    public class SnowGentle : IncidentHelper
    {
        public override bool IsPossible()
        {
            weather = DefDatabase<WeatherDef>.GetNamed("SnowGentle");
            List<Map> allMaps = Current.Game.Maps;
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
            Helper.Weather("TwitchStoriesDescription33".Translate(), weather, LetterDefOf.PositiveEvent);
        }

        private Map target = null;
        private WeatherDef weather = null;
    }

    public class SnowHard : IncidentHelper
    {
        public override bool IsPossible()
        {
            weather = DefDatabase<WeatherDef>.GetNamed("SnowHard");
            List<Map> allMaps = Current.Game.Maps;
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
            Helper.Weather("TwitchStoriesDescription34".Translate(), weather, LetterDefOf.PositiveEvent);
        }

        private Map target = null;
        private WeatherDef weather = null;
    }

    public class Fog : IncidentHelper
    {
        public override bool IsPossible()
        {
            weather = DefDatabase<WeatherDef>.GetNamed("Fog");
            List<Map> allMaps = Current.Game.Maps;
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
            Helper.Weather("TwitchStoriesDescription35".Translate(), weather, LetterDefOf.PositiveEvent);
        }

        private Map target = null;
        private WeatherDef weather = null;
    }

    public class HeatWave : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_MakeGameCondition();
            worker.def = IncidentDef.Named("HeatWave");

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
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

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class Flashstorm : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_MakeGameCondition();
            worker.def = IncidentDef.Named("Flashstorm");

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
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

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class ColdSnap : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_MakeGameCondition();
            worker.def = IncidentDef.Named("ColdSnap");

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
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

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class SolarFlare : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_MakeGameCondition();
            worker.def = IncidentDefOf.SolarFlare;

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
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

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class ToxicFallout : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_MakeGameCondition();
            worker.def = IncidentDefOf.ToxicFallout;

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
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

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class VolcanicWinter : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_MakeGameCondition();
            worker.def = IncidentDef.Named("VolcanicWinter");

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
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

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class Eclipse : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_MakeGameCondition();
            worker.def = IncidentDefOf.Eclipse;

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
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

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class Aurora : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_Aurora();
            worker.def = IncidentDef.Named("Aurora");

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
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

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }
}
