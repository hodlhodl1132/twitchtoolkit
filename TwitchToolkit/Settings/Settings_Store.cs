using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Utilities;
using TwitchToolkit.Windows;
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

            optionsListing.Gap();
            optionsListing.GapLine();

            if (optionsListing.ButtonTextLabeled("Items Edit", "Open"))
            {
                Type type = typeof(StoreItemsWindow);
                Find.WindowStack.TryRemove(type);

                Window window = new StoreItemsWindow();
                Find.WindowStack.Add(window);
            }

            optionsListing.Gap();
            optionsListing.GapLine();

            if (optionsListing.ButtonTextLabeled("Events Edit", "Open"))
            {
                Type type = typeof(StoreIncidentsWindow);
                Find.WindowStack.TryRemove(type);

                Window window = new StoreIncidentsWindow();
                Find.WindowStack.Add(window);
            }

            optionsListing.Gap();
            optionsListing.GapLine();

            optionsListing.CheckboxLabeled("TwitchToolkitPurchaseConfirmations".Translate(), ref ToolkitSettings.PurchaseConfirmations);
            optionsListing.CheckboxLabeled("TwitchToolkitRepeatViewerNames".Translate(), ref ToolkitSettings.RepeatViewerNames);
            optionsListing.CheckboxLabeled("TwitchToolkitMinifiableBuildings".Translate(), ref ToolkitSettings.MinifiableBuildings);
        }
    }
}
