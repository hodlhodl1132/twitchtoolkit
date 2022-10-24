using System.Collections.Generic;
using System.Linq;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.Settings;

public static class Settings_VoteWeights
{
	public static void Load_Votewieghts()
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
			List<VotingIncident> incidentsNotLoaded = (from s in DefDatabase<VotingIncident>.AllDefs
				where !ToolkitSettings.VoteWeights.ContainsKey(((Def)s).defName)
				select s).ToList();
			foreach (VotingIncident vote2 in incidentsNotLoaded)
			{
				ToolkitSettings.VoteWeights.Add(((Def)vote2).defName, 100);
			}
		}
		if (ToolkitSettings.VoteWeights == null || ToolkitSettings.VoteWeights.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<string, int> pair in ToolkitSettings.VoteWeights)
		{
			VotingIncident vote = DefDatabase<VotingIncident>.AllDefs.ToList().Find((VotingIncident s) => ((Def)s).defName == pair.Key);
			if (vote != null)
			{
				vote.voteWeight = pair.Value;
			}
		}
	}
}
