using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.IRC;
using TwitchToolkit.Windows.Installation;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings
{
    static class Settings_Chat
    {
        public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
        {
            if (optionsListing.ButtonTextLabeled("Chat Connection Setup", "Setup"))
            {
                ToolkitSettings.FirstTimeInstallation = true;
                Window_Install window = new Window_Install();
                Find.WindowStack.TryRemove(window.GetType());
                Find.WindowStack.Add(window);
            }

            optionsListing.Gap();

            optionsListing.CheckboxLabeled("Auto Reconnection Tool", ref Reconnecter.autoReconnect, "Always on by Default");

            optionsListing.Gap(24);

            optionsListing.Label("<b>Warning</b>: Setting interval to less than 45 seconds may be unnecessarily reconnecting. Too low of a value can crash your game.");

            optionsListing.Gap();

            optionsListing.SliderLabeled("Check Connection Every X Seconds", ref Reconnecter.reconnectInterval, Reconnecter.reconnectInterval.ToString(), 45, 300);

            optionsListing.GapLine();
            optionsListing.Gap();

            optionsListing.CheckboxLabeled("Should whispers be responded to in separate chat room?", ref ToolkitSettings.WhispersGoToChatRoom);

            optionsListing.CheckboxLabeled(Helper.ReplacePlaceholder("TwitchToolkitWhisperAllowed".Translate(), first: ToolkitSettings.Username), ref ToolkitSettings.WhisperCmdsAllowed);
            optionsListing.CheckboxLabeled(Helper.ReplacePlaceholder("TwitchToolkitWhisperOnly".Translate(), first: ToolkitSettings.Username), ref ToolkitSettings.WhisperCmdsOnly);

        }
    }
}
