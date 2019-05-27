using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Store;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace TwitchToolkit.Windows
{
    public class StoreItemsWindow : Window
    {
        public StoreItemsWindow()
        {
            this.doCloseButton = true;
            GetTradeables();
        }

        public override void DoWindowContents(Rect inRect)
        {
            if (searchQuery != lastSearch)
            {
                GetTradeables();
            }


            GUI.BeginGroup(inRect);
            Rect rect2 = new Rect(inRect.width - 590f, 0f, 590f, 58f);
            GameFont old = Text.Font;
            Text.Font = GameFont.Medium;
            Widgets.Label(rect2, "Store Items");
            Text.Font = old;

            Rect searchBar = new Rect(0f, 30f, 300f, 26f);
            searchQuery = Widgets.TextEntryLabeled(searchBar, "Search:", searchQuery);

            Rect resetButton = new Rect(inRect.width - 150f, 0f, 100f, 26f);
            if (Widgets.ButtonText(resetButton, "Reset Items"))
            {
                Store_ItemEditor.ResetItemsToDefault();
                GetTradeables(false);
            }

            Rect sortLabel = new Rect(inRect.width - 470f, 32f, 100f, 24f);
            Rect sortButton = new Rect(inRect.width - 370f, 28f, 100f, 28f);

            Color defaultColor = GUI.color;
            GUI.color = ColorLibrary.Grey;
            Widgets.Label(sortLabel, "Sort items by:");
            GUI.color = defaultColor;
        
            if (Widgets.ButtonText(sortButton, "category"))
            {
                if (ascending)
                {
                    ascending = false;
                    cachedTradeables = cachedTradeables.OrderBy(s => s.FirstThingCategory != null ? s.FirstThingCategory.LabelCap : "Animal").ToList();
                }
                else
                {
                    ascending = true;
                    cachedTradeables = cachedTradeables.OrderByDescending(s => s.FirstThingCategory != null ? s.FirstThingCategory.LabelCap : "Animal").ToList();
                }
                
                GetTradeablesPrices();
            }

            sortButton.x += 110f;

            if (Widgets.ButtonText(sortButton, "price"))
            {
                if (ascending)
                {
                    ascending = false;
                    cachedTradeables = cachedTradeables.OrderBy(s => s.BaseMarketValue).ToList();
                }
                else
                {
                    ascending = true;
                    cachedTradeables = cachedTradeables.OrderByDescending(s => s.BaseMarketValue).ToList();
                }
                
                GetTradeablesPrices();
            }

            sortButton.x += 110f;

            if (Widgets.ButtonText(sortButton, "name"))
            {
                if (ascending)
                {
                    ascending = false;
                    cachedTradeables = cachedTradeables.OrderBy(s => s.LabelCap).ToList();
                }
                else
                {
                    ascending = true;
                    cachedTradeables = cachedTradeables.OrderByDescending(s => s.LabelCap).ToList();
                }
                
                GetTradeablesPrices();
            }

            Rect mainRect = new Rect(0f, 58f, inRect.width, inRect.height - 58f - 38f - 20f);
            this.FillMainRect(mainRect);

            GUI.EndGroup();
        }

        private void FillMainRect(Rect mainRect)
        {
			Text.Font = GameFont.Small;
			float height = 6f + (float)this.cachedTradeables.Count * 30f;
			Rect viewRect = new Rect(0f, 0f, mainRect.width - 16f, height);
			Widgets.BeginScrollView(mainRect, ref this.scrollPosition, viewRect, true);
			float num = 6f;
			float num2 = this.scrollPosition.y - 30f;
			float num3 = this.scrollPosition.y + mainRect.height;
			int num4 = 0;
			for (int i = 0; i < this.cachedTradeables.Count; i++)
			{
				if (num > num2 && num < num3)
				{
					Rect rect = new Rect(0f, num, viewRect.width, 30f);
					DrawItemRow(rect, this.cachedTradeables[i], num4);
				}
				num += 30f;
				num4++;
			}
			Widgets.EndScrollView();
        }

        private void DrawItemRow(Rect rect, ThingDef thing, int index)
        {
			if (index % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}

            Color white = GUI.color;
            if (tradeablesPrices[index] < 1)
            {
                GUI.color = ColorLibrary.Grey;
            }

			Text.Font = GameFont.Small;
			GUI.BeginGroup(rect);
			float num = rect.width;

            Rect rect1 = new Rect(num - 100f, 0f, 100f, rect.height);
            rect1 = rect1.Rounded();
            int newPrice = tradeablesPrices[index];
			string label = newPrice.ToString();
			rect1.xMax -= 5f;
			rect1.xMin += 5f;
			if (Text.Anchor == TextAnchor.MiddleLeft)
			{
				rect1.xMax += 300f;
			}
			if (Text.Anchor == TextAnchor.MiddleRight)
			{
				rect1.xMin -= 300f;
			}

            Rect rect2 = new Rect(num - 560f, 0f, 240f, rect.height);

            if (newPrice > 0)
            {
                if (Widgets.ButtonText(rect1, "Disable"))
                {
                    newPrice = -10;
                    tradeablesPrices[index] = newPrice;
                }
                tradeablesPrices[index] = newPrice;
            }

            if (newPrice > 0)
            {
                Widgets.IntEntry(rect2, ref newPrice, ref label, 50);
                tradeablesPrices[index] = newPrice;
            }
            else
            {
                if (Widgets.ButtonText(rect2, "Reset"))
                {
                    newPrice = Convert.ToInt32(thing.BaseMarketValue * 10 / 6);

                    if (newPrice < 1)
                    {
                        newPrice = 1;
                    }

                    tradeablesPrices[index] = newPrice;
                }
            }

            Rect categoryLabel = new Rect(num - 300, 0f, 200f, rect.height);
            Widgets.Label(categoryLabel, thing.FirstThingCategory != null ? thing.FirstThingCategory.LabelCap : "Animal");

            Rect rect3 = new Rect(0f, 0f, 27f, 27f);
            Widgets.ThingIcon(rect3, thing);
            Widgets.InfoCardButton(40f, 0f, thing);

			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect4 = new Rect(80f, 0f, rect.width - 80f, rect.height);
			Text.WordWrap = false;
			GUI.color = Color.white;
			Widgets.Label(rect4, thing.LabelCap);
        	Text.WordWrap = true;

			GenUI.ResetLabelAlign();
			GUI.EndGroup();

            GUI.color = white;
        }

        private void DrawPriceAdjuster(Rect rect, ref int val, int countChange = 50, int min = 0)
        {
			rect.width = 42f;
			if (Widgets.ButtonText(rect, "-" + countChange, true, false, true))
			{
				SoundDefOf.AmountDecrement.PlayOneShotOnCamera(null);
				val -= countChange * GenUI.CurrentAdjustmentMultiplier();
				if (val < min)
				{
					val = min;
				}
			}
			rect.x += rect.width + 2f;
			if (Widgets.ButtonText(rect, "+" + countChange, true, false, true))
			{
				SoundDefOf.AmountIncrement.PlayOneShotOnCamera(null);
				val += countChange * GenUI.CurrentAdjustmentMultiplier();
				if (val < min)
				{
					val = min;
				}
			}
        }

        private void GetTradeables(bool save = true)
        {
            if (save && cachedTradeables.Count > 0)
            {
                Store_ItemEditor.UpdateStoreItems(cachedTradeables, tradeablesPrices);
            }

            lastSearch = searchQuery;

            cachedTradeables = new List<ThingDef>();
            
            string searchShort = string.Join("", searchQuery.Split(' ')).ToLower();

            IEnumerable<ThingDef> tradeableitems = from t in DefDatabase<ThingDef>.AllDefs
                    where (t.tradeability.TraderCanSell() || ThingSetMakerUtility.CanGenerate(t) ) &&
                    (t.building == null || t.Minifiable || ToolkitSettings.MinifiableBuildings) &&
                    (t.FirstThingCategory != null || t.race != null) &&
                    (t.BaseMarketValue > 0) &&
                    (searchQuery == "" || 
                        (
                            t.defName.ToLower().Contains(searchShort) ||
                            string.Join("", t.label.Split(' ')).ToLower().Contains(searchShort) ||
                            t.defName.ToLower() == searchShort ||
                            string.Join("", t.label.Split(' ')).ToLower() == searchShort ||
                            (
                                (t.race != null &&
                                t.race.Animal &&
                                    (t.race.ToString().ToLower().Contains(searchQuery) ||
                                    t.race.ToString().ToLower() == searchQuery ||
                                    "animal".Contains(searchQuery) ||
                                    searchQuery == "animal")
                                )
                                ||
                                (t.race == null &&
                                    (t.FirstThingCategory == null ||
                                    string.Join("", t.FirstThingCategory.LabelCap.Split(' ')).ToLower().Contains(searchShort) ||
                                    t.FirstThingCategory.LabelCap.ToLower() == searchShort)
                                )
                            )
                        )
                    )
                    orderby t.LabelCap
                    select t;

            foreach(ThingDef item in tradeableitems)
            {
                if (item.BaseMarketValue > 0f)
                {
                    cachedTradeables.Add(item);
                }
            }

            GetTradeablesPrices();
        }

        private void GetTradeablesPrices()
        {
            tradeablesPrices = new List<int>();

            foreach (ThingDef item in cachedTradeables)
            {
                Item storeItem = Item.GetItemFromDefName(item.defName);
                if (storeItem != null)
                {
                    tradeablesPrices.Add(storeItem.price);
                }
                else
                {
                    tradeablesPrices.Add(Convert.ToInt32(item.BaseMarketValue * 10 / 6));
                }
            } 
        }

        public override void PostClose()
        {
            Store_ItemEditor.UpdateStoreItems(cachedTradeables, tradeablesPrices);
        }

        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(1024f, (float)UI.screenHeight);
            }
        }

        private Vector2 scrollPosition = Vector2.zero;

        private List<ThingDef> cachedTradeables = new List<ThingDef>();

        private List<int> tradeablesPrices = new List<int>();

        private string searchQuery = "";

        private string lastSearch = "";

        private bool ascending = true;
    }
}
