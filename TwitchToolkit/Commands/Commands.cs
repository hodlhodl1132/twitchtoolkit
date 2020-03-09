using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchLib.Client.Models;
using TwitchToolkit.Incidents;
using TwitchToolkit.PawnQueue;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit
{
    public static class CommandsHandler
    {
        public static void CheckCommand(ChatMessage msg)
        {

            if (msg == null)
            {
                return;
            }

            if (msg.Message == null)
            {
                return;
            }

            string message = msg.Message;
            string user = msg.Username;
            if (message.Split(' ')[0] == "/w")
            {
                List<string> messagewhisper = message.Split(' ').ToList();
                messagewhisper.RemoveAt(0);
                message = string.Join(" ", messagewhisper.ToArray());
                Helper.Log(message);
            }

            Viewer viewer = Viewers.GetViewer(user);
            viewer.last_seen = DateTime.Now;

            if (viewer.IsBanned)
            {
                return;
            }

            Command commandDef = DefDatabase<Command>.AllDefs.ToList().Find(s => msg.Message.StartsWith("!" + s.command));

            if (commandDef != null)
            {
                bool runCommand = true;

                if (commandDef.requiresMod && (!viewer.mod && viewer.username.ToLower() != ToolkitSettings.Channel.ToLower()))
                {
                    runCommand = false;
                }

                if (commandDef.requiresAdmin && msg.Username.ToLower() != ToolkitSettings.Channel.ToLower())
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

            //Deprecated,  Use RimTwitch Library instead

            //List<TwitchInterfaceBase> modExtensions = Current.Game.components.OfType<TwitchInterfaceBase>().ToList();

            //if (modExtensions == null)
            //{
            //    return;
            //}

            //foreach(TwitchInterfaceBase parser in modExtensions)
            //{
            //    parser.ParseCommand(msg);
            //}
        }

        public static bool AllowCommand(ChatMessage msg)
        {
            return true;
        }

        public static bool SendToChatroom(ChatMessage msg)
        {
            return true;
        }

        static DateTime modsCommandCooldown = DateTime.MinValue;
        static DateTime aliveCommandCooldown = DateTime.MinValue;
    }
}
