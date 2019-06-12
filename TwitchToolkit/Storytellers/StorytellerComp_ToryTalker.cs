using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.Storytellers
{
    public class StorytellerComp_ToryTalker : StorytellerComp
    {
        protected StorytellerCompProperties_ToryTalker Props
        {
            get
            {
                return (StorytellerCompProperties_ToryTalker)this.props;
            }
        }

        public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
        {
            voteTracker = Current.Game.GetComponent<StoryTellerVoteTracker>();

            if ((!VoteHandler.voteActive &&
                Rand.MTBEventOccurs(ToolkitSettings.ToryTalkerMTBDays, 60000f, 1000f)) || forced)
            {
                List<VotingIncidentEntry> entries = VotingIncidentsByWeight();
                List<VotingIncidentEntry> winners = new List<VotingIncidentEntry>();

                for (int i = 0; i < ToolkitSettings.VoteOptions && i < 12; i++)
                {
                    winners.Add(
                        entries.Where(s =>
                        !winners.Contains(s)
                        ).RandomElementByWeight(
                            (VotingIncidentEntry vi) => vi.weight)
                            );

                    int index = Math.Max(winners.Count - 1, 0);

                    winners[index].incident.helper = VotingIncidentMaker.makeVotingHelper(winners[index].incident);
                    winners[index].incident.helper.target = target;

                    if (!winners[index].incident.helper.IsPossible())
                    {
                        entries.RemoveAt(i);
                        i--;
                        winners.RemoveAt(index);
                    }
                }

                Dictionary<int, VotingIncident> incidents = new Dictionary<int, VotingIncident>();

                for (int i = 0; i < winners.Count; i++)
                {
                    incidents.Add(i, winners[i].incident);
                }

                StorytellerPack pack = DefDatabase<StorytellerPack>.GetNamed("ToryTalker");

                VoteHandler.QueueVote(new Vote_ToryTalker(incidents, pack, "Which event should happen next?"));
            }

            yield break;
        }

        public virtual List<VotingIncidentEntry> VotingIncidentsByWeight()
        {
            voteTracker = Current.Game.GetComponent<StoryTellerVoteTracker>();
            List<VotingIncident> candidates;
            if (voteTracker.VoteHistory.ContainsKey(voteTracker.lastID))
            {
                List<KeyValuePair<int, int>> history = voteTracker.VoteHistory.ToList();
                Helper.Log("History count " + history.Count);
                history.OrderBy(s => s.Value);
                IEnumerable<VotingIncident> search = DefDatabase<VotingIncident>.AllDefs.Where(s => s.defName == voteTracker.VoteIDs[history[0].Key]);
                Helper.Log("Search count " + search.Count());
                if (search != null && search.Count() > 0)
                {
                    previousVote = search.ElementAt(0);
                }
            }

            if (previousVote != null)
            {
                candidates = new List<VotingIncident>(DefDatabase<VotingIncident>.AllDefs.Where(s =>
                    s != previousVote
                    ));

                Helper.Log($"Previous vote was {previousVote.defName}");
            }
            else
            {
                candidates = new List<VotingIncident>(DefDatabase<VotingIncident>.AllDefs);
            }

            List<VotingIncidentEntry> voteEntries = new List<VotingIncidentEntry>();

            foreach (VotingIncident incident in candidates)
            {
                int weight = CalculateVotingIncidentWeight(incident);
                Helper.Log($"Incident {incident.LabelCap} weighted at {weight}");
                voteEntries.Add(new VotingIncidentEntry(incident, weight));
            }

            return voteEntries;
        }

        public virtual int CalculateVotingIncidentWeight(VotingIncident incident)
        {
            int previousWinsInVotingPeriod = voteTracker.TimesVoteHasBeenWonInVotingPeriod(incident);

            int initialWeight = (5 / incident.weight) * 100;

            int weightRemovedFromVotingPeriodWins = previousWinsInVotingPeriod * 20;

            int weightRemovedFromPreviousCategory = incident.eventCategory == voteTracker.previousCategory ? 50 : 0;

            int weightRemovedFromPreviousType = incident.eventType == voteTracker.previousType ? 50 : 0;

            return Convert.ToInt32(Math.Max(initialWeight -
                weightRemovedFromVotingPeriodWins -
                weightRemovedFromPreviousCategory -
                weightRemovedFromPreviousType, 0) * ((float)incident.voteWeight / 100f));
        }

        private bool TimerHasElapsed()
        {
            return true;
        }

        private VotingIncident previousVote = null;

        public StoryTellerVoteTracker voteTracker;

        public bool forced = false;
    }
}
