using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TwitchToolkit.Utilities;
using TwitchToolkitDev;
using Verse;

namespace TwitchToolkit
{
    [StaticConstructorOnStartup]
    static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            if (Settings.SyncStreamElements)
                StreamElements.ImportPoints();

            Settings.LoadItemsIfNotLoaded();
            Settings.LoadIncItemsIfNotLoaded();

            HarmonyInstance harmony = HarmonyInstance.Create("com.github.harmony.rimworld.mod.twitchtoolkit");

            MethodInfo saveMethod = AccessTools.Method(typeof(Verse.GameDataSaveLoader), "SaveGame");

            HarmonyMethod savePostfix = new HarmonyMethod(typeof(HarmonyPatches).GetMethod("SaveGame_Postfix"));

            harmony.Patch(saveMethod, savePostfix, null);
        }

        public static void SaveGame_Postfix()
        {
            SaveHelper.SaveAllModData();
        }
    }
}
