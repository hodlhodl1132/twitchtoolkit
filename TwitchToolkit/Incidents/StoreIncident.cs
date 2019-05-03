using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.IRC;
using TwitchToolkit.Settings;
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
        public void RegisterCustomSettings()
        {
            bool ScanningFloats = false;
            int index = 0;

            foreach(string key in customSettingKeys)
            {
                if (customSettingStringValues != null && customSettingStringValues.Count > index && CustomSettings.LookupStringSetting(key) == null)
                {
                    Log.Warning("Registering custom setting from " + LabelCap + " " + key + " " + customSettingStringValues[index]);
                    CustomSettings.SetStringSetting(key, customSettingStringValues[index]);
                    index++;
                    continue;
                }
                else
                {
                    index = 0;
                    ScanningFloats = true;
                }

                if (ScanningFloats && customSettingFloatValues != null && customSettingFloatValues.Count > index && CustomSettings.LookupFloatSetting(key) <= 0)
                {
                    Log.Warning("Registering custom setting from " + LabelCap + " " + key + " " + customSettingFloatValues[index]);
                    CustomSettings.SetFloatSetting(key, customSettingFloatValues[index]);
                    index++;
                    continue;
                }
            }
        }

        public int minPointsToFire = 0;

        public int maxWager = 0;

        public int variables = 0;

        public string syntax = null;

        public new Type incidentHelper = typeof(IncidentHelperVariables);

        public List<string> customSettingKeys = new List<string>();

        public List<string> customSettingStringValues = new List<string>();

        public List<float> customSettingFloatValues = new List<float>();
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
