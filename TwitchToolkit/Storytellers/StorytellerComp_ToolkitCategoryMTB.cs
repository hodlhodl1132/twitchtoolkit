using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.Storytellers
{
    public class StorytellerComp_ToolkitCategoryMTB : StorytellerComp
    {
        protected StorytellerCompProperties_ToolkitCategoryMTB Props
        {
            get
            {
                return (StorytellerCompProperties_ToolkitCategoryMTB)this.props;
            }
        }

        public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
        {
            if (VoteHandler.currentVote is Vote_Mercurius)
            {
                yield break;
            }

            if (GenDate.DaysPassed <= Props.minDaysPassed)
            {
                yield break;
            }

            float mtbNow = Props.mtbDays;
            if (Props.mtbDaysFactorByDaysPassedCurve != null)
            {
                mtbNow *= Props.mtbDaysFactorByDaysPassedCurve.Evaluate(GenDate.DaysPassedFloat);
            }

            if (Rand.MTBEventOccurs(mtbNow, 60000f, 1000f))
            {
                GenerateIncidentsByWeight(target);
            }

            yield break;
        }

        private void GenerateIncidentsByWeight(IIncidentTarget target)
        {
            List<IncidentDef> defs = new List<IncidentDef>();

            IncidentParms parms = StorytellerUtility.DefaultParmsNow(this.Props.category, target);

            for (int i = 0; i < 3; i++)
            {
                if ((from def in base.UsableIncidentsInCategory(this.Props.category, parms)
                     where parms.points >= def.minThreatPoints && !defs.Contains(def)
                     select def).TryChooseRandomElementByWeight(new Func<IncidentDef, float>(base.IncidentChanceFinal), out IncidentDef def2))
                {
                    defs.Add(def2);
                }
            }

            GenerateVotesFromDefs(defs, parms);
        }

        private void GenerateVotesFromDefs(List<IncidentDef> defs, IncidentParms parms)
        {
            if (defs != null && defs.Count() > 2)
            {
                defs.Shuffle();
                Dictionary<int, IncidentDef> incidents = new Dictionary<int, IncidentDef>();

                for (int i = 0; i < defs.Count; i++)
                {
                    incidents.Add(i, defs[i]);
                }

                VoteHandler.QueueVote(new Vote_Mercurius(incidents, this, parms, "Which Event should happen next?"));
            }
            else
            {
                Log.Error("Only generated " + defs.Count + " incidents");
            }
        }
    }
}
