using System;
using System.Collections.Generic;
using System.Linq;
using TwitchToolkit.Incidents;
using TwitchToolkit.Store;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Windows;

public class StoreIncidentsWindow : Window
{
	private string searchQuery = "";

	private string lastSearch;

	private List<StoreIncident> storeIncidents = new List<StoreIncident>();

	private List<StoreIncident> disabledIncidents = new List<StoreIncident>();

	private Vector2 scrollPosition;

	private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

	public StoreIncidentsWindow()
	{
        Store_IncidentEditor.UpdatePriceSheet();
	}

	public override void DoWindowContents(Rect inRect)
	{
        if (searchQuery != lastSearch)
		{
			UpdateList();
		}
		Rect rect = new Rect(0f, 0f, inRect.width, 60f);
		Text.Font =((GameFont)2);
		Text.Anchor =((TextAnchor)4);
		Widgets.Label(rect, "Store Incidents");
		Text.Font =((GameFont)1);
		Text.Anchor = TextAnchor.UpperLeft;
		Rect search = new Rect(0f, rect.height, inRect.width - 20f, 26f);
        searchQuery = Widgets.TextEntryLabeled(search, "Search:", searchQuery);
		Rect resetButton = new Rect (search.x, search.y + 28f, inRect.width / 2f - 20f, 26f);
        if (Widgets.ButtonText(resetButton, "Reset all Incidents"))
		{
			Store_IncidentEditor.LoadBackups();
		}
		resetButton.x = resetButton.x + resetButton.width + 10f;
		if (Widgets.ButtonText(resetButton, "Disable All"))
		{
			foreach (StoreIncident incident in DefDatabase<StoreIncident>.AllDefs)
			{
				incident.cost = -10;
			}
		}
		inRect.y = inRect.yMin + 120f;
		Widgets.DrawMenuSection(inRect);
		inRect = GenUI.ContractedBy(inRect, 17f);
		GUI.BeginGroup(inRect);
		Rect rect2 = GenUI.AtZero(inRect);
		DoBottomButtons(rect2);
		Rect outRect = rect2;
		outRect.y = outRect.yMax - 65f;
		if (storeIncidents.Count > 0)
		{
			float height = storeIncidents.Count * 24f;
			float num = 0f;
			Rect viewRect = new Rect(0f, 0f, ((Rect)(outRect)).width - 16f, height);
			Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect, true);
			float num2 = scrollPosition.y - 24f;
			float num3 = scrollPosition.y + ((Rect)( outRect)).height;
			Rect rect3;
			for (int i = 0; i < storeIncidents.Count; i++)
			{
				if (num > num2 && num < num3)
				{
					rect3 = new Rect(0f, num, viewRect.width, 24f);
					DoRow(rect3, storeIncidents[i], i);
				}
				num += 24f;
			}
			Widgets.EndScrollView();
		}
		else
		{
			Widgets.NoneLabel(0f, outRect.width);
		}
		GUI.EndGroup();
	}

	private void DoRow(Rect rect, StoreIncident thingDef, int index)
	{
        Widgets.DrawHighlightIfMouseover(rect);
		TooltipHandler.TipRegion(rect, (TipSignal)(((Def)thingDef).description));
		GUI.BeginGroup(rect);
		Rect rect2 = new Rect(4f, (rect.height - 20f) / 2f, 20f, 20f);
		Rect rect3 = new Rect(rect2.xMax + 4f, 0f, ((Rect)(rect)).width - 60f, 24f);
		Text.Anchor =((TextAnchor)3);
		Text.WordWrap =(false);
		if (thingDef.cost < 1 && ((Def)thingDef).defName != "Item")
		{
			GUI.color =(Color.grey);
		}
		Widgets.Label(rect3, GenText.CapitalizeFirst(((Def)thingDef).label));
		Rect rect4 = new Rect(rect3.width, rect3.y, 60f, rect3.height);
		if (Widgets.ButtonText(rect4, "Edit", true, true, true))
		{
			Type type = typeof(StoreIncidentEditor);
			Find.WindowStack.TryRemove(type, true);
			StoreIncidentEditor window = new StoreIncidentEditor(thingDef);
			Find.WindowStack.Add((Window)(object)window);
		}
		Text.Anchor =((TextAnchor)0);
		Text.WordWrap =(true);
		GUI.color =(Color.white);
		GUI.EndGroup();
	}

	private void DoBottomButtons(Rect rect)
	{
        Rect rect2 = new Rect(rect.width / 2f - BottomButtonSize.x / 2f, rect.height - 55f, BottomButtonSize.x, BottomButtonSize.y);
		if (Widgets.ButtonText(rect2, (TaggedString)(Translator.Translate("CloseButton")), true, false, true))
		{
			Close();
		}
	}

	private void UpdateList()
	{
		lastSearch = searchQuery;
		if (searchQuery == "")
		{
			storeIncidents = (from s in DefDatabase<StoreIncident>.AllDefs
				orderby s.label
				select s).ToList();
			return;
		}
		storeIncidents = (from s in DefDatabase<StoreIncident>.AllDefs
			orderby s.defName
			where s.label.ToLower().Contains(searchQuery.ToLower()) || s.abbreviation.ToLower().Contains(searchQuery.ToLower())
			select s).ToList();
	}

	public override void Close(bool doCloseSound = true)
	{
		Store_IncidentEditor.UpdatePriceSheet();
		base.Close(doCloseSound);
	}
}
