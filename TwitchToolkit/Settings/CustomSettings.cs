using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace TwitchToolkit.Settings
{
    public static class CustomSettings
    {
        private static void SettingsDictionariesNotNull()
        {
            if (ToolkitSettings.CustomSettingsStrings == null)
                ToolkitSettings.CustomSettingsStrings = new Dictionary<string, string>();

            if (ToolkitSettings.CustomSettingsFloats == null)
                ToolkitSettings.CustomSettingsFloats = new Dictionary<string, float>();
        }

        public static string LookupStringSetting(string key)
        {
            SettingsDictionariesNotNull();

            if (ToolkitSettings.CustomSettingsStrings.ContainsKey(key))
            {
                return ToolkitSettings.CustomSettingsStrings[key];
            }

            return null;
        }

        public static float LookupFloatSetting(string key)
        {
            SettingsDictionariesNotNull();

            if (ToolkitSettings.CustomSettingsFloats.ContainsKey(key))
            {
                return ToolkitSettings.CustomSettingsFloats[key];
            }

            return 0;
        }

        public static void SetStringSetting(string key, string value)
        {
            SettingsDictionariesNotNull();

            if (ToolkitSettings.CustomSettingsStrings.ContainsKey(key))
            {
                ToolkitSettings.CustomSettingsStrings[key] = value;
            }
            else
            {
                ToolkitSettings.CustomSettingsStrings.Add(key, value);
            }
        }

        public static void SetFloatSetting(string key, float value)
        {
            SettingsDictionariesNotNull();

            if (ToolkitSettings.CustomSettingsFloats.ContainsKey(key))
            {
                ToolkitSettings.CustomSettingsFloats[key] = value;
            }
            else
            {
                ToolkitSettings.CustomSettingsFloats.Add(key, value);
            }
        }
    }
}
