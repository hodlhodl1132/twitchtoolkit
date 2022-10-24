using System;

namespace TwitchToolkit.Votes;

public static class VotingIncidentMaker
{
	public static VotingHelper makeVotingHelper(VotingIncident def)
	{
		return (VotingHelper)Activator.CreateInstance(def.votingHelper);
	}
}
