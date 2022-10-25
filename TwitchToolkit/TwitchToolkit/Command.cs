using System;
using System.Threading.Tasks;
using TwitchLib.Client.Models.Interfaces;
using Verse;

namespace TwitchToolkit;

public class Command : Def
{
	public string command = null;

	public bool enabled = true;

	public bool shouldBeInSeparateRoom = false;

	public Type commandDriver = typeof(CommandDriver);

	public bool requiresMod = false;

	public bool requiresAdmin = false;

	public string outputMessage = "";

	public bool isCustomMessage = false;

	public string Label
	{
		get
		{
			if (base.label != null && base.label != "")
			{
				return base.label;
			}
			return base.defName;
		}
	}

	public void RunCommand(ITwitchMessage twitchMessage)
	{
		Task.Run(() =>
		{
            if (command == null)
            {
                throw new Exception("Command is null");
            }
            CommandDriver driver = (CommandDriver)Activator.CreateInstance(commandDriver);
            driver.command = this;
            driver.RunCommand(twitchMessage);
        });
	}
}
