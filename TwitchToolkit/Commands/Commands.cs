using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Chat;
using TwitchToolkit.Incidents;
using TwitchToolkit.IRC;
using TwitchToolkit.Viewers;
using TwitchToolkit.PawnQueue;
using TwitchToolkit.Store;
using TwitchToolkit.Utilities;
using Verse;

namespace TwitchToolkit
{
    public static class CommandsHandler
    {
        public static void CheckCommand(TwitchIRCMessage msg)
        {

            if (msg == null)
            {
                return;
            }

            if (msg.Message == null)
            {
                return;
            }

            Viewer viewer = msg.Viewer;

            viewer.WasSeen();


            if (viewer.IsBanned)
            {
                return;
            }

            if (!msg.Message.StartsWith("!"))
            {
                IRCMessageLog.AddNewMessage(msg);
            }

            Command commandDef = DefDatabase<Command>.AllDefs.ToList().Find(s => msg.Message.StartsWith("!" + s.command));

            if (commandDef != null)
            {
                bool runCommand = true;

                if (commandDef.requiresMod && (!viewer.Mod && viewer.UsernameLower != ToolkitSettings.Channel.ToLower()))
                {
                    runCommand = false;
                }

                if (commandDef.requiresAdmin && msg.User.ToLower() != ToolkitSettings.Channel.ToLower())
                {
                    runCommand = false;
                }

                if (!commandDef.enabled)
                {
                    runCommand = false;
                }

                if (commandDef.shouldBeInSeparateRoom && !AllowCommand(msg))
                {
                    runCommand = false;
                }

                if (runCommand)
                {
                    commandDef.RunCommand(msg);
                }
                
            }

            List<TwitchInterfaceBase> modExtensions = Current.Game.components.OfType<TwitchInterfaceBase>().ToList();

            if (modExtensions == null)
            {
                return;
            }

            foreach(TwitchInterfaceBase parser in modExtensions)
            {
                parser.ParseCommand(msg);
            }
        }

        public static bool AllowCommand(TwitchIRCMessage msg)
        {
            if (!ToolkitSettings.UseSeparateChatRoom && (msg.Whisper || ToolkitSettings.AllowBothChatRooms || msg.Channel == "#" + ToolkitSettings.Channel.ToLower())) return true;
            if (msg.Channel == "#chatrooms:" + ToolkitSettings.ChannelID + ":" + ToolkitSettings.ChatroomUUID) return true;
            if (ToolkitSettings.AllowBothChatRooms && ToolkitSettings.UseSeparateChatRoom || (msg.Whisper)) return true;
            return false;
        }

        public static bool SendToChatroom(TwitchIRCMessage msg)
        {
            if (msg.Whisper && ToolkitSettings.WhispersGoToChatRoom)
            {
                return true;
            }
            else if (msg.Whisper)
            {
                return false;
            }

            if (msg.Channel == "#" + ToolkitSettings.Channel.ToLower()) return false;
            if (ToolkitSettings.UseSeparateChatRoom && !ToolkitSettings.AllowBothChatRooms) return true;
            if (msg.Channel == "#chatrooms:" + ToolkitSettings.ChannelID + ":" + ToolkitSettings.ChatroomUUID && ToolkitSettings.UseSeparateChatRoom) return true;
            return false;
        }

        static DateTime modsCommandCooldown = DateTime.MinValue;
        static DateTime aliveCommandCooldown = DateTime.MinValue;
    }
}
