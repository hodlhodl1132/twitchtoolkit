using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.Storytellers;

public class StorytellerComp_ToryTalker : StorytellerComp
{
	private VotingIncident previousVote = null;

	public StoryTellerVoteTracker voteTracker;

	public bool forced = false;

	protected StorytellerCompProperties_ToryTalker Props => (StorytellerCompProperties_ToryTalker)(object)base.props;

	public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
	{
		voteTracker = Current.Game.GetComponent<StoryTellerVoteTracker>();
		if ((VoteHandler.voteActive || !Rand.MTBEventOccurs(ToolkitSettings.ToryTalkerMTBDays, 60000f, 1000f)) && !forced)
		{
			yield break;
		}
		List<VotingIncidentEntry> entries = VotingIncidentsByWeight();
		List<VotingIncidentEntry> winners = new List<VotingIncidentEntry>();
		for (int i = 0; i < ToolkitSettings.VoteOptions && i < 12; i++)
		{
			winners.Add(GenCollection.RandomElementByWeight<VotingIncidentEntry>(entries.Where((VotingIncidentEntry s) => !winners.Contains(s)), (Func<VotingIncidentEntry, float>)((VotingIncidentEntry vi) => vi.weight)));
			int index = Math.Max(winners.Count - 1, 0);
			winners[index].incident.helper = VotingIncidentMaker.makeVotingHelper(winners[index].incident);
			winners[index].incident.helper.target = target;
			if (!winners[index].incident.helper.IsPossible())
			{
				entries.RemoveAt(i);
				i--;
				winners.RemoveAt(index);
			}
		}
		Dictionary<int, VotingIncident> incidents = new Dictionary<int, VotingIncident>();
		for (int j = 0; j < winners.Count; j++)
		{
			incidents.Add(j, winners[j].incident);
		}
		StorytellerPack pack = DefDatabase<StorytellerPack>.GetNamed("ToryTalker", true);
		VoteHandler.QueueVote(new Vote_ToryTalker(incidents, pack, "Which event should happen next?"));
	}

	public virtual List<VotingIncidentEntry> VotingIncidentsByWeight()
	{
		//IL_0181: Unknown result type (might be due to invalid IL or missing erences)
		voteTracker = Current.Game.GetComponent<StoryTellerVoteTracker>();
		if (voteTracker.VoteHistory.ContainsKey(voteTracker.lastID))
		{
			List<KeyValuePair<int, int>> history = voteTracker.VoteHistory.ToList();
			Helper.Log("History count " + history.Count);
			history.OrderBy((KeyValuePair<int, int> s) => s.Value);
			IEnumerable<VotingIncident> search = from s in DefDatabase<VotingIncident>.AllDefs
				where ((Def)s).defName == voteTracker.VoteIDs[history[0].Key]
				select s;
			Helper.Log("Search count " + search.Count());
			if (search != null && search.Count() > 0)
			{
				previousVote = search.ElementAt(0);
			}
		}
		List<VotingIncident> candidates;
		if (previousVote != null)
		{
			candidates = new List<VotingIncident>(from s in DefDatabase<VotingIncident>.AllDefs
				where s != previousVote
				select s);
			Helper.Log("Previous vote was " + ((Def)previousVote).defName);
		}
		else
		{
			candidates = new List<VotingIncident>(DefDatabase<VotingIncident>.AllDefs);
		}
		List<VotingIncidentEntry> voteEntries = new List<VotingIncidentEntry>();
		foreach (VotingIncident incident in candidates)
		{
			int weight = CalculateVotingIncidentWeight(incident);
			Helper.Log($"Incident {((Def)incident).LabelCap} weighted at {weight}");
			voteEntries.Add(new VotingIncidentEntry(incident, weight));
		}
		return voteEntries;
	}

	public virtual int CalculateVotingIncidentWeight(VotingIncident incident)
	{
		int previousWinsInVotingPeriod = voteTracker.TimesVoteHasBeenWonInVotingPeriod(incident);
		int initialWeight = 5 / incident.weight * 100;
		int weightRemovedFromVotingPeriodWins = previousWinsInVotingPeriod * 20;
		int weightRemovedFromPreviousCategory = ((incident.eventCategory == voteTracker.previousCategory) ? 50 : 0);
		int weightRemovedFromPreviousType = ((incident.eventType == voteTracker.previousType) ? 50 : 0);
		return Convert.ToInt32((float)Math.Max(initialWeight - weightRemovedFromVotingPeriodWins - weightRemovedFromPreviousCategory - weightRemovedFromPreviousType, 0) * ((float)incident.voteWeight / 100f));
	}

	private bool TimerHasElapsed()
	{
		return true;
	}
}
