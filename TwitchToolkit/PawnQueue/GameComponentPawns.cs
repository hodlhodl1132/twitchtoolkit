using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace TwitchToolkit.PawnQueue
{
    public class GameComponentPawns : GameComponent
    {
        public GameComponentPawns(Game game)
        {

        }

        public override void GameComponentTick()
        {
            if (Find.TickManager.TicksGame % 1000 != 0)
                return;

            List<Pawn> currentColonists = Find.ColonistBar.GetColonistsInOrder();
            foreach (KeyValuePair<string, Pawn> pair in pawnHistory)
            {
                if (!currentColonists.Contains(pair.Value))
                {
                    pawnHistory.Remove(pair.Key);
                }
            }
        }

        public void AssignUserToPawn(string username, Pawn pawn)
        {
            pawnHistory.Add(username, pawn);
            if (viewerNameQueue.Contains(username))
            {
                viewerNameQueue.Remove(username);
            }
        }

        public bool HasUserBeenNamed(string username)
        {
            return pawnHistory.ContainsKey(username);
        }

        public bool HasPawnBeenNamed(Pawn pawn)
        {
            return pawnHistory.ContainsValue(pawn);
        }

        public string UserAssignedToPawn(Pawn pawn)
        {
            if (!HasPawnBeenNamed(pawn)) return null;
            return pawnHistory.FirstOrDefault(s => s.Value == pawn).Key;
        }

        public Pawn PawnAssignedToUser(string username)
        {
            if (pawnHistory.ContainsKey(username))
            {
                return pawnHistory[username];
            }
            return null;
        }

        public bool UserInViewerQueue(string username)
        {
            return viewerNameQueue.Contains(username);
        }

        public void AddViewerToViewerQueue(string username)
        {
            if (!UserInViewerQueue(username))
            {
                viewerNameQueue.Add(username);
            }
        }

        public string GetNextViewerFromQueue()
        {
            if (viewerNameQueue.Count < 1)
            {
                return null;
            }
            return viewerNameQueue[0];
        }

        public string GetRandomViewerFromQueue()
        {
            if (viewerNameQueue.Count < 1)
            {
                return null;
            }
            return viewerNameQueue[Verse.Rand.Range(0, viewerNameQueue.Count - 1)];
        }

        public int ViewersInQueue()
        {
            return viewerNameQueue.Count;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref pawnHistory, "pawnHistory", LookMode.Value, LookMode.Reference, ref pawnNames, ref listPawns);
            Scribe_Collections.Look(ref viewerNameQueue, "viewerNameQueue", LookMode.Value);
        }

        public Dictionary<string, Pawn> pawnHistory = new Dictionary<string, Pawn>();
        public List<string> viewerNameQueue = new List<string>();
        public List<Pawn> listPawns = new List<Pawn>();
        public List<string> pawnNames = new List<string>();
    }
}
