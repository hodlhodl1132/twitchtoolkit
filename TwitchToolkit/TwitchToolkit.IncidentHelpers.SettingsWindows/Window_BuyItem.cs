using TwitchToolkit.IncidentHelpers.IncidentHelper_Settings;
using UnityEngine;
using Verse;

namespace TwitchToolkit.IncidentHelpers.SettingsWindows;

public class Window_BuyItem : Window
{
	private string traitsBuffer = "";

	public Window_BuyItem()
	{
		base.doCloseButton = true;
	}

	public override void DoWindowContents(Rect inRect)
	{
        Listing_Standard listing = new Listing_Standard();
		((Listing)listing).Begin(inRect);
		listing.Label("Buy Item Settings", -1f, (string)null);
		traitsBuffer = AddTraitSettings.maxTraits.ToString();
		listing.CheckboxLabeled("Should items be researched before being buyable?", ref BuyItemSettings.mustResearchFirst, (string)null);
		((Listing)listing).End();
	}
}
