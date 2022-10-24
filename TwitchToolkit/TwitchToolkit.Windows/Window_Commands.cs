using System.Collections.Generic;
using System.Linq;
using TwitchToolkit.Commands;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Windows;

public class Window_Commands : Window
{
	private string searchQuery = "";

	private string lastSearch = null;

	private List<Command> allCommands = new List<Command>();

	private Vector2 scrollPosition;

	private readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

	public override void DoWindowContents(Rect inRect)
	{
		if (searchQuery != lastSearch)
		{
			UpdateList();
		}
		Rect rect = new Rect(0f, 0f, ((Rect)(inRect)).width, 60f);
        Text.Font =((GameFont)2);
		Text.Anchor =((TextAnchor)4);
		Widgets.Label(rect, "All Commands");
		Text.Font =((GameFont)1);
		Text.Anchor =((TextAnchor)0);
		Rect search = new Rect(0f, ((Rect)(rect)).height, ((Rect)(inRect)).width / 2f, 26f);
		searchQuery = Widgets.TextEntryLabeled(search, "Search:", searchQuery);
		Rect resetButton = new Rect(((Rect)(search)).x, ((Rect)(search)).y + 28f, ((Rect)(search)).width, 26f);
        if (Widgets.ButtonText(resetButton, "Reset all Commands", true, true, true))
		{
			CommandEditor.LoadBackups();
		}
		resetButton.x =(((Rect)( resetButton)).x + (((Rect)( resetButton)).width + 10f));
		if (Widgets.ButtonText(resetButton, "Create Command", true, true, true))
		{
			Window_NewCustomCommand window = new Window_NewCustomCommand();
			Find.WindowStack.TryRemove(((object)window).GetType(), true);
			Find.WindowStack.Add((Window)(object)window);
			((Window)this).Close(true);
		}
		inRect.y = (((Rect)( inRect)).yMin + 120f);
		Widgets.DrawMenuSection(inRect);
		inRect = GenUI.ContractedBy(inRect, 17f);
		GUI.BeginGroup(inRect);
		Rect rect2 = GenUI.AtZero(inRect);
		DoBottomButtons(rect2);
		Rect outRect = rect2;
		outRect.y = (((Rect)( outRect)).yMax - 65f);
		if (allCommands.Count > 0)
		{
			float height = (float)allCommands.Count * 24f;
			float num = 0f;
			Rect viewRect = new Rect(0f, 0f, ((Rect)(outRect)).width - 16f, height);
			Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect, true);
			float num2 = scrollPosition.y - 24f;
			float num3 = scrollPosition.y + ((Rect)( outRect)).height;
			Rect rect3 = default(Rect);
			for (int i = 0; i < allCommands.Count; i++)
			{
				if (num > num2 && num < num3)
				{
					rect3 = new Rect(0f, num, ((Rect)( viewRect)).width, 24f);
					DoRow(rect3, allCommands[i], i);
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

	private void DoRow(Rect rect, Command command, int index)
	{
		Widgets.DrawHighlightIfMouseover(rect);
		GUI.BeginGroup(rect);
		Rect rect2 = default(Rect);
		rect2 = new Rect(4f, (((Rect)( rect)).height - 20f) / 2f, 20f, 20f);
		Rect rect3 = default(Rect);
		rect3 = new Rect(((Rect)( rect2)).xMax + 4f, 0f, ((Rect)( rect)).width - 60f, 24f);
		Text.Anchor = ((TextAnchor)3);
		Text.WordWrap = false;
		Widgets.Label(rect3, GenText.CapitalizeFirst(command.Label));
		Rect rect4 = default(Rect);
		rect4 = new Rect(((Rect)( rect3)).width, ((Rect)( rect3)).y, 60f, ((Rect)( rect3)).height);
		if (Widgets.ButtonText(rect4, "Edit", true, true, true))
		{
			Window_CommandEditor window = new Window_CommandEditor(command);
			Find.WindowStack.TryRemove(((object)window).GetType(), true);
			Find.WindowStack.Add((Window)(object)window);
		}
		Text.Anchor =((TextAnchor)0);
		Text.WordWrap = (true);
		GUI.color = (Color.white);
		GUI.EndGroup();
	}

	private void DoBottomButtons(Rect rect)
	{
		Rect rect2 = new Rect(((Rect)(rect)).width / 2f - BottomButtonSize.x / 2f, ((Rect)(rect)).height - 55f, BottomButtonSize.x, BottomButtonSize.y);
        if (Widgets.ButtonText(rect2, (TaggedString)(Translator.Translate("CloseButton")), true, false, true))
		{
			((Window)this).Close(true);
		}
	}

	private void UpdateList()
	{
		allCommands = (from s in DefDatabase<Command>.AllDefs
			where searchQuery == "" || ((Def)s).defName.ToLower().Contains(searchQuery.ToLower()) || ((Def)s).defName.ToLower() == searchQuery.ToLower() || string.Join("", ((Def)s).label.Split(' ')).ToLower().Contains(string.Join("", searchQuery.Split(' ')).ToLower()) || string.Join("", ((Def)s).label.Split(' ')).ToLower() == string.Join("", searchQuery.Split(' ')).ToLower()
			select s).ToList();
		lastSearch = searchQuery;
	}
}
