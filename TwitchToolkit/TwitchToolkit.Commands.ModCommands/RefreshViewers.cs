using ToolkitCore;
using TwitchLib.Client.Models.Interfaces;
using TwitchToolkitDev;

namespace TwitchToolkit.Commands.ModCommands;

public class RefreshViewers : CommandDriver
{
	public override void RunCommand(ITwitchMessage twitchMessage)
	{
		WebRequest_BeginGetResponse.Main("https://tmi.twitch.tv/group/user/" + ToolkitCoreSettings.channel_username.ToLower() + "/chatters", Viewers.SaveUsernamesFromJsonResponse);
		TwitchWrapper.SendChatMessage("@" + twitchMessage.Username + " viewers have been reshed.");
	}
}
