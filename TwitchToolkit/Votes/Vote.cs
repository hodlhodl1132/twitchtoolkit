using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace TwitchToolkit.Votes
{
    public abstract class Vote
    {
        public List<int> optionsKeys = null;
        public Dictionary<int, int> viewerVotes = new Dictionary<int, int>();
        public Dictionary<int, int> voteCounts = new Dictionary<int, int>();

        public Vote(List<int> optionKeys)
        {
            try
            {
                this.optionsKeys = optionKeys;
                foreach (int key in optionsKeys) voteCounts.Add(key, 0);
            }
            catch (InvalidCastException e)
            {
                Log.Error("Invalid new vote. " + e.Message);
            }
        }

        public void RecordVote(int userId, int voteKey)
        {
            Helper.Log("id " + userId + " votekey" + voteKey);
            if (!VoteKeyExists(voteKey)) return;
            if (viewerVotes.ContainsKey(userId)) viewerVotes[userId] = voteKey;
            else viewerVotes.Add(userId, voteKey);
            CountVotes();
        }

        public bool VoteKeyExists(int optionId)
        {
            return optionsKeys.Contains(optionId);
        }

        public void CountVotes()
        {
            foreach (int key in optionsKeys) voteCounts[key] = 0;
            foreach (KeyValuePair<int, int> vote in viewerVotes) voteCounts[vote.Value] += 1;
        }

        public int DecideWinner()
        {
            return voteCounts.Aggregate((k, i) => k.Value > i.Value ? k : k.Value == i.Value ? (new System.Random().Next(0, 1) == 0 ? k : i) : i).Key;
        }

        public abstract void StartVote();
        public abstract void EndVote();
        public abstract string VoteKeyLabel(int id);
    }
}
