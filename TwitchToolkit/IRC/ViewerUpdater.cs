using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace TwitchToolkit.IRC
{
    public class ViewerUpdater : TwitchInterfaceBase
    {
        public ViewerUpdater(Game game)
        {

        }

        public override void ParseCommand(IRCMessage msg)
        {
            foreach (KeyValuePair<string, string> pair in msg.Parameters)
            {
                switch (pair.Key)
                {
                    case "color":
                        if (pair.Value.Length > 6)
                        {
                            string colorCode = pair.Value.Remove(0, 1);
                            ToolkitSettings.ViewerColorCodes[msg.User.ToLower()] = colorCode;
                        }
                        break;
                    case "mod":
                        bool modValue = int.TryParse(pair.Value, out int modStatus);
                        if (modValue && modStatus == 1)
                        {
                            Viewers.GetViewer(msg.User).mod = true;
                        }
                        break;
                    case "subscriber":
                        bool subValue = int.TryParse(pair.Value, out int subStatus);
                        if (subValue && subStatus == 1)
                        {
                            Viewers.GetViewer(msg.User).subscriber = true;
                        }
                        break;
                }
            }
        }
    }
}
