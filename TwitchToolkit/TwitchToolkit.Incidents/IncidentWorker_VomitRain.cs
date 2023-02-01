using System;
using RimWorld;
using TwitchToolkit.GameConditions;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Incidents
{
	public class IncidentWorker_VomitRain : IncidentWorker
	{
		protected override bool CanFireNowSub(IncidentParms parms) => !((Map) parms.target).gameConditionManager.ConditionIsActive(GameConditionDef.Named("VomitRain"));

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map target = (Map) parms.target;
			target.weatherManager.TransitionTo(WeatherDef.Named("VomitRain"));
			int duration = Mathf.RoundToInt(this.def.durationDays.RandomInRange * 60000f);
			GameCondition_VomitRain cond = (GameCondition_VomitRain) GameConditionMaker.MakeCondition(GameConditionDef.Named("VomitRain"), duration);
			target.gameConditionManager.RegisterCondition((GameCondition) cond);
			this.SendStandardLetter(parms, (LookTargets) new TargetInfo(cond.centerLocation.ToIntVec3, target));
			if ((double) target.weatherManager.curWeather.rainRate > 0.10000000149011612)
				target.weatherDecider.StartNextWeather();
			return true;
		}
	}
}

