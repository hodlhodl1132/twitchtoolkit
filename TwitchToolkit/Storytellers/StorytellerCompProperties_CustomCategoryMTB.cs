using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace TwitchToolkit
{
    public class StorytellerCompProperties_CustomCategoryMTB : StorytellerCompProperties
    {
        public StorytellerCompProperties_CustomCategoryMTB()
        {
            this.compClass = typeof(StorytellerComp_CustomCategoryMTB);
        }

        public float mtbDays = -1f;

        public SimpleCurve mtbDaysFactorByDaysPassedCurve;

        public IncidentCategoryDef category;
    }
}
