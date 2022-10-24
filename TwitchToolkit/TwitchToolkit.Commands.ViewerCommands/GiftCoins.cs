using System.Linq;
using ToolkitCore;
using TwitchLib.Client.Models.Interfaces;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.Commands.ViewerCommands;

public class GiftCoins : CommandDriver
{
	public override void RunCommand(ITwitchMessage twitchMessage)
	{
		//IL_00f3: Unknown result type (might be due to invalid IL or missing erences)
		Viewer viewer = Viewers.GetViewer(twitchMessage.Username);
		string[] command = twitchMessage.Message.Split(' ');
		if (command.Count() < 3)
		{
			Log.Message("command not long enough");
			return;
		}
		string target = command[1].Replace("@", "");
		if (int.TryParse(command[2], out var amount) && amount > 0)
		{
			Viewer giftee = Viewers.GetViewer(target);
			if ((!ToolkitSettings.KarmaReqsForGifting || (giftee.GetViewerKarma() >= ToolkitSettings.MinimumKarmaToRecieveGifts && viewer.GetViewerKarma() >= ToolkitSettings.MinimumKarmaToSendGifts)) && viewer.GetViewerCoins() >= amount)
			{
				viewer.TakeViewerCoins(amount);
				giftee.GiveViewerCoins(amount);
				TwitchWrapper.SendChatMessage("@" + giftee.username + " " + Helper.ReplacePlaceholder((TaggedString)(Translator.Translate("TwitchToolkitGiftCoins")), null, null, null, null, null, null, null, null, amount: amount.ToString(), from: viewer.username));
				Store_Logger.LogGiftCoins(viewer.username, giftee.username, amount);
			}
		}
	}
}
