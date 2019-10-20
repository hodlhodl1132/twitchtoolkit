using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Viewers;
using Verse;

namespace TwitchToolkit.PawnQueue
{
    public class WorldPawnTrackerComponent : GameComponent
    {
        public WorldPawnTrackerComponent(Game game)
        {
        }

        public override void GameComponentTick()
        {
            if (Find.TickManager.TicksGame % 1000 != 0) return;

            FindRandomWorldPawnForViewer(Viewers.GetViewer("hodlhodl"));
        }

        public bool IsViewerAssignedAPawn(Viewer viewer, out Pawn pawn)
        {
            pawn = null;

            if (usernamesAssignedToPawns.ContainsKey(viewer.username))
            {
                pawn = usernamesAssignedToPawns[viewer.username];
                return true;
            }

            return false;
        }

        public bool IsPawnAssignedToViewer(Pawn pawn, out Viewer viewer)
        {
            viewer = null;

            if (usernamesAssignedToPawns.ContainsValue(pawn))
            {
                string username = usernamesAssignedToPawns.Where(s => s.Value == pawn).ElementAt(0).Key;
                viewer = Viewers.GetViewer(username);

                return true;
            }

            return false;
        }

        public bool TryAssignPawnToViewer(Pawn pawn, Viewer viewer)
        {
            if (!IsViewerAssignedAPawn(viewer, out Pawn pawnAssigned))
            {
                Log.Warning($"Viewer {viewer.username} not assigned to Pawn {pawn.LabelShortCap} because they are already assigned to Pawn {pawnAssigned.LabelShortCap}");
                return false;
            }

            if (!IsPawnAssignedToViewer(pawn, out Viewer viewerAssigned))
            {
                Log.Warning($"Pawn {pawn.LabelShortCap} cannot be assigned to Viewer {viewer.username} since they are assigned to {viewerAssigned.username}");
                return false;
            }

            AssignPawnToViewer(pawn, viewer);

            return true;
        }

        void AssignPawnToViewer(Pawn pawn, Viewer viewer)
        {
            usernamesAssignedToPawns.Add(viewer.username, pawn);
        }

        public void FindRandomWorldPawnForViewer(Viewer viewer)
        {
            if (IsViewerAssignedAPawn(viewer, out Pawn assignedPawn))
            {
                return;
            }

            IEnumerable<Pawn> playerOptions = new List<Pawn>();

            // look for faction leaders

            playerOptions = playerOptions.Concat(UnnassignedFactionLeaders());

            // look for important people

            playerOptions = playerOptions.Union(UnnassignedImportantPawns());

            // look for slaves

            playerOptions = playerOptions.Union(UnnassignedWorldPrisoners());

            // look for caravaners

            //

            foreach (Pawn pawn in playerOptions)
            {
                //Helper.Log("Pawn option " + pawn.LabelShortCap + ", faction: " + pawn.Faction.ToString() + ", WorldPosition: " + Find.WorldPawns.GetSituation(pawn).ToString());
            }

            this.playerOptions = playerOptions.ToList();

            ForcefullyKeepTrackedPawns();
        }

        List<Pawn> UnnassignedFactionLeaders()
        {
            return Find.WorldPawns.GetPawnsBySituation(WorldPawnSituation.FactionLeader).Where(s => !IsPawnAssignedToViewer(s, out Viewer viewer)).ToList();
        }

        List<Pawn> UnnassignedImportantPawns()
        {
            return Find.WorldPawns.ForcefullyKeptPawns.Where(s => !IsPawnAssignedToViewer(s, out Viewer viewer)).ToList();
        }

        List<Pawn> UnnassignedWorldPrisoners()
        {
            return Find.WorldPawns.GetPawnsBySituation(WorldPawnSituation.ForSaleBySettlement).Where(s => !IsPawnAssignedToViewer(s, out Viewer viewer)).ToList();
        }

        void ForcefullyKeepTrackedPawns()
        {
            List<Pawn> pawnsNotSaved = Find.WorldPawns.ForcefullyKeptPawns.Where(s => !playerOptions.Contains(s)).ToList();

            foreach (Pawn pawn in pawnsNotSaved)
            {
                Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.KeepForever);
            }
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref usernamesAssignedToPawns, "usernamesAssignedToPawns", LookMode.Value, LookMode.Reference, ref usernamesList, ref pawnReferences);
        }

        public Dictionary<string, Pawn> usernamesAssignedToPawns = new Dictionary<string, Pawn>();

        public List<string> usernamesList = new List<string>();
        public List<Pawn> pawnReferences = new List<Pawn>();

        public List<Pawn> playerOptions = new List<Pawn>();
    }
}
