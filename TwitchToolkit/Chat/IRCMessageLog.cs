using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.IRC;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Chat
{
    public static class IRCMessageLog
    {
        static List<TwitchIRCMessage> ircMessages = new List<TwitchIRCMessage>();

        public static void AddNewMessage(TwitchIRCMessage message)
        {
            ircMessages.Add(message);

            if (ircMessages.Count > ToolkitSettings.ChatBoxMessageCount)
            {
                ircMessages.Remove(ircMessages.ElementAt(0));
            }
        }

        static List<string> GetLastMessages()
        {
            List<string> lastMessages = new List<string>(ircMessages.Take(ToolkitSettings.ChatBoxMessageCount).Select(s => s.ChatBoxStringCached).ToList());
            lastMessages.Reverse();
            return lastMessages;
        }

        public static void IRCMessageReadoutOnGUI()
        {
            if (!ToolkitSettings.ChatBoxEnabled) return;

            Rect rect = new Rect(ToolkitSettings.ChatBoxPositionX, ToolkitSettings.ChatBoxPositionY, ToolkitSettings.ChatBoxMaxWidth, lineHeight);

            float lastMessageBottomPixel = 0;

            foreach (string message in GetLastMessages())
            {
                string filteredMessage = FilterForMentions(message);
                Vector2 vec2 = Text.CalcSize(filteredMessage);

                if (vec2.x > ToolkitSettings.ChatBoxMaxWidth)
                {
                    Helper.Log("vec2 " + vec2.x + " " + vec2.y);
                    rect.height = (float)Math.Ceiling(vec2.x / ToolkitSettings.ChatBoxMaxWidth) * lineHeight;
                }
                else
                {
                    rect.height = lineHeight;
                }

                Widgets.Label(rect, filteredMessage);

                lastMessageBottomPixel = rect.y + rect.height;

                rect.y += rect.height;
            }

            //GUI.Box(new Rect(ToolkitSettings.ChatBoxPositionX, ToolkitSettings.ChatBoxPositionY, ToolkitSettings.ChatBoxMaxWidth, lastMessageBottomPixel), "");
        }

        public static string FilterForMentions(string message)
        {
            return message.Replace("@" + ToolkitSettings.Channel, "<b><color=#000000>@" + ToolkitSettings.Channel + "</color></b>" ?? "");
        }

        static float lineHeight = 20;
    }
}
