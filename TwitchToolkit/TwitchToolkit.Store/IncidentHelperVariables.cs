using TwitchToolkit.Incidents;

namespace TwitchToolkit.Store;

public abstract class IncidentHelperVariables
{
	public StoreIncidentVariables storeIncident = null;

	public string message;

	public abstract Viewer Viewer { get; set; }

	public abstract bool IsPossible(string message, Viewer viewer, bool separateChannel = false);

	public abstract void TryExecute();
}
