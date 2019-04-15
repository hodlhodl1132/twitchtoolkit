using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
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
            worker.def = new IncidentDef();

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
                if (worker.CanFireNow(parms))
                {
                    return true;
                }
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

    public class RefugeeChased : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_RefugeeChased();
            worker.def = IncidentDef.Named("RefugeeChased");

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
                if (worker.CanFireNow(parms))
                {
                    return true;
                }
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

    public class AnimalTame : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new RimWorld.IncidentWorker_SelfTame();
            worker.def = IncidentDef.Named("SelfTame");

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
                if (worker.CanFireNow(parms))
                {
                    return true;
                }
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

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
                if (worker.CanFireNow(parms))
                {
                    return true;
                }
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

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
                if (worker.CanFireNow(parms))
                {
                    return true;
                }
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
            worker = new RimWorld.IncidentWorker_TransportPodCrash();
            worker.def = IncidentDefOf.ShipChunkDrop;

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
                if (worker.CanFireNow(parms))
                {
                    return true;
                }
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

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
                if (worker.CanFireNow(parms))
                {
                    return true;
                }
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

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
                if (worker.CanFireNow(parms))
                {
                    return true;
                }
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
            List<Pawn> allPawns = Current.Game.AnyPlayerHomeMap.mapPawns.FreeColonistsAndPrisonersSpawned.ToList();
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
            Helper.Log("testing if possible");
            List<Pawn> pawns = Current.Game.AnyPlayerHomeMap.mapPawns.FreeColonistsSpawned.ToList();
            pawns.Shuffle();

            foreach (Pawn pawn in pawns)
            {
                Helper.Log("testing pawn " + pawn.Name);
                if (pawn.Inspired) continue;

                randomAvailableInspirationDef = (
                    from x in DefDatabase<InspirationDef>.AllDefsListForReading
                    where true
                    select x).RandomElementByWeightWithFallback((InspirationDef x) => x.Worker.CommonalityFor(pawn), null
                );

                Helper.Log("trying inspiration " + randomAvailableInspirationDef.defName);

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

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
                if (worker.CanFireNow(parms))
                {
                    return true;
                }
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

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
                if (worker.CanFireNow(parms))
                {
                    return true;
                }
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

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
                if (worker.CanFireNow(parms))
                {
                    return true;
                }
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

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
                if (worker.CanFireNow(parms))
                {
                    return true;
                }
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

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
                if (worker.CanFireNow(parms))
                {
                    return true;
                }
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

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
                if (worker.CanFireNow(parms))
                {
                    return true;
                }
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

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
                if (worker.CanFireNow(parms))
                {
                    return true;
                }
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

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
                if (worker.CanFireNow(parms))
                {
                    return true;
                }
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

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
                if (worker.CanFireNow(parms))
                {
                    return true;
                }
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

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
                if (worker.CanFireNow(parms))
                {
                    return true;
                }
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

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
                if (worker.CanFireNow(parms))
                {
                    return true;
                }
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

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
                if (worker.CanFireNow(parms))
                {
                    return true;
                }
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

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
                if (worker.CanFireNow(parms))
                {
                    return true;
                }
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
            List<Pawn> allPawns = Current.Game.AnyPlayerHomeMap.mapPawns.FreeColonistsSpawned.ToList();
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

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
                if (worker.CanFireNow(parms))
                {
                    return true;
                }
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
            Pawn pawn = PartyUtility.FindRandomPartyOrganizer(Faction.OfPlayer, Helper.AnyPlayerMap);
            if (pawn == null)
            {
                return false;
            }

            IntVec3 intVec;
            if (!RCellFinder.TryFindPartySpot(pawn, out intVec))
            {
                return false;
            }

            return true;
        }

        public override void TryExecute()
        {
            Pawn pawn = PartyUtility.FindRandomPartyOrganizer(Faction.OfPlayer, Helper.AnyPlayerMap);
            if (pawn == null)
            {
                return;
            }
            if (!RCellFinder.TryFindPartySpot(pawn, out IntVec3 intVec))
            {
                return;
            }
            Verse.AI.Group.LordMaker.MakeNewLord(pawn.Faction, new LordJob_Joinable_Party(intVec, pawn), Helper.AnyPlayerMap, null);
            string text = "LetterNewParty".Translate(pawn.LabelShort, pawn);

            Find.LetterStack.ReceiveLetter("LetterLabelNewParty".Translate(), text, LetterDefOf.PositiveEvent, new TargetInfo(intVec, Helper.AnyPlayerMap, false), null, null);
        }
    }

    public class Alphabeavers : IncidentHelper
    {
        public override bool IsPossible()
        {
            worker = new Incidents.IncidentWorker_Alphabeavers();
            worker.def = IncidentDef.Named("Alphabeavers");

            parms = new IncidentParms();

            List<Map> allMaps = Current.Game.Maps;
            foreach (Map map in allMaps)
            {
                parms.target = map;
                if (worker.CanFireNow(parms))
                {
                    return true;
                }
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
}
