using TwitchToolkit.Incidents;

namespace TwitchToolkit.Store;

public abstract class IncidentHelper
{
	public StoreIncident storeIncident = null;

	public string message;

	public Viewer Viewer { get; set; } = null;


	public abstract bool IsPossible();

	public abstract void TryExecute();
}
