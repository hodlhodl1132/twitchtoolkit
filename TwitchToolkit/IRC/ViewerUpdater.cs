using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.PawnQueue;
using TwitchToolkit.Store;
using UnityEngine;
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
                //Log.Warning($"{pair.Key} : {pair.Value}");

                return;

                switch (pair.Key)
                {
                    case "color":
                        if (pair.Value == null) return;
                        string colorCode = "";
                        if (pair.Value.Length > 6)
                        {
                            colorCode = pair.Value.Remove(0, 1);
                            ToolkitSettings.ViewerColorCodes[msg.User.ToLower()] = colorCode;
                        }
                        else
                        {
                            break;
                        }
                        GameComponentPawns component = Current.Game.GetComponent<GameComponentPawns>();

                        if (component.HasUserBeenNamed(msg.User))
                        {
                            Pawn pawn = component.PawnAssignedToUser(msg.User);

                            pawn.story.hairColor = GetColorFromHex(colorCode);
                        }

                        break;
                    case "mod":
                        if (pair.Value == null) return;
                        bool modValue = int.TryParse(pair.Value, out int modStatus);
                        if (modValue && modStatus == 1)
                        {
                            Viewers.GetViewer(msg.User).mod = true;
                        }
                        break;
                    case "subscriber":
                        if (pair.Value == null) return;
                        bool subValue = int.TryParse(pair.Value, out int subStatus);
                        if (subValue && subStatus == 1)
                        {
                            Viewers.GetViewer(msg.User).subscriber = true;
                        }
                        break;
                }
            }

            Store_Logger.LogString("Parsed command parameters");
        }

        private static Color GetColorFromHex(string hex)
        {
            if (hex.Length != 6)
            {
                Log.Warning("Invalid RGB color generated from hex: " + hex);
                return new Color();
            }

            bool parseString = ColorUtility.TryParseHtmlString("#" + hex, out Color color);

            return color;
        }
    }
}
