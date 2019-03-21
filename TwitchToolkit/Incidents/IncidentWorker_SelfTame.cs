using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace TwitchToolkit.Incidents
{
    public class IncidentWorker_SelfTame : IncidentWorker
    {
        readonly string Quote;

        public IncidentWorker_SelfTame(string quote)
        {
            Quote = quote;
        }

        private IEnumerable<Pawn> Candidates(Map map)
        {
            return from x in map.mapPawns.AllPawnsSpawned
                   where x.RaceProps.Animal && x.Faction == null && !x.Position.Fogged(x.Map) && !x.InMentalState && !x.Downed && x.RaceProps.wildness > 0f
                   select x;
        }


		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
            var cand = this.Candidates(map);
            Helper.Log("Candidates " + cand.Count());
            return cand.Any<Pawn>();
		}

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            Pawn pawn = null;
            if (!this.Candidates(map).TryRandomElement(out pawn))
            {
                return false;
            }
            if (pawn.guest != null)
            {
                pawn.guest.SetGuestStatus(null, false);
            }
            string value = pawn.LabelIndefinite();
            bool flag = pawn.Name != null;
            pawn.SetFaction(Faction.OfPlayer, null);
            string text;
            if (!flag && pawn.Name != null)
            {
                if (pawn.Name.Numerical)
                {
                    text = "LetterAnimalSelfTameAndNameNumerical".Translate(value, pawn.Name.ToStringFull, pawn.Named("ANIMAL")).CapitalizeFirst();
                }
                else
                {
                    text = "LetterAnimalSelfTameAndName".Translate(value, pawn.Name.ToStringFull, pawn.Named("ANIMAL")).CapitalizeFirst();
                }
            }
            else
            {
                text = "LetterAnimalSelfTame".Translate(pawn).CapitalizeFirst();
            }

            if (Quote != null)
            {
                text += "\n\n";
                text += Quote;
            }

            Find.LetterStack.ReceiveLetter("LetterLabelAnimalSelfTame".Translate(pawn.KindLabel, pawn).CapitalizeFirst(), text, LetterDefOf.PositiveEvent, pawn, null, null);
            return true;
        }

        private bool TryFindStartAndEndCells(Map map, out IntVec3 start, out IntVec3 end)
        {
            if (!RCellFinder.TryFindRandomPawnEntryCell(out start, map, CellFinder.EdgeRoadChance_Animal, false, null))
            {
                end = IntVec3.Invalid;
                return false;
            }
            end = IntVec3.Invalid;
            for (int i = 0; i < 8; i++)
            {
                IntVec3 startLocal = start;
                IntVec3 intVec;
                if (!CellFinder.TryFindRandomEdgeCellWith((IntVec3 x) => map.reachability.CanReach(startLocal, x, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.Deadly), map, CellFinder.EdgeRoadChance_Ignore, out intVec))
                {
                    break;
                }
                if (!end.IsValid || intVec.DistanceToSquared(start) > end.DistanceToSquared(start))
                {
                    end = intVec;
                }
            }
            return end.IsValid;
        }
    }
}