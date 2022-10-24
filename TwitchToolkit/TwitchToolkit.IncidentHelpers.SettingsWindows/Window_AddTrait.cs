using TwitchToolkit.IncidentHelpers.IncidentHelper_Settings;
using UnityEngine;
using Verse;

namespace TwitchToolkit.IncidentHelpers.SettingsWindows;

public class Window_AddTrait : Window
{
	private string traitsBuffer = "";

	public Window_AddTrait()
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
		listing.Label("Add Trait Settings", -1f, (string)null);
		traitsBuffer = AddTraitSettings.maxTraits.ToString();
		listing.TextFieldNumericLabeled<int>("Maximum Traits", ref AddTraitSettings.maxTraits, ref traitsBuffer, 1f, 100f);
		((Listing)listing).End();
	}
}
