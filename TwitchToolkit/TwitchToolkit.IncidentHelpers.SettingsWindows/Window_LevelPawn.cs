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
		//IL_0001: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0007: Expected O, but got Unknown
		//IL_0008: Unknown result type (might be due to invalid IL or missing erences)
		//IL_001b: Unknown result type (might be due to invalid IL or missing erences)
		Listing_Standard listing = new Listing_Standard();
		((Listing)listing).Begin(inRect);
		listing.Label("Level Pawn Settings", -1f, (string)null);
		xpBuffer = LevelPawnSettings.xpMultiplier.ToString();
		listing.TextFieldNumericLabeled<float>("XP Multiplier", ref LevelPawnSettings.xpMultiplier, ref xpBuffer, 0.5f, 5f);
		((Listing)listing).End();
	}
}
