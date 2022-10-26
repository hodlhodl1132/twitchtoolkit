using TwitchToolkit.IncidentHelpers.IncidentHelper_Settings;
using UnityEngine;
using Verse;

namespace TwitchToolkit.IncidentHelpers.SettingsWindows;

public class Window_LevelPawn : Window
{
	private string xpBuffer = "";

	public Window_LevelPawn()
	{
		base.doCloseButton = true;
	}

	public override void DoWindowContents(Rect inRect)
	{
        Listing_Standard listing = new Listing_Standard();
		((Listing)listing).Begin(inRect);
		listing.Label("Level Pawn Settings", -1f, (string)null);
		xpBuffer = LevelPawnSettings.xpMultiplier.ToString();
		listing.TextFieldNumericLabeled<float>("XP Multiplier", ref LevelPawnSettings.xpMultiplier, ref xpBuffer, 0.5f, 5f);
		((Listing)listing).End();
	}
}
