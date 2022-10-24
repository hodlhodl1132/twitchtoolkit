using TwitchToolkit.IncidentHelpers.SettingsWindows;
using TwitchToolkit.Incidents;
using Verse;

namespace TwitchToolkit.IncidentHelpers.IncidentHelper_Settings;

public class AddTraitSettings : IncidentHelperVariablesSettings
{
	public static int maxTraits = 4;

	public override void ExposeData()
	{
		Scribe_Values.Look<int>(ref maxTraits, "AddTraitSettings.maxTraits", 4, false);
	}

	public override void EditSettings()
	{
		Window_AddTrait window = new Window_AddTrait();
		Find.WindowStack.TryRemove(typeof(Window_AddTrait), true);
		Find.WindowStack.Add((Window)(object)window);
	}
}
