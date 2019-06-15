using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.IncidentHelpers.Misc;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Colonists
{
    public class WildHuman : VotingHelper
    {
        public override bool IsPossible()
        {
            worker = new IncidentWorker_WildManWandersIn();
            worker.def = IncidentDef.Named("WildManWandersIn");

            parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, target);

            return worker.CanFireNow(parms);
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentWorker worker;
        private IncidentParms parms;
    }

    public class WandererJoins : VotingHelper
    {
        public override bool IsPossible()
        {
            worker = new IncidentWorker_WandererJoin();
            worker.def = IncidentDef.Named("WandererJoin");

            parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, target);

            return worker.CanFireNow(parms);
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentWorker worker;
        private IncidentParms parms;
    }

    public class GenderSwap : VotingHelper
    {
        public override bool IsPossible()
        {
            if (target is Map map)
            {
                map = target as Map;
                List<Pawn> candidates = map.mapPawns.FreeColonistsSpawned.ToList();

                if (candidates != null && candidates.Count > 0)
                {
                    pawn = candidates[0];
                    return true;
                }
            }

            return false;
        }

        public override void TryExecute()
        {
            pawn.gender = pawn.gender == Gender.Female ? Gender.Male : Gender.Female;

            string text = pawn.Name + " has switched genders and is now " + pawn.gender.ToString();

            Find.LetterStack.ReceiveLetter("GenderSwap", text, LetterDefOf.NeutralEvent, pawn);
        }

        private Map map;
        private Pawn pawn;
    }

    public class Party : VotingHelper
    {
        public override bool IsPossible()
        {
            if (target is Map map)
            {
                map = target as Map;
            }
            else
            {
                return false;
            }

            pawn = PartyUtility.FindRandomPartyOrganizer(Faction.OfPlayer, Helper.AnyPlayerMap);
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
            if (!RCellFinder.TryFindPartySpot(pawn, out IntVec3 intVec))
            {
                return;
            }
            Verse.AI.Group.LordMaker.MakeNewLord(pawn.Faction, new LordJob_Joinable_Party(intVec, pawn), Helper.AnyPlayerMap, null);
            string text = "LetterNewParty".Translate(pawn.LabelShort, pawn);

            Find.LetterStack.ReceiveLetter("LetterLabelNewParty".Translate(), text, LetterDefOf.PositiveEvent, new TargetInfo(intVec, Helper.AnyPlayerMap, false), null, null);
        }

        private Map map;
        private Pawn pawn;
    }

    public class Cannibal : VotingHelper
    {
        public override bool IsPossible()
        {
            if (target is Map map)
            {
                map = target as Map;
                List<Pawn> candidates = map.mapPawns.FreeColonistsSpawned.ToList();

                Helper.Log("finding candidates");

                traitDef = DefDatabase<TraitDef>.GetNamed("Cannibal");

                Helper.Log("finding specific candidate");

                if (candidates != null && candidates.Count > 0)
                {
                    candidates.Shuffle();
                    foreach (Pawn candidate in candidates)
                    {
                        if (pawn.story.traits.allTraits == null) continue;

                        if (pawn.story.traits.allTraits.Count <= 3)
                        {
                            if (!pawn.story.traits.allTraits.Any(s => s.def == traitDef))
                            {
                                pawn = candidate;
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public override void TryExecute()
        {
            Trait trait = new Trait();
            pawn.story.traits.GainTrait(trait);
            string text = pawn.Name + " has a sudden appetite for human flesh and now has the Cannibal Trait.";
            Find.LetterStack.ReceiveLetter("Cannibal", text, LetterDefOf.NeutralEvent, pawn);
        }

        private Pawn pawn;
        private TraitDef traitDef;
    }

    public class Luciferium : VotingHelper
    {
        public override bool IsPossible()
        {
            if (target is Map map)
            {
                map = target as Map;
                List<Pawn> candidates = map.mapPawns.FreeColonistsSpawned.ToList();

                if (candidates != null && candidates.Count > 0)
                {
                    candidates.Shuffle();

                    foreach (Pawn candidate in candidates)
                    {
                        if (!candidate.health.hediffSet.HasHediff(HediffDef.Named("LuciferiumAddiction")))
                        {
                            pawn = candidate;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public override void TryExecute()
        {
            pawn.health.AddHediff(HediffDef.Named("LuciferiumHigh"));
            pawn.health.AddHediff(HediffDef.Named("LuciferiumAddiction"));

            string text = pawn.Name + " has taken a secret red pill they had hiding away. They are now addicted to Luciferium.";
            Find.LetterStack.ReceiveLetter("Luciferium", text, LetterDefOf.NegativeEvent, pawn);
        }

        private Map map;
        private Pawn pawn;
    }

    public class IncreaseRandomSkillVote : VotingHelper
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

}
