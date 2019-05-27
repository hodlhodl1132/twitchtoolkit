using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TwitchToolkit.Incidents;
using TwitchToolkit.IRC;
using TwitchToolkit.Store;
using TwitchToolkit.Storytellers.StorytellerPackWindows;
using TwitchToolkit.Utilities;
using TwitchToolkitDev;
using UnityEngine;
using Verse;

namespace TwitchToolkit
{
    [StaticConstructorOnStartup]
    static class HarmonyPatches
    {
        public static TwitchToolkit _mod = LoadedModManager.GetMod<TwitchToolkit>();
        private static readonly Type patchType = typeof(HarmonyPatches);

        static HarmonyPatches()
        {
            if (ToolkitSettings.SyncStreamElements)
                StreamElements.ImportPoints();

            SaveHelper.LoadListOfViewers();

            HarmonyInstance harmony = HarmonyInstance.Create("com.github.harmony.rimworld.mod.twitchtoolkit");

            MethodInfo saveMethod = AccessTools.Method(typeof(Verse.GameDataSaveLoader), "SaveGame");

            HarmonyMethod savePostfix = new HarmonyMethod(typeof(HarmonyPatches).GetMethod("SaveGame_Postfix"));

            harmony.Patch(saveMethod, null, savePostfix, null);
            harmony.Patch(original: AccessTools.Method(type: typeof(LetterMaker), name: "MakeLetter", parameters: new[] { typeof(string), typeof(string), typeof(LetterDef) }), prefix: new HarmonyMethod(type: patchType, name: nameof(AddLastPlayerMessagePrefix)));

            harmony.Patch(
                original: AccessTools.Method(
                        type: typeof(StorytellerUI),
                        name: "DrawStorytellerSelectionInterface"
                    ),
                    postfix: new HarmonyMethod(type: patchType, name: nameof(DrawCustomStorytellerInterface)
                )
            );
        }

        public static void SaveGame_Postfix()
        {
            var mod = LoadedModManager.GetMod<TwitchToolkit>();
            SaveHelper.SaveAllModData();
        }

        public static void AddLastPlayerMessagePrefix(string label, ref string text, LetterDef def)
        {
            if (Helper.playerMessages.Count > 0)
            {
                string msg = Helper.playerMessages[0];
                text += msg;
                Helper.playerMessages.RemoveAt(0);
            }
        }

        public static void DrawCustomStorytellerInterface(Rect rect, ref StorytellerDef chosenStoryteller, ref DifficultyDef difficulty, Listing_Standard infoListing)
        {
            if (chosenStoryteller.defName != "StorytellerPacks") return;

            Rect storytellerPacksButton = new Rect(140f, rect.height - 50f, 190f, 38f);

            if (Widgets.ButtonText(storytellerPacksButton, "Storyteller Packs"))
            {
                Window_StorytellerPacks window = new Window_StorytellerPacks();
                Find.WindowStack.TryRemove(window.GetType());
                Find.WindowStack.Add(window);
            }

        }
    }



}
