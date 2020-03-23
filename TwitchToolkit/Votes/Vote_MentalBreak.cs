using ToolkitCore;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace TwitchToolkit.Votes
{
    public class Vote_MentalBreak : Vote
    {
        public Vote_MentalBreak(Dictionary<int, Pawn> pawnOptions) : base (new List<int>(pawnOptions.Keys))
        {
            try
            {
                this.pawnOptions = pawnOptions;
            }
            catch (InvalidCastException e)
            {
                Helper.Log(e.Message);
            }
        }
        public override void EndVote()
        {
            Pawn pawn = pawnOptions[DecideWinner()];
            float minorBreak = pawn.mindState.mentalBreaker.BreakThresholdMinor - 0.05f;
            IEnumerable<MentalBreakDef> breaks = from d in DefDatabase<MentalBreakDef>.AllDefsListForReading
					where d.intensity == MentalBreakIntensity.Minor && d.Worker.BreakCanOccur(pawn)
					select d;

            bool breakPawn = breaks.TryRandomElementByWeight((MentalBreakDef d) => d.Worker.CommonalityFor(pawn), out MentalBreakDef mentalBreakDef);
        	string text = "MentalStateReason_Mood".Translate();
			text = text + "\n\n" + "FinalStraw".Translate("Chat said something that upset them.");

            if (!pawn.Awake())
            {
                pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
            }

            if (breakPawn && mentalBreakDef.Worker.TryStart(pawn, text, false))
            {        
                Messages.Message(new Message("Chat caused a mental break for: " + pawnOptions[DecideWinner()].LabelCap, MessageTypeDefOf.NegativeEvent), true);
            }
                
            Find.WindowStack.TryRemove(typeof(VoteWindow));
        }

        public override void StartVote()
        {
            if (ToolkitSettings.VotingWindow || (!ToolkitSettings.VotingWindow && !ToolkitSettings.VotingChatMsgs))
            {
                VoteWindow window = new VoteWindow(this, "Which colonist should experience a mental break?");
                Find.WindowStack.Add(window);
            }

            if (ToolkitSettings.VotingChatMsgs)
            {
                TwitchWrapper.SendChatMessage("Which colonist should experience a mental break?");
                foreach (KeyValuePair<int, Pawn> pair in pawnOptions)
                {
                    TwitchWrapper.SendChatMessage($"[{pair.Key + 1}]  {VoteKeyLabel(pair.Key)}");
                }
            }
        }

        public override string VoteKeyLabel(int id)
        {
            string nick = (pawnOptions[id].Name as NameTriple).Nick;
            if (nick != null)
                return nick;
            return pawnOptions[id].Name.ToString();
        }

        Dictionary<int, Pawn> pawnOptions = null;
    }
}
