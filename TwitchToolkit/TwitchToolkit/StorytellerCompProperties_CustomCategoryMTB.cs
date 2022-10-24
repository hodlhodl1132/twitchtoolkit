using RimWorld;
using Verse;

namespace TwitchToolkit;

public class StorytellerCompProperties_CustomCategoryMTB : StorytellerCompProperties
{
	public float mtbDays = -1f;

	public SimpleCurve mtbDaysFactorByDaysPassedCurve;

	public IncidentCategoryDef category;

	public StorytellerCompProperties_CustomCategoryMTB()
	{
		base.compClass = typeof(StorytellerComp_CustomCategoryMTB);
	}
}
