using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.IRC;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.Incidents
{
    public class StoreIncident : Def
    {
        public string abbreviation;

        public int cost;

        public int eventCap;

        public Type incidentHelper = typeof(IncidentHelper);

        public KarmaType karmaType;
    }

    public class StoreIncidentSimple : StoreIncident
    {
    }

    public class StoreIncidentVariables : StoreIncident
    {
        public int minPointsToFire = 0;

        public int maxWager = 0;

        public int variables = 0;

        public string syntax = null;

        public new Type incidentHelper = typeof(IncidentHelperVariables);
    }

    public static class StoreIncidentMaker
    {
        public static IncidentHelper MakeIncident(StoreIncidentSimple def)
        {
            IncidentHelper helper = (IncidentHelper)Activator.CreateInstance(def.incidentHelper);
            helper.storeIncident = def;
            return helper;
        }

        public static IncidentHelperVariables MakeIncidentVariables(StoreIncidentVariables def)
        {
            IncidentHelperVariables helper = (IncidentHelperVariables)Activator.CreateInstance(def.incidentHelper);
            helper.storeIncident = def;
            return helper;
        }      
    }

    [DefOf]
    public class StoreIncidentDefOf
    {
        public static StoreIncidentVariables Item;
    }
}
