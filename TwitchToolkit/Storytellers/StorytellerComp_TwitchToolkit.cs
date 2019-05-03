using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitchToolkit.Storytellers
{
    public class StorytellerComp_TwitchToolkit : StorytellerComp
    {
        protected StorytellerCompProperties_TwitchToolkit Props
        {
            get
            {
                return (StorytellerCompProperties_TwitchToolkit)this.props;
            }
        }

        public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
        {
            // get different voting packs

            // check weights of different vote types

            // decide on vote type

            // redirect to vote
                // track vote type

            yield break;
        }
    }
}
