using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace TwitchToolkit.Storytellers
{
    public class StorytellerCompProperties_ToolkitCategoryMTB : StorytellerCompProperties
    {
        public StorytellerCompProperties_ToolkitCategoryMTB() => compClass = typeof(StorytellerComp_ToolkitCategoryMTB);

        public float mtbDays = 3f;

        public SimpleCurve mtbDaysFactorByDaysPassedCurve;

        public IncidentCategoryDef category = IncidentCategoryDefOf.Misc;

        public new float minDaysPassed = 5;
    }
}
