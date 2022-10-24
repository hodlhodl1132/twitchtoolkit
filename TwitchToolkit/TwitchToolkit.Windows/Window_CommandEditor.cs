using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using JetBrains.Annotations;
using TwitchToolkit.Commands;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Windows;

public class Window_CommandEditor : Window
{
	private static int removedDefs;

	private readonly Command command;

	private bool checkedForBackup = false;

	private bool haveBackup = false;

	private bool deleteWarning = false;

	public Window_CommandEditor(Command command)
	{
		base.doCloseButton = true;
		this.command = command;
		if (command == null)
		{
			throw new ArgumentNullException();
		}
		MakeSureSaveExists(force: true);
	}

	public override void DoWindowContents(Rect inRect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0007: Expected O, but got Unknown
		//IL_0008: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0030: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0209: Unknown result type (might be due to invalid IL or missing erences)
		Listing_Standard listing = new Listing_Standard();
		((Listing)listing).Begin(inRect);
		listing.Label("Editing Command " + GenText.CapitalizeFirst(((Def)command).label), -1f, (string)null);
		command.command = listing.TextEntryLabeled("Command - !", command.command, 1);
		listing.CheckboxLabeled("Enabled", ref command.enabled, (string)null);
		if (command.isCustomMessage)
		{
			listing.CheckboxLabeled("Require Separate Room if Enabled", ref command.shouldBeInSeparateRoom, "Will require viewers to only use this command in secondary channel if enabled");
			listing.CheckboxLabeled("Require Mod Status", ref command.requiresMod, "Will require viewers to have mod status to use this command");
			listing.CheckboxLabeled("Require Admin Status", ref command.requiresAdmin, "Will only allow channel owner to run this command");
			command.outputMessage = listing.TextEntry(command.outputMessage, 5);
			((Listing)listing).Gap(12f);
			if (listing.ButtonText("View Available Tags", (string)null))
			{
				Application.OpenURL("https://github.com/hodldeeznuts/twitchtoolkit/wiki/Commands#tags");
			}
			((Listing)listing).Gap(24f);
			if (!deleteWarning && listing.ButtonTextLabeled("Delete Custom Command", "Delete"))
			{
				deleteWarning = true;
			}
			if (deleteWarning && listing.ButtonTextLabeled("Delete Custom Command", "Are you sure?"))
			{
				if (ToolkitSettings.CustomCommandDefs.Contains(((Def)command).defName))
				{
					ToolkitSettings.CustomCommandDefs = ToolkitSettings.CustomCommandDefs.Where((string s) => s != ((Def)command).defName).ToList();
					IEnumerable<Command> toRemove = Enumerable.Empty<Command>();
					toRemove = toRemove.Concat(new Command[1] { command });
					RemoveStuffFromDatabase(((object)command).GetType(), toRemove.Cast<Def>());
				}
				((Window)this).Close(true);
			}
			if (deleteWarning)
			{
				listing.Label("(Must restart for deletions to take effect)", -1f, (string)null);
			}
		}
		((Listing)listing).End();
	}

	private void MakeSureSaveExists(bool force = false)
	{
		checkedForBackup = true;
		haveBackup = CommandEditor.CopyExists(command);
		if (!haveBackup || force)
		{
			CommandEditor.SaveCopy(command);
		}
	}

	public override void PostClose()
	{
		MakeSureSaveExists(force: true);
		((Mod)Toolkit.Mod).WriteSettings();
	}

	private static void RemoveStuffFromDatabase(Type databaseType, [NotNull] IEnumerable<Def> defs)
	{
		IEnumerable<Def> enumerable = (defs as Def[]) ?? defs.ToArray();
		if (!enumerable.Any())
		{
			return;
		}
		Traverse rm = Traverse.Create(databaseType).Method("Remove", new object[1] { enumerable.First() });
		foreach (Def def in enumerable)
		{
			removedDefs++;
			rm.GetValue(new object[1] { def });
		}
	}
}
