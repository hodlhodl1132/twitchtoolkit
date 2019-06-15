using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.IncidentHelpers.IncidentHelper_Settings;
using TwitchToolkit.Incidents;
using TwitchToolkit.PawnQueue;
using TwitchToolkit.Settings;
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

            return true;
        }

        public override void TryExecute()
        {
            try
            {
                pawn.ClearAllReservations();
                ResurrectionUtility.ResurrectWithSideEffects(pawn);
                PawnTracker.pawnsToRevive.Remove(pawn);
                Find.LetterStack.ReceiveLetter("Pawn Revived", $"{pawn.Name} has been revived but is experiencing some side effects.", LetterDefOf.PositiveEvent, pawn);
            }
            catch (Exception e)
            {
                Log.Error("Submit this bug to TwitchToolkit Discord: " + e.Message);
            }
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
            this.Viewer = viewer;
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
            pawn.Name = new NameTriple(old.First, Viewer.username, old.Last);
			GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
			string label = "Viewer Joins";
			string text = $"A new pawn has been purchased by {Viewer.username}, let's welcome them to the colony.";
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref label, pawn);
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.PositiveEvent, pawn, null, null);

            Current.Game.GetComponent<GameComponentPawns>().AssignUserToPawn(Viewer.username, pawn);
            Viewer.TakeViewerCoins(this.storeIncident.cost);
            Viewer.CalculateNewKarma(this.storeIncident.karmaType, storeIncident.cost);

            VariablesHelpers.SendPurchaseMessage($"@{Viewer.username} has purchased a pawn and is joining the colony.", separateChannel);
        }

        private IntVec3 loc;
        private Map map = null;
        private IncidentParms parms = null;
        private bool separateChannel = false;

        public override Viewer Viewer { get; set; }
    }

    public class SpawnAnimal : IncidentHelperVariables
    {
        public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
        {
            this.separateChannel = separateChannel;
            this.Viewer = viewer;
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

            target = Helper.AnyPlayerMap;
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
                Viewer.TakeViewerCoins(pointsWager);
                Viewer.CalculateNewKarma(this.storeIncident.karmaType, pointsWager);
                VariablesHelpers.SendPurchaseMessage($"Spawning animal {pawnKind.LabelCap} with {pointsWager} coins wagered and {(int)parms.points} animal points purchased by {Viewer.username}", separateChannel);
                return;
            }
            Toolkit.client.SendMessage($"@{Viewer.username} not enough points spent for diseases.", separateChannel);
        }

        private int pointsWager = 0;
        private IncidentWorker worker = null;
        private IncidentParms parms = null;
        private IIncidentTarget target = null;
        private bool separateChannel = false;
        private PawnKindDef pawnKind = null;

        public override Viewer Viewer { get; set; }
    }

    public class LevelPawn : IncidentHelperVariables
    {
        public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
        {
            this.separateChannel = separateChannel;
            this.Viewer = viewer;
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
            float customMultiplier = LevelPawnSettings.xpMultiplier > 0 ? LevelPawnSettings.xpMultiplier : 0.5f;
            float xpWon = pawn.skills.GetSkill(skill).XpRequiredForLevelUp * customMultiplier * ((float)Verse.Rand.Range(0.5f, 1.5f));
            xpWon = IncidentHelper_PointsHelper.RollProportionalGamePoints(storeIncident, pointsWager, xpWon);

            pawn.skills.Learn(skill, xpWon, true);
            Viewer.TakeViewerCoins(pointsWager);
            Viewer.CalculateNewKarma(this.storeIncident.karmaType, pointsWager);

            SkillRecord record = pawn.skills.GetSkill(skill);
            string increaseText = $" Level {record.levelInt}: {(int)record.xpSinceLastLevel} / {(int)record.XpRequiredForLevelUp}.";

            float percent = 35;
            string passionPlus = "";
            Passion passion = record.passion;
            if (passion == Passion.Minor)
            {
                percent = 100;
                passionPlus = "+";
            }
            if (passion == Passion.Major)
            {
                percent = 150;
                passionPlus = "++";
            }
            
            VariablesHelpers.SendPurchaseMessage($"Increasing skill {skill.LabelCap} for {pawn.LabelCap} with {pointsWager} coins wagered and ({(int)xpWon} * {percent}%){passionPlus} {(int)xpWon * (percent / 100f)} xp purchased by {Viewer.username}. {increaseText}", separateChannel);
            string text = Helper.ReplacePlaceholder("TwitchStoriesDescription55".Translate(), colonist: pawn.Name.ToString(), skill: skill.defName, first: Math.Round(xpWon).ToString());
            Current.Game.letterStack.ReceiveLetter("TwitchToolkitIncreaseSkill".Translate(), text, LetterDefOf.PositiveEvent, pawn);
        }

        private int pointsWager = 0;
        private bool separateChannel = false;
        private Pawn pawn = null;
        private SkillDef skill = null;

        public override Viewer Viewer { get; set; }
    }

    public class ChangeGender : IncidentHelperVariables
    {
        public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
        {
            this.separateChannel = separateChannel;
            this.Viewer = viewer;

            GameComponentPawns gameComponent = Current.Game.GetComponent<GameComponentPawns>();

            if (!gameComponent.HasUserBeenNamed(viewer.username))
            {
                Toolkit.client.SendMessage($"@{viewer.username} you must be in the colony to use this command.", separateChannel);
                return false;
            }

            pawn = gameComponent.PawnAssignedToUser(viewer.username);

            Helper.Log("changing gneder");
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

            Viewer.TakeViewerCoins(storeIncident.cost);
            Viewer.CalculateNewKarma(storeIncident.karmaType, storeIncident.cost);
            VariablesHelpers.SendPurchaseMessage($"@{Viewer.username} has just swapped genders to " + pawn.gender.GetLabel() + ".", separateChannel);
            string text = $"{Viewer.username} has just swapped genders to " + pawn.gender.GetLabel() + ".";
            Current.Game.letterStack.ReceiveLetter("GenderSwap", text, LetterDefOf.PositiveEvent, pawn);
        }

        public override Viewer Viewer { get; set; }

        private bool separateChannel = false;
        private Pawn pawn = null;
    }

    public class Item : IncidentHelperVariables
    {
        public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
        {
            this.separateChannel = separateChannel;
            this.Viewer = viewer;
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

            ThingDef itemThingDef = ThingDef.Named(item.defname);

            bool isResearched = true;
            ResearchProjectDef researchProject = null;

            Helper.Log("Checking researched");
            if (itemThingDef.recipeMaker != null &&
                itemThingDef.recipeMaker.researchPrerequisite != null &&
                !itemThingDef.recipeMaker.researchPrerequisite.IsFinished)
            {
                Helper.Log("Recipe not researched");
                isResearched = false;
                researchProject = itemThingDef.recipeMaker.researchPrerequisite;
            }
            else if (!itemThingDef.IsResearchFinished)
            {
                Helper.Log("Building not researched");
                isResearched = false;
                researchProject = itemThingDef.researchPrerequisites.ElementAt(0);
            }

            if (BuyItemSettings.mustResearchFirst && !isResearched)
            {
                string output = $"@{viewer.username} {itemThingDef.LabelCap} has not been researched yet, must finish research project {researchProject.LabelCap} first.";

                Toolkit.client.SendMessage(output, separateChannel);

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
                Helper.Log(e.Message);
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

            if (itemThingDef.race != null)
            {
                TryExecuteAnimal(itemThingDef, quantity);
                return;
            }

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

            Map map = Helper.AnyPlayerMap;
            IntVec3 vec = DropCellFinder.TradeDropSpot(map);
            

            if (itemThingDef.Minifiable)
            {
                itemThingDef = itemThingDef.minifiedDef;
                MinifiedThing minifiedThing = (MinifiedThing)ThingMaker.MakeThing(itemThingDef, null);
			    minifiedThing.InnerThing = itemThing;
                minifiedThing.stackCount = quantity;
                TradeUtility.SpawnDropPod(vec, map, minifiedThing);
            }
            else
            {
                itemThing.stackCount = quantity;
                TradeUtility.SpawnDropPod(vec, map, itemThing);
            }

            string letter = Helper.ReplacePlaceholder("TwitchStoriesDescription80".Translate(), from: Viewer.username, amount: quantity.ToString(), item: item.abr.CapitalizeFirst());

            LetterDef letterDef = ItemHelper.GetLetterFromValue(price);

            Find.LetterStack.ReceiveLetter(item.abr.Truncate(15, true).CapitalizeFirst(), letter, letterDef, new TargetInfo(vec, map, false));

            EndCarePackage();
        }

        private void TryExecuteAnimal(ThingDef animal, int count)
        {
            string letter = Helper.ReplacePlaceholder("TwitchStoriesDescription80".Translate(), from: Viewer.username, amount: quantity.ToString(), item: item.abr.CapitalizeFirst());
            IncidentWorker worker = new IncidentWorker_SpecificAnimalsWanderIn(animal.LabelCap + "'s join", PawnKindDef.Named(animal.defName), true, count, false, true);
            worker.def = IncidentDef.Named("FarmAnimalsWanderIn");
            worker.TryExecute(StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, Helper.AnyPlayerMap));

            EndCarePackage();
        }

        private void EndCarePackage()
        {
            Viewer.TakeViewerCoins(price);
            Viewer.CalculateNewKarma(storeIncident.karmaType, price);
            VariablesHelpers.SendPurchaseMessage(Helper.ReplacePlaceholder("TwitchToolkitItemPurchaseConfirm".Translate(), amount: quantity.ToString(), item: item.abr, viewer: Viewer.username), separateChannel);
        }

        private int price = 0;
        private int quantity = 0;
        private Store.Item item = null;
        private bool separateChannel;

        public override Viewer Viewer { get; set; }
    }

    public class BuyPrisoner : IncidentHelperVariables
    {
        public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
        {
            this.separateChannel = separateChannel;
            this.Viewer = viewer;
            string[] command = message.Split(' ');


            GameComponentPawns gameComponent = Current.Game.GetComponent<GameComponentPawns>();

            if (gameComponent.HasUserBeenNamed(viewer.username))
            {
                Toolkit.client.SendMessage($"@{viewer.username} you are in colony and cannot be a prisoner.", separateChannel);
                return false;
            }

            worker = new IncidentWorker_PrisonerJoins(viewer);
            worker.def = IncidentDefOf.WandererJoin;

            parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, Helper.AnyPlayerMap);

            return true;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);

            Viewer.TakeViewerCoins(storeIncident.cost);
            Viewer.CalculateNewKarma(storeIncident.karmaType, storeIncident.cost);

            VariablesHelpers.SendPurchaseMessage($"@{Viewer.username} has escaped from maximum security space prison.");
        }

        private IncidentWorker_PrisonerJoins worker;
        private IncidentParms parms;

        private bool separateChannel;

        public override Viewer Viewer { get; set; }
    }

    public class VisitColony : IncidentHelperVariables
    {
        public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
        {
            this.separateChannel = separateChannel;
            this.Viewer = viewer;
            string[] command = message.Split(' ');


            GameComponentPawns gameComponent = Current.Game.GetComponent<GameComponentPawns>();

            if (gameComponent.HasUserBeenNamed(viewer.username))
            {
                Toolkit.client.SendMessage($"@{viewer.username} you are in the colony and cannot visit.", separateChannel);
                return false;
            }

            worker = new IncidentWorker_VisitColony(viewer);
            worker.def = IncidentDef.Named("VisitColony");

            parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, Helper.AnyPlayerMap);

            return true;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);

            Viewer.TakeViewerCoins(storeIncident.cost);
            Viewer.CalculateNewKarma(storeIncident.karmaType, storeIncident.cost);

            VariablesHelpers.SendPurchaseMessage($"@{Viewer.username} is visiting the colony.");
        }

        private IncidentWorker_VisitColony worker;
        private IncidentParms parms;

        private bool separateChannel;

        public override Viewer Viewer { get; set; }
    }

    public class BeRescued : IncidentHelperVariables
    {
        public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
        {
            this.separateChannel = separateChannel;
            this.Viewer = viewer;
            string[] command = message.Split(' ');


            GameComponentPawns gameComponent = Current.Game.GetComponent<GameComponentPawns>();

            if (gameComponent.HasUserBeenNamed(viewer.username))
            {
                Toolkit.client.SendMessage($"@{viewer.username} you are in the colony and not in need of rescuing.", separateChannel);
                return false;
            }

            worker = new IncidentWorker_QuestViewerRescue(viewer);
            worker.def = IncidentDef.Named("QuestViewerRescue");

            parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, Helper.AnyPlayerMap);

            return true;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);

            Viewer.TakeViewerCoins(storeIncident.cost);
            Viewer.CalculateNewKarma(storeIncident.karmaType, storeIncident.cost);

            VariablesHelpers.SendPurchaseMessage($"@{Viewer.username} is being held prisoner at another faction.");
        }

        private IncidentWorker_QuestViewerRescue worker;
        private IncidentParms parms;

        private bool separateChannel;

        public override Viewer Viewer { get; set; }
    }

    public class Inspiration : IncidentHelperVariables
    {
        public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
        {
            this.separateChannel = separateChannel;
            string[] command = message.Split(' ');

            // check if command has enough variables
            if (command.Length - 2 < storeIncident.variables) // subtract 2 for the first part of the command (!buy item)
            {
                // let viewer know what correct way is
                Toolkit.client.SendMessage($"@{viewer.username} syntax is {this.storeIncident.syntax}", separateChannel);
                return false;
            }

            // other checks

            return true;
        }

        public override void TryExecute()
        {
            // take actions for incident
            List<Pawn> pawns = Helper.AnyPlayerMap.mapPawns.FreeColonistsSpawned.ToList();
            pawns.Shuffle();

            bool successfulInspiration = false;
            InspirationDef randomAvailableInspirationDef;

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

            if (successfulInspiration)
            {
                Viewer.TakeViewerCoins(storeIncident.cost);

                // calculate new karma if needed
                Viewer.CalculateNewKarma(storeIncident.karmaType, storeIncident.cost);

                // send a purchase confirmation message
                VariablesHelpers.SendPurchaseMessage($"@{Viewer.username} purchased a random inspiration.", separateChannel);
            }
            else
            {
                VariablesHelpers.SendPurchaseMessage($"@{Viewer.username} attempted to inspired a pawn, but none were successful.");
            }

        }

        public bool separateChannel = false;
        public override Viewer Viewer { get; set; }
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

        public static LetterDef GetLetterFromValue(int value)
        {
            if (value <= 250)
            {
                return LetterDefOf.NeutralEvent;
            } else if (value > 250 && value <= 750)
            {
                return DefDatabase<LetterDef>.GetNamed("BlueLetter");
            } else if (value > 750 && value <= 1500)
            {
                return DefDatabase<LetterDef>.GetNamed("GreenLetter");
            }
            else
            {
                return DefDatabase<LetterDef>.GetNamed("GoldLetter");
            }
        }
    }
}
