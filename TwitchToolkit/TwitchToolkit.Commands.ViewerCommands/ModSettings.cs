using ToolkitCore;
using TwitchLib.Client.Models.Interfaces;
using Verse;

namespace TwitchToolkit.Commands.ViewerCommands;

public class ModSettings : CommandDriver
{
	public override void RunCommand(ITwitchMessage twitchMessage)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing erences)
		//IL_003b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0052: Unknown result type (might be due to invalid IL or missing erences)
		//IL_005e: Unknown result type (might be due to invalid IL or missing erences)
		//IL_006e: Unknown result type (might be due to invalid IL or missing erences)
		Command buyCommand = DefDatabase<Command>.GetNamed("Buy", true);
		string minutess = ((ToolkitSettings.CoinInterval > 1) ? "s" : "");
		string storeon = (TaggedString)(buyCommand.enabled ? Translator.Translate("TwitchToolkitOn") : Translator.Translate("TwitchToolkitOff"));
		string earningcoins = (TaggedString)(ToolkitSettings.EarningCoins ? Translator.Translate("TwitchToolkitOn") : Translator.Translate("TwitchToolkitOff"));
		string quote = (TaggedString)(Translator.Translate("TwitchToolkitModSettings"));
		string amount = ToolkitSettings.CoinAmount.ToString();
		string first = ToolkitSettings.CoinInterval.ToString();
		string second = storeon;
		string third = earningcoins;
		string stats_message = Helper.ReplacePlaceholder(quote, null, null, null, null, null, null, null, null, null, null, amount, null, null, null, ToolkitSettings.KarmaCap.ToString(), first, second, third);
		TwitchWrapper.SendChatMessage(stats_message);
	}
}
