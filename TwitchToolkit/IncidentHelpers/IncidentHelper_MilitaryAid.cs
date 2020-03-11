using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Incidents;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.MilitaryAid
{
    public class CallForAid : IncidentHelper
    {
        public override bool IsPossible()
        {
            return true;
        }

        public override void TryExecute()
        {
            Map currentMap = Find.CurrentMap;

            IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.AllyAssistance, currentMap);
            incidentParms.forced = true;
            incidentParms.target = currentMap;
            incidentParms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
            incidentParms.raidStrategy = RaidStrategyDefOf.ImmediateAttackFriendly;

            var incident = new IncidentWorker_CallForAid()
            {
                def = IncidentDef.Named("RaidFriendly")
            };

            incident.TryExecute(incidentParms);
        }
    }
}
