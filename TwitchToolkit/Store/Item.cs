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

        public void PutItemInCargoPod(string quote, int amount, string username)
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

            itemThing = ThingMaker.MakeThing(itemThingDef, (stuff != null) ? stuff : null);

            QualityCategory q = new QualityCategory();

            if (itemThing.TryGetQuality(out q))
            {
                setItemQualityRandom(itemThing);
            }

            IntVec3 vec;

            if (itemThingDef.Minifiable)
            {
                itemThingDef = itemThingDef.minifiedDef;
                MinifiedThing minifiedThing = (MinifiedThing)ThingMaker.MakeThing(itemThingDef, null);
			    minifiedThing.InnerThing = itemThing;
                minifiedThing.stackCount = amount;
                vec = Helper.Rain(itemDef, minifiedThing);
            }
            else
            {
                itemThing.stackCount = amount;
                vec = Helper.Rain(itemDef, itemThing);
            }

            quote = Helper.ReplacePlaceholder(quote, from: username, amount: amount.ToString(), item: this.abr);

            Helper.CarePackage(quote, LetterDefOf.PositiveEvent, vec);
        }

        public static void setItemQualityRandom(Thing thing)
        {
            QualityCategory qual = QualityUtility.GenerateQualityTraderItem();
            thing.TryGetComp<CompQuality>().SetQuality(qual, ArtGenerationContext.Outsider);
        }

        public static void TryMakeAllItems()
        {
            IEnumerable<ThingDef> tradeableitems = from t in DefDatabase<ThingDef>.AllDefs
                             where (t.tradeability.TraderCanSell() || ThingSetMakerUtility.CanGenerate(t) ) && (t.building == null || t.Minifiable || Settings.MinifiableBuildings)
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
