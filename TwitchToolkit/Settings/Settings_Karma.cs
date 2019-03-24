using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings
{
    public static class Settings_Karma
    {
        public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
        {
            optionsListing.SliderLabeled("TwitchToolkitStartingKarma".Translate(), ref ToolkitSettings.StartingKarma, Math.Round((double)ToolkitSettings.StartingKarma).ToString(), 50, 250);
            optionsListing.SliderLabeled("TwitchToolkitKarmaCap".Translate(), ref ToolkitSettings.KarmaCap, Math.Round((double)ToolkitSettings.KarmaCap).ToString(), 150, 600);
            optionsListing.CheckboxLabeled("TwitchToolkitBanViewersWhoAreBad".Translate(), ref ToolkitSettings.BanViewersWhoPurchaseAlwaysBad);
            optionsListing.CheckboxLabeled("TwitchToolkitKarmaReqsForGifting".Translate(), ref ToolkitSettings.KarmaReqsForGifting);
            
            optionsListing.Gap();

            optionsListing.SliderLabeled("TwitchToolkitMinKarmaForGifts".Translate(), ref ToolkitSettings.MinimumKarmaToRecieveGifts, Math.Round((double)ToolkitSettings.MinimumKarmaToRecieveGifts).ToString(), 10, 100);
            optionsListing.SliderLabeled("TwitchToolkitMinKarmaSendGifts".Translate(), ref ToolkitSettings.MinimumKarmaToSendGifts, Math.Round((double)ToolkitSettings.MinimumKarmaToSendGifts).ToString(), 20, 150);

            optionsListing.Gap();
            optionsListing.GapLine();

            optionsListing.Label("TwitchToolkitGoodViewers".Translate());
            optionsListing.SliderLabeled("TwitchToolkitGoodKarma".Translate(), ref ToolkitSettings.TierOneGoodBonus, Math.Round((double)ToolkitSettings.TierOneGoodBonus).ToString(), 1, 100);
            optionsListing.SliderLabeled("TwitchToolkitNeutralKarma".Translate(), ref ToolkitSettings.TierOneNeutralBonus, Math.Round((double)ToolkitSettings.TierOneNeutralBonus).ToString(), 1, 100);
            optionsListing.SliderLabeled("TwitchToolkitBadKarma".Translate(), ref ToolkitSettings.TierOneBadBonus, Math.Round((double)ToolkitSettings.TierOneBadBonus).ToString(), 1, 100);

            optionsListing.Gap();
            optionsListing.GapLine();

            optionsListing.Label("TwitchToolkitNeutralViewers".Translate());
            optionsListing.SliderLabeled("TwitchToolkitGoodKarma".Translate(), ref ToolkitSettings.TierTwoGoodBonus, Math.Round((double)ToolkitSettings.TierTwoGoodBonus).ToString(), 1, 100);
            optionsListing.SliderLabeled("TwitchToolkitNeutralKarma".Translate(), ref ToolkitSettings.TierTwoNeutralBonus, Math.Round((double)ToolkitSettings.TierTwoNeutralBonus).ToString(), 1, 100);
            optionsListing.SliderLabeled("TwitchToolkitBadKarma".Translate(), ref ToolkitSettings.TierTwoBadBonus, Math.Round((double)ToolkitSettings.TierTwoBadBonus).ToString(), 1, 100);

            optionsListing.Gap();
            optionsListing.GapLine();

            optionsListing.Label("TwitchToolkitBadViewers".Translate());
            optionsListing.SliderLabeled("TwitchToolkitGoodKarma".Translate(), ref ToolkitSettings.TierThreeGoodBonus, Math.Round((double)ToolkitSettings.TierThreeGoodBonus).ToString(), 1, 100);
            optionsListing.SliderLabeled("TwitchToolkitNeutralKarma".Translate(), ref ToolkitSettings.TierThreeNeutralBonus, Math.Round((double)ToolkitSettings.TierThreeNeutralBonus).ToString(), 1, 100);
            optionsListing.SliderLabeled("TwitchToolkitBadKarma".Translate(), ref ToolkitSettings.TierThreeBadBonus, Math.Round((double)ToolkitSettings.TierThreeBadBonus).ToString(), 1, 100);

            optionsListing.Gap();
            optionsListing.GapLine();

            optionsListing.Label("TwitchToolkitDoomViewers".Translate());
            optionsListing.SliderLabeled("TwitchToolkitGoodKarma".Translate(), ref ToolkitSettings.TierFourGoodBonus, Math.Round((double)ToolkitSettings.TierFourGoodBonus).ToString(), 1, 100);
            optionsListing.SliderLabeled("TwitchToolkitNeutralKarma".Translate(), ref ToolkitSettings.TierFourNeutralBonus, Math.Round((double)ToolkitSettings.TierFourNeutralBonus).ToString(), 1, 100);
            optionsListing.SliderLabeled("TwitchToolkitBadKarma".Translate(), ref ToolkitSettings.TierFourBadBonus, Math.Round((double)ToolkitSettings.TierFourBadBonus).ToString(), 1, 100);

           }
    }
}
