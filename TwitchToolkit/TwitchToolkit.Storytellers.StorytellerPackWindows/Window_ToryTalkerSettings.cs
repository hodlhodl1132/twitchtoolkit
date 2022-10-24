using System;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Storytellers.StorytellerPackWindows;

public class Window_ToryTalkerSettings : Window
{
	public Window_ToryTalkerSettings()
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
		Text.Font = ((GameFont)2);
		listing.Label("<color=#6441A4>ToryTalker</color> Settings", -1f, (string)null);
		Text.Font =((GameFont)1);
		((Listing)listing).Gap(12f);
		((Listing)listing).GapLine(12f);
		listing.Label("Tory Talker uses the global weights and it's own weighting system based on events that have happened recently.", -1f, (string)null);
		((Listing)listing).Gap(12f);
		if (listing.ButtonTextLabeled("Edit Global Vote Weights", "Edit Weights"))
		{
			Window_GlobalVoteWeights window = new Window_GlobalVoteWeights();
			Find.WindowStack.TryRemove(((object)window).GetType(), true);
			Find.WindowStack.Add((Window)(object)window);
		}
		((Listing)listing).Gap(12f);
		string toryTalkerMTBDays = Math.Truncate((double)ToolkitSettings.ToryTalkerMTBDays * 100.0 / 100.0).ToString();
		listing.TextFieldNumericLabeled<float>("Average Days Between Events", ref ToolkitSettings.ToryTalkerMTBDays, ref toryTalkerMTBDays, 0.5f, 10f);
		((Listing)listing).End();
	}
}
