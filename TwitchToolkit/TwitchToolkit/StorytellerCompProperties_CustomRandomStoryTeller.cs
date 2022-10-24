using System.Collections.Generic;
using RimWorld;
using Verse;

namespace TwitchToolkit;

public class StorytellerCompProperties_CustomRandomStoryTeller : StorytellerCompProperties
{
	public float mtbDays;

	public List<IncidentCategoryEntry> categoryWeights = new List<IncidentCategoryEntry>();

	public float maxThreatBigIntervalDays = 99999f;

	public FloatRange randomPointsFactorRange = new FloatRange(0.5f, 1.5f);

	public bool skipThreatBigIfRaidBeacon;

	public StorytellerCompProperties_CustomRandomStoryTeller()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0026: Unknown result type (might be due to invalid IL or missing erences)
		base.compClass = typeof(StorytellerComp_CustomRandomStoryTeller);
	}
}
