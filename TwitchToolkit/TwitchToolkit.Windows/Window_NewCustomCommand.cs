using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Windows;

public class Window_NewCustomCommand : Window
{
	private string defName = "";

	private string output = "";

	private int outputFrames = 0;

	public Window_NewCustomCommand()
	{
		base.doCloseButton = true;
	}

	public override void DoWindowContents(Rect inRect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0007: Expected O, but got Unknown
		//IL_0008: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0051: Unknown result type (might be due to invalid IL or missing erences)
		Listing_Standard listing = new Listing_Standard();
		((Listing)listing).Begin(inRect);
		defName = listing.TextEntryLabeled("Command Name", defName, 1);
		if (listing.ButtonTextLabeled("New Command", "Create"))
		{
			TrySubmitNewCommand();
		}
		listing.Label(output, -1f, (string)null);
		if (outputFrames > 0)
		{
			outputFrames--;
		}
		else
		{
			output = "";
		}
		((Listing)listing).End();
	}

	private void TrySubmitNewCommand()
	{
		if (defName == "")
		{
			NewOutputMessage("Name must be longer than 0 chars.");
			return;
		}
		string sanitizedDefName = string.Join("", defName.Split(' ')).ToLower();
		if (DefDatabase<Command>.AllDefs.Any((Command s) => ((Def)s).defName.ToLower() == sanitizedDefName))
		{
			NewOutputMessage("Command name is already taken");
			return;
		}
		Command newCommand = new Command();
		((Def)newCommand).defName = GenText.CapitalizeFirst(sanitizedDefName);
		newCommand.isCustomMessage = true;
		((Def)newCommand).label = sanitizedDefName;
		DefDatabase<Command>.Add(newCommand);
		if (ToolkitSettings.CustomCommandDefs == null)
		{
			ToolkitSettings.CustomCommandDefs = new List<string>();
		}
		ToolkitSettings.CustomCommandDefs.Add(((Def)newCommand).defName);
		((Mod)Toolkit.Mod).WriteSettings();
		Window_CommandEditor window = new Window_CommandEditor(newCommand);
		Find.WindowStack.TryRemove(((object)window).GetType(), true);
		Find.WindowStack.Add((Window)(object)window);
		((Window)this).Close(true);
	}

	private void NewOutputMessage(string message)
	{
		output = message;
		outputFrames = 240;
	}
}
