using TwitchToolkit.IncidentHelpers.SettingsWindows;
using TwitchToolkit.Incidents;
using Verse;

namespace TwitchToolkit.IncidentHelpers.IncidentHelper_Settings;

public class LevelPawnSettings : IncidentHelperVariablesSettings
{
	public static float xpMultiplier = 1f;

	public override void ExposeData()
	{
		Scribe_Values.Look<float>(ref xpMultiplier, "LevelPawnSettings.xpMultiplier", 1f, false);
	}

	public override void EditSettings()
	{
		Window_LevelPawn window = new Window_LevelPawn();
		Find.WindowStack.TryRemove(typeof(Window_LevelPawn), true);
		Find.WindowStack.Add((Window)(object)window);
	}
}
