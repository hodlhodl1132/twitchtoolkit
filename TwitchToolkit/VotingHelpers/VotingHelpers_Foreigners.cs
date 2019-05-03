using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Votes;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Foreigners
{
    public class ManInBlack : VotingHelper
    {
        public override bool IsPossible()
        {
            worker = new IncidentWorker_WandererJoin();
            worker.def = IncidentDef.Named("StrangerInBlackJoin");

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

    public class RefugeeChased : VotingHelper
    {
        public override bool IsPossible()
        {
            worker = new IncidentWorker_RefugeeChased();
            worker.def = IncidentDef.Named("RefugeeChased");

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

    public class Traveler : VotingHelper
    {
        public override bool IsPossible()
        {
            worker = new IncidentWorker_TravelerGroup();
            worker.def = IncidentDef.Named("TravelerGroup");

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

    public class Visitor : VotingHelper
    {
        public override bool IsPossible()
        {
            worker = new IncidentWorker_VisitorGroup();
            worker.def = IncidentDef.Named("VisitorGroup");

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

    public class TraderCaravan : VotingHelper
    {
        public override bool IsPossible()
        {
            worker = new IncidentWorker_TraderCaravanArrival();
            worker.def = IncidentDefOf.TraderCaravanArrival;

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

    public class TraderShip : VotingHelper
    {
        public override bool IsPossible()
        {
            worker = new IncidentWorker_OrbitalTraderArrival();
            worker.def = IncidentDefOf.OrbitalTraderArrival;

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
}
