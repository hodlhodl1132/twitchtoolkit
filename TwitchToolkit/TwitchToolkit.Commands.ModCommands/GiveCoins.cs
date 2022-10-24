using System;
using ToolkitCore;
using TwitchLib.Client.Models.Interfaces;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.Commands.ModCommands;

public class GiveCoins : CommandDriver
{
	public override void RunCommand(ITwitchMessage twitchMessage)
	{
		//IL_0094: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0099: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0107: Unknown result type (might be due to invalid IL or missing erences)
		try
		{
			string[] command = twitchMessage.Message.Split(' ');
			if (command.Length >= 3)
			{
				string receiver = command[1].Replace("@", "");
				int amount;
				if (twitchMessage.Username.ToLower() != ToolkitSettings.Channel.ToLower() && receiver.ToLower() == twitchMessage.Username.ToLower())
				{
					TwitchWrapper.SendChatMessage((TaggedString)("@" + twitchMessage.Username + " " + Translator.Translate("TwitchToolkitModCannotGiveCoins")));
				}
				else if (int.TryParse(command[2], out amount))
				{
					Viewer giftee = Viewers.GetViewer(receiver);
					Helper.Log($"Giving viewer {giftee.username} {amount} coins");
					giftee.GiveViewerCoins(amount);
					TwitchWrapper.SendChatMessage("@" + twitchMessage.Username + " " + Helper.ReplacePlaceholder((TaggedString)(Translator.Translate("TwitchToolkitGivingCoins")), null, null, null, null, null, null, null, null, null, null, viewer: giftee.username, amount: amount.ToString(), mod: null, newbalance: giftee.coins.ToString()));
					Store_Logger.LogGiveCoins(twitchMessage.Username, giftee.username, amount);
				}
			}
		}
		catch (InvalidCastException e)
		{
			Helper.Log("Invalid Give Viewer Coins Command " + e.Message);
		}
	}
}
