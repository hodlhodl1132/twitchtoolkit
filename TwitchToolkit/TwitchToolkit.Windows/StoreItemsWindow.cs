using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using TwitchToolkit.Store;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace TwitchToolkit.Windows;

public class StoreItemsWindow : Window
{
	private Vector2 scrollPosition = Vector2.zero;

	private List<ThingDef> cachedTradeables = new List<ThingDef>();

	private List<int> tradeablesPrices = new List<int>();

	private string searchQuery = "";

	private string lastSearch = "";

	private bool ascending = true;

	public override Vector2 InitialSize => new Vector2(1024f, (float)UI.screenHeight);

	public StoreItemsWindow()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0006: Unknown result type (might be due to invalid IL or missing erences)
		base.doCloseButton = true;
		GetTradeables();
	}

	public override void DoWindowContents(Rect inRect)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing erences)
		//IL_004c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0051: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0059: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0065: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0088: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0101: Unknown result type (might be due to invalid IL or missing erences)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing erences)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing erences)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing erences)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing erences)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing erences)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0288: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0347: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0424: Unknown result type (might be due to invalid IL or missing erences)
		if (searchQuery != lastSearch)
		{
			GetTradeables();
		}
		GUI.BeginGroup(inRect);
		Rect rect2 = new Rect(((Rect)(inRect)).width - 590f, 0f, 590f, 58f);
		GameFont old = Text.Font;
		Text.Font =((GameFont)2);
		Widgets.Label(rect2, "Store Items");
		Text.Font =(old);
		Rect searchBar = new Rect(0f, 30f, 300f, 26f);
		searchQuery = Widgets.TextEntryLabeled(searchBar, "Search:", searchQuery);
		Rect resetButton = new Rect(((Rect)(inRect)).width - 270f, 0f, 100f, 26f);
		if (Widgets.ButtonText(resetButton, "Reset Items", true, true, true))
		{
			Store_ItemEditor.ResetItemsToDefault();
			GetTradeables(save: false);
		}
		resetButton.x =(((Rect)( resetButton)).x + (((Rect)( resetButton)).width + 20f));
		if (Widgets.ButtonText(resetButton, "Disable All", true, true, true))
		{
			foreach (Item item in StoreInventory.items)
			{
				item.price = -10;
			}
			GetTradeables(save: false);
		}
		Rect sortLabel = new Rect(((Rect)(inRect)).width - 470f, 32f, 100f, 24f);
		Rect sortButton = new Rect(((Rect)(inRect)).width - 370f, 28f, 100f, 28f);
		Color defaultColor = GUI.color;
		GUI.color =(ColorLibrary.Grey);
		Widgets.Label(sortLabel, "Sort items by:");
		GUI.color =(defaultColor);
		if (Widgets.ButtonText(sortButton, "category", true, true, true))
		{
			if (ascending)
			{
				ascending = false;
				cachedTradeables = cachedTradeables.OrderBy(delegate(ThingDef s)
				{
					//IL_0015: Unknown result type (might be due to invalid IL or missing erences)
					//IL_001a: Unknown result type (might be due to invalid IL or missing erences)
					object result2;
					if (s.FirstThingCategory == null)
					{
						result2 = "Animal";
					}
					else
					{
						TaggedString labelCap4 = ((Def)s.FirstThingCategory).LabelCap;
						result2 = ((TaggedString)( labelCap4)).RawText;
					}
					return (string)result2;
				}).ToList();
			}
			else
			{
				ascending = true;
				cachedTradeables = cachedTradeables.OrderByDescending(delegate(ThingDef s)
				{
					//IL_0015: Unknown result type (might be due to invalid IL or missing erences)
					//IL_001a: Unknown result type (might be due to invalid IL or missing erences)
					object result;
					if (s.FirstThingCategory == null)
					{
						result = "Animal";
					}
					else
					{
						TaggedString labelCap3 = ((Def)s.FirstThingCategory).LabelCap;
						result = ((TaggedString)( labelCap3)).RawText;
					}
					return (string)result;
				}).ToList();
			}
			GetTradeablesPrices();
		}
		sortButton.x =(((Rect)( sortButton)).x + 110f);
		if (Widgets.ButtonText(sortButton, "price", true, true, true))
		{
			if (ascending)
			{
				ascending = false;
				cachedTradeables = cachedTradeables.OrderBy((ThingDef s) => s.BaseMarketValue).ToList();
			}
			else
			{
				ascending = true;
				cachedTradeables = cachedTradeables.OrderByDescending((ThingDef s) => s.BaseMarketValue).ToList();
			}
			GetTradeablesPrices();
		}
		sortButton.x =(((Rect)( sortButton)).x + 110f);
		if (Widgets.ButtonText(sortButton, "name", true, true, true))
		{
			if (ascending)
			{
				ascending = false;
				cachedTradeables = cachedTradeables.OrderBy(delegate(ThingDef s)
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing erences)
					//IL_0006: Unknown result type (might be due to invalid IL or missing erences)
					TaggedString labelCap2 = ((Def)s).LabelCap;
					return ((TaggedString)( labelCap2)).RawText;
				}).ToList();
			}
			else
			{
				ascending = true;
				cachedTradeables = cachedTradeables.OrderByDescending(delegate(ThingDef s)
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing erences)
					//IL_0006: Unknown result type (might be due to invalid IL or missing erences)
					TaggedString labelCap = ((Def)s).LabelCap;
					return ((TaggedString)( labelCap)).RawText;
				}).ToList();
			}
			GetTradeablesPrices();
		}
		Rect mainRect = new Rect(0f, 58f, ((Rect)(inRect)).width, ((Rect)(inRect)).height - 58f - 38f - 20f);
		FillMainRect(mainRect);
		GUI.EndGroup();
	}

	private void FillMainRect(Rect mainRect)
	{
		Text.Font =((GameFont)1);
		float height = 6f + (float)cachedTradeables.Count * 30f;
		Rect viewRect = new Rect(0f, 0f, ((Rect)(mainRect)).width - 16f, height);
		Widgets.BeginScrollView(mainRect, ref scrollPosition, viewRect, true);
		float num = 6f;
		float num2 = scrollPosition.y - 30f;
		float num3 = scrollPosition.y + ((Rect)( mainRect)).height;
		int num4 = 0;
		Rect rect = default(Rect);
		for (int i = 0; i < cachedTradeables.Count; i++)
		{
			if (num > num2 && num < num3)
			{
				rect = new Rect(0f, num, ((Rect)( viewRect)).width, 30f);
				DrawItemRow(rect, cachedTradeables[i], num4);
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
			GUI.color = (ColorLibrary.Grey);
		}
		Text.Font = ((GameFont)1);
		GUI.BeginGroup(rect);
		float num = ((Rect)( rect)).width;
		Rect rect2 = new Rect(num - 100f, 0f, 100f, ((Rect)(rect)).height);
		rect2 = GenUI.Rounded(rect2);
		int newPrice = tradeablesPrices[index];
		string label = newPrice.ToString();
		rect2.x =(((Rect)( rect2)).xMax - 5f);
		rect2.x =(((Rect)( rect2)).xMin + 5f);
		if ((int)Text.Anchor == 3)
		{
			rect2.x =(((Rect)( rect2)).xMax + 300f);
		}
		if ((int)Text.Anchor == 5)
		{
			rect2.x =(((Rect)( rect2)).xMin - 300f);
		}
		Rect rect3 = new Rect(num - 560f, 0f, 240f, ((Rect)(rect)).height);
		if (newPrice > 0)
		{
			if (Widgets.ButtonText(rect2, "Disable", true, true, true))
			{
				newPrice = -10;
				tradeablesPrices[index] = newPrice;
			}
			tradeablesPrices[index] = newPrice;
		}
		if (newPrice > 0)
		{
			Widgets.IntEntry(rect3, ref newPrice, ref label, 50);
			tradeablesPrices[index] = newPrice;
		}
		else if (Widgets.ButtonText(rect3, "Reset", true, true, true))
		{
			newPrice = Convert.ToInt32(thing.BaseMarketValue * 10f / 6f);
			if (newPrice < 1)
			{
				newPrice = 1;
			}
			tradeablesPrices[index] = newPrice;
		}
		Rect categoryLabel = new Rect(num - 300f, 0f, 200f, ((Rect)(rect)).height);
		Widgets.Label(categoryLabel, (thing.FirstThingCategory != null) ? ((Def)thing.FirstThingCategory).LabelCap : (TaggedString)("Animal"));
		Rect rect4 = new Rect(0f, 0f, 27f, 27f);
		Widgets.ThingIcon(rect4, thing, (ThingDef)null, (ThingStyleDef)null, 1f, (Color?)null);
		Widgets.InfoCardButton(40f, 0f, (Def)(object)thing);
		Text.Anchor =((TextAnchor)3);
		Rect rect5 = new Rect(80f, 0f, ((Rect)(rect)).width - 80f, ((Rect)(rect)).height);
		Text.WordWrap =(false);
		GUI.color =(Color.white);
		Widgets.Label(rect5, ((Def)thing).LabelCap);
		Text.WordWrap =(true);
		GenUI.ResetLabelAlign();
		GUI.EndGroup();
		GUI.color =(white);
	}

	private void DrawPriceAdjuster(Rect rect,  int val, int countChange = 50, int min = 0)
	{
		rect.width = (42f);
		if (Widgets.ButtonText(rect, "-" + countChange, true, false, true))
		{
			SoundStarter.PlayOneShotOnCamera(SoundDefOf.Click, (Map)null);
			val -= countChange * GenUI.CurrentAdjustmentMultiplier();
			if (val < min)
			{
				val = min;
			}
		}
		rect.x =(((Rect)( rect)).x + (((Rect)( rect)).width + 2f));
		if (Widgets.ButtonText(rect, "+" + countChange, true, false, true))
		{
			SoundStarter.PlayOneShotOnCamera(SoundDefOf.Click, (Map)null);
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
		Helper.Log("Finding tradeables");
		IEnumerable<ThingDef> tradeableitems = DefDatabase<ThingDef>.AllDefs.Where(delegate(ThingDef t)
		{
			if ((!TradeabilityUtility.TraderCanSell(t.tradeability) && !ThingSetMakerUtility.CanGenerate(t)) || (t.building != null && !t.Minifiable && !ToolkitSettings.MinifiableBuildings) || (t.FirstThingCategory == null && t.race == null) || !(t.BaseMarketValue > 0f))
			{
				goto IL_0228;
			}
			if (!(searchQuery == "") && !((Def)t).defName.ToLower().Contains(searchShort) && !string.Join("", ((Def)t).label.Split(' ')).ToLower().Contains(searchShort) && !(((Def)t).defName.ToLower() == searchShort) && !(string.Join("", ((Def)t).label.Split(' ')).ToLower() == searchShort) && (t.race == null || !t.race.Animal || (!((object)t.race).ToString().ToLower().Contains(searchQuery) && !(((object)t.race).ToString().ToLower() == searchQuery) && !"animal".Contains(searchQuery) && !(searchQuery == "animal"))))
			{
				if (t.race != null)
				{
					goto IL_0228;
				}
				if (t.FirstThingCategory != null)
				{
					TaggedString labelCap2 = ((Def)t.FirstThingCategory).LabelCap;
					if (!string.Join("", ((TaggedString)( labelCap2)).RawText.Split(' ')).ToLower().Contains(searchShort))
					{
						labelCap2 = ((Def)t.FirstThingCategory).LabelCap;
						if (!(((TaggedString)( labelCap2)).RawText.ToLower() == searchShort))
						{
							goto IL_0228;
						}
					}
				}
			}
			int result = ((((Def)t).defName != "Human") ? 1 : 0);
			goto IL_0229;
			IL_0229:
			return (byte)result != 0;
			IL_0228:
			result = 0;
			goto IL_0229;
		}).OrderBy(delegate(ThingDef t)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing erences)
			//IL_0006: Unknown result type (might be due to invalid IL or missing erences)
			TaggedString labelCap = ((Def)t).LabelCap;
			return ((TaggedString)( labelCap)).RawText;
		});
		foreach (ThingDef item in tradeableitems)
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
			Item storeItem = Item.GetItemFromDefName(((Def)item).defName);
			if (storeItem != null)
			{
				tradeablesPrices.Add(storeItem.price);
			}
			else
			{
				tradeablesPrices.Add(Convert.ToInt32(item.BaseMarketValue * 10f / 6f));
			}
		}
	}

	public override void PostClose()
	{
		Store_ItemEditor.UpdateStoreItems(cachedTradeables, tradeablesPrices);
	}
}
