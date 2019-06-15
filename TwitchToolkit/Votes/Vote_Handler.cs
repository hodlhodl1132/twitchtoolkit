using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Utilities;

namespace TwitchToolkit.Votes
{
    public static class VoteHandler
    {

        public static bool voteActive = false;
        public static DateTime voteStartedAt;
        static List<Vote> voteQueue = new List<Vote>();
        public static Vote currentVote = null;
        private static bool forceEnd = false;

        public static void QueueVote(Vote vote) => voteQueue.Add(vote);

        public static void ForceEnd()
        {
            forceEnd = true;
        }

        public static void CheckForQueuedVotes()
        {
            if (voteActive == false && voteQueue.Count > 0 && currentVote == null)
            {
                voteActive = true;
                voteStartedAt = DateTime.Now;
                currentVote = voteQueue[0];
                voteQueue.Remove(currentVote);
                currentVote.StartVote();
            }

            if (voteActive == true && (TimeHelper.MinutesElapsed(voteStartedAt) >= ToolkitSettings.VoteTime || forceEnd))
            {
                forceEnd = false;
                currentVote.EndVote();
                currentVote = null;
                voteActive = false;
            }
        }
    }
}
