using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Incidents;
using TwitchToolkit.Store;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Windows
{
    public class Window_Cooldowns : Window
    {
        public Window_Cooldowns()
        {
            this.doCloseButton = true;
            UpdateTrackerStats();
        }

        public override void DoWindowContents(Rect inRect)
        {
            Rect topBox = new Rect(0, 0, 400f, 28f);

            Widgets.Label(topBox, "Viewers: " + viewerCount);
            topBox.y += topBox.height;


            string cooldownBuffer = ToolkitSettings.EventCooldownInterval.ToString();
            Widgets.TextFieldNumericLabeled(topBox, "Days per cooldown period ", ref ToolkitSettings.EventCooldownInterval, ref cooldownBuffer, 1);

            topBox.y = 0;
            topBox.x += topBox.width + 20f;

            Widgets.Label(topBox, "Tracker is Cached and will refresh in " + (800 - cachedFramesCount));

            topBox.y += topBox.height;

            if (Widgets.ButtonText(topBox, "Refresh"))
            {
                UpdateTrackerStats();
            }

            Rect karmaBox = new Rect(0, 120f, inRect.width / 2f, 28f);

            Widgets.Label(karmaBox, "Limit Events By Type:");
            Widgets.Checkbox(new Vector2(200f, karmaBox.y), ref ToolkitSettings.MaxEvents);
            karmaBox.y += karmaBox.height;

            // side one


            Rect sideOne = new Rect(0, karmaBox.y + 32f, 100f, 32f);
            Rect sideTwo = new Rect(sideOne)
            {
                x = 140f
            };
            Rect sideThree = new Rect(sideTwo)
            {
                x = 160,
                y = sideTwo.y - 2,
                width = 30,
                height = 26
            };

            Widgets.Label(sideOne, "Good");
            sideOne.y += sideOne.height;

            Widgets.Label(sideTwo, goodEventsInLog + " /");
            string goodBuffer = ToolkitSettings.MaxGoodEventsPerInterval.ToString();
            Widgets.TextFieldNumeric(sideThree, ref ToolkitSettings.MaxGoodEventsPerInterval, ref goodBuffer, 0);
            bool goodBool = goodEventsMaxed;
            Widgets.Checkbox(new Vector2(sideTwo.x + 60f, sideTwo.y), ref goodBool);
            sideTwo.y += sideTwo.height;
            sideThree.y += sideTwo.height;

            Widgets.Label(sideOne, "Bad");
            sideOne.y += sideOne.height;

            Widgets.Label(sideTwo, badEventsInLog + " /");
            string badBuffer = ToolkitSettings.MaxBadEventsPerInterval.ToString();
            Widgets.TextFieldNumeric(sideThree, ref ToolkitSettings.MaxBadEventsPerInterval, ref badBuffer, 0);
            bool badBool = badEventsMaxed;
            Widgets.Checkbox(new Vector2(sideTwo.x + 60f, sideTwo.y), ref badBool);
            sideTwo.y += sideTwo.height;
            sideThree.y += sideTwo.height;

            Widgets.Label(sideOne, "Neutral");
            sideOne.y += sideOne.height;

            Widgets.Label(sideTwo, neutralEventsInLog + " /");
            string neutralBuffer = ToolkitSettings.MaxNeutralEventsPerInterval.ToString();
            Widgets.TextFieldNumeric(sideThree, ref ToolkitSettings.MaxNeutralEventsPerInterval, ref neutralBuffer, 0);
            bool neutralBool = neutralEventsMaxed;
            Widgets.Checkbox(new Vector2(sideTwo.x + 60f, sideTwo.y), ref neutralBool);
            sideTwo.y += sideTwo.height;
            sideThree.y += sideTwo.height;

            Widgets.Label(sideOne, "Care Packages");
            sideOne.y += sideOne.height;

            Widgets.Label(sideTwo, carePackagesInLog + " /");
            string carePackageBuffer = ToolkitSettings.MaxCarePackagesPerInterval.ToString();
            Widgets.TextFieldNumeric(sideThree, ref ToolkitSettings.MaxCarePackagesPerInterval, ref carePackageBuffer, 0);
            bool careBool = carePackagesMaxed;
            Widgets.Checkbox(new Vector2(sideTwo.x + 60f, sideTwo.y), ref careBool);
            sideTwo.y += sideTwo.height;
            sideThree.y += sideTwo.height;


            // SIDE TWO
            Rect eventBox = new Rect(inRect.width / 2f - 200f, 120f, inRect.width / 2f, 28f);

            Widgets.Label(eventBox, "Limit Events By Event:");
            Widgets.Checkbox(new Vector2(eventBox.x + 180f, eventBox.y), ref ToolkitSettings.EventsHaveCooldowns);
            eventBox.y += eventBox.height;

            sideOne = new Rect(eventBox.x, eventBox.y + 32f, 250f, 28f);
            sideTwo = new Rect(sideOne)
            {
                x = sideOne.x + sideOne.width + 40f
            };

            foreach (KeyValuePair<StoreIncident, int> incidentPair in storeIncidentsLogged)
            {
                if (incidentPair.Value < 1) continue;

                Widgets.Label(sideOne, incidentPair.Key.LabelCap);
                sideOne.y += sideOne.height;

                Widgets.Label(sideTwo, incidentPair.Value + "/" + storeIncidentMax[incidentPair.Key]);
                bool maxed = storeIncidentMaxed[incidentPair.Key];
                Widgets.Checkbox(new Vector2(sideTwo.x + 40f, sideTwo.y), ref maxed);

                sideTwo.x += 100f;
                Widgets.Label(sideTwo, storeIncidentsDayTillUsuable[incidentPair.Key] + " days");

                sideTwo.x += 100f;
                sideTwo.width = 100f;
                if (Widgets.ButtonText(sideTwo, "Edit"))
                {
                    StoreIncidentEditor window = new StoreIncidentEditor(incidentPair.Key);
                    Find.WindowStack.TryRemove(window.GetType());
                    Find.WindowStack.Add(window);
                }

                sideOne = new Rect(eventBox.x, sideOne.y, 250f, 28f);
                sideTwo = new Rect(sideOne)
                {
                    x = sideOne.x + sideOne.width + 40f
                };
            }

            cachedFramesCount++;

            if (cachedFramesCount >= 800)
            {
                UpdateTrackerStats();
            }
        }

        public override Vector2 InitialSize => new Vector2(900f, 700f);

        void UpdateTrackerStats()
        {
            cachedFramesCount = 0;
            viewerCount = Viewers.jsonallviewers == null ? 0 : Viewers.ParseViewersFromJsonAndFindActiveViewers().Count;

            cooldownsByTypeEnabled = ToolkitSettings.MaxEvents;

            Store_Component component = Current.Game.GetComponent<Store_Component>();

            goodEventsInLog = component.KarmaTypesInLogOf(KarmaType.Good);
            badEventsInLog = component.KarmaTypesInLogOf(KarmaType.Bad);
            neutralEventsInLog = component.KarmaTypesInLogOf(KarmaType.Neutral);
            carePackagesInLog = component.IncidentsInLogOf(DefDatabase<StoreIncident>.GetNamed("Item").abbreviation);

            goodEventsMax = ToolkitSettings.MaxGoodEventsPerInterval;
            badEventsMax = ToolkitSettings.MaxBadEventsPerInterval;
            neutralEventsMax = ToolkitSettings.MaxNeutralEventsPerInterval;
            carePackagesMax = ToolkitSettings.MaxCarePackagesPerInterval;

            goodEventsMaxed = goodEventsInLog >= goodEventsMax;
            badEventsMaxed = badEventsInLog >= badEventsMax;
            neutralEventsMaxed = neutralEventsInLog >= neutralEventsMax;
            carePackagesMaxed = carePackagesInLog >= carePackagesMax;

            cooldownsByIncidentEnabled = ToolkitSettings.EventsHaveCooldowns;

            List<StoreIncident> storeIncidents = DefDatabase<StoreIncident>.AllDefs.ToList();

            storeIncidentsLogged = new Dictionary<StoreIncident, int>();
            storeIncidentMax = new Dictionary<StoreIncident, int>();
            storeIncidentMaxed = new Dictionary<StoreIncident, bool>();
            storeIncidentsDayTillUsuable = new Dictionary<StoreIncident, float>();

            foreach (StoreIncident incident in storeIncidents)
            {
                storeIncidentsLogged.Add(incident, component.IncidentsInLogOf(incident.abbreviation));
                storeIncidentMax.Add(incident, incident.eventCap);
                storeIncidentMaxed.Add(incident, storeIncidentsLogged[incident] >= incident.eventCap);

                if (storeIncidentsLogged[incident] >= incident.eventCap)
                {
                    storeIncidentsDayTillUsuable.Add(incident, component.DaysTillIncidentIsPurchaseable(incident));
                }
                else
                {
                    storeIncidentsDayTillUsuable.Add(incident, 0);
                }
                
            }

        }

        int cachedFramesCount = 0;

        int viewerCount;

        bool cooldownsByTypeEnabled;

        int goodEventsInLog;
        int badEventsInLog;
        int neutralEventsInLog;
        int carePackagesInLog;

        int goodEventsMax;
        int badEventsMax;
        int neutralEventsMax;
        int carePackagesMax;

        bool goodEventsMaxed;
        bool badEventsMaxed;
        bool neutralEventsMaxed;
        bool carePackagesMaxed;

        bool cooldownsByIncidentEnabled;

        Dictionary<StoreIncident, int> storeIncidentsLogged = new Dictionary<StoreIncident, int>();
        Dictionary<StoreIncident, int> storeIncidentMax = new Dictionary<StoreIncident, int>();
        Dictionary<StoreIncident, bool> storeIncidentMaxed = new Dictionary<StoreIncident, bool>();
        Dictionary<StoreIncident, float> storeIncidentsDayTillUsuable = new Dictionary<StoreIncident, float>();
        
    }
}
