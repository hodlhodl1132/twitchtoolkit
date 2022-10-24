using System.Collections.Generic;
using Verse;

namespace TwitchToolkit.Settings;

[StaticConstructorOnStartup]
public static class Settings_ToolkitExtensions
{
	public static List<ToolkitExtension> GetExtensions { get; private set; }

	static Settings_ToolkitExtensions()
	{
		GetExtensions = new List<ToolkitExtension>();
	}

	public static void RegisterExtension(ToolkitExtension extension)
	{
		GetExtensions.Add(extension);
	}
}
