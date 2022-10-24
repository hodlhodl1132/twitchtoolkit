using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using ToolkitCore.Interfaces;
using ToolkitCore.Windows;
using TwitchToolkit.PawnQueue;
using TwitchToolkit.Store;
using TwitchToolkit.Utilities;
using TwitchToolkit.Windows;
using UnityEngine;
using Verse;

namespace TwitchToolkit;

public class AddonMenu : IAddonMenu
{
	private List<FloatMenuOption> floatMenuOptions()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0041: Expected O, but got Unknown
		//IL_0079: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0083: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00bf: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00fb: Expected O, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0137: Expected O, but got Unknown
		//IL_0169: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0173: Expected O, but got Unknown
		//IL_01a5: Unknown result type (might be due to invalid IL or missing erences)
		//IL_01af: Expected O, but got Unknown
		//IL_01e1: Unknown result type (might be due to invalid IL or missing erences)
		//IL_01eb: Expected O, but got Unknown
		//IL_021d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0227: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0263: Expected O, but got Unknown
		List<FloatMenuOption> toolkitUtilOption = new List<FloatMenuOption>
		{
			new FloatMenuOption("ToolkitUtils", (Action)delegate
			{
				Window_ToolkitUtils window_ToolkitUtils = new Window_ToolkitUtils();
				Find.WindowStack.TryRemove(((object)window_ToolkitUtils).GetType(), true);
				Find.WindowStack.Add((Window)(object)window_ToolkitUtils);
			}, (MenuOptionPriority)4, (Action<Rect>)null, (Thing)null, 0f, (Func<Rect, bool>)null, (WorldObject)null, true, 0)
		};
		List<FloatMenuOption> options = new List<FloatMenuOption>
		{
			new FloatMenuOption("Settings", (Action)delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing erences)
				//IL_000c: Expected O, but got Unknown
				Window_ModSettings val = new Window_ModSettings((Mod)(object)LoadedModManager.GetMod<TwitchToolkit>());
				Find.WindowStack.TryRemove(((object)val).GetType(), true);
				Find.WindowStack.Add((Window)(object)val);
			}, (MenuOptionPriority)4, (Action<Rect>)null, (Thing)null, 0f, (Func<Rect, bool>)null, (WorldObject)null, true, 0),
			new FloatMenuOption("Events", (Action)delegate
			{
				StoreIncidentsWindow storeIncidentsWindow = new StoreIncidentsWindow();
				Find.WindowStack.TryRemove(((object)storeIncidentsWindow).GetType(), true);
				Find.WindowStack.Add((Window)(object)storeIncidentsWindow);
			}, (MenuOptionPriority)4, (Action<Rect>)null, (Thing)null, 0f, (Func<Rect, bool>)null, (WorldObject)null, true, 0),
			new FloatMenuOption("Items", (Action)delegate
			{
				StoreItemsWindow storeItemsWindow = new StoreItemsWindow();
				Find.WindowStack.TryRemove(((object)storeItemsWindow).GetType(), true);
				Find.WindowStack.Add((Window)(object)storeItemsWindow);
			}, (MenuOptionPriority)4, (Action<Rect>)null, (Thing)null, 0f, (Func<Rect, bool>)null, (WorldObject)null, true, 0),
			new FloatMenuOption("Commands", (Action)delegate
			{
				Window_Commands window_Commands = new Window_Commands();
				Find.WindowStack.TryRemove(((object)window_Commands).GetType(), true);
				Find.WindowStack.Add((Window)(object)window_Commands);
			}, (MenuOptionPriority)4, (Action<Rect>)null, (Thing)null, 0f, (Func<Rect, bool>)null, (WorldObject)null, true, 0),
			new FloatMenuOption("Viewers", (Action)delegate
			{
				Window_Viewers window_Viewers = new Window_Viewers();
				Find.WindowStack.TryRemove(((object)window_Viewers).GetType(), true);
				Find.WindowStack.Add((Window)(object)window_Viewers);
			}, (MenuOptionPriority)4, (Action<Rect>)null, (Thing)null, 0f, (Func<Rect, bool>)null, (WorldObject)null, true, 0),
			new FloatMenuOption("Name Queue", (Action)delegate
			{
				QueueWindow queueWindow = new QueueWindow();
				Find.WindowStack.TryRemove(((object)queueWindow).GetType(), true);
				Find.WindowStack.Add((Window)(object)queueWindow);
			}, (MenuOptionPriority)4, (Action<Rect>)null, (Thing)null, 0f, (Func<Rect, bool>)null, (WorldObject)null, true, 0),
			new FloatMenuOption("Tracker", (Action)delegate
			{
				Window_Trackers window_Trackers = new Window_Trackers();
				Find.WindowStack.TryRemove(((object)window_Trackers).GetType(), true);
				Find.WindowStack.Add((Window)(object)window_Trackers);
			}, (MenuOptionPriority)4, (Action<Rect>)null, (Thing)null, 0f, (Func<Rect, bool>)null, (WorldObject)null, true, 0),
			new FloatMenuOption("Toggle Earning Coins", (Action)delegate
			{
				ToolkitSettings.EarningCoins = !ToolkitSettings.EarningCoins;
				if (ToolkitSettings.EarningCoins)
				{
					Messages.Message("Earning Coins is Enabled", MessageTypeDefOf.NeutralEvent, true);
				}
				else
				{
					Messages.Message("Earning Coins is Disabled", MessageTypeDefOf.NeutralEvent, true);
				}
			}, (MenuOptionPriority)4, (Action<Rect>)null, (Thing)null, 0f, (Func<Rect, bool>)null, (WorldObject)null, true, 0),
			new FloatMenuOption("Debug Fix", (Action)delegate
			{
				Helper.playerMessages = new List<string>();
				Purchase_Handler.viewerNamesDoingVariableCommands = new List<string>();
			}, (MenuOptionPriority)4, (Action<Rect>)null, (Thing)null, 0f, (Func<Rect, bool>)null, (WorldObject)null, true, 0)
		};
		if (ToolkitUtilsChecker.ToolkitUtilsActive)
		{
			return options;
		}
		return toolkitUtilOption.Concat(options).ToList();
	}

	List<FloatMenuOption> IAddonMenu.MenuOptions()
	{
		return floatMenuOptions();
	}
}
