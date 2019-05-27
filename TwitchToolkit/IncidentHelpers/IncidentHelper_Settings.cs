using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.IncidentHelpers.SettingsWindows;
using TwitchToolkit.Incidents;
using Verse;

namespace TwitchToolkit.IncidentHelpers.IncidentHelper_Settings
{
    public class LevelPawnSettings : IncidentHelperVariablesSettings
    {
        public override void ExposeData()
        {
            Scribe_Values.Look(ref xpMultiplier, "LevelPawnSettings.xpMultiplier", 1);
        }

        public override void EditSettings()
        {
            Window_LevelPawn window = new Window_LevelPawn();
            Find.WindowStack.TryRemove(typeof(Window_LevelPawn));
            Find.WindowStack.Add(window);
        }

        public static float xpMultiplier = 1;
    }

    public class AddTraitSettings : IncidentHelperVariablesSettings
    {
        public override void ExposeData()
        {
            Scribe_Values.Look(ref maxTraits, "AddTraitSettings.maxTraits", 4);
        }

        public override void EditSettings()
        {
            Window_AddTrait window = new Window_AddTrait();
            Find.WindowStack.TryRemove(typeof(Window_AddTrait));
            Find.WindowStack.Add(window);
        }

        public static int maxTraits = 4;
    }

    public class BuyItemSettings : IncidentHelperVariablesSettings
    {
        public override void ExposeData()
        {
            Scribe_Values.Look(ref mustResearchFirst, "BuyItemSettings.mustResearchFirst", true);
        }

        public override void EditSettings()
        {
            Window_BuyItem window = new Window_BuyItem();
            Find.WindowStack.TryRemove(typeof(Window_BuyItem));
            Find.WindowStack.Add(window);
        }

        public static bool mustResearchFirst = true;
    }
}
