using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Windows;
using Verse;

namespace TwitchToolkit.Votes
{
    public class VoteIncidentDef : Vote
    {

        public VoteIncidentDef(Dictionary<int, IncidentDef> incidents, StorytellerComp source, IncidentParms parms = null) : base(new List<int>(incidents.Keys))
        {
            this.parms = parms;
            try
            {
                this.incidents = incidents;
                this.source = source;
            }
            catch (InvalidCastException e)
            {
                Log.Error("Invalid VoteIncidentDef. " + e.Message);
            }
        }

        public override void StartVote()
        {
            VoteWindow window = new VoteWindow(this);
            Find.WindowStack.Add(window);
        }

        public override void EndVote()
        {
            Ticker.FiringIncidents.Enqueue(new FiringIncident(incidents[DecideWinner()], source, parms));
            Find.WindowStack.TryRemove(typeof(VoteWindow));
            Messages.Message(new Message("Chat voted for: " + incidents[DecideWinner()].LabelCap, MessageTypeDefOf.NeutralEvent), true);
        }

        public override string VoteKeyLabel(int id)
        {
            return incidents[id].LabelCap;
        }

        Dictionary<int, IncidentDef> incidents = null;
        StorytellerComp source = null;
        IncidentParms parms = null;
    }
}
