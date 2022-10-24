using System.Collections.Generic;
using RimWorld;
using Verse;

namespace TwitchToolkit.Storytellers;

public class StorytellerCompProperties_ToolkitOnOffCycle : StorytellerCompProperties
{
	public float onDays = 4.6f;

	public float offDays = 6f;

	public float minSpacingDays = 1.9f;

	public float minDaysPassed = 11f;

	public FloatRange numIncidentsRange = new FloatRange(1f, 2f);

	public SimpleCurve acceptFractionByDaysPassedCurve;

	public SimpleCurve acceptPercentFactorPerThreatPointsCurve;

	public IncidentDef incident;

	private IncidentCategoryDef category = IncidentCategoryDefOf.ThreatBig;

	public bool applyRaidBeaconThreatMtbFactor;

	public float forceRaidEnemyBeforeDaysPassed = 20f;

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

	public StorytellerCompProperties_ToolkitOnOffCycle()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing erences)
		//IL_003c: Unknown result type (might be due to invalid IL or missing erences)
		base.compClass = typeof(StorytellerComp_ToolkitOnOffCycle);
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
