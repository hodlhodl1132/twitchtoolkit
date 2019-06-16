using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace TwitchToolkit.Storytellers
{
    public class StorytellerCompProperties_ToolkitOnOffCycle : StorytellerCompProperties
    {
        public StorytellerCompProperties_ToolkitOnOffCycle() => compClass = typeof(StorytellerComp_ToolkitOnOffCycle);

        public IncidentCategoryDef IncidentCategory
        {
            get
            {
                if (this.incident != null)
                {
                    return this.incident.category;
                }
                return this.category;
            }
        }

        public override IEnumerable<string> ConfigErrors(StorytellerDef parentDef)
        {
            if (this.incident != null && this.category != null)
            {
                yield return "incident and category should not both be defined";
            }
            if (this.onDays <= 0f)
            {
                yield return "onDays must be above zero";
            }
            if (this.numIncidentsRange.TrueMax <= 0f)
            {
                yield return "numIncidentRange not configured";
            }
            if (this.minSpacingDays * this.numIncidentsRange.TrueMax > this.onDays * 0.9f)
            {
                yield return "minSpacingDays too high compared to max number of incidents.";
            }
            yield break;
        }

        public float onDays = 4.6f;

        public float offDays = 6f;

        public float minSpacingDays = 1.9f;

        public new float minDaysPassed = 11.0f;

        public FloatRange numIncidentsRange = new FloatRange(1, 2);

        public SimpleCurve acceptFractionByDaysPassedCurve;

        public SimpleCurve acceptPercentFactorPerThreatPointsCurve;

        public IncidentDef incident;

        private IncidentCategoryDef category = IncidentCategoryDefOf.ThreatBig;

        public bool applyRaidBeaconThreatMtbFactor;

        public float forceRaidEnemyBeforeDaysPassed = 20f;
    }
}
