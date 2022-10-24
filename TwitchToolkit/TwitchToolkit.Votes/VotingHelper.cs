using RimWorld;
using TwitchToolkit.Store;

namespace TwitchToolkit.Votes;

public abstract class VotingHelper : IncidentHelper
{
	public IIncidentTarget target;

	public override bool IsPossible()
	{
		return true;
	}
}
