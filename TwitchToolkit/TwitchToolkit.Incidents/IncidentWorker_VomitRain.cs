using System;
using RimWorld;
using TwitchToolkit.GameConditions;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Incidents;

public class IncidentWorker_VomitRain : IncidentWorker
{
	protected override bool CanFireNowSub(IncidentParms parms)
	{
        Map map = (Map)parms.target;
		return !map.gameConditionManager.ConditionIsActive(GameConditionDef.Named("VomitRain"));
	}

	protected override bool TryExecuteWorker(IncidentParms parms)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing erences)
		//IL_000d: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0071: Unknown result type (might be due to invalid IL or missing erences)
		Map map = (Map)parms.target;
		map.weatherManager.TransitionTo(WeatherDef.Named("VomitRain"));
		int duration = Mathf.RoundToInt(((FloatRange)( base.def.durationDays)).RandomInRange * 60000f);
		GameCondition_VomitRain gameCondition_VomitRain = (GameCondition_VomitRain)(object)GameConditionMaker.MakeCondition(GameConditionDef.Named("VomitRain"), duration);
		map.gameConditionManager.RegisterCondition((GameCondition)(object)gameCondition_VomitRain);
		SendStandardLetter(parms, (LookTargets)(new TargetInfo(((IntVec2)( ((GameCondition_Flashstorm)gameCondition_VomitRain).centerLocation)).ToIntVec3, map, false)), Array.Empty<NamedArgument>());
		if (map.weatherManager.curWeather.rainRate > 0.1f)
		{
			map.weatherDecider.StartNextWeather();
		}
		return true;
	}
}
