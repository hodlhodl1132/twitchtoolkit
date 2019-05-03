using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Windows;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings
{
    public static class Settings_Viewers
    {
        public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
        {
            optionsListing.CheckboxLabeled("TwitchToolkitViewerColonistQueue".Translate(), ref ToolkitSettings.ViewerNamedColonistQueue);
            optionsListing.CheckboxLabeled("Charge viewers to join queue?", ref ToolkitSettings.ChargeViewersForQueue);
            optionsListing.AddLabeledNumericalTextField("Cost to join queue:", ref ToolkitSettings.CostToJoinQueue, 0.8f);

            optionsListing.Gap();
            optionsListing.GapLine();

            if (optionsListing.CenteredButton("Edit Viewers"))
            {
                Type type = typeof(Window_Viewers);
                Find.WindowStack.TryRemove(type);

                Window window = new Window_Viewers();
                Find.WindowStack.Add(window);
            }
        }
    }
}
