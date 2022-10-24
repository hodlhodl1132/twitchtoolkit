using ToolkitCore;
using TwitchLib.Client.Models.Interfaces;
using Verse;

namespace TwitchToolkit.Commands.ModCommands;

public class ToggleCoins : CommandDriver
{
	public override void RunCommand(ITwitchMessage twitchMessage)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0030: Unknown result type (might be due to invalid IL or missing erences)
		//IL_003a: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0044: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0049: Unknown result type (might be due to invalid IL or missing erences)
		//IL_007d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0082: Unknown result type (might be due to invalid IL or missing erences)
		//IL_008c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0096: Unknown result type (might be due to invalid IL or missing erences)
		//IL_009b: Unknown result type (might be due to invalid IL or missing erences)
		if (ToolkitSettings.EarningCoins)
		{
			ToolkitSettings.EarningCoins = false;
			TwitchWrapper.SendChatMessage((TaggedString)("@" + twitchMessage.Username + " " + Translator.Translate("TwitchToolkitEarningCoinsMessage") + " " + Translator.Translate("TwitchToolkitOff")));
		}
		else
		{
			ToolkitSettings.EarningCoins = true;
			TwitchWrapper.SendChatMessage((TaggedString)("@" + twitchMessage.Username + " " + Translator.Translate("TwitchToolkitEarningCoinsMessage") + " " + Translator.Translate("TwitchToolkitOn")));
		}
	}
}
