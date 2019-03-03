using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace TwitchToolkit
{
    public class Item
    {
        public int id;
        public int price;
        public string abr;
        public string defname;
        public string stuffname;

        public Item(int price, string abr, string defname, int id = -1, string stuffname = "null")
        {
            if (id < 0)
            {
                this.id = Settings.items.Count();
            }
            else
            {
                this.id = id;
            }
            
            this.price = price;
            this.abr = abr;
            this.defname = defname;
            this.stuffname = stuffname;
        }

        public static Item GetItemFromAbr(string abr)
        {
            return Settings.items.Find(x => x.abr == abr);
        }

        public static Item GetItemFromDefName(string defname)
        {
            return Settings.items.Find(x => x.defname == defname);
        }

        public void SetItemPrice(int price)
        {
            this.price = price;
            Settings.ItemPrices[this.id] = price;
        }

        public int CalculatePrice(int quanity)
        {
            return quanity * this.price;
        }

        public void PutItemInCargoPod(string quote, int amount)
        {
            var itemDef = ThingDef.Named("DropPodIncoming");
            var itemThing = new Thing();

            // Lets see if a new item needs to be made from stuff
            ThingDef stuff = null;
            ThingDef itemThingDef = ThingDef.Named(this.defname);

			if (itemThingDef.MadeFromStuff)
			{
				if (!(from x in GenStuff.AllowedStuffsFor(itemThingDef, TechLevel.Undefined)
				where !PawnWeaponGenerator.IsDerpWeapon(itemThingDef, x)
				select x).TryRandomElementByWeight((ThingDef x) => x.stuffProps.commonality, out stuff))
				{
					stuff = GenStuff.RandomStuffByCommonalityFor(itemThingDef, TechLevel.Undefined);
				}
			}

            itemThing = ThingMaker.MakeThing(ThingDef.Named(this.defname), (stuff != null) ? stuff : null);

            QualityCategory q = new QualityCategory();

            if (itemThing.TryGetQuality(out q))
            {
                setItemQualityRandom(itemThing);
            }

            itemThing.stackCount = amount;
            IntVec3 vec = Helper.Rain(itemDef, itemThing);

            Helper.CarePackage(quote, LetterDefOf.PositiveEvent, vec);
        }

        public static void setItemQualityRandom(Thing thing)
        {
            QualityCategory qual = QualityUtility.GenerateQualityTraderItem();
            thing.TryGetComp<CompQuality>().SetQuality(qual, ArtGenerationContext.Outsider);
        }

        public static Item[] GetDefaultItems()
        {
            Item[] defaultitems =
            {
                new Item(5, "silver", "Silver", 0),
                new Item(30, "uranium", "Uranium", 1),
                new Item(30, "survivalmeal", "MealSurvivalPack", 2),
                new Item(15, "pastemeal", "MealNutrientPaste", 3),
                new Item(45, "simplemeal", "MealSimple", 4),
                new Item(60, "finemeal", "MealFine", 5),
                new Item(5, "kibble", "Kibble", 6),
                new Item(2, "hay", "Hay", 7),
                new Item(4, "humanmeat", "Meat_Human", 8),
                new Item(300, "luciferium", "Luciferium", 9),
                new Item(4, "pemmican", "Pemmican", 10),
                new Item(5000, "techprofsubpersonacore", "TechprofSubpersonaCore", 11),
                new Item(12, "wort", "Wort", 12),
                new Item(50, "gold", "Gold", 13),
                new Item(8, "steel", "Steel", 14),
                new Item(2, "wood", "WoodLog", 15),
                new Item(25, "herbalmedicine", "MedicineHerbal", 16),
                new Item(80, "industrialmedicine", "MedicineIndustrial", 17),
                new Item(200, "glitterworldmedicine", "MedicineUltratech", 18),
                new Item(4, "graniteblocks", "BlocksGranite", 19),
                new Item(45, "plasteel", "Plasteel", 20),
                new Item(40, "beer", "Beer", 21),
                new Item(8000, "aipersonacore", "AIPersonaCore", 22),
                new Item(55, "smokeleafjoint", "SmokeleafJoint", 23),
                new Item(60, "industrialcomponent", "ComponentIndustrial", 24),
                new Item(1000, "advcomponent", "ComponentSpacer", 25),
                new Item(40, "insectjelly", "InsectJelly", 26),
                new Item(40, "cloth", "Cloth", 27),
                new Item(10, "plainleather", "Leather_plain", 28),
                new Item(45, "hyperweave", "Hyperweave", 29),
                new Item(15, "chocolate", "Chocolate", 30),
                new Item(400, "elephanttusk", "", 31),
                new Item(5, "potatoes", "RawPotatoes", 32),
                new Item(5, "berries", "RawBerries", 33),
                new Item(2500, "heart", "Heart", 34),
                new Item(5000, "chargerifle", "Gun_ChargeRifle", 35),
                new Item(100, "revolver", "Gun_Revolver", 36),
                new Item(250, "boltactionrifle", "Gun_BoltActionRifle", 37),
                new Item(350, "chainshotgun", "Gun_ChainShotgun", 38),
                new Item(4500, "doomsdaylauncher", "Gun_DoomsdayRocket", 39),
                new Item(450, "advancedhelmet", "Apparel_AdvancedHelmet", 40, "Steel"),
                new Item(1100, "marinehelmet", "Apparel_PowerArmorHelmet", 41),
                new Item(300, "duster", "Apparel_Duster", 42, "Cloth"),
                new Item(50, "tribalwear", "Apparel_TribalA", 43, "Cloth"),
                new Item(55, "tshirt", "Apparel_BasicShirt", 44, "Cloth"),
                new Item(55, "pants", "Apparel_Pants", 45, "Cloth"),
                new Item(25, "cowboyhat", "Apparel_CowboyHat", 46, "Cloth"),
            };

            return defaultitems;
        }

        public static void TryMakeAllItems()
        {
            IEnumerable<ThingDef> tradeableitems = from t in DefDatabase<ThingDef>.AllDefs
                             where t.tradeability.TraderCanSell()
                             select t;


            Helper.Log("Found " + tradeableitems.Count() + " items");
            foreach(ThingDef item in tradeableitems)
            {
                string label = string.Join("", item.label.Split(' ')).ToLower();
                Item checkforexistingitembydefname = Item.GetItemFromDefName(item.defName);
                Item checkforexistingitembylabel = Item.GetItemFromAbr(label);
                if (checkforexistingitembydefname == null && checkforexistingitembylabel == null)
                {
                    try
                    { 
                        // item needs to be worth money, also not an animal
                        if (item.BaseMarketValue > 0f && item.race == null)
                        {
                            Helper.Log("Adding item " + item.label);
                            int id = Settings.items.Count();
                            Settings.items.Add(new Item(Convert.ToInt32(item.BaseMarketValue * 10 / 6), label, item.defName));
                    
                            Settings.ItemIds.Add(label, id);
                            Settings.ItemPrices.Add(id, Convert.ToInt32(item.BaseMarketValue * 10 / 6));
                            Settings.ItemDefnames.Add(id, item.defName);
                            Settings.ItemStuffnames.Add(id, "null");
                        }
                    }
                    catch (InvalidCastException e)
                    {
                        Helper.Log("Existing item except " + e.Message);
                    }
                }
            }
        }
    }
}
