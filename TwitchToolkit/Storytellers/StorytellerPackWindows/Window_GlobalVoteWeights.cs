using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Votes;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Storytellers.StorytellerPackWindows
{
    public class Window_GlobalVoteWeights : Window
    {
        public Window_GlobalVoteWeights()
        {
            this.doCloseButton = true;
        }

        private Vector2 scrollPosition;

        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();

            List<VotingIncident> allVotes = DefDatabase<VotingIncident>.AllDefs.ToList();

            Rect outRect = new Rect(0, 0, inRect.width, inRect.height - 50f);
            Rect viewRect = new Rect(0f, 0f, outRect.width - 20, allVotes.Count * 31f);

            listing.Begin(inRect);
            listing.BeginScrollView(outRect, ref scrollPosition, ref viewRect);

            listing.Label("Change the weights given to votes. Setting to 0% disables it.");

            listing.Gap(12);

            if (ToolkitSettings.VoteWeights != null)
            {

                foreach (VotingIncident vote in allVotes)
                {
                    int index = vote.index;
                    listing.SliderLabeled(vote.defName, ref vote.voteWeight, vote.voteWeight.ToString(), 0, 100);
                    ToolkitSettings.VoteWeights[vote.defName] = vote.voteWeight;

                    listing.Gap(6);
                }
            }

            listing.EndScrollView(ref viewRect);
            listing.End();
        }

        public override Vector2 InitialSize => new Vector2(400f, 560f);
    }
}
