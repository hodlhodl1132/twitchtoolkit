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
            var incident = new IncidentWorker_CallForAid();
            
            FactionManager manager = Find.FactionManager;

            Faction ofPlayer = Faction.OfPlayer;
            
            Faction tryAlly = manager.RandomAlliedFaction(false, false, true, TechLevel.Industrial);

            if (tryAlly == null)
            {
                (from x in manager.AllFactions
                where !x.IsPlayer && (false || !x.def.hidden) && (false || !x.defeated) && (true || x.def.humanlikeFaction) && (x.def.techLevel >= TechLevel.Industrial) && x.PlayerRelationKind == FactionRelationKind.Neutral
                select x).TryRandomElement(out tryAlly);
            }
            
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = Helper.AnyPlayerMap;
			incidentParms.faction = tryAlly;
			incidentParms.raidArrivalModeForQuickMilitaryAid = true;
			incidentParms.points = DiplomacyTuning.RequestedMilitaryAidPointsRange.RandomInRange;
			tryAlly.lastMilitaryAidRequestTick = Find.TickManager.TicksGame;
            incident.TryExecute(incidentParms);
        }
    }
}
