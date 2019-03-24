using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit
{
    public class StorytellerComp_CustomCategoryMTB : StorytellerComp
    {

        protected StorytellerCompProperties_CustomCategoryMTB Props
        {
            get
            {
                return (StorytellerCompProperties_CustomCategoryMTB)this.props;
            }
        }

        public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
        {
            if (VoteHandler.voteActive)
                yield break;
            float mtbNow = this.Props.mtbDays;
            IEnumerable<IncidentDef> options;
            List<IncidentDef> pickedoptions = new List<IncidentDef>();
            if (this.Props.mtbDaysFactorByDaysPassedCurve != null)
            {
                mtbNow *= this.Props.mtbDaysFactorByDaysPassedCurve.Evaluate(GenDate.DaysPassedFloat);
            }
            if (Rand.MTBEventOccurs(mtbNow, 60000f, 1000f))
            {

                IncidentDef selectedDef;
                options = base.UsableIncidentsInCategory(this.Props.category, target);
                Helper.Log("Trying to create events");
                if (options.TryRandomElementByWeight(new Func<IncidentDef, float>(base.IncidentChanceFinal), out selectedDef))
                {
                    if (options.Count() > ToolkitSettings.VoteOptions)
                    {
                        options = options.Where(k => k != selectedDef);
                        pickedoptions.Add(selectedDef);
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
                        VoteHandler.QueueVote(new VoteIncidentDef(incidents, this, this.GenerateParms(selectedDef.category, target)));
                        Helper.Log("Events created");
                        yield break;
                    }
                    else if (options.Count() == 1)
                    {
                        yield return new FiringIncident(selectedDef, this, this.GenerateParms(selectedDef.category, target));
                    }

                    yield return new FiringIncident(selectedDef, this, this.GenerateParms(selectedDef.category, target));
                }
                yield break;
            }
        }

        public override string ToString()
        {
            return base.ToString() + " " + this.Props.category;
        }


    }
}
