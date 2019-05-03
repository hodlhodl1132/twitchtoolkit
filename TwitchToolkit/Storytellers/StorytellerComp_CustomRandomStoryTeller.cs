using RimWorld;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using TwitchToolkit.Votes;
using UnityEngine;
using Verse;
using System.Threading;

namespace TwitchToolkit
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

        readonly TwitchToolkit _twitchstories = LoadedModManager.GetMod<TwitchToolkit>();
        private IIncidentTarget incidentTarget;
        private FiringIncident singleIncident = null;
        private VoteIncidentDef incidentOptions = null;
        private bool makeIncidentOptions = false;
        private Thread thread = null;

        public IncidentParms parms { get; private set; }

        public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
        {
            if (VoteHandler.voteActive)
                yield break;

            if (VoteHelper.TimeForEventVote())
            {
                MakeRandomVoteEvent(target);
                yield break;                        
            }
            if (thread !=null)
            {
                thread = new Thread(new ThreadStart(ThreadProc));

                thread.Start();
            }
            else
            {
                if (singleIncident != null)
                {
                    thread.Join();
                    thread = null;
                    yield return singleIncident;
                }

                if (makeIncidentOptions && incidentOptions != null)
                {
                    thread.Join();
                    thread = null;
                    VoteHandler.QueueVote(incidentOptions);
                }
                Thread.Sleep(0);
                yield break;
            }
            yield break;
        }

        public void ThreadProc()
        {
            var result = MakeIntervalIncidentsThread();
            if (result == typeof(FiringIncident))
            {
                singleIncident = result as FiringIncident;
            }
            else if (incidentOptions != null)
            {
                makeIncidentOptions = true;
            }
        }

        public IEnumerable<FiringIncident> MakeIntervalIncidentsThread()
        {
            if (Rand.MTBEventOccurs(Props.mtbDays, 60000f, 1000f) && !ToolkitSettings.TimedStorytelling)
            {
                bool targetIsRaidBeacon = incidentTarget.IncidentTargetTags().Contains(IncidentTargetTagDefOf.Map_RaidBeacon);
                List<IncidentCategoryDef> triedCategories = new List<IncidentCategoryDef>();
                IncidentDef incDef;
                List<IncidentDef> pickedoptions = new List<IncidentDef>();
                IEnumerable<IncidentDef> options;
                for (; ; )
                {
                    IncidentCategoryDef category = this.ChooseRandomCategory(incidentTarget, triedCategories);
                    Helper.Log($"Trying Category {category}");
                    parms = this.GenerateParms(category, incidentTarget);
                    options = from d in base.UsableIncidentsInCategory(category, incidentTarget)
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
                    Thread.Sleep(0);
                }

                Helper.Log($"Events Possible: {options.Count()}");

                if (options.Count() > 1)
                {
                    options = options.Where(k => k != incDef);
                    pickedoptions.Add(incDef);
                    for (int x = 0; x < ToolkitSettings.VoteOptions - 1 && x < options.Count(); x++)
                    {
                        options.TryRandomElementByWeight(new Func<IncidentDef, float>(base.IncidentChanceFinal), out IncidentDef picked);
                        if (picked != null)
                        {
                            options = options.Where(k => k != picked);
                            pickedoptions.Add(picked);
                            Thread.Sleep(0);
                        }
                    }

                    Dictionary<int, IncidentDef> incidents = new Dictionary<int, IncidentDef>();
                    for (int i = 0; i < pickedoptions.Count(); i++)
                    {
                        incidents.Add(i, pickedoptions.ToList()[i]);
                    }
                    incidentOptions = new VoteIncidentDef(incidents, this, parms);
                    Helper.Log("Events created");
                    yield return null;
                }
                else if (options.Count() == 1)
                {
                    yield return new FiringIncident(incDef, this, parms);
                }

                if (!this.Props.skipThreatBigIfRaidBeacon || !targetIsRaidBeacon || incDef.category != IncidentCategoryDefOf.ThreatBig)
                {

                    // yield return new FiringIncident(incDef, this, parms);
                    // _twitchstories.StartVote(options, this, parms);
                }

            Block_6:;
            Thread.Sleep(0);
            }
        }

        public IncidentCategoryDef ChooseRandomCategory(IIncidentTarget target, List<IncidentCategoryDef> skipCategories)
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

        public void MakeRandomVoteEvent(IIncidentTarget target)
        {
            Helper.Log("Forcing vote event");
            if (!VoteHandler.voteActive)
            {
                if (ToolkitSettings.TimedStorytelling)
                {
                    bool targetIsRaidBeacon = target.IncidentTargetTags().Contains(IncidentTargetTagDefOf.Map_RaidBeacon);
                    List<IncidentCategoryDef> triedCategories = new List<IncidentCategoryDef>();
                    IncidentDef incDef;
                    List<IncidentDef> pickedoptions = new List<IncidentDef>();
                    IEnumerable<IncidentDef> options;

                        IncidentCategoryDef category = this.ChooseRandomCategory(target, triedCategories);
                        Helper.Log($"Trying Category{category}");
                        parms = this.GenerateParms(category, target);
                        options = from d in base.UsableIncidentsInCategory(category, target)
                                    where !d.NeedsParmsPoints || parms.points >= d.minThreatPoints
                                    select d;


                        if (options.TryRandomElementByWeight(new Func<IncidentDef, float>(base.IncidentChanceFinal), out incDef))
                        {

                        }
                        triedCategories.Add(category);

                        if (triedCategories.Count >= this.Props.categoryWeights.Count)
                        {

                        }


                    Helper.Log($"Events Possible: {options.Count()}");

                    if (options.Count() > 1)
                    {
                        options = options.Where(k => k != incDef);
                        pickedoptions.Add(incDef);
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
                        if (incidents.Count > 1)
                        {
                            VoteHandler.QueueVote(new VoteIncidentDef(incidents, this, parms));
                            Helper.Log("Events created");
                        }
                    }
                }
            }
        }

        public IIncidentTarget GetRandomTarget()
        {
            List<IIncidentTarget> targets = Find.Storyteller.AllIncidentTargets;

            if (targets == null)
                throw new Exception("No valid targets");

            if (targets.Count() > 1)
            {
                return targets[Verse.Rand.Range(1, targets.Count()) - 1];
            }

            return targets[0];           
        }
    }
}
