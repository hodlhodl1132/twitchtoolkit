using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace TwitchToolkit
{
    public class StorytellerCompProperties_CustomOnOffCycle : StorytellerCompProperties
    {
        // Token: 0x06000FA2 RID: 4002 RVA: 0x00074894 File Offset: 0x00072C94
        public StorytellerCompProperties_CustomOnOffCycle()
        {
            this.compClass = typeof(StorytellerComp_CustomOnOffCycle);
        }

        // Token: 0x17000239 RID: 569
        // (get) Token: 0x06000FA3 RID: 4003 RVA: 0x000748B7 File Offset: 0x00072CB7
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

        // Token: 0x06000FA4 RID: 4004 RVA: 0x000748D8 File Offset: 0x00072CD8
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

        // Token: 0x040009AC RID: 2476
        public float onDays;

        // Token: 0x040009AD RID: 2477
        public float offDays;

        // Token: 0x040009AE RID: 2478
        public float minSpacingDays;

        // Token: 0x040009AF RID: 2479
        public FloatRange numIncidentsRange = FloatRange.Zero;

        // Token: 0x040009B0 RID: 2480
        public SimpleCurve acceptFractionByDaysPassedCurve;

        // Token: 0x040009B1 RID: 2481
        public SimpleCurve acceptPercentFactorPerThreatPointsCurve;

        // Token: 0x040009B2 RID: 2482
        public IncidentDef incident;

        // Token: 0x040009B3 RID: 2483
        private IncidentCategoryDef category;

        // Token: 0x040009B4 RID: 2484
        public bool applyRaidBeaconThreatMtbFactor;

        // Token: 0x040009B5 RID: 2485
        public float forceRaidEnemyBeforeDaysPassed;

    }
}
