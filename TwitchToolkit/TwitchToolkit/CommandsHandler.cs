using System;
using System.Linq;
using TwitchLib.Client.Models;
using TwitchLib.Client.Models.Interfaces;
using Verse;

namespace TwitchToolkit;

public static class CommandsHandler
{
	private static DateTime modsCommandCooldown = DateTime.MinValue;

	private static DateTime aliveCommandCooldown = DateTime.MinValue;

	public static void CheckCommand(ITwitchMessage twitchMessage)
	{
		Log.Message("Checking command - " + twitchMessage.Message);
		if (twitchMessage == null || twitchMessage.Message == null)
		{
			return;
		}
		string message = twitchMessage.Message;
		string user = twitchMessage.Username;
		Viewer viewer = Viewers.GetViewer(user);
		viewer.last_seen = DateTime.Now;
		if (viewer.IsBanned)
		{
            Log.Message("viewer is banned.");
            return;
		}
		Command commandDef = DefDatabase<Command>.AllDefs.ToList().Find((Command s) => twitchMessage.Message.StartsWith("!" + s.command));
		if (commandDef != null)
		{
			bool runCommand = true;
			if (commandDef.requiresMod && !viewer.mod && viewer.username.ToLower() != ToolkitSettings.Channel.ToLower())
			{
				runCommand = false;
            }
			if (commandDef.requiresAdmin && twitchMessage.Username.ToLower() != ToolkitSettings.Channel.ToLower())
			{
				runCommand = false;

            }
            if (!commandDef.enabled)
			{
				runCommand = false;
            }
            if (runCommand)
			{
                commandDef.RunCommand(twitchMessage);
			}
		}
	}

	public static bool SendToChatroom(ChatMessage msg)
	{
		return true;
	}
}
