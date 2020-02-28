using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.Storytellers
{
    public class StorytellerComp_ToolkitOnOffCycle : StorytellerComp
    {
        protected StorytellerCompProperties_ToolkitOnOffCycle Props
        {
            get
            {
                return (StorytellerCompProperties_ToolkitOnOffCycle)this.props;
            }
        }

        public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
        {
            if (VoteHandler.currentVote is Vote_Milasandra)
            {
                yield break;
            }

            if (GenDate.DaysPassed <= Props.minDaysPassed)
            {
                yield break;
            }

            float acceptFraction = 1f;
            if (this.Props.acceptFractionByDaysPassedCurve != null)
            {
                acceptFraction *= this.Props.acceptFractionByDaysPassedCurve.Evaluate(GenDate.DaysPassedFloat);
            }
            if (this.Props.acceptPercentFactorPerThreatPointsCurve != null)
            {
                acceptFraction *= this.Props.acceptPercentFactorPerThreatPointsCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(target));
            }
            int rand = Rand.Range(1, 25);
            //Helper.Log($"{rand} {this.Props.minDaysPassed} {this.Props.onDays} {this.Props.offDays} {this.Props.minSpacingDays}");
            int incCount = IncidentCycleUtility.IncidentCountThisInterval(target, rand, this.Props.minDaysPassed, this.Props.onDays, this.Props.offDays, this.Props.minSpacingDays, this.Props.numIncidentsRange.min, this.Props.numIncidentsRange.max, acceptFraction);

            for (int i = 0; i < incCount; i++)
            {
                FiringIncident fi = this.GenerateIncident(target);
            }
            // will never return an incident because it has to be voted on

            yield break;
        }

        private FiringIncident GenerateIncident(IIncidentTarget target)
        {
            IncidentParms parms = this.GenerateParms(this.Props.IncidentCategory, target);

            if ((float)GenDate.DaysPassed < this.Props.forceRaidEnemyBeforeDaysPassed)
            {
                if (!IncidentDefOf.RaidEnemy.Worker.CanFireNow(parms, false))
                {
                    return null;
                }
                GenerateForcedIncidents(target, parms);
                return null;
            }
            else
            {
                GenerateIncidentsByWeight(target, parms);
            }
            return null;
        }

        private void GenerateForcedIncidents(IIncidentTarget target, IncidentParms parms)
        {
            List<IncidentDef> defs = (from def in base.UsableIncidentsInCategory(this.Props.IncidentCategory, parms)
                                             where parms.points >= def.minThreatPoints
                                             select def).ToList();

            GenerateVotesFromDefs(defs, parms);
        }

        private void GenerateIncidentsByWeight(IIncidentTarget target, IncidentParms parms)
        {
            List<IncidentDef> defs = new List<IncidentDef>();
            
            for (int i = 0; i < 3; i ++)
            {
                if ((from def in base.UsableIncidentsInCategory(this.Props.IncidentCategory, parms)
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

                VoteHandler.QueueVote(new Votes.Vote_Milasandra(incidents, this, parms, "Which Big Threat should occur?"));
            }
            else
            {
                Log.Error("Only generated " + defs.Count + " incidents");
            }
        }

        public override string ToString()
        {
            return base.ToString() + " (" + ((this.Props.incident == null) ? this.Props.IncidentCategory.defName : this.Props.incident.defName) + ")";
        }

        Stopwatch stopwatch;
    }
}
