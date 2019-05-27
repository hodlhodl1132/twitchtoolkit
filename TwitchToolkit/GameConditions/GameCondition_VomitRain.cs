using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace TwitchToolkit.GameConditions
{
    public class GameCondition_VomitRain : GameCondition_Flashstorm
    {
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<IntVec2>(ref this.centerLocation, "centerLocation", default(IntVec2), false);
            Scribe_Values.Look<int>(ref this.areaRadius, "areaRadius", 0, false);
        }

        public override void GameConditionTick()
        {
            base.GameConditionTick();

            IntVec3 newFilthLoc = CellFinderLoose.RandomCellWith((IntVec3 sq) => sq.Standable(AffectedMaps[0]) && !AffectedMaps[0].roofGrid.Roofed(sq), AffectedMaps[0], 1000);
            FilthMaker.MakeFilth(newFilthLoc, AffectedMaps[0], ThingDefOf.Filth_Vomit);
        }

        public override void End()
        {
            base.End();
            base.SingleMap.weatherDecider.StartNextWeather();
        }

        private int areaRadius;
    }
}
