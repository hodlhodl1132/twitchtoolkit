using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Incidents;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Enviroment
{
    public class Eclipse : VotingHelper_GameCondition
    {
        public Eclipse()
        {
            this.incidentDefName = "Eclipse";
        }
    }

    public class Aurora : VotingHelper_GameCondition
    {
        public Aurora()
        {
            this.incidentDefName = "Aurora";
        }
    }

    public class VomitRain : VotingHelper
    {
        public override bool IsPossible()
        {
            if (target is Map)
            {
                map = target as Map;
                return map.IsPlayerHome;
            }

            return false;
        }

        public override void TryExecute()
        {
            IncidentWorker worker = new IncidentWorker_VomitRain
            {
                def = IncidentDef.Named("VomitRain")
            };

            IncidentParms parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, map);

            worker.TryExecute(parms);
        }

        private Map map;
    }

    public class AmbrosiaSprout : VotingHelper
    {
        public override bool IsPossible()
        {
            worker = new IncidentWorker_AmbrosiaSprout();
            worker.def = IncidentDef.Named("AmbrosiaSprout");

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

    public class Meteorite : VotingHelper
    {
        public override bool IsPossible()
        {
            worker = new IncidentWorker_MeteoriteImpact();
            worker.def = IncidentDef.Named("MeteoriteImpact");

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

    public class MeteoriteShower : VotingHelper
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
           
            List<Thing> meteorites = new List<Thing>();
            for (int i = 0; i < 5; i++)
            {
                int nxt = Verse.Rand.Range(0, 10);
                meteorites.Add(Helper.MeteoriteSpawn(Helper.AnyPlayerMap, nxt == 0));
            }

            string text = "A meteorite shower is happening near your colony.";
            Find.LetterStack.ReceiveLetter("Meteors", text, LetterDefOf.NeutralEvent, meteorites);
        }

        private Map map;
    }
}
