using System;
using RimWorld;
using Verse;

namespace TwitchToolkit.GameConditions;

public class GameCondition_VomitRain : GameCondition_Flashstorm
{
	private int areaRadius;

	public override void ExposeData()
	{
		Scribe_Defs.Look(ref def, "def");
		Scribe_Values.Look<IntVec2>(ref base.centerLocation, "centerLocation", default(IntVec2), false);
		Scribe_Values.Look<int>(ref areaRadius, "areaRadius", 0, false);
	}

	public override void GameConditionTick()
	{
		IntVec3 newFilthLoc = CellFinderLoose.RandomCellWith((Predicate<IntVec3>)((IntVec3 sq) => GenGrid.Standable(sq, ((GameCondition)this).AffectedMaps[0]) && !((GameCondition)this).AffectedMaps[0].roofGrid.Roofed(sq)), ((GameCondition)this).AffectedMaps[0], 1000);
		FilthMaker.TryMakeFilth(newFilthLoc, ((GameCondition)this).AffectedMaps[0], ThingDefOf.Filth_Vomit, 1, (FilthSourceFlags)0);
	}

	public override void End()
	{
		SingleMap.weatherDecider.StartNextWeather();
	}
}
