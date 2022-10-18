using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit
{
    public class StorytellerComp_CustomOnOffCycle : StorytellerComp
    {
        private IEnumerable<IncidentDef> options;

        protected StorytellerCompProperties_CustomOnOffCycle Props
        {
            get
            {
                return (StorytellerCompProperties_CustomOnOffCycle)this.props;
            }
        }

        public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
        {
            if (VoteHandler.voteActive)
                yield break;
            float acceptFraction = 1f;
            if (this.Props.acceptFractionByDaysPassedCurve != null)
            {
                acceptFraction *= this.Props.acceptFractionByDaysPassedCurve.Evaluate(GenDate.DaysPassedFloat);
            }
            if (this.Props.acceptPercentFactorPerThreatPointsCurve != null)
            {
                acceptFraction *= this.Props.acceptPercentFactorPerThreatPointsCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(target));
            }
            int incCount = IncidentCycleUtility.IncidentCountThisInterval(target, Find.Storyteller.storytellerComps.IndexOf(this), this.Props.minDaysPassed, this.Props.onDays, this.Props.offDays, this.Props.minSpacingDays, this.Props.numIncidentsRange.min, this.Props.numIncidentsRange.max, acceptFraction);
            for (int i = 0; i < incCount; i++)
            {
                Helper.Log("Trying to gen OFC Inc");
                FiringIncident fi = this.GenerateIncident(target);
                if (fi != null)
                {
                    yield return fi;
                }
            }
            yield break;
        }

        private FiringIncident GenerateIncident(IIncidentTarget target)
        {
            Helper.Log("Trying OnOffCycle Incident");
            List<IncidentDef> pickedoptions = new List<IncidentDef>();
            IncidentParms parms = this.GenerateParms(this.Props.IncidentCategory, target);
            IncidentDef def2;
            if ((float)GenDate.DaysPassed < this.Props.forceRaidEnemyBeforeDaysPassed)
            {
                if (!IncidentDefOf.RaidEnemy.Worker.CanFireNow(parms))
                {
                    return null;
                }
                def2 = IncidentDefOf.RaidEnemy;
            }
            else if (this.Props.incident != null)
            {
                if (!this.Props.incident.Worker.CanFireNow(parms))
                {
                    return null;
                }
                def2 = this.Props.incident;
            }
            else
            {
                options = from def in base.UsableIncidentsInCategory(this.Props.IncidentCategory, parms)
                          where parms.points >= def.minThreatPoints
                          select def;
                Helper.Log($"Trying OFC Category: ${this.Props.IncidentCategory}");
                if (options.TryRandomElementByWeight(new Func<IncidentDef, float>(base.IncidentChanceFinal), out def2))
                {
                    if (options.Count() > 1)
                    {
                        options = options.Where(k => k != def2);
                        pickedoptions.Add(def2);
                        for (int x = 0; x < ToolkitSettings.VoteOptions - 1 && x < options.Count(); x++)
                        {
                            options.TryRandomElementByWeight(new Func<IncidentDef, float>(base.IncidentChanceFinal), out IncidentDef picked);
                            if (picked != null)
                            {
                                options = options.Where(k => k != picked);
                                pickedoptions.Add(picked);
                            }
                        }

                        Dictionary<int, IncidentDef> incidents = new Dictionary<int, IncidentDef>();
                        for (int i = 0; i < pickedoptions.Count(); i++)
                        {
                            incidents.Add(i, pickedoptions.ToList()[i]);
                        }
                        VoteHandler.QueueVote(new VoteIncidentDef(incidents, this, parms));
                        Helper.Log("Events created");
                        return null;
                    }
                    else if (options.Count() == 1)
                    {
                        Helper.Log("Firing one incident OFC");
                        return new FiringIncident(def2, this, parms);
                    }
                }

                return null;
            }
            return new FiringIncident(def2, this, parms);
        }

        public override string ToString()
        {
            return base.ToString() + " (" + ((this.Props.incident == null) ? this.Props.IncidentCategory.defName : this.Props.incident.defName) + ")";
        }
    }
}
