using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Votes;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings
{
    public static class Settings_VoteWeights
    {
        public static void Load_VoteWeights()
        {
            if (ToolkitSettings.VoteWeights != null && ToolkitSettings.VoteWeights.Count < 1)
            {
                if (DefDatabase<VotingIncident>.AllDefs == null || DefDatabase<VotingIncident>.AllDefs.Count() < 1)
                {
                    return;
                }
            }
            else
            {
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
    }
}
