using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using ToolkitCore;
using TwitchToolkit.IncidentHelpers.IncidentHelper_Settings;
using TwitchToolkit.Incidents;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Special;

public class Item : IncidentHelperVariables
{
	private int price = 0;

	private int quantity = 0;

	private global::TwitchToolkit.Store.Item item = null;

	private bool separateChannel;

	public override Viewer Viewer { get; set; }

	public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
	{
		//IL_01de: Unknown result type (might be due to invalid IL or missing erences)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing erences)
		//IL_030e: Unknown result type (might be due to invalid IL or missing erences)
		this.separateChannel = separateChannel;
		Viewer = viewer;
		string[] command = message.Split(' ');
		if (command.Length < 4)
		{
			VariablesHelpers.ViewerDidWrongSyntax(viewer.username, storeIncident.syntax);
			return false;
		}
		string itemKey = command[2].ToLower();
		if (itemKey == null || itemKey == "")
		{
			VariablesHelpers.ViewerDidWrongSyntax(viewer.username, storeIncident.syntax);
			return false;
		}
		IEnumerable<global::TwitchToolkit.Store.Item> itemSearch = StoreInventory.items.Where((global::TwitchToolkit.Store.Item s) => s.price > 0 && (s.abr == itemKey || s.defname.ToLower() == itemKey));
		if (itemSearch.Count() > 0)
		{
			item = itemSearch.ElementAt(0);
		}
		if (item == null || item.price < 1)
		{
			TwitchWrapper.SendChatMessage("@" + viewer.username + " item not found.");
			return false;
		}
		ThingDef itemThingDef = ThingDef.Named(item.defname);
		bool isResearched = true;
		ResearchProjectDef researchProject = null;
		Helper.Log("Checking researched");
		if (itemThingDef.recipeMaker != null && itemThingDef.recipeMaker.researchPrerequisite != null && !itemThingDef.recipeMaker.researchPrerequisite.IsFinished)
		{
			Helper.Log("Recipe not researched");
			isResearched = false;
			researchProject = itemThingDef.recipeMaker.researchPrerequisite;
		}
		else if (!((BuildableDef)itemThingDef).IsResearchFinished)
		{
			Helper.Log("Building not researched");
			isResearched = false;
			researchProject = ((BuildableDef)itemThingDef).researchPrerequisites.ElementAt(0);
		}
		if (BuyItemSettings.mustResearchFirst && !isResearched)
		{
			string output = $"@{viewer.username} {((Def)itemThingDef).LabelCap} has not been researched yet, must finish research project {((Def)researchProject).LabelCap} first.";
			TwitchWrapper.SendChatMessage(output);
			return false;
		}
		string quantityKey = command[3];
		if (quantityKey == null || quantityKey == "")
		{
			VariablesHelpers.ViewerDidWrongSyntax(viewer.username, storeIncident.syntax);
			return false;
		}
		try
		{
			if (!int.TryParse(quantityKey, out quantity))
			{
				return false;
			}
			price = checked(item.price * quantity);
		}
		catch (OverflowException e)
		{
			Helper.Log(e.Message);
			return false;
		}
		if (quantity < 1 || price < 1)
		{
			VariablesHelpers.ViewerDidWrongSyntax(viewer.username, storeIncident.syntax);
			return false;
		}
		if (!Purchase_Handler.CheckIfViewerHasEnoughCoins(viewer, price))
		{
			return false;
		}
		if (price < ToolkitSettings.MinimumPurchasePrice)
		{
			TwitchWrapper.SendChatMessage(Helper.ReplacePlaceholder((TaggedString)(Translator.Translate("TwitchToolkitMinPurchaseNotMet")), null, null, null, null, null, null, null, null, null, null, viewer: viewer.username, amount: price.ToString(), mod: null, newbalance: null, karma: null, first: ToolkitSettings.MinimumPurchasePrice.ToString()));
			return false;
		}
		return true;
	}

	public override void TryExecute()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0018: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0101: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0106: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0137: Unknown result type (might be due to invalid IL or missing erences)
		//IL_013e: Expected O, but got Unknown
		//IL_0154: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0170: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0181: Unknown result type (might be due to invalid IL or missing erences)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0200: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0207: Unknown result type (might be due to invalid IL or missing erences)
		//IL_020c: Unknown result type (might be due to invalid IL or missing erences)
		ThingDef itemDef = ThingDef.Named("DropPodIncoming");
		Thing itemThing = new Thing();
		ThingDef stuff = null;
		ThingDef itemThingDef = ThingDef.Named(item.defname);
		if (itemThingDef.race != null)
		{
			TryExecuteAnimal(itemThingDef, quantity);
			return;
		}
		if (((BuildableDef)itemThingDef).MadeFromStuff && !GenCollection.TryRandomElementByWeight<ThingDef>(from x in GenStuff.AllowedStuffsFor((BuildableDef)(object)itemThingDef, (TechLevel)0)
			where !PawnWeaponGenerator.IsDerpWeapon(itemThingDef, x)
			select x, (Func<ThingDef, float>)((ThingDef x) => x.stuffProps.commonality), out stuff))
		{
			stuff = GenStuff.RandomStuffByCommonalityFor(itemThingDef, (TechLevel)0);
		}
		itemThing = ThingMaker.MakeThing(itemThingDef, (stuff != null) ? stuff : null);
		QualityCategory q = (QualityCategory)0;
		if (QualityUtility.TryGetQuality(itemThing, out q))
		{
			ItemHelper.setItemQualityRandom(itemThing);
		}
		Map map = Helper.AnyPlayerMap;
		IntVec3 vec = DropCellFinder.TradeDropSpot(map);
		if (itemThingDef.Minifiable)
		{
			itemThingDef = itemThingDef.minifiedDef;
			MinifiedThing minifiedThing = (MinifiedThing)ThingMaker.MakeThing(itemThingDef, (ThingDef)null);
			minifiedThing.InnerThing = (itemThing);
			((Thing)minifiedThing).stackCount = quantity;
			TradeUtility.SpawnDropPod(vec, map, (Thing)(object)minifiedThing);
		}
		else
		{
			itemThing.stackCount = quantity;
			TradeUtility.SpawnDropPod(vec, map, itemThing);
		}
		string letter = Helper.ReplacePlaceholder((TaggedString)(Translator.Translate("TwitchStoriesDescription80")), null, null, null, null, null, null, from: Viewer.username, amount: quantity.ToString(), item: GenText.CapitalizeFirst(item.abr));
		LetterDef letterDef = ItemHelper.GetLetterFromValue(price);
		Find.LetterStack.ReceiveLetter((TaggedString)(GenText.CapitalizeFirst(item.abr.Truncate(15, dots: true))), (TaggedString)(letter), letterDef, (LookTargets)(new TargetInfo(vec, map, false)), (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
		EndCarePackage();
	}

	private void TryExecuteAnimal(ThingDef animal, int count)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0050: Unknown result type (might be due to invalid IL or missing erences)
		//IL_005a: Unknown result type (might be due to invalid IL or missing erences)
		string letter = Helper.ReplacePlaceholder((TaggedString)(Translator.Translate("TwitchStoriesDescription80")), null, null, null, null, null, null, from: Viewer.username, amount: quantity.ToString(), item: GenText.CapitalizeFirst(item.abr));
		IncidentWorker worker = (IncidentWorker)(object)new IncidentWorker_SpecificAnimalsWanderIn((TaggedString)(((Def)animal).LabelCap + "'s join"), PawnKindDef.Named(((Def)animal).defName), joinColony: true, count, manhunter: false, defaultText: true);
		worker.def = IncidentDef.Named("FarmAnimalsWanderIn");
		worker.TryExecute(StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, (IIncidentTarget)(object)Helper.AnyPlayerMap));
		EndCarePackage();
	}

	private void EndCarePackage()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing erences)
		Viewer.TakeViewerCoins(price);
		Viewer.CalculateNewKarma(storeIncident.karmaType, price);
		VariablesHelpers.SendPurchaseMessage(Helper.ReplacePlaceholder((TaggedString)(Translator.Translate("TwitchToolkitItemPurchaseConfirm")), null, null, null, null, null, null, amount: quantity.ToString(), item: item.abr, animal: null, from: null, to: null, mod: null, viewer: Viewer.username));
	}
}
