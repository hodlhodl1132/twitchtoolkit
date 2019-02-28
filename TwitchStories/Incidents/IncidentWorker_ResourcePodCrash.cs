using System;
using System.Collections.Generic;
using Verse;
using RimWorld;

namespace TwitchToolkit.Incidents
{
    public class IncidentWorker_ResourcePodCrash : IncidentWorker
    {
        readonly string Quote;

        public IncidentWorker_ResourcePodCrash(string quote)
        {
            Quote = quote;
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            List<Thing> things = ThingSetMakerDefOf.ResourcePod.root.Generate();
            IntVec3 intVec = DropCellFinder.RandomDropSpot(map);
            DropPodUtility.DropThingsNear(intVec, map, things, 110, false, true, true);
            var text = "CargoPodCrash".Translate();

            if (Quote != null)
            {
                text += "\n\n";
                text += Helper.ReplacePlaceholder(Quote, item: things[0].LabelShort);
            }

            Find.LetterStack.ReceiveLetter("LetterLabelCargoPodCrash".Translate(), text, LetterDefOf.PositiveEvent, new TargetInfo(intVec, map, false), null, null);
            return true;
        }
    }
}