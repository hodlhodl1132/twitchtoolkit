using System.Linq;
using TwitchLib.Client.Models.Interfaces;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.Commands.ViewerCommands;

public class Buy : CommandDriver
{
	public override void RunCommand(ITwitchMessage twitchMessage)
	{
		Log.Warning("reached the override");
		Viewer viewer = Viewers.GetViewer(twitchMessage.Username);
		if (twitchMessage.Message.Split(' ').Count() >= 2)
		{
			Purchase_Handler.ResolvePurchase(viewer, twitchMessage);
		}
	}
}
