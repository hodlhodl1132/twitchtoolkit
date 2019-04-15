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
    partial class Settings_Events
    {
        public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
        {
            Rect smallBtn = new Rect(320f, 0, 160f, 24f);

            if ( Widgets.ButtonText(smallBtn, "Edit Events"))
            {
                Type type = typeof(StoreIncidentsWindow);
                Find.WindowStack.TryRemove(type);
                
                Window window = new StoreIncidentsWindow();
                Find.WindowStack.Add(window);
            }
        }
    }
}
