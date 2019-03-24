using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings
{
    public static class Settings_Options
    {
        public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
        {
            optionsListing.CheckboxLabeled(Helper.ReplacePlaceholder("TwitchToolkitWhisperAllowed".Translate(), first: ToolkitSettings.Username), ref ToolkitSettings.WhisperCmdsAllowed);
            optionsListing.CheckboxLabeled(Helper.ReplacePlaceholder("TwitchToolkitWhisperOnly".Translate(), first: ToolkitSettings.Username), ref ToolkitSettings.WhisperCmdsOnly);
            optionsListing.CheckboxLabeled("TwitchToolkitPurchaseConfirmations".Translate(), ref ToolkitSettings.PurchaseConfirmations);
            optionsListing.CheckboxLabeled("TwitchToolkitRepeatViewerNames".Translate(), ref ToolkitSettings.RepeatViewerNames);
            optionsListing.CheckboxLabeled("TwitchToolkitViewerColonistQueue".Translate(), ref ToolkitSettings.ViewerNamedColonistQueue);
            optionsListing.CheckboxLabeled("TwitchToolkitMinifiableBuildings".Translate(), ref ToolkitSettings.MinifiableBuildings);

            optionsListing.GapLine();
            optionsListing.Gap();

            optionsListing.SliderLabeled("TwitchToolkitVoteTime".Translate(), ref ToolkitSettings.VoteTime, Math.Round((double)ToolkitSettings.VoteTime).ToString(), 1f, 15f);
            optionsListing.SliderLabeled("TwitchToolkitVoteOptions".Translate(), ref ToolkitSettings.VoteOptions, Math.Round((double)ToolkitSettings.VoteOptions).ToString(), 2f, 5f);
            optionsListing.CheckboxLabeled("TwitchToolkitVotingChatMsgs".Translate(), ref ToolkitSettings.VotingChatMsgs);
            optionsListing.CheckboxLabeled("TwitchToolkitVotingWindow".Translate(), ref ToolkitSettings.VotingWindow);
            optionsListing.CheckboxLabeled("TwitchToolkitLargeVotingWindow".Translate(), ref ToolkitSettings.LargeVotingWindow);

            optionsListing.GapLine();
            optionsListing.Gap();

            optionsListing.CheckboxLabeled("TwitchToolkitChatReqsForCoins".Translate(), ref ToolkitSettings.ChatReqsForCoins);
            optionsListing.SliderLabeled("TwitchToolkitTimeBeforeHalfCoins".Translate(), ref ToolkitSettings.TimeBeforeHalfCoins, Math.Round((double)ToolkitSettings.TimeBeforeHalfCoins).ToString(), 15f, 120f);
            optionsListing.SliderLabeled("TwitchToolkitTimeBeforeNoCoins".Translate(), ref ToolkitSettings.TimeBeforeNoCoins, Math.Round((double)ToolkitSettings.TimeBeforeNoCoins).ToString(), 30f, 240f);
        }
    }
}
