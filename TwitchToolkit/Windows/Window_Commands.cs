using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Commands;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Windows
{
    public class Window_Commands : Window
    {
        public override void DoWindowContents(Rect inRect)
        {
            if (searchQuery != lastSearch)
            {
                UpdateList();
            }

            Rect rect = new Rect(0f, 0f, inRect.width, 60f);
            Text.Font = GameFont.Medium;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(rect, "All Commands");
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;
            Rect search = new Rect(0, rect.height, inRect.width / 2, 26f);
            searchQuery = Widgets.TextEntryLabeled(search, "Search:", searchQuery);
            Rect resetButton = new Rect(search.x, search.y + 28f, search.width, 26f);
            if (Widgets.ButtonText(resetButton, "Reset all Commands"))
            {
                CommandEditor.LoadBackups();
            }

            resetButton.x += resetButton.width + 10f;

            if (Widgets.ButtonText(resetButton, "Create Command"))
            {
                Window_NewCustomCommand window = new Window_NewCustomCommand();
                Find.WindowStack.TryRemove(window.GetType());
                Find.WindowStack.Add(window);
                Close();
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
            if (allCommands.Count > 0)
            {
                float height = (float)allCommands.Count * 24f;
                float num = 0f;
                Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, height);
                Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
                float num2 = this.scrollPosition.y - 24f;
                float num3 = this.scrollPosition.y + outRect.height;
                for (int i = 0; i < allCommands.Count; i++)
                {
                    if (num > num2 && num < num3)
                    {
                        Rect rect3 = new Rect(0f, num, viewRect.width, 24f);
                        this.DoRow(rect3, allCommands[i], i);
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

        private void DoRow(Rect rect, Command command, int index)
        {
            Widgets.DrawHighlightIfMouseover(rect);
            GUI.BeginGroup(rect);
            Rect rect2 = new Rect(4f, (rect.height - 20f) / 2f, 20f, 20f);
            //Widgets.ThingIcon(rect2, thingDef);
            Rect rect3 = new Rect(rect2.xMax + 4f, 0f, rect.width - 60, 24f);
            Text.Anchor = TextAnchor.MiddleLeft;
            Text.WordWrap = false;

            //if (command.Enabled < 1 && thingDef.defName != "Item")
            //{
            //    GUI.color = Color.grey;
            //}

            Widgets.Label(rect3, command.label.CapitalizeFirst());
            Rect rect4 = new Rect(rect3.width, rect3.y, 60, rect3.height);
            if (Widgets.ButtonText(rect4, "Edit"))
            {
                Window_CommandEditor window = new Window_CommandEditor(command);
                Find.WindowStack.TryRemove(window.GetType());
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
            allCommands = DefDatabase<Command>.AllDefs.Where(s =>
                searchQuery == "" ||
                s.defName.ToLower().Contains(searchQuery.ToLower()) ||
                s.defName.ToLower() == searchQuery.ToLower() ||
                string.Join("", s.label.Split(' ')).ToLower().Contains(string.Join("", searchQuery.Split(' ')).ToLower()) ||
                string.Join("", s.label.Split(' ')).ToLower() == string.Join("", searchQuery.Split(' ')).ToLower()
            ).ToList();

            lastSearch = searchQuery;
        }

        private string searchQuery = "";
        private string lastSearch = null;

        private List<Command> allCommands = new List<Command>();

        private Vector2 scrollPosition;

        private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);
    }
}
