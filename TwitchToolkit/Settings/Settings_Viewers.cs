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
            optionsListing.CheckboxLabeled("Allow viewers to !buy ticket to join name queue?", ref ToolkitSettings.EnableViewerQueue);
            optionsListing.CheckboxLabeled("TwitchToolkitViewerColonistQueue".Translate(), ref ToolkitSettings.ViewerNamedColonistQueue);
            optionsListing.CheckboxLabeled("Charge viewers to join queue?", ref ToolkitSettings.ChargeViewersForQueue);
            optionsListing.AddLabeledNumericalTextField("Cost to join queue:", ref ToolkitSettings.CostToJoinQueue, 0.8f);

            optionsListing.Gap();
            optionsListing.GapLine();

            optionsListing.Label("Special Viewers");

            optionsListing.Gap();
            optionsListing.Label("<color=#D9BB25>Subscribers</color>");

            string subExtraCoinBuffer = ToolkitSettings.SubscriberExtraCoins.ToString();
            string subCoinMultiplierBuffer = ToolkitSettings.SubscriberCoinMultiplier.ToString();
            string subExtraVotesBuffer = ToolkitSettings.SubscriberExtraVotes.ToString();

            optionsListing.TextFieldNumericLabeled("Extra coins", ref ToolkitSettings.SubscriberExtraCoins, ref subExtraCoinBuffer, 0, 100);
            optionsListing.TextFieldNumericLabeled<float>("Coin bonus multiplier", ref ToolkitSettings.SubscriberCoinMultiplier, ref subCoinMultiplierBuffer, 1f, 5f);
            optionsListing.TextFieldNumericLabeled("Extra votes", ref ToolkitSettings.SubscriberExtraVotes, ref subExtraVotesBuffer, 0, 100);

            optionsListing.Gap();
            optionsListing.Label("<color=#5F49F2>VIPs</color>");

            string vipExtraCoinBuffer = ToolkitSettings.VIPExtraCoins.ToString();
            string vipCoinMultiplierBuffer = ToolkitSettings.VIPCoinMultiplier.ToString();
            string vipExtraVotesBuffer = ToolkitSettings.VIPExtraVotes.ToString();

            optionsListing.TextFieldNumericLabeled("Extra coins", ref ToolkitSettings.VIPExtraCoins, ref vipExtraCoinBuffer, 0, 100);
            optionsListing.TextFieldNumericLabeled<float>("Coin bonus multiplier", ref ToolkitSettings.VIPCoinMultiplier, ref vipCoinMultiplierBuffer, 1f, 5f);
            optionsListing.TextFieldNumericLabeled("Extra votes", ref ToolkitSettings.VIPExtraVotes, ref vipExtraVotesBuffer, 0, 100);

            optionsListing.Gap();
            optionsListing.Label("<color=#238C48>Mods</color>");

            string modExtraCoinBuffer = ToolkitSettings.ModExtraCoins.ToString();
            string modCoinMultiplierBuffer = ToolkitSettings.ModCoinMultiplier.ToString();
            string modExtraVotesBuffer = ToolkitSettings.ModExtraVotes.ToString();

            optionsListing.TextFieldNumericLabeled("Extra coins", ref ToolkitSettings.ModExtraCoins, ref modExtraCoinBuffer, 0, 100);
            optionsListing.TextFieldNumericLabeled<float>("Coin bonus multiplier", ref ToolkitSettings.ModCoinMultiplier, ref modCoinMultiplierBuffer, 1f, 5f);
            optionsListing.TextFieldNumericLabeled("Extra votes", ref ToolkitSettings.ModExtraVotes, ref modExtraVotesBuffer, 0, 100);

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
