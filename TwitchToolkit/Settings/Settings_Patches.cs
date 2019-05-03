using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TwitchToolkit.Windows;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings
{
    class Settings_Patches
    {
        public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
        {
            foreach (ToolkitExtension extension in Settings_ToolkitExtensions.GetExtensions)
            {
                if (optionsListing.ButtonTextLabeled(extension.mod.SettingsCategory(), "Settings"))
                {
                    ConstructorInfo constructor = extension.windowType.GetConstructor(new[] { typeof(Mod) });
                    SettingsWindow window = constructor.Invoke(new object[] { extension.mod }) as SettingsWindow;
                    Type type = typeof(SettingsWindow);
                    Find.WindowStack.TryRemove(type);
                    Find.WindowStack.Add(window);
                }
            }
        }
    }
}
