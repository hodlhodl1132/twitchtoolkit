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

            StoreIncidentVariables storeIncidentVariables = DefDatabase<StoreIncidentVariables>.AllDefs.ToList().Find(s =>
                s.defName == storeIncident.defName
            );

            if (storeIncidentVariables != null)
            {
                this.storeIncidentVariables = storeIncidentVariables;
                this.variableIncident = true;

                this.extraWindowHeight = storeIncidentVariables.customSettingKeys != null ?
                    (storeIncidentVariables.customSettingKeys.Count * 28f) + 28f :
                    0f;
            }

            karmaTypeStrings = Enum.GetNames(typeof(KarmaType));

            setKarmaType = storeIncident.karmaType.ToString();
        }

        public override Vector2 InitialSize => new Vector2(500f, 500f + extraWindowHeight);

        public override void PostClose()
        {
            MakeSureSaveExists(true);
            Store_IncidentEditor.UpdatePriceSheet();
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

                if (variableIncident)
                {
                    ls.Gap();
                    ls.GapLine();
                    ls.Label("Custom Settings");

                    int KeyIndex = 0;

                    if (storeIncidentVariables.customSettingStringValues != null)
                    {
                        foreach (string str in storeIncidentVariables.customSettingStringValues)
                        {
                            string key = storeIncidentVariables.customSettingKeys[KeyIndex];
                            CustomSettings.SetStringSetting(key, ls.TextEntryLabeled(StripSettingKey(key), CustomSettings.LookupStringSetting(key)));
                            KeyIndex++;
                        }
                    }

                    if (storeIncidentVariables.customSettingFloatValues != null)
                    {
                        foreach (float flt in storeIncidentVariables.customSettingFloatValues)
                        {
                            string key = storeIncidentVariables.customSettingKeys[KeyIndex];
                            float newValue = CustomSettings.LookupFloatSetting(key);
                            string newValueBuffer = newValue.ToString();
                            ls.TextFieldNumericLabeled<float>(StripSettingKey(key), ref newValue, ref newValueBuffer);
                            CustomSettings.SetFloatSetting(key, newValue);
                            KeyIndex++;
                        }
                    }


                    ls.GapLine();
                }

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
            haveBackup = Store_IncidentEditor.CopyExists(storeIncident);
            if (!haveBackup || forceSave)
            {
                Store_IncidentEditor.SaveCopy(storeIncident);
            }
        }

        public string StripSettingKey(string key)
        {
            string[] split = key.Split('.');

            return split[split.Count() - 1];
        }

        public float extraWindowHeight = 0;

        public bool checkedForBackup = false;
        public bool haveBackup = false;

        public bool variableIncident = false;

        public StoreIncident storeIncident = null;
        public StoreIncidentVariables storeIncidentVariables = null;

        private string[] karmaTypeStrings = null;
        private string setKarmaType = "";

    }
}
