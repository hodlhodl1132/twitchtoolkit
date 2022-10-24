using System.Collections.Generic;
using System.Linq;
using TwitchToolkit.Incidents;
using TwitchToolkit.Store;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Windows;

public class Window_Trackers : Window
{
	private int cachedFramesCount = 0;

	private int viewerCount;

	private bool cooldownsByTypeEnabled;

	private int goodEventsInLog;

	private int badEventsInLog;

	private int neutralEventsInLog;

	private int carePackagesInLog;

	private int goodEventsMax;

	private int badEventsMax;

	private int neutralEventsMax;

	private int carePackagesMax;

	private bool goodEventsMaxed;

	private bool badEventsMaxed;

	private bool neutralEventsMaxed;

	private bool carePackagesMaxed;

	private bool cooldownsByIncidentEnabled;

	private Dictionary<StoreIncident, int> storeIncidentsLogged = new Dictionary<StoreIncident, int>();

	private Dictionary<StoreIncident, int> storeIncidentMax = new Dictionary<StoreIncident, int>();

	private Dictionary<StoreIncident, bool> storeIncidentMaxed = new Dictionary<StoreIncident, bool>();

	private Dictionary<StoreIncident, float> storeIncidentsDayTillUsuable = new Dictionary<StoreIncident, float>();

	public override Vector2 InitialSize => new Vector2(900f, 700f);

	public Window_Trackers()
	{
		base.doCloseButton = true;
		UpdateTrackerStats();
	}

	public override void DoWindowContents(Rect inRect)
	{
		Rect topBox = new Rect(0f, 0f, 300f, 28f);
		Widgets.Label(topBox, "Viewers: " + viewerCount);
		topBox.y =(((Rect)( topBox)).y + ((Rect)( topBox)).height);
		Widgets.Label(topBox, "Days per cooldown period: " + ToolkitSettings.EventCooldownInterval + " days");
		topBox.y =(((Rect)( topBox)).y + ((Rect)( topBox)).height);
		if (Widgets.ButtonText(topBox, "Cooldown Settings", true, true, true))
		{
			SettingsWindow window = new SettingsWindow((Mod)(object)Toolkit.Mod);
			Find.WindowStack.TryRemove(((object)window).GetType(), true);
			Find.WindowStack.Add((Window)(object)window);
			ToolkitSettings.currentTab = ToolkitSettings.SettingsTab.Cooldowns;
		}
		Rect karmaBox = new Rect(0f, 120f, ((Rect)(inRect)).width / 2f, 28f);
		Widgets.Label(karmaBox, "Limit Events By Type:");
		Widgets.Checkbox(new Vector2(180f, ((Rect)( karmaBox)).y), ref ToolkitSettings.MaxEvents, 24f, false, false, (Texture2D)null, (Texture2D)null);
		karmaBox.y =(((Rect)( karmaBox)).y + ((Rect)( karmaBox)).height);
		Rect sideOne = new Rect(0f, ((Rect)(karmaBox)).y + 32f, 100f, 28f);
		Rect val = new Rect(sideOne);
		val.x =(140f);
		Rect sideTwo = val;
		Widgets.Label(sideOne, "Good");
		sideOne.y =(((Rect)( sideOne)).y + ((Rect)( sideOne)).height);
		Widgets.Label(sideTwo, goodEventsInLog + "/" + goodEventsMax);
		bool goodBool = goodEventsMaxed;
		Widgets.Checkbox(new Vector2(((Rect)( sideTwo)).x + 40f, ((Rect)( sideTwo)).y), ref goodBool, 24f, false, false, (Texture2D)null, (Texture2D)null);
		sideTwo.y =(((Rect)( sideTwo)).y + ((Rect)( sideTwo)).height);
		Widgets.Label(sideOne, "Bad");
		sideOne.y =(((Rect)( sideOne)).y + ((Rect)( sideOne)).height);
		Widgets.Label(sideTwo, badEventsInLog + "/" + badEventsMax);
		bool badBool = badEventsMaxed;
		Widgets.Checkbox(new Vector2(((Rect)( sideTwo)).x + 40f, ((Rect)( sideTwo)).y), ref badBool, 24f, false, false, (Texture2D)null, (Texture2D)null);
		sideTwo.y =(((Rect)( sideTwo)).y + ((Rect)( sideTwo)).height);
		Widgets.Label(sideOne, "Neutral");
		sideOne.y =(((Rect)( sideOne)).y + ((Rect)( sideOne)).height);
		Widgets.Label(sideTwo, neutralEventsInLog + "/" + neutralEventsMax);
		bool neutralBool = neutralEventsMaxed;
		Widgets.Checkbox(new Vector2(((Rect)( sideTwo)).x + 40f, ((Rect)( sideTwo)).y),ref neutralBool, 24f, false, false, (Texture2D)null, (Texture2D)null);
		sideTwo.y =(((Rect)( sideTwo)).y + ((Rect)( sideTwo)).height);
		Widgets.Label(sideOne, "Care Packages");
		sideOne.y =(((Rect)( sideOne)).y + ((Rect)( sideOne)).height);
		Widgets.Label(sideTwo, carePackagesInLog + "/" + carePackagesMax);
		bool careBool = carePackagesMaxed;
		Widgets.Checkbox(new Vector2(((Rect)( sideTwo)).x + 40f, ((Rect)( sideTwo)).y), ref careBool, 24f, false, false, (Texture2D)null, (Texture2D)null);
		sideTwo.y =(((Rect)( sideTwo)).y + ((Rect)( sideTwo)).height);
		Rect eventBox = new Rect(((Rect)(inRect)).width / 2f - 200f, 120f, ((Rect)(inRect)).width / 2f, 28f);
		Widgets.Label(eventBox, "Limit Events By Event:");
		Widgets.Checkbox(new Vector2(((Rect)( eventBox)).x + 180f, ((Rect)( eventBox)).y), ref ToolkitSettings.EventsHaveCooldowns, 24f, false, false, (Texture2D)null, (Texture2D)null);
		eventBox.y =(((Rect)( eventBox)).y + ((Rect)( eventBox)).height);
		sideOne = new Rect(((Rect)( eventBox)).x, ((Rect)( eventBox)).y + 32f, 250f, 28f);
		val = new Rect(sideOne);
		val.x =(((Rect)( sideOne)).x + ((Rect)( sideOne)).width + 40f);
		sideTwo = val;
		foreach (KeyValuePair<StoreIncident, int> incidentPair in storeIncidentsLogged)
		{
			if (incidentPair.Value >= 1)
			{
				Widgets.Label(sideOne, ((Def)incidentPair.Key).LabelCap);
				sideOne.y =(((Rect)( sideOne)).y + ((Rect)( sideOne)).height);
				Widgets.Label(sideTwo, incidentPair.Value + "/" + storeIncidentMax[incidentPair.Key]);
				bool maxed = storeIncidentMaxed[incidentPair.Key];
				Widgets.Checkbox(new Vector2(((Rect)( sideTwo)).x + 40f, ((Rect)( sideTwo)).y), ref maxed, 24f, false, false, (Texture2D)null, (Texture2D)null);
				sideTwo.x =(((Rect)( sideTwo)).x + 100f);
				Widgets.Label(sideTwo, storeIncidentsDayTillUsuable[incidentPair.Key] + " days");
				sideTwo.x =(((Rect)( sideTwo)).x + 100f);
				sideTwo.width = (100f);
				if (Widgets.ButtonText(sideTwo, "Edit", true, true, true))
				{
					StoreIncidentEditor window2 = new StoreIncidentEditor(incidentPair.Key);
					Find.WindowStack.TryRemove(((object)window2).GetType(), true);
					Find.WindowStack.Add((Window)(object)window2);
				}
				sideOne = new Rect(((Rect)( eventBox)).x, ((Rect)( sideOne)).y, 250f, 28f);
				val = new Rect(sideOne);
				val.x =(((Rect)( sideOne)).x + ((Rect)( sideOne)).width + 40f);
				sideTwo = val;
			}
		}
		cachedFramesCount++;
		if (cachedFramesCount >= 800)
		{
			UpdateTrackerStats();
			cachedFramesCount = 0;
		}
	}

	private void UpdateTrackerStats()
	{
		viewerCount = ((Viewers.jsonallviewers != null) ? Viewers.ParseViewersFromJsonAndFindActiveViewers().Count : 0);
		cooldownsByTypeEnabled = ToolkitSettings.MaxEvents;
		Store_Component component = Current.Game.GetComponent<Store_Component>();
		goodEventsInLog = component.KarmaTypesInLogOf(KarmaType.Good);
		badEventsInLog = component.KarmaTypesInLogOf(KarmaType.Bad);
		neutralEventsInLog = component.KarmaTypesInLogOf(KarmaType.Neutral);
		carePackagesInLog = component.IncidentsInLogOf(DefDatabase<StoreIncident>.GetNamed("Item", true).abbreviation);
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
				storeIncidentsDayTillUsuable.Add(incident, 0f);
			}
		}
	}
}
