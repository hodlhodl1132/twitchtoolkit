using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Storytellers;
using Verse;

namespace TwitchToolkit.Votes
{
    public class Vote_ToryTalker : Vote_VotingIncident
    {
        public Vote_ToryTalker(Dictionary<int, VotingIncident> incidents, StorytellerPack pack, string title = null) : base(incidents, title)
        {
            this.title = title;
            this.pack = pack;
            labelType = VoteLabelType.Label;
        }

        public override void StartVote()
        {
            if (ToolkitSettings.VotingWindow || (!ToolkitSettings.VotingWindow && !ToolkitSettings.VotingChatMsgs))
            {
                VoteWindow window = new VoteWindow(this, "<color=#6441A4>" + title + "</color>");
                Find.WindowStack.Add(window);
            }

            if (ToolkitSettings.VotingChatMsgs)
            {
                Toolkit.client.SendMessage(title ?? "TwitchStoriesChatMessageNewVote".Translate() + ": " + "TwitchToolKitVoteInstructions".Translate());
                foreach (KeyValuePair<int, VotingIncident> pair in incidents)
                {
                    Toolkit.client.SendMessage($"[{pair.Key + 1}]  {VoteKeyLabel(pair.Key)}");
                }
            }
        }

        public override void EndVote()
        {
            Current.Game.GetComponent<StoryTellerVoteTracker>().LogStorytellerCompVote(pack);
            base.EndVote();
        }

        StorytellerPack pack = null;
    }
}
