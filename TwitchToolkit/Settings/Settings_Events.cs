using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Store;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings
{
    partial class Settings_Events
    {
        public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
        {
            // unlock price spreadsheet button
            Settings_Store.pricesMayHaveChanged = true;

            // top bar
            Widgets.Label(rect, "TwitchToolkitSearch".Translate() + ":");
            Rect searchBar = new Rect(60f, 0, 250f, 24f);
            searchQuery = Widgets.TextField(searchBar, searchQuery);

            Rect smallBtn = new Rect(320f, 0, 40f, 24f);
            if (Widgets.ButtonText(smallBtn, "x0.5")) IncidentItems.MultiplyIncItemPrices(0.5);
            smallBtn.x += 42f;
            if (Widgets.ButtonText(smallBtn, "x2")) IncidentItems.MultiplyIncItemPrices(2);
            smallBtn.x += 42f;
            if (Widgets.ButtonText(smallBtn, "x5")) IncidentItems.MultiplyIncItemPrices(5);
            smallBtn.x += 42f;
            if (Widgets.ButtonText(smallBtn, "x10")) IncidentItems.MultiplyIncItemPrices(10);
            smallBtn.x += 42f;
            smallBtn.width = 120f;
            
            if ( !resetWarning && Widgets.ButtonText(smallBtn, "TwitchToolkitResetToDefault".Translate()) ) resetWarning = true;
            if (resetWarning && Widgets.ButtonText(smallBtn, "TwitchToolkitAreYouSure".Translate()))
            {
                StoreInventory.ResetIncItemData();
                resetWarning = false;
            }

            Rect eventLabel = new Rect(0, 40f, 250f, 24f);
            Rect priceRect = new Rect(440f, 62f, 200f, 24f);
            Rect disableRect = new Rect(642f, 62f, 60f, 24f);

            Rect karmaRect = new Rect(0, 66f, 100f, 24f);
            Rect karmaLabel = new Rect(110f, 62f, 60f, 24f);

            Rect maxEventsRect = new Rect(180f, 66f, 200f, 24f);
            Rect maxEventsLabel = new Rect(390f, 62f, 60f, 24f);

            //SortEvents(ref StoreInventory.incItems);

            foreach (IncItem incItem in StoreInventory.incItems)
            {
                if (searchQuery != "" && !incItem.name.ToLower().Contains(searchQuery.ToLower())) continue;
                string price = incItem.price.ToString();

                // price adjuster
                Widgets.Label(eventLabel, incItem.name + ": " + ((incItem.price > 0) ? price : "Disabled"));
                if (incItem.price > 0) Widgets.IntEntry(priceRect, ref incItem.price, ref price, 50);
                if (incItem.price > 0 && Widgets.ButtonText(disableRect, "Disable")) incItem.price = -10;
                if (incItem.price < 0 && Widgets.ButtonText(disableRect, "Reset"))
                {
                    incItem.price = IncidentItems.GenerateDefaultProducts().ToList()[incItem.id].price;
                    if (incItem.price < 0) incItem.price = 50;
                }

                eventLabel.y += eventRowHeight;
                priceRect.y += eventRowHeight;
                disableRect.y += eventRowHeight;

                // karma type adjuster
                incItem.karmatype = Karma.GetKarmaTypeFromInt( (int) Widgets.HorizontalSlider(karmaRect, (float)(int)incItem.karmatype, 0, 3) );
                Widgets.Label(karmaLabel, incItem.karmatype.ToString());

                karmaRect.y += eventRowHeight;
                karmaLabel.y += eventRowHeight;

                // max events adjuster
                incItem.maxEvents = (int) Widgets.HorizontalSlider(maxEventsRect, incItem.maxEvents, 1f, 10f);
                Widgets.Label(maxEventsLabel, "Cap: " + incItem.maxEvents.ToString());

                maxEventsRect.y += eventRowHeight;
                maxEventsLabel.y += eventRowHeight;

                SettingsHelper.DrawTexturedLineHorizontal(0, eventLabel.y - 6, rect.width - 20f, BaseContent.GreyTex);
            }
            
        }

        static int eventRowHeight = 60;
        static string searchQuery = "";
        static bool resetWarning = false;
    }
}
