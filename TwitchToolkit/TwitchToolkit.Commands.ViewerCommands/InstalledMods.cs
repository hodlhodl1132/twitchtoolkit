using System;
using System.Linq;
using ToolkitCore;
using TwitchLib.Client.Models.Interfaces;
using Verse;

namespace TwitchToolkit.Commands.ViewerCommands;

public class InstalledMods : CommandDriver
{
	public override void RunCommand(ITwitchMessage twitchMessage)
	{
		if ((DateTime.Now - Cooldowns.modsCommandCooldown).TotalSeconds <= 15.0)
		{
			return;
		}
		Cooldowns.modsCommandCooldown = DateTime.Now;
		string modmsg = "Version: " + Toolkit.Mod.Version + ", Mods: ";
		string[] mods = (from m in LoadedModManager.RunningMods
			select m.Name).ToArray();
		for (int i = 0; i < mods.Length; i++)
		{
			modmsg = modmsg + mods[i] + ", ";
			if (i == mods.Length - 1 || modmsg.Length > 256)
			{
				modmsg = modmsg.Substring(0, modmsg.Length - 2);
				TwitchWrapper.SendChatMessage(modmsg);
				modmsg = "";
			}
		}
	}
}
