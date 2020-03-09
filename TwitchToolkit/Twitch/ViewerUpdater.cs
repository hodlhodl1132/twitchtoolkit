using rim_twitch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchLib.Client.Models;
using TwitchToolkit.PawnQueue;
using TwitchToolkit.Store;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Twitch
{
    public class ViewerUpdater : TwitchInterfaceBase
    {
        public ViewerUpdater(Game game)
        {

        }

        public override void ParseCommand(ChatMessage msg)
        {
            Viewer viewer = Viewers.GetViewer(msg.Username);

            //foreach (KeyValuePair<string, string> pair in msg.Parameters)
            //{
            //    //Helper.Log(pair.Key + " : " + pair.Value);
            //    switch (pair.Key)
            //    {
            //        case "color":
            //            if (pair.Value == null) break;
            //            string colorCode = "";
            //            if (pair.Value.Length > 6)
            //            {
            //                colorCode = pair.Value.Remove(0, 1);
            //                ToolkitSettings.ViewerColorCodes[msg.User.ToLower()] = colorCode;
            //            }
            //            else
            //            {
            //                break;
            //            }
            //            GameComponentPawns component = Current.Game.GetComponent<GameComponentPawns>();

            //            if (component.HasUserBeenNamed(msg.User))
            //            {
            //                Pawn pawn = component.PawnAssignedToUser(msg.User);

            //                pawn.story.hairColor = GetColorFromHex(colorCode);
            //            }

            //            break;
            //        case "mod":
            //            if (viewer.mod || pair.Value == null) break;
            //            bool modValue = int.TryParse(pair.Value, out int modStatus);
            //            if (modValue && modStatus == 1)
            //            {
            //                viewer.mod = true;
            //            }
            //            break;
            //        case "subscriber":
            //            if (pair.Value == null) break;
            //            bool subValue = int.TryParse(pair.Value, out int subStatus);
            //            if (subValue && subStatus == 1)
            //            {
            //                viewer.subscriber = true;
            //            }
            //            break;

            //        case "badges":
            //            if (pair.Value == null) break;
            //            IEnumerable<string> badges = pair.Value.Split(',');
            //            foreach (string badge in badges)
            //            {
            //                if (badge == "vip/1")
            //                {
            //                    viewer.vip = true;
            //                    break;
            //                }
            //            }
            //            break;
            //    }
            //}

            Store_Logger.LogString("Parsed command parameters");
        }
    }
}
