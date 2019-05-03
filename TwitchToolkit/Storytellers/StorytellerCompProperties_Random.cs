using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Storytellers;
using Verse;

namespace TwitchToolkit
{

    public class StorytellerCompProperties_Random : StorytellerCompProperties
    {

        public StorytellerCompProperties_Random() => compClass = typeof(StorytellerComp_Random);

        public float mtbDays;

        public List<IncidentCategoryEntry> categoryWeights = new List<IncidentCategoryEntry>();

        public float maxThreatBigIntervalDays = 99999f;

        public FloatRange randomPointsFactorRange = new FloatRange(0.5f, 1.5f);

        public bool skipThreatBigIfRaidBeacon;
    }
}
