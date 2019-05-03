using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.Storytellers
{
    public class StorytellerComp_Random : StorytellerComp
    {
        protected StorytellerCompProperties_Random Props
        {
            get
            {
                return (StorytellerCompProperties_Random)this.props;
            }
        }

        public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
        {
            List<IncidentCategoryDef> triedCategories = new List<IncidentCategoryDef>();
            List<IncidentDef> incidentDefs;

            if (!VoteHandler.voteActive && Rand.MTBEventOccurs(ToolkitSettings.HodlBotMTBDays, 60000f, 1000f))
            {
                while (true)
                {
                    incidentDefs = new List<IncidentDef>();
                    IncidentCategoryDef category = ChooseRandomCategory(target, triedCategories);
                    IncidentParms parms = this.GenerateParms(category, target);
                    IEnumerable<IncidentDef> options = from d in base.UsableIncidentsInCategory(category, target)
                                                       where d.Worker.CanFireNow(parms) && (!d.NeedsParmsPoints || parms.points >= d.minThreatPoints)
                                                       select d;

                    for (int i = 0; options.Count() > 0 && incidentDefs.Count < ToolkitSettings.VoteOptions && i < 10; i++)
                    {
                        if (!options.TryRandomElementByWeight(new Func<IncidentDef, float>(base.IncidentChanceFinal), out IncidentDef incDef))
                        {
                            triedCategories.Add(category);
                            break;
                        }
                        else
                        {
                            incidentDefs.Add(incDef);
                            options = options.Where(s => s != incDef);
                        }
                    }

                    if (incidentDefs.Count >= 2)
                    {
                        Dictionary<int, IncidentDef> incidents = new Dictionary<int, IncidentDef>();
                        for (int i = 0; i < incidentDefs.Count(); i++)
                        {
                            incidents.Add(i, incidentDefs.ToList()[i]);
                        }
                        VoteHandler.QueueVote(new VoteIncidentDef(incidents, this, parms));
                        yield break;
                    }
                }
            }
        }

        private IncidentCategoryDef ChooseRandomCategory(IIncidentTarget target, List<IncidentCategoryDef> skipCategories)
        {
            if (!skipCategories.Contains(IncidentCategoryDefOf.ThreatBig))
            {
                int num = Find.TickManager.TicksGame - target.StoryState.LastThreatBigTick;
                if (target.StoryState.LastThreatBigTick >= 0 && (float)num > 60000f * this.Props.maxThreatBigIntervalDays)
                {
                    return IncidentCategoryDefOf.ThreatBig;
                }
            }

            return (from cw in this.Props.categoryWeights
                    where !skipCategories.Contains(cw.category)
                    select cw).RandomElementByWeight((IncidentCategoryEntry cw) => cw.weight).category;
        }
    }
}
