using ToolkitCore;
using TwitchLib.Client.Models.Interfaces;
using Verse;

namespace TwitchToolkit.Commands.ViewerCommands;

public class ModInfo : CommandDriver
{
	public override void RunCommand(ITwitchMessage twitchMessage)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0020: Unknown result type (might be due to invalid IL or missing erences)
		//IL_002a: Unknown result type (might be due to invalid IL or missing erences)
		TwitchWrapper.SendChatMessage((TaggedString)("@" + twitchMessage.Username + " " + Translator.Translate("TwitchToolkitModInfo") + " https://discord.gg/qrtg224 !"));
	}
}
