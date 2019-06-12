using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Votes;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings
{
    [StaticConstructorOnStartup]
    public static class Settings_VoteWeights
    {
        static Settings_VoteWeights()
        {
            if (ToolkitSettings.VoteWeights == null || ToolkitSettings.VoteWeights.Count < 1)
            {
                if (DefDatabase<VotingIncident>.AllDefs == null || DefDatabase<VotingIncident>.AllDefs.Count() < 0)
                {
                    return;
                }

                Helper.Log("Loading brand new vote weights");

                ToolkitSettings.VoteWeights = new Dictionary<string, int>();

                foreach (VotingIncident vote in DefDatabase<VotingIncident>.AllDefs)
                {
                    ToolkitSettings.VoteWeights.Add(vote.defName, 100);
                }
            }
            else
            {
                Helper.Log("Loading vote weights not saved");

                List<VotingIncident> incidentsNotLoaded = DefDatabase<VotingIncident>.AllDefs.Where(s =>
                    !ToolkitSettings.VoteWeights.ContainsKey(s.defName)
                ).ToList();

                foreach (VotingIncident vote in incidentsNotLoaded)
                {
                    ToolkitSettings.VoteWeights.Add(vote.defName, 100);
                }
            }

            if (ToolkitSettings.VoteWeights != null && ToolkitSettings.VoteWeights.Count > 0)
            {
                Helper.Log("Loading vote weights from player settings");

                foreach (KeyValuePair<string, int> pair in ToolkitSettings.VoteWeights)
                {
                    VotingIncident vote = DefDatabase<VotingIncident>.AllDefs.ToList().Find(s =>
                        s.defName == pair.Key
                    );

                    if (vote != null)
                    {
                        vote.voteWeight = pair.Value;
                    }
                }
            }
        }

        public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
        {
            optionsListing.ColumnWidth = rect.width / 1.75f;

            optionsListing.Label("Change the weights given to votes. Setting to 0% disables it.");

            optionsListing.Gap(12);

            if (ToolkitSettings.VoteWeights != null)
            {
                List<VotingIncident> allVotes = DefDatabase<VotingIncident>.AllDefs.ToList();

                foreach (VotingIncident vote in allVotes)
                {
                    int index = vote.index;
                    optionsListing.SliderLabeled(vote.defName, ref vote.voteWeight, vote.voteWeight.ToString(), 0, 100);
                    ToolkitSettings.VoteWeights[vote.defName] = vote.voteWeight;

                    optionsListing.Gap(6);
                }
            }
        }
    }
}
