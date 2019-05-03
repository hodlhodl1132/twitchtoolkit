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
            if (!VoteHandler.voteActive && Rand.MTBEventOccurs(ToolkitSettings.ToryTalkerMTBDays, 60000f, 1000f))
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

                VoteHandler.QueueVote(new Vote_VotingIncident(incidents));
            }

            yield break;
        }

        public List<VotingIncidentEntry> VotingIncidentsByWeight()
        {
            List<VotingIncident> candidates;
            if (previousVote != null)
            {
                candidates = new List<VotingIncident>(DefDatabase<VotingIncident>.AllDefs.Where(s =>
                    s != previousVote
                    ));
            }
            else
            {
                candidates = new List<VotingIncident>(DefDatabase<VotingIncident>.AllDefs);
            }

            List<VotingIncidentEntry> voteEntries = new List<VotingIncidentEntry>();

            foreach (VotingIncident incident in candidates)
            {
                voteEntries.Add(new VotingIncidentEntry(incident, CalculateVotingIncidentWeight(incident)));
            }

            return voteEntries;
        }

        public int CalculateVotingIncidentWeight(VotingIncident incident)
        {
            int previousWinsInVotingPeriod = voteTracker.TimesVoteHasBeenWonInVotingPeriod(incident);

            int initialWeight = (5 / incident.weight) * 100;

            int weightRemovedFromVotingPeriodWins = previousWinsInVotingPeriod * 20;

            int weightRemovedFromPreviousCategory = incident.eventCategory == previousCategory ? 50 : 0;

            int weightRemovedFromPreviousType = incident.eventType == previousType ? 50 : 0;

            return Convert.ToInt32(Math.Max(initialWeight -
                weightRemovedFromVotingPeriodWins -
                weightRemovedFromPreviousCategory -
                weightRemovedFromPreviousType, 0) * ((float)incident.voteWeight / 100f));
        }

        private bool TimerHasElapsed()
        {
            return true;
        }

        private EventCategory previousCategory = EventCategory.Invasion;

        private EventType previousType = EventType.Bad;

        private VotingIncident previousVote = null;

        private StoryTellerVoteTracker voteTracker = Current.Game.GetComponent<StoryTellerVoteTracker>();
    }
}
