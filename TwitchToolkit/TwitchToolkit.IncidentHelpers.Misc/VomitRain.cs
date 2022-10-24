using TwitchToolkit.Store;

namespace TwitchToolkit.IncidentHelpers.Misc;

public class VomitRain : IncidentHelper
{
	public override bool IsPossible()
	{
		return true;
	}

	public override void TryExecute()
	{
		int count = 50;
		for (int i = 0; i < count; i++)
		{
			Helper.Vomit(Helper.AnyPlayerMap);
		}
	}
}
