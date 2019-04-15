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
