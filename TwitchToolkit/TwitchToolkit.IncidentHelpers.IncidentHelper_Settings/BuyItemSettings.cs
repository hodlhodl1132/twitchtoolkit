using TwitchToolkit.IncidentHelpers.SettingsWindows;
using TwitchToolkit.Incidents;
using Verse;

namespace TwitchToolkit.IncidentHelpers.IncidentHelper_Settings;

public class BuyItemSettings : IncidentHelperVariablesSettings
{
	public static bool mustResearchFirst = true;

	public override void ExposeData()
	{
		Scribe_Values.Look<bool>(ref mustResearchFirst, "BuyItemSettings.mustResearchFirst", true, false);
	}

	public override void EditSettings()
	{
		Window_BuyItem window = new Window_BuyItem();
		Find.WindowStack.TryRemove(typeof(Window_BuyItem), true);
		Find.WindowStack.Add((Window)(object)window);
	}
}
