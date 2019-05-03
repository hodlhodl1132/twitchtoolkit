using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings
{
    public static class Settings_Storyteller
    {
        public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
        {
            optionsListing.Label("All");
            optionsListing.GapLine();

            optionsListing.SliderLabeled("TwitchToolkitVoteTime".Translate(), ref ToolkitSettings.VoteTime, Math.Round((double)ToolkitSettings.VoteTime).ToString(), 1f, 15f);
            optionsListing.SliderLabeled("TwitchToolkitVoteOptions".Translate(), ref ToolkitSettings.VoteOptions, Math.Round((double)ToolkitSettings.VoteOptions).ToString(), 2f, 5f);
            optionsListing.CheckboxLabeled("TwitchToolkitVotingChatMsgs".Translate(), ref ToolkitSettings.VotingChatMsgs);
            optionsListing.CheckboxLabeled("TwitchToolkitVotingWindow".Translate(), ref ToolkitSettings.VotingWindow);
            optionsListing.CheckboxLabeled("TwitchToolkitLargeVotingWindow".Translate(), ref ToolkitSettings.LargeVotingWindow);

            optionsListing.Gap();
            optionsListing.Label("Tory Talker");
            optionsListing.GapLine();

            string toryTalkerMTBDays = Math.Truncate(((double)ToolkitSettings.ToryTalkerMTBDays * 100) / 100).ToString();
            optionsListing.TextFieldNumericLabeled<float>("Average Days Between Events", ref ToolkitSettings.ToryTalkerMTBDays, ref toryTalkerMTBDays, 0.5f, 10f);

            optionsListing.Gap();
            optionsListing.Label("Hodlbot");
            optionsListing.GapLine();

            string hodlbotMTBDays = Math.Truncate(((double)ToolkitSettings.HodlBotMTBDays * 100) / 100).ToString();
            optionsListing.TextFieldNumericLabeled<float>("Average Days Between Events", ref ToolkitSettings.HodlBotMTBDays, ref hodlbotMTBDays, 0.5f, 10f);

            optionsListing.CheckboxLabeled("Use ToryTalker Storyteller Alongside Hodlbot?", ref ToolkitSettings.UseToryTalkerWithHodlBot);
        }
    }
}
