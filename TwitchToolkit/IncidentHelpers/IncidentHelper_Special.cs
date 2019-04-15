using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Incidents;
using TwitchToolkit.PawnQueue;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Special
{
    public class ReviveRandomPawn : IncidentHelper
    {
        public override bool IsPossible()
        {
            List<Pawn> allCorpses = Find.ColonistBar.GetColonistsInOrder().Where(s => s.Dead && !PawnTracker.pawnsToRevive.Contains(s)).ToList();
            if (allCorpses == null || allCorpses.Count < 1)
            {
                return false;
            }

            allCorpses.Shuffle();

            pawn = allCorpses[0];
            PawnTracker.pawnsToRevive.Add(pawn);
            

            Helper.Log("Found candidate " + pawn.Name);
            return true;
        }

        public override void TryExecute()
        {
            ResurrectionUtility.ResurrectWithSideEffects(pawn);
            PawnTracker.pawnsToRevive.Remove(pawn);
            Find.LetterStack.ReceiveLetter("Pawn Revived", $"{pawn.Name} has been revived but is experiencing some side effects.", LetterDefOf.PositiveEvent, pawn);
        }

        private Pawn pawn = null;
    }

    public class BuyPawn : IncidentHelperVariables
    {
        public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
        {
            if (!Purchase_Handler.CheckIfViewerHasEnoughCoins(viewer, this.storeIncident.cost, separateChannel)) return false;
            if(Current.Game.GetComponent<GameComponentPawns>().HasUserBeenNamed(viewer.username))
            {
                Toolkit.client.SendMessage($"@{viewer.username} you are already in the colony.", separateChannel);
                return false;
            }

            this.separateChannel = separateChannel;
            this.viewer = viewer;
            IIncidentTarget target = Helper.AnyPlayerMap;
            parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, target);
            map = (Map)parms.target;
        	
            bool cell = CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c) && !c.Fogged(map), map, CellFinder.EdgeRoadChance_Neutral, out loc);
            if (!cell) return false;
			return true;
        }

        public override void TryExecute()
        {
            PawnKindDef pawnKind = PawnKindDefOf.Colonist;
			Faction ofPlayer = Faction.OfPlayer;
			bool pawnMustBeCapableOfViolence = true;
			PawnGenerationRequest request = new PawnGenerationRequest(pawnKind, ofPlayer, PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, pawnMustBeCapableOfViolence, 20f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
			Pawn pawn = PawnGenerator.GeneratePawn(request);
            NameTriple old = pawn.Name as NameTriple;
            pawn.Name = new NameTriple(old.First, viewer.username, old.Last);
			GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
			string label = "Viewer Joins";
			string text = $"A new pawn has been purchased by {viewer.username}, let's welcome them to the colony.";
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref label, pawn);
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.PositiveEvent, pawn, null, null);

            Current.Game.GetComponent<GameComponentPawns>().AssignUserToPawn(viewer.username, pawn);
            viewer.TakeViewerCoins(this.storeIncident.cost);
            viewer.CalculateNewKarma(this.storeIncident.karmaType, storeIncident.cost);

            VariablesHelpers.SendPurchaseMessage($"@{viewer.username} has purchased a pawn and is joining the colony.", separateChannel);
        }

        private IntVec3 loc;
        private Map map = null;
        private IncidentParms parms = null;
        private bool separateChannel = false;

        public override Viewer viewer { get; set; }
    }

    public class SpawnAnimal : IncidentHelperVariables
    {
        public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
        {
            this.separateChannel = separateChannel;
            this.viewer = viewer;
            string[] command = message.Split(' ');
            if (command.Length < 4)
            {
                Toolkit.client.SendMessage($"@{viewer.username} syntax is {this.storeIncident.syntax}", separateChannel);
                return false;
            }

            if (!VariablesHelpers.PointsWagerIsValid(
                    command[3],
                    viewer,
                    ref pointsWager,
                    ref storeIncident,
                    separateChannel
                ))
            {
                return false;
            }

            string animalKind = command[2].ToLower();

            
            List<PawnKindDef> allAnimals = DefDatabase<PawnKindDef>.AllDefs.Where(
                    s => s.RaceProps.Animal &&
                    string.Join("", s.defName.Split(' ') ).ToLower() == animalKind
                ).ToList();

            if (allAnimals.Count < 1)
            {
                Toolkit.client.SendMessage($"@{viewer.username} no animal {animalKind} found.", separateChannel);
                return false;
            }

            target = Current.Game.AnyPlayerHomeMap;
            if (target == null)
            {
                return false;
            }

            float points = IncidentHelper_PointsHelper.RollProportionalGamePoints(storeIncident, pointsWager, StorytellerUtility.DefaultThreatPointsNow(target));
            pawnKind = allAnimals[0];
            int num = ManhunterPackIncidentUtility.GetAnimalsCount(pawnKind, points);
            worker = new IncidentWorker_SpecificAnimalsWanderIn(null, pawnKind, true, num, false, true);

            worker.def = IncidentDef.Named("FarmAnimalsWanderIn");

            float defaultThreatPoints = StorytellerUtility.DefaultSiteThreatPointsNow();
            parms = StorytellerUtility.DefaultParmsNow(worker.def.category, target);
            parms.points = points;
            return worker.CanFireNow(parms);
        }

        public override void TryExecute()
        {
            if (worker.TryExecute(parms))
            {
                viewer.TakeViewerCoins(pointsWager);
                viewer.CalculateNewKarma(this.storeIncident.karmaType, pointsWager);
                VariablesHelpers.SendPurchaseMessage($"Spawning animal {pawnKind.LabelCap} with {pointsWager} coins wagered and {(int)parms.points} animal points purchased by {viewer.username}", separateChannel);
                return;
            }
            Toolkit.client.SendMessage($"@{viewer.username} not enough points spent for diseases.", separateChannel);
        }

        private int pointsWager = 0;
        private IncidentWorker worker = null;
        private IncidentParms parms = null;
        private IIncidentTarget target = null;
        private bool separateChannel = false;
        private PawnKindDef pawnKind = null;

        public override Viewer viewer { get; set; }
    }

    public class LevelPawn : IncidentHelperVariables
    {
        public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
        {
            this.separateChannel = separateChannel;
            this.viewer = viewer;
            string[] command = message.Split(' ');
            if (command.Length < 4)
            {
                Toolkit.client.SendMessage($"@{viewer.username} syntax is {this.storeIncident.syntax}", separateChannel);
                return false;
            }

            GameComponentPawns gameComponent = Current.Game.GetComponent<GameComponentPawns>();

            if (!gameComponent.HasUserBeenNamed(viewer.username))
            {
                Toolkit.client.SendMessage($"@{viewer.username} you must be in the colony to use this command.", separateChannel);
                return false;
            }

            if (!VariablesHelpers.PointsWagerIsValid(
                    command[3],
                    viewer,
                    ref pointsWager,
                    ref storeIncident,
                    separateChannel
                ))
            {
                return false;
            }

            string skillKind = command[2].ToLower();
            List<SkillDef> allSkills = DefDatabase<SkillDef>.AllDefs.Where(s =>
                string.Join("", s.defName.Split(' ') ).ToLower() == skillKind ||
                string.Join("", s.label.Split(' ')).ToLower() == skillKind
            ).ToList();

            if (allSkills.Count < 1)
            {
                Toolkit.client.SendMessage($"@{viewer.username} skill {skillKind} not found.", separateChannel);
                return false;
            }

            skill = allSkills[0];
            pawn = gameComponent.PawnAssignedToUser(viewer.username);

            if (pawn.skills.GetSkill(skill).TotallyDisabled)
            {
                Toolkit.client.SendMessage($"@{viewer.username} skill {skillKind} disabled on your pawn.", separateChannel);
                return false;
            }

            if (pawn.skills.GetSkill(skill).levelInt >= 20)
            {
                Toolkit.client.SendMessage($"@{viewer.username} skill {skillKind} disabled on your pawn.", separateChannel);
                return false;
            }

            return true;
        }

        public override void TryExecute()
        {
            System.Random rand = new System.Random();
            float xpWon = pawn.skills.GetSkill(skill).XpRequiredForLevelUp * ((float)rand.Next(50, 150) / 100f);
            pawn.skills.Learn(skill, xpWon, true);
            viewer.TakeViewerCoins(pointsWager);
            viewer.CalculateNewKarma(this.storeIncident.karmaType, pointsWager);
            VariablesHelpers.SendPurchaseMessage($"Increasing skill for {pawn.LabelCap} with {pointsWager} coins wagered and {(int)xpWon} xp for {skill.defName} purchased by {viewer.username}", separateChannel);
            string text = Helper.ReplacePlaceholder("TwitchStoriesDescription55".Translate(), colonist: pawn.Name.ToString(), skill: skill.defName, first: Math.Round(xpWon).ToString());
            Current.Game.letterStack.ReceiveLetter("TwitchToolkitIncreaseSkill".Translate(), text, LetterDefOf.PositiveEvent, pawn);
        }

        private int pointsWager = 0;
        private bool separateChannel = false;
        private Pawn pawn = null;
        private SkillDef skill = null;

        public override Viewer viewer { get; set; }
    }

    public class AddTrait : IncidentHelperVariables
    {
        public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
        {
            this.separateChannel = separateChannel;
            this.viewer = viewer;
            string[] command = message.Split(' ');
            if (command.Length < 3)
            {
                Toolkit.client.SendMessage($"@{viewer.username} syntax is {this.storeIncident.syntax}", separateChannel);
                return false;
            }

            GameComponentPawns gameComponent = Current.Game.GetComponent<GameComponentPawns>();

            if (!gameComponent.HasUserBeenNamed(viewer.username))
            {
                Toolkit.client.SendMessage($"@{viewer.username} you must be in the colony to use this command.", separateChannel);
                return false;
            }

            pawn = gameComponent.PawnAssignedToUser(viewer.username);

            if (pawn.story.traits.allTraits != null && pawn.story.traits.allTraits.Count >= 4)
            {
                Toolkit.client.SendMessage($"@{viewer.username} your pawn already has max 4 traits.", separateChannel);
                return false;
            }

            string traitKind = command[2].ToLower();

            List<TraitDef> allTraits = DefDatabase<TraitDef>.AllDefs.Where(s =>
                string.Join("", s.defName.Split(' ')).ToLower() == traitKind
            ).ToList();


            if (allTraits.Count < 1)
            {
                Toolkit.client.SendMessage($"@{viewer.username} skill {traitKind} not found.", separateChannel);
                return false;
            }

            traitDef = allTraits[0];

            if (!pawn.story.traits.allTraits.Any((Trait tr) => 
                traitDef.ConflictsWith(tr)) && 
                (traitDef.conflictingTraits == null || 
                !traitDef.conflictingTraits.Any((TraitDef tr) => pawn.story.traits.HasTrait(tr))))
            {
                return true;
            }

            foreach (Trait tr in pawn.story.traits.allTraits)
            {
                if (tr.def.ConflictsWith(trait) || traitDef.ConflictsWith(tr))
                {
                    Toolkit.client.SendMessage($"@{viewer.username} {traitDef.defName} conflicts with your pawn's trait {tr.LabelCap}.");
                    return false;
                }
            }

            return true;
        }

        public override void TryExecute()
        {
            trait = new Trait(traitDef, PawnGenerator.RandomTraitDegree(traitDef));
            pawn.story.traits.GainTrait(trait);
            viewer.TakeViewerCoins(storeIncident.cost);
            viewer.CalculateNewKarma(storeIncident.karmaType, storeIncident.cost);
            VariablesHelpers.SendPurchaseMessage($"{viewer.username} has purchased " + trait.LabelCap + " for " + pawn.Name + ".", separateChannel);
            string text = $"@{viewer.username} just added the trait " + traitDef.LabelCap + " to " + pawn.Name + ".";
            Current.Game.letterStack.ReceiveLetter("Trait", text, LetterDefOf.PositiveEvent, pawn);
        }

        public override Viewer viewer { get; set; }

        private bool separateChannel = false;
        private Pawn pawn = null;
        private TraitDef traitDef = null;
        private Trait trait = null;
    }

    public class ChangeGender : IncidentHelperVariables
    {
        public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
        {
            this.separateChannel = separateChannel;
            this.viewer = viewer;

            GameComponentPawns gameComponent = Current.Game.GetComponent<GameComponentPawns>();

            if (!gameComponent.HasUserBeenNamed(viewer.username))
            {
                Toolkit.client.SendMessage($"@{viewer.username} you must be in the colony to use this command.", separateChannel);
                return false;
            }

            pawn = gameComponent.PawnAssignedToUser(viewer.username);

            Log.Warning("changing gneder");
            return true;
        }

        public override void TryExecute()
        {
            if (pawn.gender == Gender.Female)
            {
                pawn.gender = Gender.Male;

            }
            else
            {
                pawn.gender = Gender.Female;
            }

            pawn.story.hairColor = PawnHairColors.RandomHairColor(pawn.story.SkinColor, pawn.ageTracker.AgeBiologicalYears);
            pawn.story.hairDef = PawnHairChooser.RandomHairDefFor(pawn, FactionDefOf.PlayerColony);

            if (pawn.story.adulthood != null)
            {
                pawn.story.bodyType = pawn.story.adulthood.BodyTypeFor(pawn.gender);
            }
            else if (Rand.Value < 0.5f)
            {
                pawn.story.bodyType = BodyTypeDefOf.Thin;
            }
            else
            {
                pawn.story.bodyType = ((pawn.gender != Gender.Female) ? BodyTypeDefOf.Male : BodyTypeDefOf.Female);
            }

            viewer.TakeViewerCoins(storeIncident.cost);
            viewer.CalculateNewKarma(storeIncident.karmaType, storeIncident.cost);
            VariablesHelpers.SendPurchaseMessage($"@{viewer.username} has just swapped genders to " + pawn.gender.GetLabel() + ".", separateChannel);
            string text = $"{viewer.username} has just swapped genders to " + pawn.gender.GetLabel() + ".";
            Current.Game.letterStack.ReceiveLetter("GenderSwap", text, LetterDefOf.PositiveEvent, pawn);
        }

        public override Viewer viewer { get; set; }

        private bool separateChannel = false;
        private Pawn pawn = null;
    }

    public class Item : IncidentHelperVariables
    {
        public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
        {
            this.separateChannel = separateChannel;
            this.viewer = viewer;
            string[] command = message.Split(' ');
            if (command.Length < 4)
            {
                VariablesHelpers.ViewerDidWrongSyntax(viewer.username, storeIncident.syntax, separateChannel);
                return false;
            }

            string itemKey = command[2].ToLower();

            if (itemKey == null || itemKey == "")
            {
                VariablesHelpers.ViewerDidWrongSyntax(viewer.username, storeIncident.syntax, separateChannel);
                return false;
            }

            IEnumerable<Store.Item> itemSearch = StoreInventory.items.Where(s =>
                        s.price > 0 &&
                        (s.abr == itemKey ||
                        s.defname.ToLower() == itemKey)
                    );
           
            if (itemSearch.Count() > 0)
            {
                item = itemSearch.ElementAt(0);
            }

            if (item == null || item.price < 1)
            {
                Toolkit.client.SendMessage($"@{viewer.username} item not found.", separateChannel);
                return false;
            }
            
            string quantityKey = command[3];

            if (quantityKey == null || quantityKey == "")
            {
                VariablesHelpers.ViewerDidWrongSyntax(viewer.username, storeIncident.syntax, separateChannel);
                return false;
            }

            try
            {
                if (!int.TryParse(quantityKey, out checked(quantity)))
                {
                    return false;
                }

                price = checked( item.price * quantity );
            }
            catch (OverflowException e)
            {
                Log.Warning(e.Message);
                return false;
            }

            if (quantity < 1 || price < 1)
            {
                VariablesHelpers.ViewerDidWrongSyntax(viewer.username, storeIncident.syntax, separateChannel);
                return false;
            }

            if (!Purchase_Handler.CheckIfViewerHasEnoughCoins(viewer, price, separateChannel)) return false;

            if (price < ToolkitSettings.MinimumPurchasePrice)
            {
                Toolkit.client.SendMessage(Helper.ReplacePlaceholder(
                    "TwitchToolkitMinPurchaseNotMet".Translate(), 
                    viewer: viewer.username, 
                    amount: price.ToString(), 
                    first: ToolkitSettings.MinimumPurchasePrice.ToString()
                ), separateChannel);
                return false;
            }

            return true;
        }

        public override void TryExecute()
        {
            ThingDef itemDef = ThingDef.Named("DropPodIncoming");
            Thing itemThing = new Thing();

            ThingDef stuff = null;
            ThingDef itemThingDef = ThingDef.Named(item.defname);

            if (itemThingDef.MadeFromStuff)
			{
				if (!(from x in GenStuff.AllowedStuffsFor(itemThingDef, TechLevel.Undefined)
				where !PawnWeaponGenerator.IsDerpWeapon(itemThingDef, x)
				select x).TryRandomElementByWeight((ThingDef x) => x.stuffProps.commonality, out stuff))
				{
					stuff = GenStuff.RandomStuffByCommonalityFor(itemThingDef, TechLevel.Undefined);
				}
			}

            itemThing = ThingMaker.MakeThing(itemThingDef, (stuff != null) ? stuff : null);

            QualityCategory q = new QualityCategory();

            if (itemThing.TryGetQuality(out q))
            {
                ItemHelper.setItemQualityRandom(itemThing);
            }

            IntVec3 vec;

            if (itemThingDef.Minifiable)
            {
                itemThingDef = itemThingDef.minifiedDef;
                MinifiedThing minifiedThing = (MinifiedThing)ThingMaker.MakeThing(itemThingDef, null);
			    minifiedThing.InnerThing = itemThing;
                minifiedThing.stackCount = quantity;
                vec = Helper.Rain(itemDef, minifiedThing);
            }
            else
            {
                itemThing.stackCount = quantity;
                vec = Helper.Rain(itemDef, itemThing);
            }

            string letter = Helper.ReplacePlaceholder("TwitchStoriesDescription80".Translate(), from: viewer.username, amount: quantity.ToString(), item: item.abr.CapitalizeFirst());

            Helper.CarePackage(letter, LetterDefOf.PositiveEvent, vec);

            viewer.TakeViewerCoins(price);
            viewer.CalculateNewKarma(storeIncident.karmaType, price);
            VariablesHelpers.SendPurchaseMessage(Helper.ReplacePlaceholder("TwitchToolkitItemPurchaseConfirm".Translate(), amount: quantity.ToString(), item: item.abr, viewer: viewer.username), separateChannel);
        }

        private int price = 0;
        private int quantity = 0;
        private Store.Item item = null;
        private bool separateChannel;

        public override Viewer viewer { get; set; }
    }

    public static class PawnTracker
    {
        public static List<Pawn> pawnsToRevive = new List<Pawn>();
    }

    public static class ItemHelper
    {
        public static void setItemQualityRandom(Thing thing)
        {
            QualityCategory qual = QualityUtility.GenerateQualityTraderItem();
            thing.TryGetComp<CompQuality>().SetQuality(qual, ArtGenerationContext.Outsider);
        }
    }

    
}
