using System.Collections.Generic;
using System.Linq;
using ToolkitCore;
using TwitchLib.Client.Models.Interfaces;
using Verse;

namespace TwitchToolkit.Commands.ViewerCommands;

public class AvailableCommands : CommandDriver
{
	public override void RunCommand(ITwitchMessage twitchMessage)
	{
		List<Command> commands = (from s in DefDatabase<Command>.AllDefs
			where !s.requiresAdmin && !s.requiresMod && s.enabled
			select s).ToList();
		string output = "@" + twitchMessage.Username + " viewer commands: ";
		for (int i = 0; i < commands.Count; i++)
		{
			output = output + "!" + commands[i].command;
			if (i < commands.Count - 1)
			{
				output += ", ";
			}
		}
		TwitchWrapper.SendChatMessage(output);
	}
}
