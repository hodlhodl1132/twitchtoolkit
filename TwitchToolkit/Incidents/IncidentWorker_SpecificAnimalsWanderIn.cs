using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using UnityEngine;
using System.Linq;

namespace TwitchToolkit.Incidents
{
    public class IncidentWorker_SpecificAnimalsWanderIn : IncidentWorker
    {
        string Label;
        PawnKindDef PawnKindDef;
        bool JoinColony;
        int Count;
        bool Manhunter;
        bool DefaultText;

        const float MaxWildness = 0.35f;

        const float TotalBodySizeToSpawn = 2.5f;

        public IncidentWorker_SpecificAnimalsWanderIn(string label = null, PawnKindDef pawnKindDef = null, bool joinColony = false, int count = 0, bool manhunter = false, bool defaultText = false)
        {
            Label = label;
            PawnKindDef = pawnKindDef;
            JoinColony = joinColony;
            Count = count;
            Manhunter = manhunter;
            DefaultText = defaultText;
        }

        bool TryFindAnimalKind(int tile, out PawnKindDef animalKind)
        {
            return (from k in DefDatabase<PawnKindDef>.AllDefs
                    where k.RaceProps.Animal && k.RaceProps.wildness < 0.35f && Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(tile, k.race)
                    select k).TryRandomElementByWeight((PawnKindDef k) => 0.420000017f - k.RaceProps.wildness, out animalKind);
        }

        protected override bool CanFireNowSub(IncidentParms parms)
        {
            if (!base.CanFireNowSub(parms))
            {
                return false;
            }
            var map = (Map)parms.target;
            IntVec3 intVec;
            return RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null);
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            if (PawnKindDef == null && !TryFindAnimalKind(parms.target.Tile, out PawnKindDef))
            {
                return false;
            }

            var map = (Map)parms.target;
            IntVec3 intVec;
            if (!RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null))
            {
                return false;
            }
            if (Count <= 0)
            {
                Count = Mathf.Clamp(GenMath.RoundRandom(TotalBodySizeToSpawn / PawnKindDef.RaceProps.baseBodySize), 2, 10); ;
            }
            for (int i = 0; i < Count; i++)
            {
                var loc = CellFinder.RandomClosewalkCellNear(intVec, map, 12, null);
                var pawn = PawnGenerator.GeneratePawn(PawnKindDef, null);

                GenSpawn.Spawn(pawn, loc, map, Rot4.Random, WipeMode.Vanish, false);

                if (JoinColony)
                {
                    pawn.SetFaction(Faction.OfPlayer, null);
                }

                if (Manhunter)
                {
                    pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter);
                }
            }

            var text = DefaultText ? "LetterFarmAnimalsWanderIn".Translate(PawnKindDef.GetLabelPlural(-1)) : "";

            Find.LetterStack.ReceiveLetter((Label ?? "LetterLabelFarmAnimalsWanderIn").Translate(PawnKindDef.GetLabelPlural(-1)).CapitalizeFirst(), text, Manhunter ? LetterDefOf.NegativeEvent : LetterDefOf.PositiveEvent, new TargetInfo(intVec, map, false), null, null);
            return true;
        }
    }
}
