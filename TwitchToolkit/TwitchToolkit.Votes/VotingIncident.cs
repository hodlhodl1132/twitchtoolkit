using System;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.Votes;

public class VotingIncident : Def
{
	public int weight;

	public int voteWeight = 100;

	public Storyteller storyteller;

	public EventType eventType;

	public EventCategory eventCategory;

	public Type votingHelper = typeof(IncidentHelper);

	public VotingHelper helper = null;

	public VotingHelper Helper
	{
		get
		{
			if (helper == null)
			{
				Log.Warning("Casting " + base.label);
				helper = VotingIncidentMaker.makeVotingHelper(this);
			}
			return helper;
		}
	}
}
