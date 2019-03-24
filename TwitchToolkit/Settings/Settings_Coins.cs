using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings
{
    public static class Settings_Coins
    {
        public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
        {
            optionsListing.SliderLabeled("TwitchToolkitStartingBalance".Translate(), ref ToolkitSettings.StartingBalance, Math.Round(ToolkitSettings.StartingBalance).ToString(), 0f, 500f);
            optionsListing.SliderLabeled("TwitchToolkitCoinInterval".Translate(), ref ToolkitSettings.CoinInterval, Math.Round(ToolkitSettings.CoinInterval).ToString(), 1f, 15f);
            optionsListing.SliderLabeled("TwitchToolkitCoinAmount".Translate(), ref ToolkitSettings.CoinAmount, Math.Round(ToolkitSettings.CoinAmount).ToString(), 0f, 100f);
            optionsListing.SliderLabeled("TwitchToolkitMinimumPurchasePrice".Translate(), ref ToolkitSettings.MinimumPurchasePrice, Math.Round(ToolkitSettings.MinimumPurchasePrice).ToString(), 1f, 250f);

            optionsListing.CheckboxLabeled("TwitchToolkitUnlimitedCoins".Translate(), ref ToolkitSettings.UnlimitedCoins);
            optionsListing.CheckboxLabeled("TwitchToolkitGiftingCoins".Translate(), ref ToolkitSettings.GiftingCoins);
        }
    }
}
