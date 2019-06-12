using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Store;
using TwitchToolkit.Storytellers;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Votes
{
    public class ToryTalkerVote : IncidentHelperVariables
    {
        public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
        {
            this.Viewer = viewer;
            toryTalker = DefDatabase<StorytellerPack>.GetNamed("ToryTalker");

            if (toryTalker != null && toryTalker.enabled)
            {
                return true;
            }

            return false;
        }

        public override void TryExecute()
        {
            Map map = Helper.AnyPlayerMap;
            StorytellerComp_ToryTalker storytellerComp = new StorytellerComp_ToryTalker();
            storytellerComp.forced = true;
            foreach (FiringIncident incident in storytellerComp.MakeIntervalIncidents(map))
            {
                Ticker.FiringIncidents.Enqueue(incident);
                break;
            }

            Viewer.TakeViewerCoins(storeIncident.cost);

            Viewer.CalculateNewKarma(storeIncident.karmaType, storeIncident.cost);

            VariablesHelpers.SendPurchaseMessage($"@{Viewer.username} purchased a ToryTalker Vote.");
        }

        StorytellerPack toryTalker;

        public override Viewer Viewer { get; set; }
    }

    public class HodlBotVote : IncidentHelperVariables
    {
        public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
        {
            this.Viewer = viewer;
            hodlBot = DefDatabase<StorytellerPack>.GetNamed("HodlBot");

            if (hodlBot != null && hodlBot.enabled)
            {
                return true;
            }

            return false;
        }

        public override void TryExecute()
        {
            Map map = Helper.AnyPlayerMap;
            StorytellerComp_HodlBot storytellerComp = new StorytellerComp_HodlBot();
            storytellerComp.forced = true;
            foreach (FiringIncident incident in storytellerComp.MakeIntervalIncidents(map))
            {
                Ticker.FiringIncidents.Enqueue(incident);
                break;
            }

            Viewer.TakeViewerCoins(storeIncident.cost);

            Viewer.CalculateNewKarma(storeIncident.karmaType, storeIncident.cost);

            VariablesHelpers.SendPurchaseMessage($"@{Viewer.username} purchased a HodlBot Vote.");
        }

        StorytellerPack hodlBot;

        public override Viewer Viewer { get; set; }
    }
}
