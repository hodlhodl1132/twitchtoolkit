using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.GameConditions;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Incidents
{
    public class IncidentWorker_VomitRain : IncidentWorker
    {
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            return !map.gameConditionManager.ConditionIsActive(GameConditionDef.Named("VomitRain"));
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            map.weatherManager.TransitionTo(WeatherDef.Named("VomitRain"));
            int duration = Mathf.RoundToInt(this.def.durationDays.RandomInRange * 60000f);
            GameCondition_VomitRain gameCondition_VomitRain = (GameCondition_VomitRain)GameConditionMaker.MakeCondition(GameConditionDef.Named("VomitRain"), duration, 0);
            map.gameConditionManager.RegisterCondition(gameCondition_VomitRain);
            base.SendStandardLetter(new TargetInfo(gameCondition_VomitRain.centerLocation.ToIntVec3, map, false), null, new string[0]);
            if (map.weatherManager.curWeather.rainRate > 0.1f)
            {
                map.weatherDecider.StartNextWeather();
            }
            return true;
        }
    }
}
