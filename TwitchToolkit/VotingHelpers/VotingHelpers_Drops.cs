using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Drops
{
    public class ShipChunkDrop : VotingHelper
    {
        public override bool IsPossible()
        {
            worker = new IncidentWorker_ShipChunkDrop();
            worker.def = IncidentDef.Named("ShipChunkDrop");

            parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, target);

            return worker.CanFireNow(parms);
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentWorker worker;
        private IncidentParms parms;
    }

    public class CargoPodDropped : VotingHelper
    {
        public override bool IsPossible()
        {
            worker = new IncidentWorker_ResourcePodCrash();
            worker.def = IncidentDefOf.ShipChunkDrop;

            parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, target);

            return worker.CanFireNow(parms);
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentWorker worker;
        private IncidentParms parms;
    }

    public class TransportPodDropped : VotingHelper
    {
        public override bool IsPossible()
        {
            worker = new IncidentWorker_TransportPodCrash();
            worker.def = IncidentDefOf.ShipChunkDrop;

            parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, target);

            return worker.CanFireNow(parms);
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentWorker worker;
        private IncidentParms parms;
    }

    public class ShipPartPoison : VotingHelper
    {
        public override bool IsPossible()
        {
            worker = new IncidentWorker_ShipChunkDrop();
            worker.def = IncidentDef.Named("PoisonShipPartCrash");

            parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, target);

            return worker.CanFireNow(parms);
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentWorker worker;
        private IncidentParms parms;
    }

    public class ShipPartPsychic : VotingHelper
    {
        public override bool IsPossible()
        {
            worker = new IncidentWorker_ShipChunkDrop();
            worker.def = IncidentDef.Named("PsychicEmanatorShipPartCrash");

            parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, target);

            return worker.CanFireNow(parms);
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentWorker worker;
        private IncidentParms parms;
    }

    public class Gold : VotingHelper
    {
        public override bool IsPossible()
        {
            if (target is Map)
            {
                map = target as Map;
                return true;
            }

            return false;
        }

        public override void TryExecute()
        {
            ThingDef dropPod = ThingDef.Named("DropPodIncoming");
            Thing gold = new Thing();
            gold.def = ThingDefOf.Gold;
            gold.stackCount = Verse.Rand.Range(5, 10) * Find.ColonistBar.GetColonistsInOrder().Count;

            IntVec3 vec = Helper.Rain(dropPod, gold);

            string text = "A drop pod full of gold has arrived for your colony.";
            Find.LetterStack.ReceiveLetter("Gold", text, LetterDefOf.PositiveEvent, new TargetInfo(vec, map, false));
        }

        private Map map;
    }
}
