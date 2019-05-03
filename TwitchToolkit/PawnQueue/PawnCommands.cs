using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.IRC;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.PawnQueue
{
    public class PawnCommands : TwitchInterfaceBase
    {
        public PawnCommands(Game game)
        {

        }

        public override void ParseCommand(IRCMessage msg)
        {
            Viewer viewer = Viewers.GetViewer(msg.User);

            GameComponentPawns component = Current.Game.GetComponent<GameComponentPawns>();
            
            if (msg.Message.StartsWith("!mypawnskills") && Commands.AllowCommand(msg.Channel))
            {
                
                if (!component.HasUserBeenNamed(viewer.username))
                {
                    Toolkit.client.SendMessage($"@{viewer.username} you are not in the colony.", true);
                    return;
                }

                Pawn pawn = component.PawnAssignedToUser(viewer.username);
                string output = $"@{viewer.username} {pawn.Name.ToStringShort.CapitalizeFirst()}'s skill levels are ";

                List<SkillRecord> skills = pawn.skills.skills;

                for (int i = 0; i < skills.Count; i++)
                {
                    output += $"{skills[i].def.LabelCap}: {skills[i].levelInt}";

                    if (skills[i].passion == Passion.Minor) output += "+";
                    if (skills[i].passion == Passion.Major) output += "++";

                    if (i != skills.Count - 1)
                    {
                        output += ", ";
                    }
                }

                Toolkit.client.SendMessage(output, true);
            }

            if (msg.Message.StartsWith("!mypawnstory") && Commands.AllowCommand(msg.Channel))
            {
                if (!component.HasUserBeenNamed(viewer.username))
                {
                    Toolkit.client.SendMessage($"@{viewer.username} you are not in the colony.", true);
                    return;
                }

                Pawn pawn = component.PawnAssignedToUser(viewer.username);

                string output = $"@{viewer.username} About {pawn.Name.ToStringShort.CapitalizeFirst()}: ";

                List<Backstory> backstories = pawn.story.AllBackstories.ToList();

                for (int i = 0; i < backstories.Count; i++)
                {
                    output += backstories[i].titleShort;
                    if (i != backstories.Count - 1)
                    {
                        output += ", ";
                    }
                }

                output += " | Traits: ";

                List<Trait> traits = pawn.story.traits.allTraits;
                for (int i = 0; i < traits.Count; i++)
                {
                    output += traits[i].LabelCap;

                    if (i != traits.Count - 1)
                    {
                        output += ", ";
                    }
                }

                Toolkit.client.SendMessage(output, true);
            }

            if (msg.Message.StartsWith("!changepawnname") && Commands.AllowCommand(msg.Channel))
            {
                string[] command = msg.Message.Split(' ');

                if (command.Length < 2) return;

                string newName = command[1];

                if (newName == null || newName == "" || newName.Length > 16)
                {
                    Toolkit.client.SendMessage($"@{viewer.username} your name can be up to 16 characters.", true);
                    return;
                }

                if (!component.HasUserBeenNamed(viewer.username))
                {
                    Toolkit.client.SendMessage($"@{viewer.username} you are not in the colony.", Commands.SendToChatroom(msg.Channel));
                    return;
                }

                if (!Purchase_Handler.CheckIfViewerHasEnoughCoins(viewer, 500, true)) return;

                viewer.TakeViewerCoins(500);
                nameRequests.Add(viewer.username, newName);
                Toolkit.client.SendMessage($"@{ToolkitSettings.Channel} {viewer.username} has requested to be named {newName}, use !approvename @{viewer.username} or !declinename @{viewer.username}", false);
            }

            if (Viewer.IsModerator(viewer.username) || viewer.username == ToolkitSettings.Channel)
            {
                if (msg.Message.StartsWith("!unstickpeople"))
                {
                    Purchase_Handler.viewerNamesDoingVariableCommands = new List<string>();
                }

                if (msg.Message.StartsWith("!approvename"))
                {

                    string[] command = msg.Message.Split(' ');
                    
                    if (command.Length < 2) return;

                    string username = command[1].Replace("@", "");

                    if (username == null || username == "" || !nameRequests.ContainsKey(username))
                    {
                        Toolkit.client.SendMessage($"@{viewer.username} invalid username", false);
                        return;
                    }

                    if (!component.HasUserBeenNamed(username)) return;

                    Pawn pawn = component.PawnAssignedToUser(username);
                    NameTriple old = pawn.Name as NameTriple;
                    pawn.Name = new NameTriple(old.First, nameRequests[username], old.Last);
                    Toolkit.client.SendMessage($"@{viewer.username} approved request for name change from {old} to {pawn.Name}");
                }

                if (msg.Message.StartsWith("!declinename"))
                {

                    string[] command = msg.Message.Split(' ');
                    
                    if (command.Length < 2) return;

                    string username = command[1].Replace("@", "");

                    if (username == null || username == "" || !nameRequests.ContainsKey(username))
                    {
                        Toolkit.client.SendMessage($"@{viewer.username} invalid username", false);
                        return;
                    }

                    if (!component.HasUserBeenNamed(username)) return;

                    nameRequests.Remove(username);
                    Toolkit.client.SendMessage($"@{viewer.username} declined name change request from {username}");
                }
            }

            Store_Logger.LogString("Parsed pawn command");
        }

        public Dictionary<string, string> nameRequests = new Dictionary<string, string>();
    }
}
