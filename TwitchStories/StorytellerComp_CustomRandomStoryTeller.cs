using RimWorld;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchStories
{
    public class StorytellerComp_CustomRandomStoryTeller : StorytellerComp
    {
        protected StorytellerCompProperties_CustomRandomStoryTeller Props
        {
            get
            {
            return (StorytellerCompProperties_CustomRandomStoryTeller)this.props;
            }
        }

        readonly TwitchStories _twitchstories = LoadedModManager.GetMod<TwitchStories>();

        public IncidentParms parms { get; private set; }

        public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
        {
            if (Rand.MTBEventOccurs(this.Props.mtbDays, 60000f, 1000f))
            {
                bool targetIsRaidBeacon = target.IncidentTargetTags().Contains(IncidentTargetTagDefOf.Map_RaidBeacon);
                List<IncidentCategoryDef> triedCategories = new List<IncidentCategoryDef>();
                IncidentDef incDef;
                IEnumerable<IncidentDef> options;
                for (;;)
                {
                    IncidentCategoryDef category = this.ChooseRandomCategory(target, triedCategories);
                    Helper.Log($"Trying Category{category}");
                    parms = this.GenerateParms(category, target);
                    options = from d in base.UsableIncidentsInCategory(category, target)
                    where !d.NeedsParmsPoints || parms.points >= d.minThreatPoints
                    select d;

                
                    if (options.TryRandomElementByWeight(new Func<IncidentDef, float>(base.IncidentChanceFinal), out incDef))
                    {
                        break;
                    }
                    triedCategories.Add(category);
                    if (triedCategories.Count >= this.Props.categoryWeights.Count)
                    {
                        goto Block_6;
                    }
                 }
                
                Helper.Log($"Events Possible: {options.Count()}");
                
                // _twitchstories.StartVote(options, this, parms);
                if (options.Count() > 1)
                { 
                    VoteEvent evt = new VoteEvent(options, this, parms);
                    Ticker.VoteEvents.Enqueue(evt);
                } else if (options.Count() == 1) { 
                    yield return new FiringIncident(incDef, this, parms);
                }

                if (!this.Props.skipThreatBigIfRaidBeacon || !targetIsRaidBeacon || incDef.category != IncidentCategoryDefOf.ThreatBig)
                {

                    // yield return new FiringIncident(incDef, this, parms);
                    // _twitchstories.StartVote(options, this, parms);
                }
                    Block_6:;
                }
                yield break;
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

        public override IncidentParms GenerateParms(IncidentCategoryDef incCat, IIncidentTarget target)
        {
            IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(incCat, target);
            incidentParms.points *= this.Props.randomPointsFactorRange.RandomInRange;
            return incidentParms;
        }
    }
}
