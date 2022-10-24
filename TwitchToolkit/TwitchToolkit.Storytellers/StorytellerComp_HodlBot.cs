using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.Storytellers;

public class StorytellerComp_HodlBot : StorytellerComp_ToryTalker
{
	private Vote_VotingIncident.VoteLabelType voteType = Vote_VotingIncident.VoteLabelType.Category;

	protected new StorytellerCompProperties_HodlBot Props => (StorytellerCompProperties_HodlBot)(object)((StorytellerComp)this).props;

	public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
	{
		if ((VoteHandler.voteActive || !Rand.MTBEventOccurs(ToolkitSettings.HodlBotMTBDays, 60000f, 1000f)) && !forced)
		{
			yield break;
		}
		if (Rand.Range(0, 1) == 1)
		{
			voteType = Vote_VotingIncident.VoteLabelType.Type;
		}
		List<VotingIncidentEntry> source = VotingIncidentsByWeight();
		List<VotingIncidentEntry> winners = new List<VotingIncidentEntry>();
		string str = null;
		switch (voteType)
		{
		case Vote_VotingIncident.VoteLabelType.Category:
		{
			EventCategory category = RandomCategory();
			source = source.Where((VotingIncidentEntry s) => s.incident.eventCategory == category).ToList();
			str = "Which " + category.ToString() + " event should happen?";
			Helper.Log("rand cat picked " + category);
			break;
		}
		case Vote_VotingIncident.VoteLabelType.Type:
		{
			EventType randType = RandomType();
			source = source.Where((VotingIncidentEntry s) => s.incident.eventType == randType).ToList();
			str = string.Concat("Which ", randType, " event should happen?");
			Helper.Log("rand type picked " + randType);
			break;
		}
		}
		VotingIncidentEntry votingIncidentEntry = default(VotingIncidentEntry);
		for (int j = 0; j < ToolkitSettings.VoteOptions; j++)
		{
			if (GenCollection.TryRandomElementByWeight<VotingIncidentEntry>(source.Where((VotingIncidentEntry s) => !winners.Contains(s)), (Func<VotingIncidentEntry, float>)((VotingIncidentEntry vi) => vi.weight), out votingIncidentEntry))
			{
				votingIncidentEntry.incident.Helper.target = target;
				if (votingIncidentEntry.incident.Helper.IsPossible())
				{
					winners.Add(votingIncidentEntry);
				}
			}
			votingIncidentEntry = null;
		}
		if (winners.Count < 3)
		{
			Helper.Log("Less than 3 possible votes were found");
			yield break;
		}
		Dictionary<int, VotingIncident> dictionary = new Dictionary<int, VotingIncident>();
		for (int i = 0; i < winners.Count; i++)
		{
			dictionary.Add(i, winners[i].incident);
		}
		StorytellerPack named = DefDatabase<StorytellerPack>.GetNamed("HodlBot", true);
		VoteHandler.QueueVote(new Vote_HodlBot(dictionary, named, voteType, str));
	}

	public EventType RandomType()
	{
		List<EventType> list = Enum.GetValues(typeof(EventType)).Cast<EventType>().ToList();
		Dictionary<EventType, float> dictionary = new Dictionary<EventType, float>();
		foreach (EventType key in list)
		{
			dictionary.Add(key, ToolkitSettings.VoteCategoryWeights[key.ToString()]);
		}
		return GenCollection.RandomElementByWeight<KeyValuePair<EventType, float>>((IEnumerable<KeyValuePair<EventType, float>>)dictionary, (Func<KeyValuePair<EventType, float>, float>)((KeyValuePair<EventType, float> pair) => pair.Value)).Key;
	}

	public EventCategory RandomCategory()
	{
		List<EventCategory> list = Enum.GetValues(typeof(EventCategory)).Cast<EventCategory>().ToList();
		Dictionary<EventCategory, float> dictionary = new Dictionary<EventCategory, float>();
		foreach (EventCategory key in list)
		{
			dictionary.Add(key, ToolkitSettings.VoteCategoryWeights[key.ToString()]);
		}
		return GenCollection.RandomElementByWeight<KeyValuePair<EventCategory, float>>((IEnumerable<KeyValuePair<EventCategory, float>>)dictionary, (Func<KeyValuePair<EventCategory, float>, float>)((KeyValuePair<EventCategory, float> pair) => pair.Value)).Key;
	}
}
