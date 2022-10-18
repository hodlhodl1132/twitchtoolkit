using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace TwitchToolkit.Incidents
{
    public class IncidentWorker_PrisonerJoins : IncidentWorker_WandererJoin
    {
        public IncidentWorker_PrisonerJoins(Viewer viewer)
        {
            this.viewer = viewer;
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            IntVec3 loc;
            if (!this.TryFindEntryCell(map, out loc))
            {
                return false;
            }
            Gender? gender = null;
            if (this.def.pawnFixedGender != Gender.None)
            {
                gender = new Gender?(this.def.pawnFixedGender);
            }
            PawnKindDef pawnKind = PawnKindDefOf.Slave;
            Faction ofAncients = Faction.OfAncients;
            bool pawnMustBeCapableOfViolence = this.def.pawnMustBeCapableOfViolence;
            Gender? fixedGender = gender;
            PawnGenerationRequest request = new PawnGenerationRequest(
                kind: pawnKind,
                faction: ofAncients,
                context: PawnGenerationContext.NonPlayer,
                tile: map.Tile,
                forceGenerateNewPawn: false,
                allowDead: false,
                allowDowned: false,
                canGeneratePawnRelations: true,
                mustBeCapableOfViolence: false,
                colonistRelationChanceFactor: 1f,
                forceAddFreeWarmLayerIfNeeded: false,
                allowGay: true,
                allowPregnant: true,
                allowFood: true,
                allowAddictions: false,
                inhabitant: false,
                certainlyBeenInCryptosleep: false,
                forceRedressWorldPawnIfFormerColonist: false,
                worldPawnFactionDoesntMatter: false,
                biocodeWeaponChance: 0f,
                biocodeApparelChance: 0f,
                extraPawnForExtraRelationChance: null,
                relationWithExtraPawnChanceFactor: 1f,
                validatorPostGear: null,
                validatorPreGear: null,
                forcedTraits: null,
                prohibitedTraits: null,
                minChanceToRedressWorldPawn: null,
                fixedBiologicalAge: null,
                fixedGender: gender,
                fixedLastName: null,
                fixedBirthName: null,
                fixedTitle: null,
                fixedIdeo: null,
                forceNoIdeo: false,
                forceNoBackstory: false,
                forbidAnyTitle: false,
                forceDead: false,
                forcedXenogenes: null,
                forcedEndogenes: null,
                forcedXenotype: null,
                forcedCustomXenotype: null,
                allowedXenotypes: null,
                forceBaselinerChance: 0,
                developmentalStages: DevelopmentalStage.Adult,
                pawnKindDefGetter: null,
                excludeBiologicalAgeRange: null,
                biologicalAgeRange: null,
                forceRecruitable: false); ;
            List<Pawn> prisoners = new List<Pawn>();
            Pawn pawn = PawnGenerator.GeneratePawn(request);
            NameTriple oldName = pawn.Name as NameTriple;
            NameTriple newName = new NameTriple(oldName.First, viewer.username.CapitalizeFirst(), oldName.Last);
            pawn.Name = newName;
            pawn.guest.SetGuestStatus(Faction.OfPlayer);
            prisoners.Add(pawn);
            parms.raidArrivalMode = PawnsArrivalModeDefOf.CenterDrop;
            if (!parms.raidArrivalMode.Worker.TryResolveRaidSpawnCenter(parms))
            {
                return false;
            }
            parms.raidArrivalMode.Worker.Arrive(prisoners, parms);
            //GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
            TaggedString text = $"A prisoner named {viewer.username.CapitalizeFirst()} has escaped from maximum security space prison. Will you capture or let them go?";
            TaggedString label = "Prisoner: " + viewer.username.CapitalizeFirst();
            PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref label, pawn);
            Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, pawn, null, null);
            return true;
        }

        private bool TryFindEntryCell(Map map, out IntVec3 cell)
        {
            return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c) && !c.Fogged(map), map, CellFinder.EdgeRoadChance_Neutral, out cell);
        }

        private Viewer viewer;
    }
}
