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

namespace TwitchToolkit {
  public class AddonMenu : IAddonMenu
{
	private List<FloatMenuOption> floatMenuOptions()
	{
		List<FloatMenuOption> toolkitUtilOption = new List<FloatMenuOption>
		{
			new FloatMenuOption("ToolkitUtils", (Action) (() =>
			{
				Window_ToolkitUtils windowToolkitUtils = new Window_ToolkitUtils();
				Find.WindowStack.TryRemove(windowToolkitUtils.GetType());
				Find.WindowStack.Add((Window) windowToolkitUtils);
			}))
		};
		List<FloatMenuOption> options = new List<FloatMenuOption>
		{
			new FloatMenuOption("Settings", (Action) (() =>
        {
          Window_ModSettings windowModSettings = new Window_ModSettings((Mod) LoadedModManager.GetMod<TwitchToolkit>());
          Find.WindowStack.TryRemove(windowModSettings.GetType());
          Find.WindowStack.Add((Window) windowModSettings);
        })),
        new FloatMenuOption("Events", (Action) (() =>
        {
          StoreIncidentsWindow storeIncidentsWindow = new StoreIncidentsWindow();
          Find.WindowStack.TryRemove(storeIncidentsWindow.GetType());
          Find.WindowStack.Add((Window) storeIncidentsWindow);
        })),
        new FloatMenuOption("Items", (Action) (() =>
        {
          StoreItemsWindow storeItemsWindow = new StoreItemsWindow();
          Find.WindowStack.TryRemove(storeItemsWindow.GetType());
          Find.WindowStack.Add((Window) storeItemsWindow);
        })),
        new FloatMenuOption("Commands", (Action) (() =>
        {
          Window_Commands windowCommands = new Window_Commands();
          Find.WindowStack.TryRemove(windowCommands.GetType());
          Find.WindowStack.Add((Window) windowCommands);
        })),
        new FloatMenuOption("Viewers", (Action) (() =>
        {
          Window_Viewers windowViewers = new Window_Viewers();
          Find.WindowStack.TryRemove(windowViewers.GetType());
          Find.WindowStack.Add((Window) windowViewers);
        })),
        new FloatMenuOption("Name Queue", (Action) (() =>
        {
          QueueWindow queueWindow = new QueueWindow();
          Find.WindowStack.TryRemove(queueWindow.GetType());
          Find.WindowStack.Add((Window) queueWindow);
        })),
        new FloatMenuOption("Tracker", (Action) (() =>
        {
          Window_Trackers windowTrackers = new Window_Trackers();
          Find.WindowStack.TryRemove(windowTrackers.GetType());
          Find.WindowStack.Add((Window) windowTrackers);
        })),
        new FloatMenuOption("Toggle Earning Coins", (Action) (() =>
        {
          ToolkitSettings.EarningCoins = !ToolkitSettings.EarningCoins;
          if (ToolkitSettings.EarningCoins)
            Messages.Message("Earning Coins is Enabled", MessageTypeDefOf.NeutralEvent);
          else
            Messages.Message("Earning Coins is Disabled", MessageTypeDefOf.NeutralEvent);
        })),
        new FloatMenuOption("Debug Fix", (Action) (() =>
        {
          Helper.playerMessages = new List<string>();
          Purchase_Handler.viewerNamesDoingVariableCommands = new List<string>();
        }))
      };
      return ToolkitUtilsChecker.ToolkitUtilsActive ? options : toolkitUtilOption.Concat<FloatMenuOption>((IEnumerable<FloatMenuOption>) options).ToList<FloatMenuOption>();
    }

    List<FloatMenuOption> IAddonMenu.MenuOptions() => this.floatMenuOptions();
  }
}
