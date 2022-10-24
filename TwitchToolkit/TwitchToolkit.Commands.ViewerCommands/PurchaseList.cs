using ToolkitCore;
using TwitchLib.Client.Models.Interfaces;
using Verse;

namespace TwitchToolkit.Commands.ViewerCommands;

public class PurchaseList : CommandDriver
{
	public override void RunCommand(ITwitchMessage twitchMessage)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0020: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0034: Unknown result type (might be due to invalid IL or missing erences)
		TwitchWrapper.SendChatMessage((TaggedString)("@" + twitchMessage.Username + " " + Translator.Translate("TwitchToolkitPurchaseList") + (" " + ToolkitSettings.CustomPricingSheetLink)));
	}
}
