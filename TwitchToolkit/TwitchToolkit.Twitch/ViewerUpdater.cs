using ToolkitCore;
using TwitchLib.Client.Models.Interfaces;
using TwitchToolkit.PawnQueue;
using Verse;

namespace TwitchToolkit.Twitch;

public class ViewerUpdater : TwitchInterfaceBase
{
	public ViewerUpdater(Game game)
	{
	}

	public override void ParseMessage(ITwitchMessage twitchMessage)
	{
		if (twitchMessage.ChatMessage != null)
		{
			Viewer viewer = Viewers.GetViewer(twitchMessage.Username);
			GameComponentPawns component = Current.Game.GetComponent<GameComponentPawns>();
			ToolkitSettings.ViewerColorCodes[twitchMessage.Username.ToLower()] = twitchMessage.ChatMessage.ColorHex.Replace("#", "");
			if (twitchMessage.ChatMessage.IsModerator && !viewer.mod)
			{
				viewer.SetAsModerator();
			}
			if (twitchMessage.ChatMessage.IsSubscriber && !viewer.IsSub)
			{
				viewer.subscriber = true;
			}
			if (twitchMessage.ChatMessage.IsVip && !viewer.IsVIP)
			{
				viewer.vip = true;
			}
		}
	}
}
