using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            optionsListing.AddLabeledNumericalTextField("TwitchToolkitStartingBalance".Translate(), ref ToolkitSettings.CostToJoinQueue, 0.8f);

            optionsListing.Gap();
            optionsListing.GapLine();

            if (!resetCoinsWarning && optionsListing.CenteredButton("TwitchToolkitResetViewersCoinsOne".Translate())) resetCoinsWarning = true;
            if (resetCoinsWarning && optionsListing.CenteredButton("TwitchToolkitResetViewersCoinsTwo".Translate()))
            {
                Viewers.ResetViewersCoins();
                resetCoinsWarning = false;
            }

            if (!resetKarmaWarning && optionsListing.CenteredButton("TwitchToolkitResetViewersKarmaOne".Translate())) resetKarmaWarning = true;
            if (resetKarmaWarning && optionsListing.CenteredButton("TwitchToolkitResetViewersKarmaTwo".Translate()))
            {
                Viewers.ResetViewersKarma();
                resetKarmaWarning = false;
            }

            if (optionsListing.CenteredButton("TwitchToolkitKarmaRound".Translate())) Viewers.AwardViewersCoins();
            if (optionsListing.CenteredButton("TwitchToolkitRefreshViewers".Translate())) Viewers.RefreshViewers();
        }
        
        static bool resetCoinsWarning = false;
        static bool resetKarmaWarning = false;
    }
}
