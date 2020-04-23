using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using TwitchLib.Client.Models.Interfaces;
using TwitchToolkit.Incidents;
using TwitchToolkit.PawnQueue;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit
{
    public static class CommandsHandler
    {
        public static void CheckCommand(ITwitchMessage twitchMessage)
        {
            Log.Message($"Checking command - {twitchMessage.Message}");

            if (twitchMessage == null)
            {
                return;
            }

            if (twitchMessage.Message == null)
            {
                return;
            }

            string message = twitchMessage.Message;
            string user = twitchMessage.Username;

            Viewer viewer = Viewers.GetViewer(user);
            viewer.last_seen = DateTime.Now;

            if (viewer.IsBanned)
            {
                return;
            }

            Command commandDef = DefDatabase<Command>.AllDefs.ToList().Find(s => twitchMessage.Message.StartsWith("!" + s.command));

            if (commandDef != null)
            {
                bool runCommand = true;

                if (commandDef.requiresMod && (!viewer.mod && viewer.username.ToLower() != ToolkitSettings.Channel.ToLower()))
                {
                    runCommand = false;
                }

                if (commandDef.requiresAdmin && twitchMessage.Username.ToLower() != ToolkitSettings.Channel.ToLower())
                {
                    runCommand = false;
                }

                if (!commandDef.enabled)
                {
                    runCommand = false;
                }

                if (runCommand)
                {
                    commandDef.RunCommand(twitchMessage);
                }
            }
        }

        public static bool SendToChatroom(ChatMessage msg)
        {
            return true;
        }

        static DateTime modsCommandCooldown = DateTime.MinValue;
        static DateTime aliveCommandCooldown = DateTime.MinValue;
    }
}
