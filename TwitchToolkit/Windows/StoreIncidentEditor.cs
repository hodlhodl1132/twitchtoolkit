using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Incidents;
using TwitchToolkit.Settings;
using TwitchToolkit.Store;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Windows
{
    public class StoreIncidentEditor : Window
    {
        public StoreIncidentEditor(StoreIncident storeIncident)
        {
            this.doCloseButton = true;
            this.storeIncident = storeIncident;

            if (storeIncident == null)
            {
                throw new ArgumentNullException();
            }

            MakeSureSaveExists(true);

            StoreIncidentVariables storeIncidentVariables = DefDatabase<StoreIncidentVariables>.AllDefs.ToList().Find(s =>
                s.defName == storeIncident.defName
            );

            if (storeIncidentVariables != null)
            {
                this.storeIncidentVariables = storeIncidentVariables;
                this.variableIncident = true;
            }

            karmaTypeStrings = Enum.GetNames(typeof(KarmaType));

            setKarmaType = storeIncident.karmaType.ToString();
        }

        public override Vector2 InitialSize => new Vector2(500f, 500f);

        public override void PostClose()
        {
            MakeSureSaveExists(true);
            Store_IncidentEditor.UpdatePriceSheet();
            Toolkit.Mod.WriteSettings();
        }

        public override void DoWindowContents(Rect inRect)
        {
            if (!checkedForBackup || !haveBackup)
            {
                MakeSureSaveExists();
                return;
            }

            Listing_Standard ls = new Listing_Standard();
            ls.Begin(inRect);
            ls.Label(storeIncident.label.CapitalizeFirst());

            ls.Gap();

            if (storeIncident.cost > 0)
            {

                storeIncident.abbreviation = ls.TextEntryLabeled("Purchase Code:", storeIncident.abbreviation);    

                ls.Gap();

                ls.AddLabeledNumericalTextField<int>("Cost", ref storeIncident.cost);
            
                ls.SliderLabeled("Max times per " + ToolkitSettings.EventCooldownInterval + " ingame day(s)", ref storeIncident.eventCap, storeIncident.eventCap.ToString(), 0, 15);

                if (variableIncident && storeIncidentVariables.maxWager > 0)
                {
                    ls.Gap();

                    ls.SliderLabeled("Maximum coin wager", ref storeIncidentVariables.maxWager, storeIncidentVariables.cost.ToString(), storeIncident.cost, 20000f);
                }  
                

                ls.Gap();

                ls.AddLabeledRadioList("Karma Type", karmaTypeStrings, ref setKarmaType);

                storeIncident.karmaType = (KarmaType) Enum.Parse(typeof(KarmaType), setKarmaType);

                ls.Gap();

                if (ls.ButtonTextLabeled("Disable Store Incident", "Disable"))
                {
                    storeIncident.cost = -10;
                }
            }

            ls.Gap();
            
            if (storeIncident.defName == "Item")
            {
                ls.SliderLabeled("Max times per " + ToolkitSettings.EventCooldownInterval + " ingame day(s)", ref storeIncident.eventCap, storeIncident.eventCap.ToString(), 0, 15);

                ls.Gap();
            }

            if (variableIncident && storeIncidentVariables.customSettings)
            {
                ls.Gap();

                if (ls.ButtonTextLabeled("Edit Extra Settings", "Settings"))
                {
                    storeIncidentVariables.settings.EditSettings();
                }
            }

            ls.Gap();

            if (storeIncident.defName != "Item" && ls.ButtonTextLabeled("Reset to Default", "Reset"))
            {
                Store_IncidentEditor.LoadBackup(storeIncident);
                if (storeIncident.cost < 1) storeIncident.cost = 50;
                MakeSureSaveExists(true);
            }

            if (storeIncident.defName == "Item" && ls.ButtonTextLabeled("Edit item prices", "Edit"))
            {
                Type type = typeof(StoreItemsWindow);
                Find.WindowStack.TryRemove(type);
                
                Window window = new StoreItemsWindow();
                Find.WindowStack.Add(window);
            }
        
            ls.End();
        }

        public void MakeSureSaveExists(bool forceSave = true)
        {
            checkedForBackup = true;

            Log.Warning("Checking if save exists");

            if (storeIncident == null)
                Log.Error("incident is null");

            haveBackup = Store_IncidentEditor.CopyExists(storeIncident);
            if (!haveBackup || forceSave)
            {
                Store_IncidentEditor.SaveCopy(storeIncident);
            }
        }

        public bool checkedForBackup = false;
        public bool haveBackup = false;

        public bool variableIncident = false;

        public StoreIncident storeIncident = null;
        public StoreIncidentVariables storeIncidentVariables = null;

        private string[] karmaTypeStrings = null;
        private string setKarmaType = "";

    }
}
