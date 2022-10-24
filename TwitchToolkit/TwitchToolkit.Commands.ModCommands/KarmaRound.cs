using ToolkitCore;
using TwitchLib.Client.Models.Interfaces;

namespace TwitchToolkit.Commands.ModCommands;

public class KarmaRound : CommandDriver
{
	public override void RunCommand(ITwitchMessage twitchMessage)
	{
		Viewers.AwardViewersCoins();
		TwitchWrapper.SendChatMessage("@" + twitchMessage.Username + " rewarding all active viewers coins.");
	}
}
