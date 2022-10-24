using ToolkitCore;
using TwitchLib.Client.Models.Interfaces;
using Verse;

namespace TwitchToolkit.Commands.ViewerCommands;

public class CheckBalance : CommandDriver
{
	public override void RunCommand(ITwitchMessage twitchMessage)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing erences)
		Viewer viewer = Viewers.GetViewer(twitchMessage.Username);
		TwitchWrapper.SendChatMessage("@" + viewer.username + " " + Helper.ReplacePlaceholder((TaggedString)(Translator.Translate("TwitchToolkitBalanceMessage")), null, null, null, null, null, null, null, null, null, null, viewer.GetViewerCoins().ToString(), null, null, null, viewer.GetViewerKarma().ToString()));
	}
}
