using System;
using ToolkitCore;
using TwitchLib.Client.Models.Interfaces;
using Verse;

namespace TwitchToolkit.Commands.ModCommands;

public class CheckUser : CommandDriver
{
	public override void RunCommand(ITwitchMessage twitchMessage)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string[] command = twitchMessage.Message.Split(' ');
			if (command.Length >= 2)
			{
				string target = command[1].Replace("@", "");
				Viewer targeted = Viewers.GetViewer(target);
				TwitchWrapper.SendChatMessage("@" + twitchMessage.Username + " " + Helper.ReplacePlaceholder((TaggedString)(Translator.Translate("TwitchToolkitCheckUser")), null, null, null, null, null, null, null, null, null, null, viewer: targeted.username, amount: targeted.coins.ToString(), mod: null, newbalance: null, karma: targeted.GetViewerKarma().ToString()));
			}
		}
		catch (InvalidCastException e)
		{
			Helper.Log("Invalid Check User Command " + e.Message);
		}
	}
}
