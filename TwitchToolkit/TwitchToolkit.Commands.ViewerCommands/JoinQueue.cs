using ToolkitCore;
using TwitchLib.Client.Models.Interfaces;
using TwitchToolkit.PawnQueue;
using Verse;

namespace TwitchToolkit.Commands.ViewerCommands;

public class JoinQueue : CommandDriver
{
	public override void RunCommand(ITwitchMessage twitchMessage)
	{
		Viewer viewer = Viewers.GetViewer(twitchMessage.Username);
		GameComponentPawns pawnComponent = Current.Game.GetComponent<GameComponentPawns>();
		if (pawnComponent.HasUserBeenNamed(twitchMessage.Username) || pawnComponent.UserInViewerQueue(twitchMessage.Username))
		{
			return;
		}
		if (ToolkitSettings.ChargeViewersForQueue)
		{
			if (viewer.GetViewerCoins() < ToolkitSettings.CostToJoinQueue)
			{
				TwitchWrapper.SendChatMessage($"@{twitchMessage.Username} you do not have enough coins to purchase a ticket, it costs {ToolkitSettings.CostToJoinQueue} and you have {viewer.GetViewerCoins()}.");
				return;
			}
			viewer.TakeViewerCoins(ToolkitSettings.CostToJoinQueue);
		}
		pawnComponent.AddViewerToViewerQueue(twitchMessage.Username);
		TwitchWrapper.SendChatMessage("@" + twitchMessage.Username + " you have purchased a ticket and are in the queue!");
	}
}
