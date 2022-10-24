using System;
using TwitchToolkit.Store;

namespace TwitchToolkit.Incidents;

public static class StoreIncidentMaker
{
	public static IncidentHelper MakeIncident(StoreIncidentSimple def)
	{
		IncidentHelper helper = (IncidentHelper)Activator.CreateInstance(def.incidentHelper);
		helper.storeIncident = def;
		return helper;
	}

	public static IncidentHelperVariables MakeIncidentVariables(StoreIncidentVariables def)
	{
		IncidentHelperVariables helper = (IncidentHelperVariables)Activator.CreateInstance(def.incidentHelper);
		helper.storeIncident = def;
		return helper;
	}

	public static IncidentHelperVariablesSettings MakeIncidentVariablesSettings(StoreIncidentVariables def)
	{
		if (!def.customSettings)
		{
			return null;
		}
		return (IncidentHelperVariablesSettings)Activator.CreateInstance(def.customSettingsHelper);
	}
}
