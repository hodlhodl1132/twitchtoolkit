using System;
using ToolkitCore;
using TwitchLib.Client.Models.Interfaces;
using Verse;

namespace TwitchToolkit.Commands.ModCommands;

public class SetKarma : CommandDriver
{
	public override void RunCommand(ITwitchMessage twitchMessage)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing erences)
		try
		{
			string[] command = twitchMessage.Message.Split(' ');
			if (command.Length >= 3)
			{
				string target = command[1].Replace("@", "");
				if (int.TryParse(command[2], out var amount))
				{
					Viewer targeted = Viewers.GetViewer(target);
					targeted.SetViewerKarma(amount);
					TwitchWrapper.SendChatMessage("@" + twitchMessage.Username + Helper.ReplacePlaceholder((TaggedString)(Translator.Translate("TwitchToolkitSetKarma")), null, null, null, null, null, null, null, null, null, null, null, null, targeted.username, null, amount.ToString()));
				}
			}
		}
		catch (InvalidCastException e)
		{
			Helper.Log("Invalid Check User Command " + e.Message);
		}
	}
}
