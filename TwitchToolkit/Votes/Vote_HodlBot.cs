using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Storytellers;
using Verse;

namespace TwitchToolkit.Votes
{
    public class Vote_HodlBot : Vote_VotingIncident
    {
        public Vote_HodlBot(Dictionary<int, VotingIncident> incidents, StorytellerPack pack, string title = null) : base(incidents, title)
        {
            this.pack = pack;
            this.title = title;
            labelType = VoteLabelType.Label;
        }

        public override void StartVote()
        {
            if (ToolkitSettings.VotingWindow || (!ToolkitSettings.VotingWindow && !ToolkitSettings.VotingChatMsgs))
            {
                VoteWindow window = new VoteWindow(this, "<color=#4BB543>" + title + "</color>");
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

        readonly StorytellerPack pack = null;
    }
}
