using System;
using System.Collections.Generic;
using Verse;
using RimWorld;

namespace TwitchToolkit.Incidents
{
    public class IncidentWorker_ResourcePodFrenzy : IncidentWorker
    {
        readonly string Quote;

        public IncidentWorker_ResourcePodFrenzy(string quote)
        {
            Quote = quote;
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            for(int x = 0; x < 10; x++)
            {
                List<Thing> things = ThingSetMakerDefOf.ResourcePod.root.Generate();
                IntVec3 intVec = DropCellFinder.RandomDropSpot(map);
                DropPodUtility.DropThingsNear(intVec, map, things, 110, false, true, true);
            }
            
            var text = "TwitchToolkitCargoPodFrenzyInc".Translate();

            if (Quote != null)
            {
                text += "\n\n";
                text += Helper.ReplacePlaceholder(Quote);
            }

            Find.LetterStack.ReceiveLetter("TwitchToolkitCargoPodFrenzyInc".Translate(), text, LetterDefOf.PositiveEvent, null, null, null);
            return true;
        }
    }
}