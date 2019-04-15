using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Incidents;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Raids
{
    public class Raid : IncidentHelperVariables
    {
        public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
        {
            this.separateChannel = separateChannel;
            this.viewer = viewer;
            string[] command = message.Split(' ');
            if (command.Length < 3)
            {
                Toolkit.client.SendMessage($"@{viewer.username} syntax is {this.storeIncident.syntax}", separateChannel);
                return false;
            }

            if (!VariablesHelpers.PointsWagerIsValid(
                    command[2],
                    viewer,
                    ref pointsWager,
                    ref storeIncident,
                    separateChannel
                ))
            {
                return false;
            }

            target = Current.Game.AnyPlayerHomeMap;
            if (target == null)
            {
                return false;
            }

            parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.RaidBeacon, target);
            parms.points = IncidentHelper_PointsHelper.RollProportionalGamePoints(storeIncident, pointsWager, parms.points);
            parms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
            parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
            
            worker = new Incidents.IncidentWorker_RaidEnemy();
            worker.def = IncidentDefOf.RaidEnemy;
            
            return worker.CanFireNow(parms);
        }

        public override void TryExecute()
        {
            if (worker.TryExecute(parms))
            {
                viewer.TakeViewerCoins(pointsWager);
                viewer.CalculateNewKarma(this.storeIncident.karmaType, pointsWager);
                VariablesHelpers.SendPurchaseMessage($"Starting raid with {pointsWager} points wagered and {(int)parms.points} raid points purchased by {viewer.username}", separateChannel);
                return;
            }
            Toolkit.client.SendMessage($"@{viewer.username} not enough points spent for raid.", separateChannel);
        }

        public int pointsWager = 0;
        public IIncidentTarget target = null;
        public IncidentWorker worker = null;
        public IncidentParms parms = null;
        private bool separateChannel = false;

        public override Viewer viewer { get; set; }
    }

    public class DropRaid : IncidentHelperVariables
    {
        public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
        {
            this.separateChannel = separateChannel;
            this.viewer = viewer;
            string[] command = message.Split(' ');
            if (command.Length < 3)
            {
                Toolkit.client.SendMessage($"@{viewer.username} syntax is {this.storeIncident.syntax}", separateChannel);
                return false;
            }

            if (!VariablesHelpers.PointsWagerIsValid(
                    command[2],
                    viewer,
                    ref pointsWager,
                    ref storeIncident,
                    separateChannel
                ))
            {
                return false;
            }

            Log.Warning("Finding target");

            target = Current.Game.AnyPlayerHomeMap;
            if (target == null)
            {
                return false;
            }

            parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.RaidBeacon, target);
            parms.points = IncidentHelper_PointsHelper.RollProportionalGamePoints(storeIncident, pointsWager, parms.points);
            parms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
            parms.raidArrivalMode = PawnsArrivalModeDefOf.CenterDrop;
            parms.faction = Find.FactionManager.RandomEnemyFaction(false, false, false, TechLevel.Industrial);

            worker = new Incidents.IncidentWorker_RaidEnemy();
            worker.def = IncidentDefOf.RaidEnemy;

            return worker.CanFireNow(parms);
        }

        public override void TryExecute()
        {
            if (worker.TryExecute(parms))
            {
                viewer.TakeViewerCoins(pointsWager);
                viewer.CalculateNewKarma(this.storeIncident.karmaType, pointsWager);
                VariablesHelpers.SendPurchaseMessage($"Starting drop raid with {pointsWager} points wagered and {(int)parms.points} raid points purchased by {viewer.username}", separateChannel);
                return;
            }
            Toolkit.client.SendMessage($"@{viewer.username} not enough points spent for drop raid.", separateChannel);
        }

        public int pointsWager = 0;
        public IIncidentTarget target = null;
        public IncidentWorker worker = null;
        public IncidentParms parms = null;
        private bool separateChannel = false;

        public override Viewer viewer { get; set; }
    }

    public class SapperRaid : IncidentHelperVariables
    {
        public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
        {
            this.separateChannel = separateChannel;
            this.viewer = viewer;
            string[] command = message.Split(' ');
            if (command.Length < 3)
            {
                Toolkit.client.SendMessage($"@{viewer.username} syntax is {this.storeIncident.syntax}", separateChannel);
                return false;
            }

            if (!VariablesHelpers.PointsWagerIsValid(
                    command[2],
                    viewer,
                    ref pointsWager,
                    ref storeIncident,
                    separateChannel
                ))
            {
                return false;
            }

            target = Current.Game.AnyPlayerHomeMap;
            if (target == null)
            {
                return false;
            }

            parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.RaidBeacon, target);
            parms.points = IncidentHelper_PointsHelper.RollProportionalGamePoints(storeIncident, pointsWager, parms.points);
            parms.raidStrategy = DefDatabase<RaidStrategyDef>.GetNamed("ImmediateAttackSappers");
            parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeDrop;
            parms.faction = Find.FactionManager.RandomEnemyFaction(false, false, false, TechLevel.Industrial);

            worker = new Incidents.IncidentWorker_RaidEnemy();
            worker.def = IncidentDefOf.RaidEnemy;

            return worker.CanFireNow(parms);
        }

        public override void TryExecute()
        {
            if (worker.TryExecute(parms))
            {
                viewer.TakeViewerCoins(pointsWager);
                viewer.CalculateNewKarma(this.storeIncident.karmaType, pointsWager);
                VariablesHelpers.SendPurchaseMessage($"Starting sapper raid with {pointsWager} points wagered and {(int)parms.points} raid points purchased by {viewer.username}", separateChannel);
                return;
            }
            Toolkit.client.SendMessage($"@{viewer.username} not enough points spent for sapper raid.", separateChannel);
        }

        public int pointsWager = 0;
        public IIncidentTarget target = null;
        public IncidentWorker worker = null;
        public IncidentParms parms = null;
        private bool separateChannel = false;

        public override Viewer viewer { get; set; }
    }

    public class SiegeRaid : IncidentHelperVariables
    {
        public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
        {
            this.separateChannel = separateChannel;
            this.viewer = viewer;
            string[] command = message.Split(' ');
            if (command.Length < 3)
            {
                Toolkit.client.SendMessage($"@{viewer.username} syntax is {this.storeIncident.syntax}", separateChannel);
                return false;
            }

            if (!VariablesHelpers.PointsWagerIsValid(
                    command[2],
                    viewer,
                    ref pointsWager,
                    ref storeIncident,
                    separateChannel
                ))
            {
                return false;
            }

            target = Current.Game.AnyPlayerHomeMap;
            if (target == null)
            {
                return false;
            }

            parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.RaidBeacon, target);
            parms.points = IncidentHelper_PointsHelper.RollProportionalGamePoints(storeIncident, pointsWager, parms.points);
            parms.raidStrategy = DefDatabase<RaidStrategyDef>.GetNamed("Siege");
            parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeDrop;
            parms.faction = Find.FactionManager.RandomEnemyFaction(false, false, false, TechLevel.Industrial);

            worker = new Incidents.IncidentWorker_RaidEnemy();
            worker.def = IncidentDefOf.RaidEnemy;

            return worker.CanFireNow(parms);
        }

        public override void TryExecute()
        {
            if (worker.TryExecute(parms))
            {
                viewer.TakeViewerCoins(pointsWager);
                viewer.CalculateNewKarma(this.storeIncident.karmaType, pointsWager);
                VariablesHelpers.SendPurchaseMessage($"Starting siege raid with {pointsWager} points wagered and {(int)parms.points} raid points purchased by {viewer.username}", separateChannel);
                return;
            }
            Toolkit.client.SendMessage($"@{viewer.username} not enough points spent for siege raid.", separateChannel);
        }

        public int pointsWager = 0;
        public IIncidentTarget target = null;
        public IncidentWorker worker = null;
        public IncidentParms parms = null;
        private bool separateChannel = false;

        public override Viewer viewer { get; set; }
    }

    public class MechanoidRaid : IncidentHelperVariables
    {
        public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
        {
            this.separateChannel = separateChannel;
            this.viewer = viewer;
            string[] command = message.Split(' ');
            if (command.Length < 3)
            {
                Toolkit.client.SendMessage($"@{viewer.username} syntax is {this.storeIncident.syntax}", separateChannel);
                return false;
            }

            if (!VariablesHelpers.PointsWagerIsValid(
                    command[2],
                    viewer,
                    ref pointsWager,
                    ref storeIncident,
                    separateChannel
                ))
            {
                return false;
            }

            target = Current.Game.AnyPlayerHomeMap;
            if (target == null)
            {
                return false;
            }

            parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.RaidBeacon, target);
            parms.points = IncidentHelper_PointsHelper.RollProportionalGamePoints(storeIncident, pointsWager, parms.points);
            parms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
            parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeDrop;
            parms.faction = Find.FactionManager.OfMechanoids;

            worker = new Incidents.IncidentWorker_RaidEnemy();
            worker.def = IncidentDefOf.RaidEnemy;

            return worker.CanFireNow(parms);
        }

        public override void TryExecute()
        {
            if (worker.TryExecute(parms))
            {
                viewer.TakeViewerCoins(pointsWager);
                viewer.CalculateNewKarma(this.storeIncident.karmaType, pointsWager);
                VariablesHelpers.SendPurchaseMessage($"Starting mechanoid raid with {pointsWager} points wagered and {(int)parms.points} raid points purchased by {viewer.username}", separateChannel);
                return;
            }
            Toolkit.client.SendMessage($"@{viewer.username} not enough points spent for mechanoid raid.", separateChannel);
        }

        public int pointsWager = 0;
        public IIncidentTarget target = null;
        public IncidentWorker worker = null;
        public IncidentParms parms = null;
        private bool separateChannel = false;

        public override Viewer viewer { get; set; }
    }

    public class Infestation : IncidentHelperVariables
    {
        public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
        {
            this.separateChannel = separateChannel;
            this.viewer = viewer;
            string[] command = message.Split(' ');
            if (command.Length < 3)
            {
                Toolkit.client.SendMessage($"@{viewer.username} syntax is {this.storeIncident.syntax}", separateChannel);
                return false;
            }

            if (!VariablesHelpers.PointsWagerIsValid(
                    command[2],
                    viewer,
                    ref pointsWager,
                    ref storeIncident,
                    separateChannel
                ))
            {
                return false;
            }

            target = Current.Game.AnyPlayerHomeMap;
            if (target == null)
            {
                return false;
            }

            parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.RaidBeacon, target);
            parms.points = IncidentHelper_PointsHelper.RollProportionalGamePoints(storeIncident, pointsWager, parms.points);
            parms = StorytellerUtility.DefaultParmsNow(worker.def.category, target);
            parms.faction = Find.FactionManager.OfInsects;

            worker = new IncidentWorker_Infestation();
            worker.def = IncidentDef.Named("Infestation");


            return worker.CanFireNow(parms);
        }

        public override void TryExecute()
        {
            if (worker.TryExecute(parms))
            {
                viewer.TakeViewerCoins(pointsWager);
                viewer.CalculateNewKarma(this.storeIncident.karmaType, pointsWager);
                VariablesHelpers.SendPurchaseMessage($"Starting infestation raid with {pointsWager} points wagered and {(int)parms.points} raid points purchased by {viewer.username}", separateChannel);
                return;
            }
            Toolkit.client.SendMessage($"@{viewer.username} not enough points spent for infestation.", separateChannel);
        }

        public int pointsWager = 0;
        public IIncidentTarget target = null;
        public IncidentWorker worker = null;
        public IncidentParms parms = null;
        private bool separateChannel = false;

        public override Viewer viewer { get; set; }
    }

    public class ManhunterPack : IncidentHelperVariables
    {
        public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
        {
            this.separateChannel = separateChannel;
            this.viewer = viewer;
            string[] command = message.Split(' ');
            if (command.Length < 3)
            {
                VariablesHelpers.ViewerDidWrongSyntax(viewer.username, storeIncident.syntax, separateChannel);
                return false;
            }

            if (!VariablesHelpers.PointsWagerIsValid(
                    command[2],
                    viewer,
                    ref pointsWager,
                    ref storeIncident,
                    separateChannel
                ))
            {
                return false;
            }

            target = Current.Game.AnyPlayerHomeMap;
            if (target == null)
            {
                return false;
            }

            parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.RaidBeacon, target);
            parms.points = IncidentHelper_PointsHelper.RollProportionalGamePoints(storeIncident, pointsWager, parms.points);

            worker = new IncidentWorker_ManhunterPack();
            worker.def = IncidentDefOf.RaidEnemy;
            
            return worker.CanFireNow(parms);
        }

        public override void TryExecute()
        {
            if (worker.TryExecute(parms))
            {
                viewer.TakeViewerCoins(pointsWager);
                viewer.CalculateNewKarma(this.storeIncident.karmaType, pointsWager);
                VariablesHelpers.SendPurchaseMessage($"Starting manhunterpack with {pointsWager} points wagered and {(int)parms.points} raid points purchased by {viewer.username}", separateChannel);
                return;
            }
            Toolkit.client.SendMessage($"@{viewer.username} not enough points spent for manhunter pack.", separateChannel);
        }

        public int pointsWager = 0;
        public IIncidentTarget target = null;
        public IncidentWorker worker = null;
        public IncidentParms parms = null;
        private bool separateChannel = false;

        public override Viewer viewer { get; set; }
    }
}
