using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Utilities;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings
{
    public static class Settings_Store
    {
        public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
        {
            float old = rect.width;
            rect.width = 200f;
            if (pricesMayHaveChanged && optionsListing.ButtonText("TwitchToolkitUpdateSpreadsheet".Translate()))
            {
                SaveHelper.CreateStorePricesCSV();
                pricesMayHaveChanged = false;
            }
            rect.width = old;

            optionsListing.Gap();

            optionsListing.CheckboxLabeled("TwitchToolkitStoreOpen".Translate(), ref ToolkitSettings.StoreOpen);
            optionsListing.CheckboxLabeled("TwitchToolkitEarningCoins".Translate(), ref ToolkitSettings.EarningCoins);
            optionsListing.AddLabeledTextField("TwitchToolkitCustomPricingLink".Translate(), ref ToolkitSettings.CustomPricingSheetLink);
        }

        public static bool pricesMayHaveChanged = true;
    }
}
