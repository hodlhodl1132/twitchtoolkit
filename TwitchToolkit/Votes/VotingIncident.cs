using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.Votes
{
    public class VotingIncident : Def
    {
        public int weight;

        public int voteWeight = 100;

        public Storyteller storyteller;

        public EventType eventType;

        public EventCategory eventCategory;

        public Type votingHelper = typeof(IncidentHelper);

        public VotingHelper helper = null;

        public VotingHelper Helper
        {
            get
            {
                if (helper == null)
                {
                    helper = VotingIncidentMaker.makeVotingHelper(this);
                }

                return helper;
            }
        }
    }

    public abstract class VotingHelper : IncidentHelper
    {
        public IIncidentTarget target;

        public override bool IsPossible()
        {
            return true;
        }
    }

    public static class VotingIncidentMaker
    {
        public static VotingHelper makeVotingHelper(VotingIncident def)
        {
            return (VotingHelper)Activator.CreateInstance(def.votingHelper);
        }
    }

    public enum EventType
    {
        Bad = 1,
        Good = 2,
        Neutral = 4
    }

    public enum EventCategory
    {
        Animal = 8,
        Colonist = 4,
        Drop = 128,
        Enviroment = 2,
        Foreigner = 256,
        Disease = 512,
        Hazard = 32,
        Invasion = 1,
        Mind = 64,
        Weather = 16
    }

    public enum Storyteller
    {
        ToryTalker,
        SpartanBot,
        UristBot,
        HodlBot
    }
}
