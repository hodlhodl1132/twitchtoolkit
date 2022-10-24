using UnityEngine;
using Verse;

namespace TwitchToolkit.Storytellers.StorytellerPackWindows;

public class Window_StorytellerPacks : Window
{
	public override Vector2 InitialSize => new Vector2(800f, 800f);

	public Window_StorytellerPacks()
	{
		base.doCloseButton = true;
	}

	public override void DoWindowContents(Rect inRect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0007: Expected O, but got Unknown
		//IL_0008: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0022: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0047: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0065: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing erences)
		Listing_Standard listing = new Listing_Standard();
		((Listing)listing).Begin(inRect);
		Text.Font =((GameFont)2);
		listing.Label("Storyteller Packs", -1f, (string)null);
		Text.Font =((GameFont)1);
		((Listing)listing).GapLine(12f);
		listing.Label("StorytellerPacks are packs of different types of votes. They each have their own settings, can be activated at the same time, and can be activated alongside normal storytellers.", -1f, (string)null);
		((Listing)listing).GapLine(12f);
		listing.Label("All storyteller packs will consider the global weights when choosing which votes to use. A weight of 0 would never be picked, meaning it is disabled.", -1f, (string)null);
		((Listing)listing).Gap(12f);
		if (listing.ButtonTextLabeled("Edit Global Vote Weights", "Edit Weights"))
		{
			Window_GlobalVoteWeights window6 = new Window_GlobalVoteWeights();
			Find.WindowStack.TryRemove(((object)window6).GetType(), true);
			Find.WindowStack.Add((Window)(object)window6);
		}
		((Listing)listing).Gap(12f);
		((Listing)listing).GapLine(12f);
		listing.Label("Enabled Storyteller Packs", -1f, (string)null);
		((Listing)listing).Gap(24f);
		((Listing)listing).ColumnWidth =(((Rect)( inRect)).width / 2f - 20f);
		listing.CheckboxLabeled("<color=#6441A4>Torytalker</color> - Classic / Most Balanced", ref ToolkitSettings.ToryTalkerEnabled, (string)null);
		((Listing)listing).Gap(12f);
		if (listing.ButtonTextLabeled("ToryTalker Pack", "View"))
		{
			Window_ToryTalkerSettings window5 = new Window_ToryTalkerSettings();
			Find.WindowStack.TryRemove(((object)window5).GetType(), true);
			Find.WindowStack.Add((Window)(object)window5);
		}
		((Listing)listing).Gap(24f);
		listing.CheckboxLabeled("<color=#4BB543>HodlBot</color> - Random by Category / Type", ref ToolkitSettings.HodlBotEnabled, (string)null);
		((Listing)listing).Gap(12f);
		if (listing.ButtonTextLabeled("HodlBot Pack", "View"))
		{
			Window_HodlBotSettings window4 = new Window_HodlBotSettings();
			Find.WindowStack.TryRemove(((object)window4).GetType(), true);
			Find.WindowStack.Add((Window)(object)window4);
		}
		((Listing)listing).Gap(24f);
		listing.CheckboxLabeled("<color=#CF0E0F>UristBot</color> - Raids Strategies / Diseases", ref ToolkitSettings.UristBotEnabled, (string)null);
		((Listing)listing).Gap(12f);
		if (listing.ButtonTextLabeled("UristBot Pack", "View"))
		{
			Window_UristBotSettings window3 = new Window_UristBotSettings();
			Find.WindowStack.TryRemove(((object)window3).GetType(), true);
			Find.WindowStack.Add((Window)(object)window3);
		}
		((Listing)listing).Gap(24f);
		listing.CheckboxLabeled("<color=#1482CB>Milasandra</color> - Threats OnOffCycle", ref ToolkitSettings.MilasandraEnabled, (string)null);
		((Listing)listing).Gap(12f);
		if (listing.ButtonTextLabeled("Milasandra Pack", "View"))
		{
			Window_Milasandra window2 = new Window_Milasandra();
			Find.WindowStack.TryRemove(((object)window2).GetType(), true);
			Find.WindowStack.Add((Window)(object)window2);
		}
		((Listing)listing).NewColumn();
		((Listing)listing).Gap(200f);
		listing.CheckboxLabeled("<color=#BF0030>Mercurius</color> - Misc Events Cycle", ref ToolkitSettings.MercuriusEnabled, (string)null);
		((Listing)listing).Gap(12f);
		if (listing.ButtonTextLabeled("Mercurius Pack", "View"))
		{
			Window_Mercurius window = new Window_Mercurius();
			Find.WindowStack.TryRemove(((object)window).GetType(), true);
			Find.WindowStack.Add((Window)(object)window);
		}
		((Listing)listing).End();
	}
}
