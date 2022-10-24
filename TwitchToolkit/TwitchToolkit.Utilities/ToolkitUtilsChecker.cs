using Verse;

namespace TwitchToolkit.Utilities;

[StaticConstructorOnStartup]
public static class ToolkitUtilsChecker
{
	public static bool ToolkitUtilsInstalled;

	public static bool ToolkitUtilsActive;

	static ToolkitUtilsChecker()
	{
		ToolkitUtilsInstalled = false;
		ToolkitUtilsActive = false;
		foreach (ModMetaData modMetaData in ModLister.AllInstalledMods)
		{
			if (modMetaData.Name == "ToolkitUtils")
			{
				ToolkitUtilsInstalled = true;
				Log.Message("ToolkitUtils Installed");
				if (modMetaData.Active)
				{
					ToolkitUtilsActive = true;
					Log.Message("ToolkitUtils Active");
				}
				break;
			}
		}
	}
}
