using System;
using System.Collections.Generic;
using Verse;
using RimWorld;

namespace TwitchToolkit.Incidents
{
    [HasDebugOutput]
    public class IncidentWorker_ManhunterPack : IncidentWorker
    {
        readonly string Quote;

        public IncidentWorker_ManhunterPack(string quote)
        {
            Quote = quote;
        }

        private const float PointsFactor = 1f;

        private const int AnimalsStayDurationMin = 60000;

        private const int AnimalsStayDurationMax = 120000;

        protected override bool CanFireNowSub(IncidentParms parms)
        {
            if (!base.CanFireNowSub(parms))
            {
                return false;
            }
            Map map = (Map)parms.target;
            PawnKindDef pawnKindDef;
            IntVec3 intVec;
            return ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(parms.points, map.Tile, out pawnKindDef) && RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null);
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            PawnKindDef pawnKindDef;
            if (!ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(parms.points, map.Tile, out pawnKindDef))
            {
                return false;
            }
            IntVec3 intVec;
            if (!RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null))
            {
                return false;
            }
            List<Pawn> list = ManhunterPackIncidentUtility.GenerateAnimals(pawnKindDef, map.Tile, parms.points * 1f);
            Rot4 rot = Rot4.FromAngleFlat((map.Center - intVec).AngleFlat);
            for (int i = 0; i < list.Count; i++)
            {
                Pawn pawn = list[i];
                IntVec3 loc = CellFinder.RandomClosewalkCellNear(intVec, map, 10, null);
                GenSpawn.Spawn(pawn, loc, map, rot, WipeMode.Vanish, false);
                pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, false, false, null, false);
                pawn.mindState.exitMapAfterTick = Find.TickManager.TicksGame + Rand.Range(60000, 120000);
            }
            var text = "ManhunterPackArrived".Translate(pawnKindDef.GetLabelPlural(-1));

            if (Quote != null)
            {
                text += "\n\n";
                text += Helper.ReplacePlaceholder(Quote, animal: pawnKindDef.GetLabelPlural(-1));
            }

            Find.LetterStack.ReceiveLetter("LetterLabelManhunterPackArrived".Translate(), text, LetterDefOf.ThreatBig, list[0], null, null);
            Find.TickManager.slower.SignalForceNormalSpeedShort();
            LessonAutoActivator.TeachOpportunity(ConceptDefOf.ForbiddingDoors, OpportunityType.Critical);
            LessonAutoActivator.TeachOpportunity(ConceptDefOf.AllowedAreas, OpportunityType.Important);
            return true;
        }
    }
}