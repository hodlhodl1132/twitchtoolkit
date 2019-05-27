using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.Storytellers
{
    public class StorytellerComp_UristBot : StorytellerComp_ToryTalker
    {
        protected new StorytellerCompProperties_UristBot Props
        {
            get
            {
                return (StorytellerCompProperties_UristBot)this.props;
            }
        }

        public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
        {
            if (VoteHandler.voteActive || !Rand.MTBEventOccurs(ToolkitSettings.UristBotMTBDays, 60000f, 1000f))
            {
                yield break;
            }

            IncidentWorker worker = new IncidentWorker_RaidEnemy();
            IncidentParms parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatSmall, target);
            worker.def = IncidentDefOf.RaidEnemy;
            parms.points = StorytellerUtility.DefaultThreatPointsNow(target);
            parms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
            parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;

            if (!worker.CanFireNow(parms))
            {
                yield break;
            }

            int choice = Rand.Range(0, 0);

            switch (choice)
            {
                case 0:
                    parms.faction = Find.FactionManager.RandomEnemyFaction(false, false, false);

                    Dictionary<int, RaidStrategyDef> allStrategies = new Dictionary<int, RaidStrategyDef>();

                    List<RaidStrategyDef> raidStrategyDefs = new List<RaidStrategyDef>(DefDatabase<RaidStrategyDef>.AllDefs);

                    raidStrategyDefs.Shuffle();

                    foreach (RaidStrategyDef strat in raidStrategyDefs)
                    {
                        allStrategies.Add(allStrategies.Count, strat);
                    }

                    StorytellerPack named = DefDatabase<StorytellerPack>.GetNamed("UristBot", true);
                    VoteHandler.QueueVote(new Vote_RaidStrategy(allStrategies, named, worker, this, parms, "How should the next raiders attack?"));

                    break;
            }
            
        }
    }
}
