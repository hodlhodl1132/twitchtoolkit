using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings
{
    static class Settings_Chat
    {
        public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
        {
            optionsListing.CheckboxLabeled(Helper.ReplacePlaceholder("TwitchToolkitWhisperAllowed".Translate(), first: ToolkitSettings.Username), ref ToolkitSettings.WhisperCmdsAllowed);
            optionsListing.CheckboxLabeled(Helper.ReplacePlaceholder("TwitchToolkitWhisperOnly".Translate(), first: ToolkitSettings.Username), ref ToolkitSettings.WhisperCmdsOnly);

        }
    }
}
