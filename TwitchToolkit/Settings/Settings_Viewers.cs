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
            optionsListing.ColumnWidth = rect.width / 3;
            optionsListing.NewColumn();

            if (!resetCoinsWarning && optionsListing.ButtonText("TwitchToolkitResetViewersCoinsOne".Translate())) resetCoinsWarning = true;
            if (resetCoinsWarning && optionsListing.ButtonText("TwitchToolkitResetViewersCoinsTwo".Translate()))
            {
                Viewers.ResetViewersCoins();
                resetCoinsWarning = false;
            }

            if (!resetKarmaWarning && optionsListing.ButtonText("TwitchToolkitResetViewersKarmaOne".Translate())) resetKarmaWarning = true;
            if (resetKarmaWarning && optionsListing.ButtonText("TwitchToolkitResetViewersKarmaTwo".Translate()))
            {
                Viewers.ResetViewersKarma();
                resetKarmaWarning = false;
            }

            if (optionsListing.ButtonText("TwitchToolkitKarmaRound".Translate())) Viewers.AwardViewersCoins();
            if (optionsListing.ButtonText("TwitchToolkitRefreshViewers".Translate())) Viewers.RefreshViewers();
        }
        static bool resetCoinsWarning = false;
        static bool resetKarmaWarning = false;
    }
}
