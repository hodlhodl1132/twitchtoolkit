using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit;

public class StorytellerComp_CustomCategoryMTB : StorytellerComp
{
	protected StorytellerCompProperties_CustomCategoryMTB Props => (StorytellerCompProperties_CustomCategoryMTB)(object)base.props;

	public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
	{
		if (VoteHandler.voteActive)
		{
			yield break;
		}
		float mtbNow = Props.mtbDays;
		List<IncidentDef> pickedoptions = new List<IncidentDef>();
		if (Props.mtbDaysFactorByDaysPassedCurve != null)
		{
			mtbNow *= Props.mtbDaysFactorByDaysPassedCurve.Evaluate(GenDate.DaysPassedFloat);
		}
		if (!Rand.MTBEventOccurs(mtbNow, 60000f, 1000f))
		{
			yield break;
		}
		IncidentParms parms = ((StorytellerComp)this).GenerateParms(Props.category, target);
		IEnumerable<IncidentDef> options2 = UsableIncidentsInCategory(Props.category, parms);
		Helper.Log("Trying to create events");
		IncidentDef selectedDef = default(IncidentDef);
		if (!GenCollection.TryRandomElementByWeight<IncidentDef>(options2, (Func<IncidentDef, float>)base.IncidentChanceFinal, out selectedDef))
		{
			yield break;
		}
		if (options2.Count() > ToolkitSettings.VoteOptions)
		{
			options2 = options2.Where((IncidentDef k) => k != selectedDef);
			pickedoptions.Add(selectedDef);
			IncidentDef picked = default(IncidentDef);
			for (int x = 0; x < ToolkitSettings.VoteOptions - 1 && x < options2.Count(); x++)
			{
				GenCollection.TryRandomElementByWeight<IncidentDef>(options2, (Func<IncidentDef, float>)base.IncidentChanceFinal, out picked);
				if (picked != null)
				{
					options2 = options2.Where((IncidentDef k) => k != picked);
					pickedoptions.Add(picked);
				}
			}
			Dictionary<int, IncidentDef> incidents = new Dictionary<int, IncidentDef>();
			for (int i = 0; i < pickedoptions.Count(); i++)
			{
				incidents.Add(i, pickedoptions.ToList()[i]);
			}
			VoteHandler.QueueVote(new VoteIncidentDef(incidents, (StorytellerComp)(object)this, ((StorytellerComp)this).GenerateParms(selectedDef.category, target)));
			Helper.Log("Events created");
		}
		else
		{
			if (options2.Count() == 1)
			{
				yield return new FiringIncident(selectedDef, (StorytellerComp)(object)this, ((StorytellerComp)this).GenerateParms(selectedDef.category, target));
			}
			yield return new FiringIncident(selectedDef, (StorytellerComp)(object)this, ((StorytellerComp)this).GenerateParms(selectedDef.category, target));
		}
	}

	public override string ToString()
	{
		return ((StorytellerComp)this).ToString() + " " + Props.category;
	}
}
