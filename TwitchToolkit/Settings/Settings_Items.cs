using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Store;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings
{
    public static class Settings_Items
    {
        public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
        {
            Color old = GUI.color;
            Widgets.Label(rect, "TwitchToolkitSearch".Translate() + ":");
            Rect searchBar = new Rect(60f, 0, 250f, 24f);
            searchQuery = Widgets.TextField(searchBar, searchQuery);

            Rect smallBtn = new Rect(320f, 0, 40f, 24f);
            //Widgets.ButtonText(smallBtn, "x0.5");
            //smallBtn.x += 42f;
            //Widgets.ButtonText(smallBtn, "x2");
            //smallBtn.x += 42f;
            //Widgets.ButtonText(smallBtn, "x5");
            //smallBtn.x += 42f;
            //Widgets.ButtonText(smallBtn, "x10");
            //smallBtn.x += 42f;
            smallBtn.width = 120f;
            
            if ( !resetWarning && Widgets.ButtonText(smallBtn, "TwitchToolkitResetToDefault".Translate()) ) resetWarning = true;
            if (resetWarning && Widgets.ButtonText(smallBtn, "TwitchToolkitAreYouSure".Translate()))
            {
                StoreInventory.ResetItemData();
                resetWarning = false;
            }

            Rect itemLabel = new Rect(0, 40f, 250f, 24f);
            Rect priceRect = new Rect(250f, 36f, 200f, 24f);
            Rect disableRect = new Rect(452f, 36f, 60f, 24f);

            foreach(Item item in StoreInventory.items)
            {
                if (searchQuery != "" && !item.abr.Contains(searchQuery)) continue;
                string price = item.price.ToString();

        // price adjustment Convert.ToInt32(item.BaseMarketValue * 10 / 6)
        Widgets.Label(itemLabel, item.abr + ": " + price);
                if (item.price > 0) Widgets.IntEntry(priceRect, ref item.price, ref price, 25);
                if (item.price > 0 && Widgets.ButtonText(disableRect, "Disable")) item.price = -10;
                if (item.price < 0 && Widgets.ButtonText(disableRect, "Reset")) item.price = (int)(ThingDef.Named(item.defname).BaseMarketValue * 10 / 6);

                itemLabel.y += itemRowHeight;
                priceRect.y += itemRowHeight;
                disableRect.y += itemRowHeight;
            }
        }

        static int itemRowHeight = 32;
        static string searchQuery = "";
        static bool resetWarning = false;
    }
}
