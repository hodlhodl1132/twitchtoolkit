using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;

namespace TwitchToolkit.Storytellers
{
    public class StorytellerComp_ToryTalkerHodlBot : StorytellerComp_ToryTalker
    {
        public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
        {
            if (!ToolkitSettings.UseToryTalkerWithHodlBot)
            {
                return null;
            }

            return base.MakeIntervalIncidents(target);
        }
    }
}
