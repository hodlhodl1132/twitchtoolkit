using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace TwitchToolkit.Votes
{
    public class Vote_ExoticItems : Vote
    {
        public Vote_ExoticItems(Dictionary<int, List<Thing>> thingsOptions) : base(new List<int>(thingsOptions.Keys))
        {
            try
            {
                this.thingsOptions = thingsOptions;
            }
            catch (InvalidCastException e)
            {
                Helper.Log(e.Message);
            }
        }

        public override void EndVote()
        {
            Map map = Helper.AnyPlayerMap;
            Find.WindowStack.TryRemove(typeof(VoteWindow));
            Messages.Message(new Message("Chat voted for: " + VoteKeyLabel(DecideWinner()), MessageTypeDefOf.PositiveEvent), true);
            IntVec3 intVec = DropCellFinder.RandomDropSpot(map);
            DropPodUtility.DropThingsNear(intVec, map, thingsOptions[DecideWinner()], 110, false, true, true);
            Find.LetterStack.ReceiveLetter("LetterLabelCargoPodCrash".Translate(), "CargoPodCrash".Translate(), LetterDefOf.PositiveEvent, new TargetInfo(intVec, map, false), null, null);
        }

        public override void StartVote()
        {
            if (ToolkitSettings.VotingWindow || (!ToolkitSettings.VotingWindow && !ToolkitSettings.VotingChatMsgs))
            {
                VoteWindow window = new VoteWindow(this);
                Find.WindowStack.Add(window);
            }

            if (ToolkitSettings.VotingChatMsgs)
            {
                Toolkit.client.SendMessage("Which exotic item should the colony receive?" + ": " + "TwitchToolKitVoteInstructions".Translate());
                foreach (KeyValuePair<int, List<Thing>> pair in thingsOptions)
                {
                    Toolkit.client.SendMessage($"[{pair.Key + 1}]  {VoteKeyLabel(pair.Key)}");
                }
            }
        }

        public override string VoteKeyLabel(int id)
        {
            string msg = thingsOptions[id][0].LabelCap;
            for (int i = 1; i < thingsOptions[id].Count; i++)
                msg += ", " + thingsOptions[id][i].LabelCap;
            return msg;
        }

        Dictionary<int, List<Thing>> thingsOptions = null;
    }
}
