using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Incidents;
using Verse;

namespace TwitchToolkit.Store
{
    public class Store_Component : GameComponent
    {
        public Store_Component(Game game)
        {

        }

        public override void GameComponentTick()
        {
            if (Find.TickManager.TicksGame % 1000 != 0)
            return;

            CleanLog();
        }

        public void CleanLog()
        {
            int currentTick = Find.TickManager.TicksGame;

            List<int> toRemove = new List<int>();

            foreach (KeyValuePair<int, int> pair in tickHistory)
            {
                if (pair.Value + (GenDate.TicksPerDay * ToolkitSettings.EventCooldownInterval) < currentTick)
                {
                    toRemove.Add(pair.Key);
                }
            }

            foreach (int n in toRemove)
            {
                tickHistory.Remove(n);
                abbreviationHistory.Remove(n);
                karmaHistory.Remove(n);
            }
        }

        public int IncidentsInLogOf(string abbreviation)
        {
            return abbreviationHistory.Where(pair => pair.Value == abbreviation).Count();
        }

        public int KarmaTypesInLogOf(KarmaType karmaType)
        {
            return karmaHistory.Where(pair => pair.Value == karmaType.ToString()).Count();
        }

        public float DaysTillIncidentIsPurchaseable(StoreIncident incident)
        {
            Store_Component component = Current.Game.GetComponent<Store_Component>();

            List<int> associateLogIDS = new List<int>();

            bool onCooldownByKarmaType;
            bool onCooldownByIncidentCap;

            float ticksTillExpires = -1;
            float daysTillCooldownExpires = -1;

            if (incident.defName == "Item")
            {
                if (ToolkitSettings.MaxEvents)
                {
                    int logged = component.IncidentsInLogOf(incident.abbreviation);
                    onCooldownByKarmaType = logged >= ToolkitSettings.MaxCarePackagesPerInterval;
                }

                if (ToolkitSettings.EventsHaveCooldowns)
                {
                    int logged = component.IncidentsInLogOf(incident.abbreviation);
                    onCooldownByIncidentCap = logged >= incident.eventCap;
                }

                foreach (KeyValuePair<int, string> pair in abbreviationHistory)
                {
                    if (pair.Value == incident.abbreviation)
                    {
                        associateLogIDS.Add(pair.Key);
                    }
                }
            }
            else
            {
                if (ToolkitSettings.MaxEvents)
                {
                    int logged = component.KarmaTypesInLogOf(incident.karmaType);
                    onCooldownByKarmaType = Purchase_Handler.CheckTimesKarmaTypeHasBeenUsedRecently(incident);
                }

                if (ToolkitSettings.EventsHaveCooldowns)
                {
                    int logged = component.IncidentsInLogOf(incident.abbreviation);
                    onCooldownByIncidentCap = logged >= incident.eventCap;
                }

                foreach (KeyValuePair<int, string> pair in abbreviationHistory)
                {
                    if (pair.Value == incident.abbreviation)
                    {
                        associateLogIDS.Add(pair.Key);
                    }
                }

                foreach (KeyValuePair<int, string> pair in karmaHistory)
                {
                    if (pair.Value == incident.karmaType.ToString())
                    {
                        associateLogIDS.Add(pair.Key);
                    }
                }
            }

            foreach (int id in associateLogIDS)
            {
                float ticksAgo = Find.TickManager.TicksGame - tickHistory[id];
                float daysAgo = ticksAgo / GenDate.TicksPerDay;
                float ticksTillExpiration = (ToolkitSettings.EventCooldownInterval * GenDate.TicksPerDay) - ticksAgo;
                if (ticksTillExpires == -1 || ticksAgo < ticksTillExpiration)
                {
                    ticksTillExpires = ticksAgo;
                    daysTillCooldownExpires = ticksTillExpiration / GenDate.TicksPerDay;
                }
            }

            return (float) Math.Round(daysTillCooldownExpires, 1);
        }

        public void LogIncident(StoreIncident incident)
        {
            int currentTick = Find.TickManager.TicksGame;
            int logID = lastID;
            tickHistory.Add(logID, currentTick);
            abbreviationHistory.Add(logID, incident.abbreviation);
            karmaHistory.Add(logID, incident.karmaType.ToString());
            
            lastID++;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref lastID, "logID", 0);

            Scribe_Collections.Look(ref tickHistory, "tickHistory", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref abbreviationHistory, "incidentHistory", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref karmaHistory, "karmaHistory", LookMode.Value, LookMode.Value);

        }

        public int lastID = 0;

        public Dictionary<int, int> tickHistory = new Dictionary<int, int>();

        public Dictionary<int, string> abbreviationHistory = new Dictionary<int, string>();

        public Dictionary<int, string> karmaHistory = new Dictionary<int, string>();

    }
}
