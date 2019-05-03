using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Hazards
{
    public class Blight : VotingHelper
    {
        public override bool IsPossible()
        {
            worker = new IncidentWorker_CropBlight();
            worker.def = IncidentDef.Named("CropBlight");

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

    public class SolarFlare : VotingHelper_GameCondition
    {
        public SolarFlare()
        {
            incidentDefName = "SolarFlare";
        }
    }

    public class VolcanicWinter : VotingHelper_GameCondition
    {
        public VolcanicWinter()
        {
            incidentDefName = "VolcanicWinter";
        }
    }

    public class ToxicFallout : VotingHelper_GameCondition
    {
        public ToxicFallout()
        {
            incidentDefName = "ToxicFallout";
        }
    }

    public class HeatWave : VotingHelper_GameCondition
    {
        public HeatWave()
        {
            incidentDefName = "HeatWave";
        }
    }

    public class ColdSnap : VotingHelper_GameCondition
    {
        public ColdSnap()
        {
            incidentDefName = "ColdSnap";
        }
    }

    public class Tornado : VotingHelper
    {
        public override bool IsPossible()
        {
            if (target is Map map)
            {
                map = target as Map;
                return true;
            }

            return false;
        }

        public override void TryExecute()
        {
            for (int i = 0; i < 1; i++)
            {
                IntVec3 loc;
                if (!Helper.GetRandomVec3(ThingDefOf.Tornado, map, out loc, 30))
                {
                    return;
                }
                ThingDef tornado = ThingDef.Named("Tornado");
                GenSpawn.Spawn(tornado, loc, map);
            }
        }

        public Map map;
    }

    public class Tornados : Tornado
    {
        public override void TryExecute()
        {
            for (int i = 0; i < 5; i++)
            {
                IntVec3 loc;
                if (!Helper.GetRandomVec3(ThingDefOf.Tornado, map, out loc, 30))
                {
                    return;
                }
                ThingDef tornado = ThingDef.Named("Tornado");
                GenSpawn.Spawn(tornado, loc, map);
            }
        }
    }
}
