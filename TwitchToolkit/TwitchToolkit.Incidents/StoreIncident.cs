using System;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.Incidents;

public class StoreIncident : Def
{
	public string abbreviation;

	public int cost;

	public int eventCap;

	public Type incidentHelper = typeof(IncidentHelper);

	public KarmaType karmaType;

	public int variables = 0;
}
