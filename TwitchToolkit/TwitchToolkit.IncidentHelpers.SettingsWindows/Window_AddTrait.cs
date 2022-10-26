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
        Listing_Standard listing = new Listing_Standard();
		((Listing)listing).Begin(inRect);
		listing.Label("Add Trait Settings", -1f, (string)null);
		traitsBuffer = AddTraitSettings.maxTraits.ToString();
		listing.TextFieldNumericLabeled<int>("Maximum Traits", ref AddTraitSettings.maxTraits, ref traitsBuffer, 1f, 100f);
		((Listing)listing).End();
	}
}
