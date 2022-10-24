using System;
using System.Collections;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using TwitchToolkit.Storytellers.StorytellerPackWindows;
using TwitchToolkit.Utilities;
using TwitchToolkit.Windows;
using UnityEngine;
using Verse;

namespace TwitchToolkit;

[StaticConstructorOnStartup]
internal static class HarmonyPatches
{
	public static TwitchToolkit _mod;

	private static readonly Type patchType;

	static HarmonyPatches()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing erences)
		//IL_002b: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0063: Expected O, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00d9: Expected O, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0109: Expected O, but got Unknown
		//IL_013f: Unknown result type (might be due to invalid IL or missing erences)
		//IL_014b: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing erences)
		//IL_017b: Expected O, but got Unknown
		_mod = LoadedModManager.GetMod<TwitchToolkit>();
		patchType = typeof(HarmonyPatches);
		SaveHelper.LoadListOfViewers();
		Harmony harmony = new Harmony("com.github.harmony.rimworld.mod.twitchtoolkit");
		harmony.Patch((MethodBase)AccessTools.Method(typeof(GameDataSaveLoader), "SaveGame", (Type[])null, (Type[])null), (HarmonyMethod)null, new HarmonyMethod(typeof(HarmonyPatches).GetMethod("SaveGame_Postfix")), (HarmonyMethod)null, (HarmonyMethod)null);
		harmony.Patch((MethodBase)AccessTools.Method(typeof(LetterMaker), "MakeLetter", new Type[5]
		{
			typeof(TaggedString),
			typeof(TaggedString),
			typeof(LetterDef),
			typeof(Faction),
			typeof(Quest)
		}, (Type[])null), new HarmonyMethod(patchType, "AddLastPlayerMessagePix", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null);
		harmony.Patch((MethodBase)AccessTools.Method(typeof(StorytellerUI), "DrawStorytellerSelectionInterface", (Type[])null, (Type[])null), (HarmonyMethod)null, new HarmonyMethod(patchType, "DrawCustomStorytellerInterface", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null);
		harmony.Patch((MethodBase)AccessTools.Method(typeof(GameDataSaveLoader), "LoadGame", new Type[1] { typeof(string) }, (Type[])null), (HarmonyMethod)null, new HarmonyMethod(patchType, "NewTwitchConnection", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null);
		harmony.Patch((MethodBase)AccessTools.Method(typeof(MainMenuDrawer), "Init", (Type[])null, (Type[])null), (HarmonyMethod)null, new HarmonyMethod(patchType, "ToolkitUtilsNotify", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null);
	}

	public static void SaveGame_Postfix()
	{
		TwitchToolkit mod = LoadedModManager.GetMod<TwitchToolkit>();
		SaveHelper.SaveAllModData();
	}

	public static void AddLastPlayerMessagePix(TaggedString label,  TaggedString text, LetterDef def)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing erences)
		//IL_003b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0041: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0046: Unknown result type (might be due to invalid IL or missing erences)
		if (Helper.playerMessages.Count > 0)
		{
			string msg = Helper.playerMessages[0];
			if ((TaggedString)(text) != "")
			{
				text += msg;
			}
			Helper.playerMessages.RemoveAt(0);
		}
	}

	public static void DrawCustomStorytellerInterface(Rect rect,  StorytellerDef chosenStoryteller,  DifficultyDef difficulty, Listing_Standard infoListing)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing erences)
		Rect storytellerPacksButton = new Rect(140f, ((Rect)(rect)).height - 10f, 190f, 38f);
		if (Widgets.ButtonText(storytellerPacksButton, "Storyteller Packs", true, true, true))
		{
			Window_StorytellerPacks window = new Window_StorytellerPacks();
			Find.WindowStack.TryRemove(((object)window).GetType(), true);
			Find.WindowStack.Add((Window)(object)window);
		}
	}

	private static void NewTwitchConnection()
	{
	}

	private static void ToolkitUtilsNotify()
	{
		if (!ToolkitSettings.NotifiedAboutUtils && !ToolkitUtilsChecker.ToolkitUtilsActive)
		{
			ToolkitSettings.NotifiedAboutUtils = true;
			Window_ToolkitUtils window = new Window_ToolkitUtils();
			Find.WindowStack.TryRemove(((object)window).GetType(), true);
			Find.WindowStack.Add((Window)(object)window);
		}
	}
}
