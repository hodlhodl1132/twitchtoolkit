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
            lastID = lastID + 1;
            VoteIDs.Add(lastID, incident.defName);
            VoteHistory.Add(lastID, Find.TickManager.TicksGame);
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

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref VoteIDs, "VoteIDs", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref VoteHistory, "VoteHistory", LookMode.Value, LookMode.Value);
            Scribe_Values.Look(ref lastID, "LastID", -1);
        }

        public Dictionary<int, string> VoteIDs = new Dictionary<int, string>();

        public Dictionary<int, int> VoteHistory = new Dictionary<int, int>();

        public int lastID = -1;

        public int VotingPeriodDays = 25;
    }
}
