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
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models.Interfaces;

namespace TwitchToolkit.Twitch
{
    public class ViewerUpdater : TwitchInterfaceBase
    {
        public ViewerUpdater(Game game)
        {

        }

        public override void ParseMessage(ITwitchMessage twitchMessage)
        {
            // If it is a whisper, do not update viewer details
            if (twitchMessage.ChatMessage == null) return;

            Viewer viewer = Viewers.GetViewer(twitchMessage.Username);
            GameComponentPawns component = Current.Game.GetComponent<GameComponentPawns>();

            ToolkitSettings.ViewerColorCodes[twitchMessage.Username.ToLower()] = twitchMessage.ChatMessage.ColorHex.Replace("#", "");

            Log.Message("ColorHex : " + ToolkitSettings.ViewerColorCodes[twitchMessage.Username.ToLower()]);

            if (component.HasUserBeenNamed(twitchMessage.Username))
            {
                component.PawnAssignedToUser(twitchMessage.Username).story.hairColor = twitchMessage.ChatMessage.Color;
            }

            if (twitchMessage.ChatMessage.IsModerator && !viewer.mod)
            {
                viewer.SetAsModerator();
            }

            if (twitchMessage.ChatMessage.IsSubscriber && !viewer.IsSub)
            {
                viewer.subscriber = true;
            }

            if (twitchMessage.ChatMessage.IsVip && !viewer.IsVIP)
            {
                viewer.vip = true;
            }
        }
    }
}
