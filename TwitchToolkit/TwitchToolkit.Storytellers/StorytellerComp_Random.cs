using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.Storytellers;

public class StorytellerComp_Random : StorytellerComp
{
	protected StorytellerCompProperties_Random Props => (StorytellerCompProperties_Random)(object)base.props;

	public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
	{
		List<IncidentCategoryDef> triedCategories = new List<IncidentCategoryDef>();
		if (VoteHandler.voteActive || !Rand.MTBEventOccurs(ToolkitSettings.HodlBotMTBDays, 60000f, 1000f))
		{
			yield break;
		}
		List<IncidentDef> incidentDefs;
		IncidentParms parms;
		IncidentDef incDef = default(IncidentDef);
		do
		{
			incidentDefs = new List<IncidentDef>();
			IncidentCategoryDef category = ChooseRandomCategory(target, triedCategories);
			parms = ((StorytellerComp)this).GenerateParms(category, target);
			IEnumerable<IncidentDef> options = from d in UsableIncidentsInCategory(category, parms)
				where d.Worker.CanFireNow(parms) && (!d.NeedsParmsPoints || parms.points >= d.minThreatPoints)
				select d;
			int j = 0;
			while (options.Count() > 0 && incidentDefs.Count < ToolkitSettings.VoteOptions && j < 10)
			{
				if (!GenCollection.TryRandomElementByWeight<IncidentDef>(options, (Func<IncidentDef, float>)base.IncidentChanceFinal, out incDef))
				{
					triedCategories.Add(category);
					break;
				}
				incidentDefs.Add(incDef);
				options = options.Where((IncidentDef s) => s != incDef);
				j++;
			}
		}
		while (incidentDefs.Count < 2);
		Dictionary<int, IncidentDef> incidents = new Dictionary<int, IncidentDef>();
		for (int i = 0; i < incidentDefs.Count(); i++)
		{
			incidents.Add(i, incidentDefs.ToList()[i]);
		}
		VoteHandler.QueueVote(new VoteIncidentDef(incidents, (StorytellerComp)(object)this, parms));
	}

	private IncidentCategoryDef ChooseRandomCategory(IIncidentTarget target, List<IncidentCategoryDef> skipCategories)
	{
		if (!skipCategories.Contains(IncidentCategoryDefOf.ThreatBig))
		{
			int num = Find.TickManager.TicksGame - target.StoryState.LastThreatBigTick;
			if (target.StoryState.LastThreatBigTick >= 0 && (float)num > 60000f * Props.maxThreatBigIntervalDays)
			{
				return IncidentCategoryDefOf.ThreatBig;
			}
		}
		return GenCollection.RandomElementByWeight<IncidentCategoryEntry>(Props.categoryWeights.Where((IncidentCategoryEntry cw) => !skipCategories.Contains(cw.category)), (Func<IncidentCategoryEntry, float>)((IncidentCategoryEntry cw) => cw.weight)).category;
	}
}
