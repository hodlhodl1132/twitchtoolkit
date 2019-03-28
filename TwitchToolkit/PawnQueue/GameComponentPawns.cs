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

        public bool HasUserBeenNamed(string username)
        {
            if (pawnHistory.ContainsKey(username))
                return true;

            return false;
        }

        public bool HasUserBeenBanned(string username)
        {
            if (namesBanned.Contains(username))
                return true;
            return false;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref pawnHistory, "pawnHistory", LookMode.Value, LookMode.Reference, ref pawnNames, ref listPawns);
            Scribe_Collections.Look(ref namesBanned, "namesBanned", LookMode.Value);
        }

        public Dictionary<string, Pawn> pawnHistory = new Dictionary<string, Pawn>();
        public List<string> namesBanned = new List<string>();
        public List<Pawn> listPawns = new List<Pawn>();
        public List<string> pawnNames = new List<string>();
    }
}
