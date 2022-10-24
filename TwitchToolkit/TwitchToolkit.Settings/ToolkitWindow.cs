using TwitchToolkit.Windows;
using Verse;

namespace TwitchToolkit.Settings;

public abstract class ToolkitWindow : SettingsWindow
{
	public ToolkitWindow(Mod mod)
		: base(mod)
	{
	}
}
