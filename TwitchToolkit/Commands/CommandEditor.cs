using SimpleJSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TwitchToolkit.Utilities;
using Verse;

namespace TwitchToolkit.Commands
{
    [StaticConstructorOnStartup]
    public static class CommandEditor
    {
        static CommandEditor()
        {
            List<Command> allCommands = DefDatabase<Command>.AllDefs.ToList();

            foreach (Command cmd in allCommands)
            {
                Command backup = new Command() {
                    defName = cmd.defName,
                    command = cmd.command,
                    label = cmd.label,
                    enabled = cmd.enabled,
                    shouldBeInSeparateRoom = cmd.shouldBeInSeparateRoom,
                    requiresMod = cmd.requiresMod,
                    requiresAdmin = cmd.requiresAdmin,
                    outputMessage = cmd.outputMessage,
                    isCustomMessage = cmd.isCustomMessage,
                };

                commandBackups.Add(backup);
            }

            LoadCopies();
        }

        private static void LoadCopies()
        {
            if (!EditorPathExists())
            {
                Helper.Log("Path for custom commands does not exist, creating");
                return;
            }

            List<Command> allCommands = DefDatabase<Command>.AllDefs.ToList();

            foreach (Command cmd in allCommands)
            {
                if (CopyExists(cmd))
                {
                    LoadCopy(cmd);
                }
            }

            if (ToolkitSettings.CustomCommandDefs == null)
            {
                return;
            }
            
            foreach (string custom in ToolkitSettings.CustomCommandDefs)
            {
                Helper.Log("Loading custom command with defName " + custom);

                Command newCustom = new Command
                {
                    defName = custom
                };

                if (CopyExists(newCustom))
                {
                    LoadCopy(newCustom);

                    newCustom.defName = custom;

                    DefDatabase<Command>.Add(newCustom);
                }
            }
        }

        public static bool CopyExists(Command command)
        {
            if (!EditorPathExists())
            {
                throw new DirectoryNotFoundException();
            }

            string filePath = command.defName + ".json";

            return File.Exists(editorPath + filePath);
        }

        private static void LoadCopy(Command command)
        {
            string filePath = command.defName + ".json";

            try
            {
                using (StreamReader reader = File.OpenText(editorPath + filePath))
                {
                    string json = reader.ReadToEnd();
                    var node = JSON.Parse(json);

                    if (node["command"] == null)
                    {
                        Helper.Log("Copy of command file is missing critical info, delete file " + editorPath + filePath);
                    }
                    command.command = node["command"];

                    if (node["enabled"] == null)
                    {
                        Helper.Log("Copy of command file is missing critical info, delete file " + editorPath + filePath);
                    }
                    command.enabled = node["enabled"].AsBool;

                    if (node["shouldBeInSeparateRoom"] == null)
                    {
                        Helper.Log("Copy of command file is missing critical info, delete file " + editorPath + filePath);
                    }
                    command.shouldBeInSeparateRoom = node["shouldBeInSeparateRoom"].AsBool;

                    if (node["requiresMod"] == null)
                    {
                        Helper.Log("Copy of command file is missing critical info, delete file " + editorPath + filePath);
                    }
                    command.requiresMod = node["requiresMod"].AsBool;

                    if (node["requiresAdmin"] == null)
                    {
                        Helper.Log("Copy of command file is missing critical info, delete file " + editorPath + filePath);
                    }
                    command.requiresAdmin = node["requiresAdmin"].AsBool;

                    if (node["outputMessage"] == null)
                    {
                        Helper.Log("Copy of command file is missing critical info, delete file " + editorPath + filePath);
                    }
                    command.outputMessage = node["outputMessage"];

                    if (node["isCustomMessage"] == null)
                    {
                        Helper.Log("Copy of command file is missing critical info, delete file " + editorPath + filePath);
                    }
                    command.isCustomMessage = node["isCustomMessage"].AsBool;
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Helper.Log(e.Message);
            }
        }

        private static bool EditorPathExists()
        {
            bool dataPathExists = Directory.Exists(editorPath);

            if (!dataPathExists)
                Directory.CreateDirectory(editorPath);

            return dataPathExists;
        }

        public static void LoadBackups()
        {
            foreach (Command backup in commandBackups)
            {
                LoadBackup(backup);
                SaveCopy(backup);
            }
        }

        public static void LoadBackup(Command backup)
        {
            string defName = backup.defName;
            Command inDatabase = DefDatabase<Command>.GetNamed(defName);

            inDatabase.command = backup.command;
            inDatabase.enabled = backup.enabled;
        }

        public static void SaveCopy(Command command)
        {
            if (!EditorPathExists())
            {
                throw new DirectoryNotFoundException();
            }

            string filePath = command.defName + ".json";
            StringBuilder json = new StringBuilder();
            json.AppendLine("{");
            json.AppendLine($"\t\"defName\":\"" + command.defName + "\",");
            json.AppendLine($"\t\"command\":\"" + command.command + "\",");
            json.AppendLine($"\t\"enabled\":\"" + command.enabled + "\",");
            json.AppendLine($"\t\"shouldBeInSeparateRoom\":\"" + command.shouldBeInSeparateRoom + "\",");
            json.AppendLine($"\t\"requiresMod\":\"" + command.requiresMod + "\",");
            json.AppendLine($"\t\"requiresAdmin\":\"" + command.requiresAdmin + "\",");
            json.AppendLine($"\t\"outputMessage\":\"" + command.outputMessage + "\",");
            json.AppendLine($"\t\"isCustomMessage\":\"" + command.isCustomMessage + "\"");
            json.AppendLine("}");

            using (StreamWriter streamWriter = File.CreateText(editorPath + filePath))
            {
                streamWriter.Write(json.ToString());
               
            }
            
            Log.Warning($"Writing Json file to: {Path.GetFullPath(editorPath + filePath)}");
        }

        private static readonly List<Command> commandBackups = new List<Command>();

        public static string dataPath = SaveHelper.dataPath;

        public static string editorPath = dataPath + "Editor/Commands/";
    }
}
