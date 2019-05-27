using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace TwitchToolkit.Storytellers
{
    public class StorytellerComp_TwitchToolkit : StorytellerComp
    {
        protected StorytellerCompProperties_TwitchToolkit Props
        {
            get
            {
                return (StorytellerCompProperties_TwitchToolkit)this.props;
            }
        }

        public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
        {
            CheckIfPacksAreEnabled();

            StoryTellerVoteTracker voteTracker = Current.Game.GetComponent<StoryTellerVoteTracker>();

            // get different voting packs
            List<StorytellerPack> allPacks = DefDatabase<StorytellerPack>.AllDefs.Where(s =>
                s.enabled &&
                voteTracker.HaveMinimumDaysBetweenEventsPassed(s)
            ).ToList();

            if (allPacks == null || allPacks.Count < 1)
            {
                Log.Warning("No story teller packs found");
                yield break;
            }

            // randomize
            allPacks.Shuffle();

            StorytellerPack chosen = allPacks[0];

            // let the comp do the work
            foreach (FiringIncident incident in chosen.StorytellerComp.MakeIntervalIncidents(target))
            {
                yield return incident;
            }

            yield break;
        }

        private void CheckIfPacksAreEnabled()
        {
            List<StorytellerComp> comps = Current.Game.storyteller.storytellerComps;

            foreach (StorytellerPack pack in DefDatabase<StorytellerPack>.AllDefs)
            {
                if (pack.defName == "HodlBot")
                {
                    pack.enabled = ToolkitSettings.HodlBotEnabled;
                }
                else if (pack.defName == "ToryTalker")
                {
                    pack.enabled = ToolkitSettings.ToryTalkerEnabled;
                }
                else if (pack.defName == "UristBot")
                {
                    pack.enabled = ToolkitSettings.UristBotEnabled;
                }
            }
        }
    }
}
