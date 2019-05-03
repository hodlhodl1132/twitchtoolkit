using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Votes;

namespace TwitchToolkit.Storytellers
{
    public class VotingIncidentEntry
    {
        public VotingIncident incident;
        public float weight;

        public VotingIncidentEntry(VotingIncident incident, float weight)
        {
            this.incident = incident;
            this.weight = weight;
        }
    }
}
