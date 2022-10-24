using System;
using TwitchToolkit.Store;

namespace TwitchToolkit.Incidents;

public class StoreIncidentVariables : StoreIncident
{
	public int minPointsToFire = 0;

	public int maxWager = 0;

	public string syntax = null;

	public new Type incidentHelper = typeof(IncidentHelperVariables);

	public bool customSettings = false;

	public Type customSettingsHelper = typeof(IncidentHelperVariablesSettings);

	public IncidentHelperVariablesSettings settings = null;

	public void RegisterCustomSettings()
	{
		if (settings == null)
		{
			settings = StoreIncidentMaker.MakeIncidentVariablesSettings(this);
		}
	}
}
