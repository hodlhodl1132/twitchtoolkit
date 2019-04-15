using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Store;
using TwitchToolkit.Windows;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings
{
    public static class Settings_Items
    {
        public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
        {
            if (optionsListing.ButtonTextLabeled("Item Edit", "Open"))
            {
                Type type = typeof(StoreItemsWindow);
                Find.WindowStack.TryRemove(type);
                
                Window window = new StoreItemsWindow();
                Find.WindowStack.Add(window);
            }
        }

    }
}
