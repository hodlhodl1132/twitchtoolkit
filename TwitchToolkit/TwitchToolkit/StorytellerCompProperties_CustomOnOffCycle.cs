using System.Collections.Generic;
using RimWorld;
using Verse;

namespace TwitchToolkit;

public class StorytellerCompProperties_CustomOnOffCycle : StorytellerCompProperties
{
	public float onDays;

	public float offDays;

	public float minSpacingDays;

	public FloatRange numIncidentsRange = FloatRange.Zero;

	public SimpleCurve acceptFractionByDaysPassedCurve;

	public SimpleCurve acceptPercentFactorPerThreatPointsCurve;

	public IncidentDef incident;

	private IncidentCategoryDef category;

	public bool applyRaidBeaconThreatMtbFactor;

	public float forceRaidEnemyBeforeDaysPassed;

	public IncidentCategoryDef IncidentCategory
	{
		get
		{
			if (incident != null)
			{
				return incident.category;
			}
			return category;
		}
	}

	public StorytellerCompProperties_CustomOnOffCycle()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0006: Unknown result type (might be due to invalid IL or missing erences)
		base.compClass = typeof(StorytellerComp_CustomOnOffCycle);
		base.minDaysPassed = 11f;
	}

	public override IEnumerable<string> ConfigErrors(StorytellerDef parentDef)
	{
		if (incident != null && category != null)
		{
			yield return "incident and category should not both be defined";
		}
		if (onDays <= 0f)
		{
			yield return "onDays must be above zero";
		}
		if (((FloatRange)( numIncidentsRange)).TrueMax <= 0f)
		{
			yield return "numIncidentRange not configured";
		}
		if (minSpacingDays * ((FloatRange)( numIncidentsRange)).TrueMax > onDays * 0.9f)
		{
			yield return "minSpacingDays too high compared to max number of incidents.";
		}
	}
}
