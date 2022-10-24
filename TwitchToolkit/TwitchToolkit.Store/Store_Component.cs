using System;
using System.Collections.Generic;
using System.Linq;
using TwitchToolkit.Incidents;
using Verse;

namespace TwitchToolkit.Store;

public class Store_Component : GameComponent
{
	public int lastID = 0;

	public Dictionary<int, int> tickHistory = new Dictionary<int, int>();

	public Dictionary<int, string> abbreviationHistory = new Dictionary<int, string>();

	public Dictionary<int, string> karmaHistory = new Dictionary<int, string>();

	public Store_Component(Game game)
	{
	}

	public override void GameComponentTick()
	{
		if (Find.TickManager.TicksGame % 1000 == 0)
		{
			CleanLog();
		}
	}

	public void CleanLog()
	{
		int currentTick = Find.TickManager.TicksGame;
		List<int> toRemove = new List<int>();
		foreach (KeyValuePair<int, int> pair in tickHistory)
		{
			if (pair.Value + 60000 * ToolkitSettings.EventCooldownInterval < currentTick)
			{
				toRemove.Add(pair.Key);
			}
		}
		foreach (int i in toRemove)
		{
			tickHistory.Remove(i);
			abbreviationHistory.Remove(i);
			karmaHistory.Remove(i);
		}
	}

	public int IncidentsInLogOf(string abbreviation)
	{
		return abbreviationHistory.Where((KeyValuePair<int, string> pair) => pair.Value == abbreviation).Count();
	}

	public int KarmaTypesInLogOf(KarmaType karmaType)
	{
		return karmaHistory.Where((KeyValuePair<int, string> pair) => pair.Value == karmaType.ToString()).Count();
	}

	public float DaysTillIncidentIsPurchaseable(StoreIncident incident)
	{
		Store_Component component = Current.Game.GetComponent<Store_Component>();
		List<int> associateLogIDS = new List<int>();
		float ticksTillExpires = -1f;
		float daysTillCooldownExpires = -1f;
		if (((Def)incident).defName == "Item")
		{
			if (ToolkitSettings.MaxEvents)
			{
				int logged4 = component.IncidentsInLogOf(incident.abbreviation);
				bool onCooldownByKarmaType = logged4 >= ToolkitSettings.MaxCarePackagesPerInterval;
			}
			if (ToolkitSettings.EventsHaveCooldowns)
			{
				int logged3 = component.IncidentsInLogOf(incident.abbreviation);
				bool onCooldownByIncidentCap = logged3 >= incident.eventCap;
			}
			foreach (KeyValuePair<int, string> pair3 in abbreviationHistory)
			{
				if (pair3.Value == incident.abbreviation)
				{
					associateLogIDS.Add(pair3.Key);
				}
			}
		}
		else
		{
			if (ToolkitSettings.MaxEvents)
			{
				int logged2 = component.KarmaTypesInLogOf(incident.karmaType);
				bool onCooldownByKarmaType = Purchase_Handler.CheckTimesKarmaTypeHasBeenUsedRecently(incident);
			}
			if (ToolkitSettings.EventsHaveCooldowns)
			{
				int logged = component.IncidentsInLogOf(incident.abbreviation);
				bool onCooldownByIncidentCap = logged >= incident.eventCap;
			}
			foreach (KeyValuePair<int, string> pair2 in abbreviationHistory)
			{
				if (pair2.Value == incident.abbreviation)
				{
					associateLogIDS.Add(pair2.Key);
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
			float daysAgo = ticksAgo / 60000f;
			float ticksTillExpiration = (float)(ToolkitSettings.EventCooldownInterval * 60000) - ticksAgo;
			if (ticksTillExpires == -1f || ticksAgo < ticksTillExpiration)
			{
				ticksTillExpires = ticksAgo;
				daysTillCooldownExpires = ticksTillExpiration / 60000f;
			}
		}
		return (float)Math.Round(daysTillCooldownExpires, 1);
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
		((GameComponent)this).ExposeData();
		Scribe_Values.Look<int>(ref lastID, "logID", 0, false);
		Scribe_Collections.Look<int, int>(ref tickHistory, "tickHistory", (LookMode)1, (LookMode)1);
		Scribe_Collections.Look<int, string>(ref abbreviationHistory, "incidentHistory", (LookMode)1, (LookMode)1);
		Scribe_Collections.Look<int, string>(ref karmaHistory, "karmaHistory", (LookMode)1, (LookMode)1);
	}
}
