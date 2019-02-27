using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;

namespace TwitchToolkit
{
    public class VoteEvent
    {
        public IEnumerable<IncidentDef> options;
        public StorytellerComp_CustomCategoryMTB storytellerComp_CustomCategoryMTB;
        public StorytellerComp_CustomRandomStoryTeller storytellerComp_CustomStoryTeller;
        public StorytellerComp_CustomOnOffCycle storytellerComp_CustomOnOffCycle;
        public IncidentParms parms;


        public VoteEvent(IEnumerable<IncidentDef> options, StorytellerComp_CustomRandomStoryTeller storytellerComp_CustomRandomStoryTeller, IncidentParms parms)
        {
        this.options = options;
        this.storytellerComp_CustomStoryTeller = storytellerComp_CustomRandomStoryTeller;
        this.parms = parms;
        Helper.Log("VoteEvent Created, Random");
        }

        public VoteEvent(IEnumerable<IncidentDef> options, StorytellerComp_CustomCategoryMTB storytellerComp_CustomCategoryMTB, IncidentParms parms)
        {
        this.options = options;
        this.storytellerComp_CustomCategoryMTB = storytellerComp_CustomCategoryMTB;
        this.parms = parms;
        Helper.Log("VoteEvent Created, MTB");
        }

        public VoteEvent(IEnumerable<IncidentDef> options, StorytellerComp_CustomOnOffCycle storytellerComp_CustomOnOffCycle, IncidentParms parms)
        {
        this.options = options;
        this.storytellerComp_CustomOnOffCycle = storytellerComp_CustomOnOffCycle;
        this.parms = parms;
        Helper.Log("VoteEvent Created, OFC");
        }
    }
}
