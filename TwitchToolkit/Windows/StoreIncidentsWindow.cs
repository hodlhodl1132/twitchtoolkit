using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Incidents;
using TwitchToolkit.Store;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Windows
{
    public class StoreIncidentsWindow : Window
    {
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
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, "Store Incidents");
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
            Rect search = new Rect(0, rect.height, inRect.width - 20f, 26f);
            searchQuery = Widgets.TextEntryLabeled(search, "Search:", searchQuery);
            Rect resetButton = new Rect(search.x, search.y + 28f, (inRect.width / 2f) - 20f, 26f);
            if (Widgets.ButtonText(resetButton, "Reset all Incidents"))
            {
                Store_IncidentEditor.LoadBackups();
            }
            resetButton.x += resetButton.width + 10f;
            if (Widgets.ButtonText(resetButton, "Disable All"))
            {
                foreach (StoreIncident incident in DefDatabase<StoreIncident>.AllDefs)
                {
                    incident.cost = -10;
                }
            }
			inRect.yMin += 120f;
			Widgets.DrawMenuSection(inRect);
			//TabDrawer.DrawTabs(inRect, this.tabs, 2);
			inRect = inRect.ContractedBy(17f);
			GUI.BeginGroup(inRect);
			Rect rect2 = inRect.AtZero();
			this.DoBottomButtons(rect2);
			Rect outRect = rect2;
			outRect.yMax -= 65f;
			if (storeIncidents.Count > 0)
			{
				float height = (float)storeIncidents.Count * 24f;
				float num = 0f;
				Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, height);
				Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
				float num2 = this.scrollPosition.y - 24f;
				float num3 = this.scrollPosition.y + outRect.height;
				for (int i = 0; i < storeIncidents.Count; i++)
				{
					if (num > num2 && num < num3)
					{
						Rect rect3 = new Rect(0f, num, viewRect.width, 24f);
						this.DoRow(rect3, storeIncidents[i], i);
					}
					num += 24f;
				}
				Widgets.EndScrollView();
			}
			else
			{
				Widgets.NoneLabel(0f, outRect.width, null);
			}
			GUI.EndGroup();
        }

        private void DoRow(Rect rect, StoreIncident thingDef, int index)
		{
			Widgets.DrawHighlightIfMouseover(rect);
			TooltipHandler.TipRegion(rect, thingDef.description);
			GUI.BeginGroup(rect);
			Rect rect2 = new Rect(4f, (rect.height - 20f) / 2f, 20f, 20f);
			//Widgets.ThingIcon(rect2, thingDef);
			Rect rect3 = new Rect(rect2.xMax + 4f, 0f, rect.width - 60, 24f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;

            if (thingDef.cost < 1 && thingDef.defName != "Item")
            {
                GUI.color = Color.grey;
            }

			Widgets.Label(rect3, thingDef.label.CapitalizeFirst());
            Rect rect4 = new Rect(rect3.width, rect3.y, 60, rect3.height);
            if (Widgets.ButtonText(rect4, "Edit"))
            {
                Type type = typeof(StoreIncidentEditor);
                Find.WindowStack.TryRemove(type);
                StoreIncidentEditor window = new StoreIncidentEditor(thingDef);
                Find.WindowStack.Add(window);
            }
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
            GUI.color = Color.white;
			GUI.EndGroup();
		}

        private void DoBottomButtons(Rect rect)
		{
			Rect rect2 = new Rect(rect.width / 2f - this.BottomButtonSize.x / 2f, rect.height - 55f, this.BottomButtonSize.x, this.BottomButtonSize.y);
			if (Widgets.ButtonText(rect2, "CloseButton".Translate(), true, false, true))
			{
				this.Close(true);
			}
		}

        private void UpdateList()
        {
            lastSearch = searchQuery;
            if (searchQuery == "")
            {               
                storeIncidents = DefDatabase<StoreIncident>.AllDefs.OrderBy(s => s.label).ToList();       
                return;
            }

            storeIncidents = (from s in DefDatabase<StoreIncident>.AllDefs
                             orderby s.defName ascending
                             where s.label.ToLower().Contains(searchQuery.ToLower()) ||
                                s.abbreviation.ToLower().Contains(searchQuery.ToLower())
                             select s).ToList();
        }

        public override void Close(bool doCloseSound = true)
        {
            Store_IncidentEditor.UpdatePriceSheet();

            base.Close(doCloseSound);
        }

        private string searchQuery = "";
        private string lastSearch = null;
        private List<StoreIncident> storeIncidents = new List<StoreIncident>();
        private List<StoreIncident> disabledIncidents = new List<StoreIncident>();
        private Vector2 scrollPosition;
        private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);
    }
}
