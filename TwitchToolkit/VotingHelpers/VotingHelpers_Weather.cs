using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.IncidentHelpers.Weather;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Weather
{
    public abstract class Weather : VotingHelper
    {
        public override bool IsPossible()
        {
            weather = DefDatabase<WeatherDef>.GetNamed(weatherDefName);

            Map map = target as Map;

            if (map != null && map.weatherManager.curWeather != weather)
            {
                return true;
            }

            return false;
        }

        public override void TryExecute()
        {
            Helper.Weather("Chat has voted to change the weather to: " + weather.LabelCap, weather, LetterDefOf.PositiveEvent);
        }

        public string weatherDefName;
        private WeatherDef weather;
    }

    public class WeatherClear : Weather
    {
        public WeatherClear()
        {
            this.weatherDefName = "Clear";
        }
    }

    public class WeatherRain : Weather
    {
        public WeatherRain()
        {
            this.weatherDefName = "Rain";
        }
    }

    public class FoggyRain : Weather
    {
        public FoggyRain()
        {
            this.weatherDefName = "FoggyRain";
        }
    }

    public class RainyThunderstorm : Weather
    {
        public RainyThunderstorm()
        {
            this.weatherDefName = "RainyThunderstorm";
        }
    }

    public class DryThunderstorm : Weather
    {
        public DryThunderstorm()
        {
            this.weatherDefName = "DryThunderstorm";
        }
    }

    public class SnowGentle : Weather
    {
        public SnowGentle()
        {
            this.weatherDefName = "SnowGentle";
        }
    }

    public class SnowHard : Weather
    {
        public SnowHard()
        {
            this.weatherDefName = "SnowHard";
        }
    }

    public class Fog : Weather
    {
        public Fog()
        {
            this.weatherDefName = "Fog";
        }
    }

    public class Flashstorm : VotingHelper_GameCondition
    {
        public Flashstorm()
        {
            this.incidentDefName = "Flashstorm";
        }
    }
}
