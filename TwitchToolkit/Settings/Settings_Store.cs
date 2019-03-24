using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings
{
    public static class Settings_Store
    {
        public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
        {
            optionsListing.CheckboxLabeled("TwitchToolkitStoreOpen".Translate(), ref ToolkitSettings.StoreOpen);
            optionsListing.CheckboxLabeled("TwitchToolkitEarningCoins".Translate(), ref ToolkitSettings.EarningCoins);
            optionsListing.AddLabeledTextField("TwitchToolkitCustomPricingLink".Translate(), ref ToolkitSettings.CustomPricingSheetLink);
        }
    }
}
