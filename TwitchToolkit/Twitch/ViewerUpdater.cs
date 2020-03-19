using ToolkitCore;
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
            GameComponentPawns component = Current.Game.GetComponent<GameComponentPawns>();

            ToolkitSettings.ViewerColorCodes[msg.Username.ToLower()] = msg.ColorHex;

            if (component.HasUserBeenNamed(msg.Username))
            {
                component.PawnAssignedToUser(msg.Username).story.hairColor = msg.Color;
            }

            if (msg.IsModerator && !viewer.mod)
            {
                viewer.SetAsModerator();
            }

            if (msg.IsSubscriber && !viewer.IsSub)
            {
                viewer.subscriber = true;
            }

            if (msg.IsVip && !viewer.IsVIP)
            {
                viewer.vip = true;
            }
        }
    }
}
