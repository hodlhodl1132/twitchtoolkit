using System.Collections.Generic;
using RimWorld;

namespace TwitchToolkit;

public class VoteEvent
{
	public List<IncidentDef> options;

	public StorytellerComp_CustomCategoryMTB storytellerComp_CustomCategoryMTB;

	public StorytellerComp_CustomRandomStoryTeller storytellerComp_CustomStoryTeller;

	public StorytellerComp_CustomOnOffCycle storytellerComp_CustomOnOffCycle;

	public IncidentParms parms;

	public VoteEvent(List<IncidentDef> options, StorytellerComp_CustomRandomStoryTeller storytellerComp_CustomRandomStoryTeller, IncidentParms parms)
	{
		this.options = options;
		storytellerComp_CustomStoryTeller = storytellerComp_CustomRandomStoryTeller;
		this.parms = parms;
		Helper.Log("VoteEvent Created, Random");
	}

	public VoteEvent(List<IncidentDef> options, StorytellerComp_CustomCategoryMTB storytellerComp_CustomCategoryMTB, IncidentParms parms)
	{
		this.options = options;
		this.storytellerComp_CustomCategoryMTB = storytellerComp_CustomCategoryMTB;
		this.parms = parms;
		Helper.Log("VoteEvent Created, MTB");
	}

	public VoteEvent(List<IncidentDef> options, StorytellerComp_CustomOnOffCycle storytellerComp_CustomOnOffCycle, IncidentParms parms)
	{
		this.options = options;
		this.storytellerComp_CustomOnOffCycle = storytellerComp_CustomOnOffCycle;
		this.parms = parms;
		Helper.Log("VoteEvent Created, OFC");
	}
}
