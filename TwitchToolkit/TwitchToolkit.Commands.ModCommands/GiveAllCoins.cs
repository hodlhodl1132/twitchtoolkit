using System;
using ToolkitCore;
using TwitchLib.Client.Models.Interfaces;
using Verse;

namespace TwitchToolkit.Commands.ModCommands;

public class GiveAllCoins : CommandDriver
{
	public override void RunCommand(ITwitchMessage twitchMessage)
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing erences)
		try
		{
			string[] command = twitchMessage.Message.Split(' ');
			if (command.Length < 2 || !int.TryParse(command[1], out var amount))
			{
				return;
			}
			foreach (Viewer vwr in Viewers.All)
			{
				vwr.GiveViewerCoins(amount);
			}
			TwitchWrapper.SendChatMessage("@" + twitchMessage.Username + " " + Helper.ReplacePlaceholder((TaggedString)(Translator.Translate("TwitchToolkitGiveAllCoins")), null, null, null, null, null, null, null, null, null, null, amount.ToString()));
		}
		catch (InvalidCastException e)
		{
			Helper.Log("Give All Coins Syntax Error " + e.Message);
		}
	}
}
