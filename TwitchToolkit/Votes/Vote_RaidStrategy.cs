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
    public class Vote_RaidStrategy : Vote
    {
        public Vote_RaidStrategy(Dictionary<int, RaidStrategyDef> allStrategies, StorytellerPack pack, IncidentWorker worker, StorytellerComp comp, IncidentParms parms, string title = null) : base(new List<int>(allStrategies.Keys))
        {
            this.title = title;
            this.allStrategies = allStrategies;
            this.pack = pack;
            this.worker = worker;
            this.parms = parms;
            this.comp = comp;
        }

        public override void EndVote()
        {
            parms.raidStrategy = allStrategies[DecideWinner()];

            Find.WindowStack.TryRemove(typeof(VoteWindow));
            Ticker.FiringIncidents.Enqueue(new FiringIncident(worker.def, comp, parms));

            Current.Game.GetComponent<StoryTellerVoteTracker>().LogStorytellerCompVote(pack);
        }

        public override void StartVote()
        {
            if (ToolkitSettings.VotingWindow || (!ToolkitSettings.VotingWindow && !ToolkitSettings.VotingChatMsgs))
            {
                VoteWindow window = new VoteWindow(this, "<color=#CF0E0F>" + title + "</color>");
                Find.WindowStack.Add(window);
            }

            if (ToolkitSettings.VotingChatMsgs)
            {
                MessageQueue.messageQueue.Enqueue(title ?? "TwitchStoriesChatMessageNewVote".Translate() + ": " + "TwitchToolKitVoteInstructions".Translate());
                foreach (KeyValuePair<int, RaidStrategyDef> pair in allStrategies)
                {
                    MessageQueue.messageQueue.Enqueue($"[{pair.Key + 1}]  {VoteKeyLabel(pair.Key)}");
                }
            }
        }

        public override string VoteKeyLabel(int id)
        {
            switch(allStrategies[id].defName)
            {
                case "ImmediateAttack":
                    return "Immediate Attack";
                case "StageThenAttack":
                    return "Wait, Then Attack";
                case "ImmediateAttackSmart":
                    return "Avoid Traps & Turrets";
                case "ImmediateAttackSappers":
                    return "Sappers";
                case "Siege":
                    return "Siege";
                default:
                    return allStrategies[id].defName;
            }
        }

        private readonly Dictionary<int, RaidStrategyDef> allStrategies;
        private readonly StorytellerPack pack;
        private readonly StorytellerComp comp;
        private IncidentWorker worker = null;
        private IncidentParms parms = null;
        private string title;
    }
}
