using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.IRC;
using Verse;

namespace TwitchToolkit
{
    public class Command : Def
    {

        public void RunCommand(IRCMessage message)
        {
            if (command == null)
            {
                throw new Exception("Command is null");
            }

            CommandDriver driver = (CommandDriver)Activator.CreateInstance(commandDriver);
            driver.command = this;
            driver.RunCommand(message);
        }

        public string command = null;

        public bool enabled = true;

        public bool shouldBeInSeparateRoom = false;

        public Type commandDriver = typeof(CommandDriver);

        public bool requiresMod = false;

        public bool requiresAdmin = false;

        public string outputMessage = "";

        public bool isCustomMessage = false;
    }

    public class CommandDriver
    {
        public Command command = null;

        public virtual void RunCommand(IRCMessage message)
        {
            Toolkit.client.SendMessage($@"{message.User} {command.outputMessage}");
        }
    }
}
