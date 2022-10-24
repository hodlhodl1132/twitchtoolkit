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

	private string lastSearch = null;

	private List<StoreIncident> storeIncidents = new List<StoreIncident>();

	private List<StoreIncident> disabledIncidents = new List<StoreIncident>();

	private Vector2 scrollPosition;

	private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

	public StoreIncidentsWindow()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0038: Unknown result type (might be due to invalid IL or missing erences)
		Store_IncidentEditor.UpdatePriceSheet();
	}

	public override void DoWindowContents(Rect inRect)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_008c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing erences)
		//IL_010d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0172: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0179: Unknown result type (might be due to invalid IL or missing erences)
		//IL_017f: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0184: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0186: Unknown result type (might be due to invalid IL or missing erences)
		//IL_018d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_018e: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0193: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0195: Unknown result type (might be due to invalid IL or missing erences)
		//IL_019c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_019d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0206: Unknown result type (might be due to invalid IL or missing erences)
		//IL_020e: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0276: Unknown result type (might be due to invalid IL or missing erences)
		if (searchQuery != lastSearch)
		{
			UpdateList();
		}
		Rect rect = new Rect(0f, 0f, ((Rect)(inRect)).width, 60f);
		Text.Font =((GameFont)2);
		Text.Anchor =((TextAnchor)4);
		Widgets.Label(rect, "Store Incidents");
		Text.Font =((GameFont)1);
		Text.Anchor =((TextAnchor)0);
		Rect search = new Rect(0f, ((Rect)(rect)).height, ((Rect)(inRect)).width - 20f, 26f);
        searchQuery = Widgets.TextEntryLabeled(search, "Search:", searchQuery);
		Rect resetButton = new Rect(((Rect)(search)).x, ((Rect)(search)).y + 28f, ((Rect)(inRect)).width / 2f - 20f, 26f);
        if (Widgets.ButtonText(resetButton, "Reset all Incidents", true, true, true))
		{
			Store_IncidentEditor.LoadBackups();
		}
		resetButton.x =(((Rect)( resetButton)).x + (((Rect)( resetButton)).width + 10f));
		if (Widgets.ButtonText(resetButton, "Disable All", true, true, true))
		{
			foreach (StoreIncident incident in DefDatabase<StoreIncident>.AllDefs)
			{
				incident.cost = -10;
			}
		}
		inRect.y = (((Rect)( inRect)).yMin + 120f);
		Widgets.DrawMenuSection(inRect);
		inRect = GenUI.ContractedBy(inRect, 17f);
		GUI.BeginGroup(inRect);
		Rect rect2 = GenUI.AtZero(inRect);
		DoBottomButtons(rect2);
		Rect outRect = rect2;
		outRect.y = (((Rect)( outRect)).yMax - 65f);
		if (storeIncidents.Count > 0)
		{
			float height = (float)storeIncidents.Count * 24f;
			float num = 0f;
			Rect viewRect = new Rect(0f, 0f, ((Rect)(outRect)).width - 16f, height);
			Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect, true);
			float num2 = scrollPosition.y - 24f;
			float num3 = scrollPosition.y + ((Rect)( outRect)).height;
			Rect rect3 = default(Rect);
			for (int i = 0; i < storeIncidents.Count; i++)
			{
				if (num > num2 && num < num3)
				{
					rect3 = new Rect(0f, num, ((Rect)( viewRect)).width, 24f);
					DoRow(rect3, storeIncidents[i], i);
				}
				num += 24f;
			}
			Widgets.EndScrollView();
		}
		else
		{
			Widgets.NoneLabel(0f, ((Rect)( outRect)).width, (string)null);
		}
		GUI.EndGroup();
	}

	private void DoRow(Rect rect, StoreIncident thingDef, int index)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0008: Unknown result type (might be due to invalid IL or missing erences)
		//IL_000f: Unknown result type (might be due to invalid IL or missing erences)
		//IL_001a: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0136: Unknown result type (might be due to invalid IL or missing erences)
		Widgets.DrawHighlightIfMouseover(rect);
		TooltipHandler.TipRegion(rect, (TipSignal)(((Def)thingDef).description));
		GUI.BeginGroup(rect);
		Rect rect2 = new Rect(4f, (((Rect)(rect)).height - 20f) / 2f, 20f, 20f);
		Rect rect3 = new Rect(((Rect)(rect2)).xMax + 4f, 0f, ((Rect)(rect)).width - 60f, 24f);
		Text.Anchor =((TextAnchor)3);
		Text.WordWrap =(false);
		if (thingDef.cost < 1 && ((Def)thingDef).defName != "Item")
		{
			GUI.color =(Color.grey);
		}
		Widgets.Label(rect3, GenText.CapitalizeFirst(((Def)thingDef).label));
		Rect rect4 = new Rect(((Rect)(rect3)).width, ((Rect)(rect3)).y, 60f, ((Rect)(rect3)).height);
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
		//IL_0011: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0030: Unknown result type (might be due to invalid IL or missing erences)
		//IL_003b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_004a: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0050: Unknown result type (might be due to invalid IL or missing erences)
		Rect rect2 = new Rect(((Rect)(rect)).width / 2f - BottomButtonSize.x / 2f, ((Rect)(rect)).height - 55f, BottomButtonSize.x, BottomButtonSize.y);
		if (Widgets.ButtonText(rect2, (TaggedString)(Translator.Translate("CloseButton")), true, false, true))
		{
			((Window)this).Close(true);
		}
	}

	private void UpdateList()
	{
		lastSearch = searchQuery;
		if (searchQuery == "")
		{
			storeIncidents = (from s in DefDatabase<StoreIncident>.AllDefs
				orderby ((Def)s).label
				select s).ToList();
			return;
		}
		storeIncidents = (from s in DefDatabase<StoreIncident>.AllDefs
			orderby ((Def)s).defName
			where ((Def)s).label.ToLower().Contains(searchQuery.ToLower()) || s.abbreviation.ToLower().Contains(searchQuery.ToLower())
			select s).ToList();
	}

	public override void Close(bool doCloseSound = true)
	{
		Store_IncidentEditor.UpdatePriceSheet();
		((Window)this).Close(doCloseSound);
	}
}
