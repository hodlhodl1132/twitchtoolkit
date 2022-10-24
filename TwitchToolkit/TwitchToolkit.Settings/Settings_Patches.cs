using System;
using System.Collections;
using System.Reflection;
using TwitchToolkit.Windows;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings;

internal class Settings_Patches
{
	public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
	{
		foreach (ToolkitExtension extension in Settings_ToolkitExtensions.GetExtensions)
		{
			if (optionsListing.ButtonTextLabeled(extension.mod.SettingsCategory(), "Settings"))
			{
				ConstructorInfo constructor = extension.windowType.GetConstructor(new Type[1] { typeof(Mod) });
				SettingsWindow window = constructor.Invoke(new object[1] { extension.mod }) as SettingsWindow;
				Type type = typeof(SettingsWindow);
				Find.WindowStack.TryRemove(type, true);
				Find.WindowStack.Add((Window)(object)window);
			}
		}
	}
}
