using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TwitchToolkit.Incidents;
using TwitchToolkit.Store;
using UnityEngine;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Misc
{
    public class Blight : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_CropBlight();
            worker.def = IncidentDef.Named("CropBlight");

            Map map = Helper.AnyPlayerMap;

            if (map != null)
            {
                parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, map);

                return worker.CanFireNow(parms);
            }

            return false;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    // Find 1.1 equivalent

    //public class RefugeeChased : IncidentHelper
    //{
    //    public override bool IsPossible()
    //    {
    //        worker = new RimWorld.IncidentWorker_RefugeeChased();
    //        worker.def = IncidentDef.Named("RefugeeChased");

    //        Map map = Helper.AnyPlayerMap;

    //        if (map != null)
    //        {
    //            parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, map);

    //            return worker.CanFireNow(parms);
    //        }

    //        return false;
    //    }

    //    public override void TryExecute()
    //    {
    //        worker.TryExecute(parms);
    //    }

    //    private IncidentParms parms = null;
    //    private IncidentWorker worker = null;
    //}

    public class AnimalTame : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_SelfTame();
            worker.def = IncidentDef.Named("SelfTame");

            Map map = Helper.AnyPlayerMap;

            if (map != null)
            {
                parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, map);

                bool canFire = worker.CanFireNow(parms);

                Helper.Log("Can fire " + canFire);

                return canFire;
            }

            return false;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class WandererJoins : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_WandererJoin();
            worker.def = IncidentDefOf.WandererJoin;

            Map map = Helper.AnyPlayerMap;

            if (map != null)
            {
                parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, map);

                return worker.CanFireNow(parms);
            }

            return false;
        }

        public override void TryExecute()
        {

            worker.TryExecute(parms);
        }

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class ResourcePodCrash : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_ResourcePodCrash();
            worker.def = IncidentDefOf.ShipChunkDrop;

            Map map = Helper.AnyPlayerMap;

            if (map != null)
            {
                parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, map);

                return worker.CanFireNow(parms);
            }

            return false;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class TransportPodCrash : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_ResourcePodCrash();
            //worker = new RimWorld.IncidentWorker_TransportPodCrash();
            worker.def = IncidentDefOf.ShipChunkDrop;

            Map map = Helper.AnyPlayerMap;

            if (map != null)
            {
                parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, map);

                return worker.CanFireNow(parms);
            }

            return false;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class TraderCaravanArrival : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_TraderCaravanArrival();
            worker.def = IncidentDefOf.TraderCaravanArrival;

            Map map = Helper.AnyPlayerMap;

            if (map != null)
            {
                parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, map);

                return worker.CanFireNow(parms);
            }

            return false;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class OrbitalTraderArrival : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_OrbitalTraderArrival();
            worker.def = IncidentDefOf.OrbitalTraderArrival;

            Map map = Helper.AnyPlayerMap;

            if (map != null)
            {
                parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, map);

                return worker.CanFireNow(parms);
            }

            return false;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class GenderSwap : IncidentHelper
    {
        public override bool IsPossible()
        {
            List<Pawn> allPawns = Helper.AnyPlayerMap.mapPawns.FreeColonistsAndPrisonersSpawned.ToList();
            if (allPawns == null || allPawns.Count < 1)
            {
                return false;
            }
            allPawns.Shuffle();
            this.pawn = allPawns[0];

            return true;
        }

        public override void TryExecute()
        {
            pawn.gender = pawn.gender == Gender.Female ? Gender.Male : Gender.Female;

            string letterText = Helper.ReplacePlaceholder("TwitchStoriesDescription54".Translate(), colonist: pawn.Name.ToString(), gender: pawn.gender.ToString());

            Current.Game.letterStack.ReceiveLetter("TwitchStoriesVote".Translate(), letterText, LetterDefOf.NeutralEvent);
        }

        private Pawn pawn = null;
    }

    public class Inspiration : IncidentHelper
    {
        public override bool IsPossible()
        {
            List<Pawn> pawns = Helper.AnyPlayerMap.mapPawns.FreeColonistsSpawned.ToList();
            pawns.Shuffle();

            foreach (Pawn pawn in pawns)
            {
                if (pawn.Inspired) continue;

                randomAvailableInspirationDef = (
                    from x in DefDatabase<InspirationDef>.AllDefsListForReading
                    where true
                    select x).RandomElementByWeightWithFallback((InspirationDef x) => x.Worker.CommonalityFor(pawn), null
                );

                if (randomAvailableInspirationDef != null)
                {
                    successfulInspiration = pawn.mindState.inspirationHandler.TryStartInspiration(randomAvailableInspirationDef);
                    if (successfulInspiration) break;
                }
            }
            return successfulInspiration;
        }

        public override void TryExecute()
        {

        }

        private bool successfulInspiration = false;
        private InspirationDef randomAvailableInspirationDef = null;
    }

    public class Meteorite : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_MeteoriteImpact();
            worker.def = IncidentDef.Named("MeteoriteImpact");

            Map map = Helper.AnyPlayerMap;

            if (map != null)
            {
                parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, map);

                return worker.CanFireNow(parms);
            }

            return false;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class ManInBlack : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_WandererJoin();
            worker.def = IncidentDef.Named("StrangerInBlackJoin");

            Map map = Helper.AnyPlayerMap;

            if (map != null)
            {
                parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, map);

                return worker.CanFireNow(parms);
            }

            return false;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class MadAnimal : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_AnimalInsanitySingle();
            worker.def = IncidentDef.Named("AnimalInsanitySingle");

            Map map = Helper.AnyPlayerMap;

            if (map != null)
            {
                parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, map);

                return worker.CanFireNow(parms);
            }

            return false;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class PsychicDrone : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_PsychicDrone();
            worker.def = IncidentDef.Named("PsychicDrone");

            Map map = Helper.AnyPlayerMap;

            if (map != null)
            {
                parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, map);

                return worker.CanFireNow(parms);
            }

            return false;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class PsychicSoothe : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_PsychicSoothe();
            worker.def = IncidentDef.Named("PsychicSoothe");

            Map map = Helper.AnyPlayerMap;

            if (map != null)
            {
                parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, map);

                return worker.CanFireNow(parms);
            }

            return false;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class AmbrosiaSprout : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_AmbrosiaSprout();
            worker.def = IncidentDef.Named("AmbrosiaSprout");

            Map map = Helper.AnyPlayerMap;

            if (map != null)
            {
                parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, map);

                return worker.CanFireNow(parms);
            }

            return false;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class HerdMigration : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_HerdMigration();
            worker.def = IncidentDef.Named("HerdMigration");

            Map map = Helper.AnyPlayerMap;

            if (map != null)
            {
                parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, map);

                return worker.CanFireNow(parms);
            }

            return false;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class FarmAnimalsWanderIn : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_FarmAnimalsWanderIn();
            worker.def = IncidentDef.Named("FarmAnimalsWanderIn");

            Map map = Helper.AnyPlayerMap;

            if (map != null)
            {
                parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, map);

                return worker.CanFireNow(parms);
            }

            return false;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class WildManWandersIn : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_WildManWandersIn();
            worker.def = IncidentDef.Named("WildManWandersIn");

            Map map = Helper.AnyPlayerMap;

            if (map != null)
            {
                parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, map);

                return worker.CanFireNow(parms);
            }

            return false;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class ThrumboPasses : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_ThrumboPasses();
            worker.def = IncidentDef.Named("ThrumboPasses");

            Map map = Helper.AnyPlayerMap;

            if (map != null)
            {
                parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, map);

                return worker.CanFireNow(parms);
            }

            return false;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class ShipChunkDrop : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_ShipChunkDrop();
            worker.def = IncidentDef.Named("ShipChunkDrop");

            Map map = Helper.AnyPlayerMap;

            if (map != null)
            {
                parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, map);

                return worker.CanFireNow(parms);
            }

            return false;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class TravelerGroup : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_TravelerGroup();
            worker.def = IncidentDef.Named("TravelerGroup");

            Map map = Helper.AnyPlayerMap;

            if (map != null)
            {
                parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, map);

                return worker.CanFireNow(parms);
            }

            return false;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class VisitorGroup : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_VisitorGroup();
            worker.def = IncidentDef.Named("VisitorGroup");

            Map map = Helper.AnyPlayerMap;

            if (map != null)
            {
                parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, map);

                return worker.CanFireNow(parms);
            }

            return false;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class IncreaseRandomSkill : IncidentHelper
    {
        public override bool IsPossible()
        {
            List<Pawn> allPawns = Helper.AnyPlayerMap.mapPawns.FreeColonistsSpawned.ToList();
            allPawns.Shuffle();

            foreach (Pawn pawn in allPawns)
            {
                List<SkillRecord> allSkills = pawn.skills.skills;
                allSkills.Shuffle();

                foreach (SkillRecord skill in allSkills)
                {
                    if (skill.TotallyDisabled) continue;

                    this.skill = skill;
                    this.pawn = pawn;

                    break;
                }
            }

            return skill != null;
        }

        public override void TryExecute()
        {
            float xpBoost = SkillRecord.XpRequiredToLevelUpFrom(skill.Level);
            skill.Learn(xpBoost, true);
            string text = Helper.ReplacePlaceholder("TwitchStoriesDescription55".Translate(), colonist: pawn.Name.ToString(), skill: skill.def.defName, first: Math.Round(xpBoost).ToString());
            Current.Game.letterStack.ReceiveLetter("TwitchToolkitIncreaseSkill".Translate(), text, LetterDefOf.PositiveEvent, pawn);
        }

        private Pawn pawn = null;
        private SkillRecord skill = null;
    }

    public class YorkshireTerriers : IncidentHelper
    {
        public override bool IsPossible()
        {
            PawnKindDef animalKind = PawnKindDef.Named("YorkshireTerrier");
            int num = AnimalsCount.RandomInRange;
            num = Mathf.Max(num, Mathf.CeilToInt(4f / animalKind.RaceProps.baseBodySize));
            worker = new IncidentWorker_SpecificAnimalsWanderIn(null, animalKind, true, num, false, true);
            worker.def = IncidentDef.Named("FarmAnimalsWanderIn");

            Map map = Helper.AnyPlayerMap;

            if (map != null)
            {
                parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, map);

                return worker.CanFireNow(parms);
            }

            return false;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
        private static readonly IntRange AnimalsCount = new IntRange(3, 5);
    }

    public class Party : IncidentHelper
    {
        public override bool IsPossible()
        {
            pawn = GatheringsUtility.FindRandomGatheringOrganizer(Faction.OfPlayer, Helper.AnyPlayerMap, GatheringDefOf.Party);
            if (pawn == null)
            {
                return false;
            }

            if (!RCellFinder.TryFindGatheringSpot(pawn, GatheringDefOf.Party,false, out intVec))
            {
                return false;
            }

            return true;
        }

        public override void TryExecute()
        {
            GatheringDef gatheringDef = GatheringDefOf.Party;
            Verse.AI.Group.LordMaker.MakeNewLord(pawn.Faction, new LordJob_Joinable_Party(intVec, pawn, gatheringDef), Helper.AnyPlayerMap, null);
            string text = $"{pawn.LabelShort} is throwing a party! Everyone who goes will gain recreation and social energy, and a lasting positive mood boost.";

            Find.LetterStack.ReceiveLetter("Party", text, LetterDefOf.PositiveEvent, new TargetInfo(intVec, Helper.AnyPlayerMap, false), null, null);
        }

        private IntVec3 intVec;
        private Pawn pawn = null;
    }

    public class Alphabeavers : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new Incidents.IncidentWorker_Alphabeavers();
            worker.def = IncidentDef.Named("Alphabeavers");

            Map map = Helper.AnyPlayerMap;

            if (map != null)
            {
                parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, map);
                parms.forced = true;

                return worker.CanFireNow(parms);
            }

            return false;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class ShipPartPoison : IncidentHelper
    {
        public override bool IsPossible()
        {
            var innterType = typeof(IncidentWorker).Assembly.GetTypes()
                    .Where(s => s.Name == "IncidentWorker_CrashedShipPart").First();

            var innerObject = Activator.CreateInstance(innterType);

            worker = innerObject as IncidentWorker;

            worker.def = IncidentDef.Named("DefoliatorShipPartCrash");

            Map map = Helper.AnyPlayerMap;

            if (map != null)
            {
                parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, map);

                return worker.CanFireNow(parms);
            }

            return false;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class ShipPartPsychic : IncidentHelper
    {
        public override bool IsPossible()
        {
            var innterType = typeof(IncidentWorker).Assembly.GetTypes()
                .Where(s => s.Name == "IncidentWorker_CrashedShipPart").First();

            var innerObject = Activator.CreateInstance(innterType);

            worker = innerObject as IncidentWorker;

            worker.def = IncidentDef.Named("PsychicEmanatorShipPartCrash");

            Map map = Helper.AnyPlayerMap;

            if (map != null)
            {
                parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, map);

                return worker.CanFireNow(parms);
            }

            return false;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentParms parms = null;
        private IncidentWorker worker = null;
    }

    public class VomitRain : IncidentHelper
    {
        public override bool IsPossible()
        {
            return true;
        }

        public override void TryExecute()
        {
            int count = 50;

            for (int i = 0; i < count; i++)
            {
                Helper.Vomit(Helper.AnyPlayerMap);
            }
        }
    }
}
