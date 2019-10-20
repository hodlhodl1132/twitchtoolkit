using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Windows.Installation
{
    public class Window_Install : Window
    {
        public Window_Install()
        {
            this.doCloseButton = false;

            if (ToolkitSettings.FirstTimeInstallation)
            {
                StartInstallationFromStart();
            }
        }

        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            Text.Font = GameFont.Medium;
            listing.Label("Toolkit First Time Setup - Step " + (stepIterator + 1) + " of " + installSteps.Count);
            Text.Font = GameFont.Small;
            listing.GapLine();
            listing.Gap();

            if (stepIterator == 0 && listing.ButtonTextLabeled("Skip First Time Setup", "Skip"))
            {
                InstallationComplete();
            }

            listing.Gap();

            if (currentStep != null)
            {
                currentStep.DoWindowContents(listing);
            }

            listing.Gap();

            if (stepIterator != installSteps.Count && stepIterator + 1 < installSteps.Count && listing.ButtonTextLabeled("Continue to next step", "Next"))
            {
                GetNextStep();
            }

            if (stepIterator != 0 && listing.ButtonTextLabeled("Go back to previous step", "Back"))
            {
                GetPrevStep();
            }
            
            if (stepIterator + 1 == installSteps.Count)
            {
                listing.Gap(24);

                listing.GapLine();

                if (listing.CenteredButton("Save Settings"))
                {
                    InstallationComplete();
                }
            }

            listing.End();
        }

        void InstallationComplete()
        {
            currentStep.Driver.PostInstall();
            ToolkitSettings.FirstTimeInstallation = false;
            Toolkit.Mod.WriteSettings();
            Close();    
        }

        void StartInstallationFromStart()
        {
            installSteps = DefDatabase<InstallStep>.AllDefs.ToList();
            currentStep = installSteps[0];
        }

        void GetNextStep()
        {
            if (stepIterator != installSteps.Count && stepIterator + 1 < installSteps.Count)
            {
                currentStep.Driver.PostInstall();
                stepIterator += 1;
                currentStep = installSteps[stepIterator];
            }
        }

        void GetPrevStep()
        {
            if (stepIterator != 0)
            {
                currentStep.Driver.PostInstall();
                stepIterator -= 1;
                currentStep = installSteps[stepIterator];
            }
        }

        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(800f, 800f);
            }
        }

        List<InstallStep> installSteps = new List<InstallStep>();

        InstallStep currentStep = null;

        int stepIterator = 0;
    }
}
