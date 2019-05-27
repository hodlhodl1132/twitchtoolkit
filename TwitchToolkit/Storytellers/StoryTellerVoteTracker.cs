using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.Storytellers
{
    public class StoryTellerVoteTracker : GameComponent
    {
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
            lastID = lastID + 1;
            VoteIDs.Add(lastID, incident.defName);
            VoteHistory.Add(lastID, lastTick);
            previousCategory = incident.eventCategory;
            previousType = incident.eventType;
            lastFiredTick = lastTick;
        }

        public bool HasBeenLongEnoughForAnotherIncident()
        {


            if (Find.TickManager.TicksGame > lastFiredTick + (ToolkitSettings.ToryTalkerMTBDays * GenDate.TicksPerDay))
                {
                if (Prefs.DevMode)
                    Log.Warning($"Checking last voting tick {Find.TickManager.TicksGame} > {lastFiredTick} + ({ToolkitSettings.ToryTalkerMTBDays * GenDate.TicksPerDay}) : true");
                return true;
            }
            if (Prefs.DevMode)
                Log.Warning($"Checking last voting tick {Find.TickManager.TicksGame} > {lastFiredTick} + ({ToolkitSettings.ToryTalkerMTBDays * GenDate.TicksPerDay}) : false");
            return false;
        }

        public int TimesVoteHasBeenWonInVotingPeriod(VotingIncident incident)
        {
            List<int> idsOfIncidentType = new List<int>(VoteIDs.Keys.Where(s => VoteIDs[s] == incident.defName));

            if (idsOfIncidentType != null)
            {
                return idsOfIncidentType.Count;
            }

            return 0;
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
                if (VoteHistory[pair.Key] >= VotingPeriodDays * GenDate.TicksPerDay)
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
            if (!StorytellerLastTickTracker.ContainsKey(pack.defName))
            {
                StorytellerLastTickTracker.Add(pack.defName, 0);
            }

            StorytellerLastTickTracker[pack.defName] = Find.TickManager.TicksGame;
        }

        public bool HaveMinimumDaysBetweenEventsPassed(StorytellerPack pack)
        {
            if (StorytellerLastTickTracker == null)
            {
                StorytellerLastTickTracker = new Dictionary<string, int>();
            }

            if (!StorytellerLastTickTracker.ContainsKey(pack.defName))
            {
                StorytellerLastTickTracker.Add(pack.defName, GenDate.DaysPassed < 2 ? 0 : Find.TickManager.TicksGame);

                return true;
            }

            float daysBetweenEvents;

            switch(pack.defName)
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

                default:
                    Log.Error("No MTB days for storyteller pack");
                    return false;
            }

            return (daysBetweenEvents * GenDate.TicksPerDay) + StorytellerLastTickTracker[pack.defName] < Find.TickManager.TicksGame;
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref VoteIDs, "VoteIDs", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref VoteHistory, "VoteHistory", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref StorytellerLastTickTracker, "StorytellerLastTickTracker", LookMode.Value, LookMode.Value);
            Scribe_Values.Look(ref lastID, "LastID", -1);
            Scribe_Values.Look(ref previousCategory, "previousCategory", EventCategory.Invasion);
            Scribe_Values.Look(ref previousType, "previousType", EventType.Bad);
            Scribe_Values.Look(ref lastFiredTick, "lastFiredTick", 0);
        }

        public Dictionary<int, string> VoteIDs = new Dictionary<int, string>();

        public Dictionary<int, int> VoteHistory = new Dictionary<int, int>();

        public Dictionary<string, int> StorytellerLastTickTracker = new Dictionary<string, int>();

        public int lastID = -1;

        public int VotingPeriodDays = 25;

        public EventCategory previousCategory = EventCategory.Invasion;

        public EventType previousType = EventType.Bad;

        public int lastFiredTick = 0;
    }
}
