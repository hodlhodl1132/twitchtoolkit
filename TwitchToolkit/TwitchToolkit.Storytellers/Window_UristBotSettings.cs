using System;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Storytellers;

public class Window_UristBotSettings : Window
{
	public Window_UristBotSettings()
	{
		base.doCloseButton = true;
	}

	public override void DoWindowContents(Rect inRect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0007: Expected O, but got Unknown
		//IL_0008: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0022: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0053: Unknown result type (might be due to invalid IL or missing erences)
		Listing_Standard listing = new Listing_Standard();
		((Listing)listing).Begin(inRect);
		Text.Font =((GameFont)2);
		listing.Label("<color=#CF0E0F>UristBot</color> Settings", -1f, (string)null);
		Text.Font =((GameFont)1);
		((Listing)listing).Gap(12f);
		((Listing)listing).GapLine(12f);
		listing.Label("UristBot is still being developed. At the moment, it will make a small raid and let the viewers choose the raid strategy.", -1f, (string)null);
		((Listing)listing).Gap(12f);
		string uristbotMTBDays = Math.Truncate((double)ToolkitSettings.UristBotMTBDays * 100.0 / 100.0).ToString();
		listing.TextFieldNumericLabeled<float>("Average Days Between Events", ref ToolkitSettings.UristBotMTBDays, ref uristbotMTBDays, 0.5f, 10f);
		((Listing)listing).End();
	}
}
