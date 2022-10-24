using System.Collections.Generic;
using System.Linq;
using RimWorld;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.Storytellers;

public class StoryTellerVoteTracker : GameComponent
{
	public Dictionary<int, string> VoteIDs = new Dictionary<int, string>();

	public Dictionary<int, int> VoteHistory = new Dictionary<int, int>();

	public Dictionary<string, int> StorytellerLastTickTracker = new Dictionary<string, int>();

	public int lastID = -1;

	public int VotingPeriodDays = 25;

	public EventCategory previousCategory = EventCategory.Invasion;

	public EventType previousType = EventType.Bad;

	public int lastFiredTick = 0;

	public StoryTellerVoteTracker(Game game)
	{
	}

	public override void GameComponentTick()
	{
		if (Find.TickManager.TicksGame % 10000 == 0)
		{
			CleanListOfExpiredVotes();
		}
	}

	public void LogVoteIncident(VotingIncident incident)
	{
		int lastTick = Find.TickManager.TicksGame;
		lastID++;
		VoteIDs.Add(lastID, ((Def)incident).defName);
		VoteHistory.Add(lastID, lastTick);
		previousCategory = incident.eventCategory;
		previousType = incident.eventType;
		lastFiredTick = lastTick;
	}

	public bool HasBeenLongEnoughForAnotherIncident()
	{
		if ((float)Find.TickManager.TicksGame > (float)lastFiredTick + ToolkitSettings.ToryTalkerMTBDays * 60000f)
		{
			if (Prefs.DevMode)
			{
				Helper.Log($"Checking last voting tick {Find.TickManager.TicksGame} > {lastFiredTick} + ({ToolkitSettings.ToryTalkerMTBDays * 60000f}) : true");
			}
			return true;
		}
		if (Prefs.DevMode)
		{
			Helper.Log($"Checking last voting tick {Find.TickManager.TicksGame} > {lastFiredTick} + ({ToolkitSettings.ToryTalkerMTBDays * 60000f}) : false");
		}
		return false;
	}

	public int TimesVoteHasBeenWonInVotingPeriod(VotingIncident incident)
	{
		return new List<int>(VoteIDs.Keys.Where((int s) => VoteIDs[s] == ((Def)incident).defName))?.Count ?? 0;
	}

	public bool CanVoteCanUsedAgainInThisVotingPeriod(VotingIncident incident)
	{
		int timesVoteCanBeUsedInVotingPeriod = VotingPeriodDays / incident.weight / 5;
		if (TimesVoteHasBeenWonInVotingPeriod(incident) >= timesVoteCanBeUsedInVotingPeriod)
		{
			return false;
		}
		return true;
	}

	public void CleanListOfExpiredVotes()
	{
		List<int> idsToRemove = new List<int>();
		foreach (KeyValuePair<int, string> pair in VoteIDs)
		{
			if (VoteHistory[pair.Key] >= VotingPeriodDays * 60000)
			{
				idsToRemove.Add(pair.Key);
			}
		}
		foreach (int id in idsToRemove)
		{
			VoteIDs.Remove(id);
			VoteHistory.Remove(id);
		}
	}

	public void LogStorytellerCompVote(StorytellerPack pack)
	{
		if (!StorytellerLastTickTracker.ContainsKey(((Def)pack).defName))
		{
			StorytellerLastTickTracker.Add(((Def)pack).defName, 0);
		}
		StorytellerLastTickTracker[((Def)pack).defName] = Find.TickManager.TicksGame;
	}

	public bool HaveMinimumDaysBetweenEventsPassed(StorytellerPack pack)
	{
		if (StorytellerLastTickTracker == null)
		{
			StorytellerLastTickTracker = new Dictionary<string, int>();
		}
		if (!StorytellerLastTickTracker.ContainsKey(((Def)pack).defName))
		{
			StorytellerLastTickTracker.Add(((Def)pack).defName, (GenDate.DaysPassed >= 2) ? Find.TickManager.TicksGame : 0);
			return true;
		}
		float daysBetweenEvents;
		switch (((Def)pack).defName)
		{
		case "ToryTalker":
			daysBetweenEvents = ToolkitSettings.ToryTalkerMTBDays;
			break;
		case "HodlBot":
			daysBetweenEvents = ToolkitSettings.HodlBotMTBDays;
			break;
		case "UristBot":
			daysBetweenEvents = ToolkitSettings.UristBotMTBDays;
			break;
		case "Milasandra":
			daysBetweenEvents = ((StorytellerCompProperties_ToolkitOnOffCycle)(object)DefDatabase<StorytellerPack>.GetNamed("Milasandra", true).StorytellerComp.props).minSpacingDays;
			break;
		case "Mercurius":
			daysBetweenEvents = ((StorytellerCompProperties_ToolkitCategoryMTB)(object)DefDatabase<StorytellerPack>.GetNamed("Mercurius", true).StorytellerComp.props).mtbDays;
			break;
		default:
			Log.Error("No MTB days for storyteller pack");
			return false;
		}
		return daysBetweenEvents * 60000f + (float)StorytellerLastTickTracker[((Def)pack).defName] < (float)Find.TickManager.TicksGame;
	}

	public override void ExposeData()
	{
		Scribe_Collections.Look<int, string>(ref VoteIDs, "VoteIDs", (LookMode)1, (LookMode)1);
		Scribe_Collections.Look<int, int>(ref VoteHistory, "VoteHistory", (LookMode)1, (LookMode)1);
		Scribe_Collections.Look<string, int>(ref StorytellerLastTickTracker, "StorytellerLastTickTracker", (LookMode)1, (LookMode)1);
		Scribe_Values.Look<int>(ref lastID, "LastID", -1, false);
		Scribe_Values.Look<EventCategory>(ref previousCategory, "previousCategory", EventCategory.Invasion, false);
		Scribe_Values.Look<EventType>(ref previousType, "previousType", EventType.Bad, false);
		Scribe_Values.Look<int>(ref lastFiredTick, "lastFiredTick", 0, false);
	}
}
