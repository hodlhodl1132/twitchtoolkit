using RimWorld;
using Verse;

namespace TwitchToolkit.Storytellers;

public class StorytellerCompProperties_ToolkitCategoryMTB : StorytellerCompProperties
{
	public float mtbDays = 3f;

	public SimpleCurve mtbDaysFactorByDaysPassedCurve;

	public IncidentCategoryDef category = IncidentCategoryDefOf.Misc;

	public float minDaysPassed = 5f;

	public StorytellerCompProperties_ToolkitCategoryMTB()
	{
		base.compClass = typeof(StorytellerComp_ToolkitCategoryMTB);
	}
}
