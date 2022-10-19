using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolkitCore.Interfaces;
using ToolkitCore.Windows;
using TwitchToolkit.PawnQueue;
using TwitchToolkit.Store;
using TwitchToolkit.Windows;
using Verse;
using Window_Commands = TwitchToolkit.Windows.Window_Commands;

namespace TwitchToolkit
{
    public class AddonMenu : IAddonMenu
    {
        List<FloatMenuOption> IAddonMenu.MenuOptions() => new List<FloatMenuOption>
        {
            new FloatMenuOption("Settings", delegate ()
            {
                Window_ModSettings window = new Window_ModSettings(LoadedModManager.GetMod<TwitchToolkit>());
                Find.WindowStack.TryRemove(window.GetType());
                Find.WindowStack.Add(window);
            }),
            new FloatMenuOption("Events", delegate()
            {
                StoreIncidentsWindow window = new StoreIncidentsWindow();
                Find.WindowStack.TryRemove(window.GetType());
                Find.WindowStack.Add(window);
            }),
            new FloatMenuOption("Items", delegate()
            {
                StoreItemsWindow window = new StoreItemsWindow();
                Find.WindowStack.TryRemove(window.GetType());
                Find.WindowStack.Add(window);
            }),
            new FloatMenuOption("Commands", delegate()
            {
                Window_Commands window = new Window_Commands();
                Find.WindowStack.TryRemove(window.GetType());
                Find.WindowStack.Add(window);
            }),
            new FloatMenuOption("Viewers", delegate()
            {
                Window_Viewers window = new Window_Viewers();
                Find.WindowStack.TryRemove(window.GetType());
                Find.WindowStack.Add(window);
            }),
            new FloatMenuOption("Name Queue", delegate()
            {
                QueueWindow window = new QueueWindow();
                Find.WindowStack.TryRemove(window.GetType());
                Find.WindowStack.Add(window);
            }),
            new FloatMenuOption("Tracker", delegate()
            {
                Window_Trackers window = new Window_Trackers();
                Find.WindowStack.TryRemove(window.GetType());
                Find.WindowStack.Add(window);
            }),
            new FloatMenuOption("Toggle Earning Coins", delegate()
            {
                ToolkitSettings.EarningCoins = !ToolkitSettings.EarningCoins;

                if (ToolkitSettings.EarningCoins)
                {
                    Messages.Message("Earning Coins is Enabled", MessageTypeDefOf.NeutralEvent);
                }
                else
                {
                    Messages.Message("Earning Coins is Disabled", MessageTypeDefOf.NeutralEvent);
                }
            }),
            new FloatMenuOption("Debug Fix", delegate()
            {
                Helper.playerMessages = new List<string>();
                Purchase_Handler.viewerNamesDoingVariableCommands = new List<string>();
            })
        };
    }
}
