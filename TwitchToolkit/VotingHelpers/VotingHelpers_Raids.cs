using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Raids
{
    public static class RaidHelpers
    {
        public static bool RaidPossible(float points, PawnsArrivalModeDef arrival, RaidStrategyDef strategy = null, Faction faction = null)
        {
            var raidEnemy = new IncidentWorker_RaidEnemy();
            raidEnemy.def = IncidentDefOf.RaidEnemy;
            return raidEnemy.CanFireNow(new IncidentParms
            {
                target = Helper.AnyPlayerMap,
                points = Math.Max(StorytellerUtility.DefaultThreatPointsNow(Helper.AnyPlayerMap), points),
                raidArrivalMode = arrival,
                raidStrategy = strategy == null ? RaidStrategyDefOf.ImmediateAttack : strategy,
                faction = faction
            });
        }

        public static void Raid(float points, PawnsArrivalModeDef arrival, RaidStrategyDef strategy = null, Faction faction = null)
        {
            if (points < 0) return;
            var raidEnemy = new IncidentWorker_RaidEnemy();
            raidEnemy.def = IncidentDefOf.RaidEnemy;
            raidEnemy.TryExecute(new IncidentParms
            {
                target = Helper.AnyPlayerMap,
                points = Math.Max(StorytellerUtility.DefaultThreatPointsNow(Helper.AnyPlayerMap), points),
                raidArrivalMode = arrival,
                raidStrategy = strategy == null ? RaidStrategyDefOf.ImmediateAttack : strategy,
                faction = faction
            });
        }
    }

    public class SmallRaid : VotingHelper
    {
        public override bool IsPossible()
        {
            points = StorytellerUtility.DefaultThreatPointsNow(target) / 3;
            return RaidHelpers.RaidPossible(points, arrival);
        }

        public override void TryExecute()
        {
            RaidHelpers.Raid(points, arrival);
        }

        private float points;
        private PawnsArrivalModeDef arrival = PawnsArrivalModeDefOf.EdgeWalkIn;
    }

    public class MediumRaid : VotingHelper
    {
        public override bool IsPossible()
        {
            points = StorytellerUtility.DefaultThreatPointsNow(target) / 2;
            return RaidHelpers.RaidPossible(points, arrival);
        }

        public override void TryExecute()
        {
            RaidHelpers.Raid(points, arrival);
        }

        private float points;
        private PawnsArrivalModeDef arrival = PawnsArrivalModeDefOf.EdgeWalkIn;
    }

    public class BigRaid : VotingHelper
    {
        public override bool IsPossible()
        {
            points = StorytellerUtility.DefaultThreatPointsNow(target);
            return RaidHelpers.RaidPossible(points, arrival);
        }

        public override void TryExecute()
        {
            RaidHelpers.Raid(points, arrival);
        }

        private float points;
        private PawnsArrivalModeDef arrival = PawnsArrivalModeDefOf.EdgeWalkIn;
    }

    public class MediumRaidDrop : VotingHelper
    {
        public override bool IsPossible()
        {
            points = StorytellerUtility.DefaultThreatPointsNow(target) / 2;
            return RaidHelpers.RaidPossible(points, arrival);
        }

        public override void TryExecute()
        {
            RaidHelpers.Raid(points, arrival);
        }

        private float points;
        private PawnsArrivalModeDef arrival = PawnsArrivalModeDefOf.CenterDrop;
    }

    public class BigRaidDrop : VotingHelper
    {
        public override bool IsPossible()
        {
            points = StorytellerUtility.DefaultThreatPointsNow(target);
            return RaidHelpers.RaidPossible(points, arrival);
        }

        public override void TryExecute()
        {
            RaidHelpers.Raid(points, arrival);
        }

        private float points;
        private PawnsArrivalModeDef arrival = PawnsArrivalModeDefOf.CenterDrop;
    }

    public class Siege : VotingHelper
    {
        public override bool IsPossible()
        {
            points = StorytellerUtility.DefaultThreatPointsNow(target);
            return RaidHelpers.RaidPossible(points, arrival, strategy);
        }

        public override void TryExecute()
        {
            RaidHelpers.Raid(points, arrival, strategy, FactionUtility.DefaultFactionFrom(FactionDef.Named("OutlanderRough")));
        }

        private float points;
        private PawnsArrivalModeDef arrival = PawnsArrivalModeDefOf.EdgeDrop;
        private RaidStrategyDef strategy = DefDatabase<RaidStrategyDef>.GetNamed("Siege", true);
    }

    public class BigMechanoidRaid : VotingHelper
    {
        public override bool IsPossible()
        {
            points = StorytellerUtility.DefaultThreatPointsNow(target);
            return RaidHelpers.RaidPossible(points, arrival, null, faction);
        }

        public override void TryExecute()
        {
            RaidHelpers.Raid(points, arrival, null, faction);
        }

        private float points;
        private PawnsArrivalModeDef arrival = PawnsArrivalModeDefOf.EdgeDrop;
        private Faction faction = Find.FactionManager.OfMechanoids;
    }

    public class Infestation : VotingHelper
    {
        public override bool IsPossible()
        {
            parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, target);
            points = parms.points;

            return worker.CanFireNow(parms);
        }

        public override void TryExecute()
        {
            worker.def = IncidentDef.Named("Infestation");
            worker.TryExecute(parms);
        }

        private float points;
        private IncidentWorker worker = new IncidentWorker_Infestation();
        private IncidentParms parms;
    }
}
