using UnityEngine;
using Verse;

namespace TwitchToolkit.Storytellers.StorytellerPackWindows;

public class Window_Milasandra : Window
{
	public override Vector2 InitialSize => new Vector2(500f, 700f);

	public Window_Milasandra()
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
		//IL_0071: Unknown result type (might be due to invalid IL or missing erences)
		//IL_008f: Unknown result type (might be due to invalid IL or missing erences)
		Listing_Standard listing = new Listing_Standard();
		((Listing)listing).Begin(inRect);
		Text.Font =((GameFont)2);
		listing.Label("<color=#1482CB>Milasandra</color> Settings", -1f, (string)null);
		Text.Font =((GameFont)1);
		((Listing)listing).Gap(12f);
		((Listing)listing).GapLine(12f);
		listing.Label("Milasandra uses an on and off cycle to bring votes in waves, similar to Cassandra.", -1f, (string)null);
		((Listing)listing).Gap(12f);
		listing.Label("This pack is the closest to the base games basic forced raid cycle. You will experience lag generating these votes.", -1f, (string)null);
		((Listing)listing).Gap(12f);
		listing.Label("There are no settings to change because Milasandra will generate votes on the same timeline as Cassandra.", -1f, (string)null);
		((Listing)listing).End();
	}
}
