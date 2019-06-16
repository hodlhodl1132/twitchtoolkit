using Harmony;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Commands;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Windows
{
    public class Window_CommandEditor : Window
    {
        public Window_CommandEditor(Command command)
        {
            this.doCloseButton = true;
            this.command = command;

            if (command == null)
            {
                throw new ArgumentNullException();
            }

            MakeSureSaveExists(true);
        }


        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            listing.Label("Editing Command " + command.label.CapitalizeFirst());

            command.command = listing.TextEntryLabeled("Command - !", command.command);

            listing.CheckboxLabeled("Enabled", ref command.enabled);

            if (command.isCustomMessage)
            {
                listing.CheckboxLabeled("Require Separate Room if Enabled", ref command.shouldBeInSeparateRoom, "Will require viewers to only use this command in secondary channel if enabled");

                listing.CheckboxLabeled("Require Mod Status", ref command.requiresMod, "Will require viewers to have mod status to use this command");

                listing.CheckboxLabeled("Require Admin Status", ref command.requiresAdmin, "Will only allow channel owner to run this command");

                command.outputMessage = listing.TextEntry(command.outputMessage, 5);

                listing.Gap();

                if (listing.ButtonText("View Available Tags"))
                {
                    Application.OpenURL("https://github.com/hodldeeznuts/twitchtoolkit/wiki/Commands#tags");
                }

                listing.Gap(24);

                if (!deleteWarning && listing.ButtonTextLabeled("Delete Custom Command", "Delete"))
                {
                    deleteWarning = true;
                }

                if (deleteWarning && listing.ButtonTextLabeled("Delete Custom Command", "Are you sure?"))
                {
                    if (ToolkitSettings.CustomCommandDefs.Contains(command.defName))
                    {
                        ToolkitSettings.CustomCommandDefs = ToolkitSettings.CustomCommandDefs.Where(s => s != command.defName).ToList();

                        IEnumerable<Command> toRemove = Enumerable.Empty<Command>();
                        toRemove.Add(command);

                        RemoveStuffFromDatabase(command.GetType(), toRemove.Cast<Def>());
                    }

                    Close();
                }

                if (deleteWarning)
                {
                    listing.Label("(Must restart for deletions to take effect)");
                }
            }

            listing.End();
        }

        private void MakeSureSaveExists(bool force = false)
        {
            checkedForBackup = true;

            haveBackup = CommandEditor.CopyExists(command);

            if (!haveBackup || force)
            {
                CommandEditor.SaveCopy(command);
            }
        }

        public override void PostClose()
        {
            MakeSureSaveExists(true);
            Toolkit.Mod.WriteSettings();
        }

        private static int removedDefs;

        private static void RemoveStuffFromDatabase(Type databaseType, [NotNull] IEnumerable<Def> defs)
        {
            IEnumerable<Def> enumerable = defs as Def[] ?? defs.ToArray();
            if (!enumerable.Any()) return;
            Traverse rm = Traverse.Create(databaseType).Method("Remove", enumerable.First());
            foreach (Def def in enumerable)
            {
                removedDefs++;
                rm.GetValue(def);
            }
        }

        private readonly Command command;

        private bool checkedForBackup = false;

        private bool haveBackup = false;

        private bool deleteWarning = false;
    }
}
