using ToolkitCore;
using TwitchLib.Client.Models.Interfaces;
using Verse;

namespace TwitchToolkit.Commands.ViewerCommands;

public class WhatIsKarma : CommandDriver
{
	public override void RunCommand(ITwitchMessage twitchMessage)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing erences)
		//IL_002c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0046: Unknown result type (might be due to invalid IL or missing erences)
		Viewer viewer = Viewers.GetViewer(twitchMessage.Username);
		TwitchWrapper.SendChatMessage((TaggedString)("@" + viewer.username + " " + Translator.Translate("TwitchToolkitWhatIsKarma") + $" {viewer.GetViewerKarma()}%"));
	}
}
