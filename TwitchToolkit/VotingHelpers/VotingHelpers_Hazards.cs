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

    public class Tornado : IncidentHelpers.Hazards.Tornado
    {

    }

    public class Tornados : IncidentHelpers.Hazards.Tornados
    {

    }
}
