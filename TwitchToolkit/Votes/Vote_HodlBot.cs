using rim_twitch;
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
        public Vote_HodlBot(Dictionary<int, VotingIncident> incidents, StorytellerPack pack, VoteLabelType labelType, string title = null) : base(incidents, title)
        {

            PickVoteLabelType(labelType);

            this.pack = pack;
            this.title = title;
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
                MessageQueue.messageQueue.Enqueue(title ?? "TwitchStoriesChatMessageNewVote".Translate() + ": " + "TwitchToolKitVoteInstructions".Translate());
                foreach (KeyValuePair<int, VotingIncident> pair in incidents)
                {
                    MessageQueue.messageQueue.Enqueue($"[{pair.Key + 1}]  {VoteKeyLabel(pair.Key)}");
                }
            }
        }

        public override void EndVote()
        {
            Current.Game.GetComponent<StoryTellerVoteTracker>().LogStorytellerCompVote(pack);
            base.EndVote();
        }

        void PickVoteLabelType(VoteLabelType labelType)
        {
            switch (labelType)
            {
                case VoteLabelType.Category:
                    if (categoriesThatCanBeMystery.Any(s => s == incidents.ElementAt(0).Value.eventCategory) && Rand.Bool)
                    {
                        this.labelType = VoteLabelType.Type;
                    }
                    else
                    {
                        this.labelType = VoteLabelType.Label;
                    }
                    break;
            }
        }

        static readonly List<EventCategory> categoriesThatCanBeMystery = new List<EventCategory> {
            EventCategory.Animal,
            EventCategory.Colonist,
            EventCategory.Drop,
            EventCategory.Enviroment,
            EventCategory.Foreigner
        };

        readonly StorytellerPack pack = null;
    }
}
