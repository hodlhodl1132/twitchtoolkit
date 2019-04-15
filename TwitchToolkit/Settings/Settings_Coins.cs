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
            optionsListing.AddLabeledNumericalTextField("TwitchToolkitStartingBalance".Translate(), ref ToolkitSettings.StartingBalance, 0.8f);
            optionsListing.SliderLabeled("TwitchToolkitCoinInterval".Translate(), ref ToolkitSettings.CoinInterval, Math.Round((float)ToolkitSettings.CoinInterval).ToString(), 1f, 15f);
            optionsListing.SliderLabeled("TwitchToolkitCoinAmount".Translate(), ref ToolkitSettings.CoinAmount, Math.Round((float)ToolkitSettings.CoinAmount).ToString(), 0f, 100f);
            optionsListing.AddLabeledNumericalTextField("TwitchToolkitMinimumPurchasePrice".Translate(), ref ToolkitSettings.MinimumPurchasePrice, 0.8f);

            optionsListing.CheckboxLabeled("TwitchToolkitUnlimitedCoins".Translate(), ref ToolkitSettings.UnlimitedCoins);
            optionsListing.CheckboxLabeled("TwitchToolkitGiftingCoins".Translate(), ref ToolkitSettings.GiftingCoins);
        }
    }
}
