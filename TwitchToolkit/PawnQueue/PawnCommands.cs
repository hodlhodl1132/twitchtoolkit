using ToolkitCore;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchLib.Client.Models;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.PawnQueue
{
    public class PawnCommands : TwitchInterfaceBase
    {
        public PawnCommands(Game game)
        {

        }

        public override void ParseCommand(ChatMessage msg)
        {
            Viewer viewer = Viewers.GetViewer(msg.Username);

            GameComponentPawns component = Current.Game.GetComponent<GameComponentPawns>();
            
            if (msg.Message.StartsWith("!mypawnskills") && CommandsHandler.AllowCommand(msg))
            {
                
                if (!component.HasUserBeenNamed(viewer.username))
                {
                    MessageQueue.messageQueue.Enqueue($"@{viewer.username} you are not in the colony.");
                    return;
                }

                Pawn pawn = component.PawnAssignedToUser(viewer.username);
                string output = $"@{viewer.username} {pawn.Name.ToStringShort.CapitalizeFirst()}'s skill levels are ";

                List<SkillRecord> skills = pawn.skills.skills;

                for (int i = 0; i < skills.Count; i++)
                {
                    if (skills[i].TotallyDisabled)
                    {
                        output += $"{skills[i].def.LabelCap}: -";
                    }
                    else
                    {
                        output += $"{skills[i].def.LabelCap}: {skills[i].levelInt}";
                    }

                    if (skills[i].passion == Passion.Minor) output += "+";
                    if (skills[i].passion == Passion.Major) output += "++";

                    if (i != skills.Count - 1)
                    {
                        output += ", ";
                    }
                }

                MessageQueue.messageQueue.Enqueue(output);
            }

            if (msg.Message.StartsWith("!mypawnstory") && CommandsHandler.AllowCommand(msg))
            {
                if (!component.HasUserBeenNamed(viewer.username))
                {
                    MessageQueue.messageQueue.Enqueue($"@{viewer.username} you are not in the colony.");
                    return;
                }

                Pawn pawn = component.PawnAssignedToUser(viewer.username);

                string output = $"@{viewer.username} About {pawn.Name.ToStringShort.CapitalizeFirst()}: ";

                List<Backstory> backstories = pawn.story.AllBackstories.ToList();

                for (int i = 0; i < backstories.Count; i++)
                {
                    output += backstories[i].title;
                    if (i != backstories.Count - 1)
                    {
                        output += ", ";
                    }
                }

                output += " | " + pawn.gender;

                StringBuilder stringBuilder = new StringBuilder();
                WorkTags combinedDisabledWorkTags = pawn.story.DisabledWorkTagsBackstoryAndTraits;
                if (combinedDisabledWorkTags == WorkTags.None)
                {
                    stringBuilder.Append("(" + "NoneLower".Translate() + "), ");
                }
                else
                {
                    List<WorkTags> list = WorkTagsFrom(combinedDisabledWorkTags).ToList<WorkTags>();
                    bool flag2 = true;
                    foreach (WorkTags tags in list)
                    {
                        if (flag2)
                        {
                            stringBuilder.Append(tags.LabelTranslated().CapitalizeFirst());
                        }
                        else
                        {
                            stringBuilder.Append(tags.LabelTranslated());
                        }
                        stringBuilder.Append(", ");
                        flag2 = false;
                    }
                }
                string text = stringBuilder.ToString();
                text = text.Substring(0, text.Length - 2);

                output += " | Incapable of: " + text;

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

                MessageQueue.messageQueue.Enqueue(output);
            }

            if (msg.Message.StartsWith("!changepawnname") && CommandsHandler.AllowCommand(msg))
            {
                string[] command = msg.Message.Split(' ');

                if (command.Length < 2) return;

                string newName = command[1];

                if (newName == null || newName == "" || newName.Length > 16)
                {
                    MessageQueue.messageQueue.Enqueue($"@{viewer.username} your name can be up to 16 characters.");
                    return;
                }

                if (!component.HasUserBeenNamed(viewer.username))
                {
                    MessageQueue.messageQueue.Enqueue($"@{viewer.username} you are not in the colony.");
                    return;
                }

                if (!Purchase_Handler.CheckIfViewerHasEnoughCoins(viewer, 500, true)) return;

                viewer.TakeViewerCoins(500);
                nameRequests.Add(viewer.username, newName);
                MessageQueue.messageQueue.Enqueue($"@{ToolkitSettings.Channel} {viewer.username} has requested to be named {newName}, use !approvename @{viewer.username} or !declinename @{viewer.username}");
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
                        MessageQueue.messageQueue.Enqueue($"@{viewer.username} invalid username");
                        return;
                    }

                    if (!component.HasUserBeenNamed(username)) return;

                    Pawn pawn = component.PawnAssignedToUser(username);
                    NameTriple old = pawn.Name as NameTriple;
                    pawn.Name = new NameTriple(old.First, nameRequests[username], old.Last);
                    MessageQueue.messageQueue.Enqueue($"@{viewer.username} approved request for name change from {old} to {pawn.Name}");
                }

                if (msg.Message.StartsWith("!declinename"))
                {

                    string[] command = msg.Message.Split(' ');
                    
                    if (command.Length < 2) return;

                    string username = command[1].Replace("@", "");

                    if (username == null || username == "" || !nameRequests.ContainsKey(username))
                    {
                        MessageQueue.messageQueue.Enqueue($"@{viewer.username} invalid username");
                        return;
                    }

                    if (!component.HasUserBeenNamed(username)) return;

                    nameRequests.Remove(username);
                    MessageQueue.messageQueue.Enqueue($"@{viewer.username} declined name change request from {username}");
                }
            }

            Store_Logger.LogString("Parsed pawn command");
        }

        private static IEnumerable<WorkTags> WorkTagsFrom(WorkTags tags)
        {
            foreach (WorkTags workTag in tags.GetAllSelectedItems<WorkTags>())
            {
                if (workTag != WorkTags.None)
                {
                    yield return workTag;
                }
            }
            yield break;
        }

        public Dictionary<string, string> nameRequests = new Dictionary<string, string>();
    }
}
