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

            command.command = listing.TextEntryLabeled("Command", command.command);

            listing.CheckboxLabeled("Enabled", ref command.enabled);

            if (command.isCustomMessage)
            {
                listing.CheckboxLabeled("Require Separate Room if Enabled", ref command.shouldBeInSeparateRoom, "Will require viewers to only use this command in secondary channel if enabled");

                listing.CheckboxLabeled("Require Mod Status", ref command.requiresMod, "Will require viewers to have mod status to use this command");

                listing.CheckboxLabeled("Require Admin Status", ref command.requiresAdmin, "Will only allow channel owner to run this command");

                command.outputMessage = listing.TextEntry(command.outputMessage, 5);
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

        private readonly Command command;

        private bool checkedForBackup = false;

        private bool haveBackup = false;
    }
}
