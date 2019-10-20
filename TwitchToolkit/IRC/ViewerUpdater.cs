using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.PawnQueue;
using TwitchToolkit.Store;
using TwitchToolkit.Viewers;
using UnityEngine;
using Verse;

namespace TwitchToolkit.IRC
{
    public class ViewerUpdater : TwitchInterfaceBase
    {
        public ViewerUpdater(Game game)
        {

        }

        public override void ParseCommand(TwitchIRCMessage msg)
        {
            foreach (KeyValuePair<string, string> pair in msg.Parameters)
            {
                //Helper.Log(pair.Key + " : " + pair.Value);
                switch (pair.Key)
                {
                    case "color":
                        if (pair.Value == null) break;
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
                        if (msg.Viewer.Mod || pair.Value == null) break;
                        bool modValue = int.TryParse(pair.Value, out int modStatus);
                        if (!msg.Viewer.Mod && modValue && modStatus == 1)
                        {
                            msg.Viewer.ToggleMod();
                        }
                        break;
                    case "subscriber":
                        if (pair.Value == null) break;
                        bool subValue = int.TryParse(pair.Value, out int subStatus);
                        if (subValue && subStatus == 1)
                        {
                            msg.Viewer.Subscriber = true;
                        }
                        else
                        {
                            msg.Viewer.Subscriber = false;
                        }
                        break;

                    case "badges":
                        if (pair.Value == null) break;
                        IEnumerable<string> badges = pair.Value.Split(',');
                        foreach (string badge in badges)
                        {
                            if (badge == "vip/1")
                            {
                                msg.Viewer.VIP = true;
                                break;
                            }
                        }
                        msg.Viewer.VIP = false;
                        break;
                }
            }

            Store_Logger.LogString("Parsed command parameters");
        }

        private static Color GetColorFromHex(string hex)
        {
            if (hex.Length != 6)
            {
                Helper.Log("Invalid RGB color generated from hex: " + hex);
                return new Color();
            }

            bool parseString = ColorUtility.TryParseHtmlString("#" + hex, out Color color);

            return color;
        }
    }
}
