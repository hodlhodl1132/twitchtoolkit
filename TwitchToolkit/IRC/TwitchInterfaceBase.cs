using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace TwitchToolkit.IRC
{
    public abstract class TwitchInterfaceBase : GameComponent
    {
        public abstract void ParseCommand(IRCMessage msg);
    }
}
