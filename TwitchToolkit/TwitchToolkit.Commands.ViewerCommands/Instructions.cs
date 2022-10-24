using ToolkitCore;
using TwitchLib.Client.Models.Interfaces;
using Verse;

namespace TwitchToolkit.Commands.ViewerCommands;

public class Instructions : CommandDriver
{
	public override void RunCommand(ITwitchMessage twitchMessage)
	{
		Command allCommandsCommand = DefDatabase<Command>.GetNamed("AvailableCommands", true);
		TwitchWrapper.SendChatMessage("@" + twitchMessage.Username + " the toolkit is a mod where you earn coins while you watch. Check out the bit.ly/toolkit-guide  or use !" + allCommandsCommand.command + " for a short list. " + GenText.CapitalizeFirst(ToolkitSettings.Channel) + " has a list of items/events to purchase at " + ToolkitSettings.CustomPricingSheetLink);
	}
}
