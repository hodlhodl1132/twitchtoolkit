using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TwitchToolkit.Incidents;
using TwitchToolkit.Utilities;
using TwitchToolkitDev;
using Verse;

namespace TwitchToolkit
{
    [StaticConstructorOnStartup]
    static class HarmonyPatches
    {
        private static readonly Type patchType = typeof(HarmonyPatches);

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
            harmony.Patch(original: AccessTools.Method(type: typeof(IncidentWorker), name: "SendStandardLetter", parameters: new Type[] {}), prefix: new HarmonyMethod(type: patchType, name: nameof(ChangeLetterTextPrefix)));
            harmony.Patch(original: AccessTools.Method(type: typeof(IncidentWorker), name: "SendStandardLetter", parameters: new[] { typeof(LookTargets), typeof(Faction), typeof(string[])}), prefix: new HarmonyMethod(type: patchType, name: nameof(ChangeLetterTextArgsPrefix)));
        }

        public static void SaveGame_Postfix()
        {
            var mod = LoadedModManager.GetMod<TwitchToolkit>();
            mod.WriteSettings();
            SaveHelper.SaveAllModData();
        }

        public static void ChangeLetterTextPrefix(IncidentWorker __instance)
        {
            Helper.Log("Patch ran");
            var text = __instance.def.letterText;
            Helper.Log("Ran patch state " + Helper._state);
            if (Helper._state != null)
            {
                text += "\n\n";
                text += Helper._state;
                Helper._state = null;
            }
            __instance.def.letterText = text;
            
        }

        public static void ChangeLetterTextArgsPrefix(IncidentWorker __instance, LookTargets lookTargets, Faction relatedFaction = null, params string[] textArgs)
        {
            Helper.Log("Patch ran");
            var text = __instance.def.letterText;
            Helper.Log("Ran patch state " + Helper._state);
            if (Helper._state != null)
            {
                text += "\n\n";
                text += Helper._state;
                Helper._state = null;
            }
            __instance.def.letterText = text;
            
        }
    }



}
