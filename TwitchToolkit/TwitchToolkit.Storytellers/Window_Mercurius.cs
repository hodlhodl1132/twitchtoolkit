using UnityEngine;
using Verse;

namespace TwitchToolkit.Storytellers;

public class Window_Mercurius : Window
{
	public override Vector2 InitialSize => new Vector2(500f, 700f);

	public Window_Mercurius()
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
		Listing_Standard listing = new Listing_Standard();
		((Listing)listing).Begin(inRect);
		Text.Font =((GameFont)2);
		listing.Label("<color=#BF0030>Mercurius</color> Settings", -1f, (string)null);
		Text.Font =((GameFont)1);
		((Listing)listing).Gap(12f);
		((Listing)listing).GapLine(12f);
		listing.Label("Mercurius generates events in intervals through a cycle generator.", -1f, (string)null);
		((Listing)listing).Gap(12f);
		listing.Label("You will increasingly get more events the more days that pass.", -1f, (string)null);
		((Listing)listing).End();
	}
}
