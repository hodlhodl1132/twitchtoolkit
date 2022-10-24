using System.Collections.Generic;
using RimWorld;
using TwitchToolkit.Store;
using TwitchToolkit.Storytellers;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Votes;

public class ToryTalkerVote : IncidentHelperVariables
{
	private StorytellerPack toryTalker;

	public override Viewer Viewer { get; set; }

	public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
	{
		Viewer = viewer;
		toryTalker = DefDatabase<StorytellerPack>.GetNamed("ToryTalker", true);
		if (toryTalker != null && toryTalker.enabled)
		{
			return true;
		}
		return false;
	}

	public override void TryExecute()
	{
		Map map = Helper.AnyPlayerMap;
		StorytellerComp_ToryTalker storytellerComp = new StorytellerComp_ToryTalker();
		storytellerComp.forced = true;
		using (IEnumerator<FiringIncident> enumerator = ((StorytellerComp)storytellerComp).MakeIntervalIncidents((IIncidentTarget)(object)map).GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				FiringIncident incident = enumerator.Current;
				Ticker.FiringIncidents.Enqueue(incident);
			}
		}
		Viewer.TakeViewerCoins(storeIncident.cost);
		Viewer.CalculateNewKarma(storeIncident.karmaType, storeIncident.cost);
		VariablesHelpers.SendPurchaseMessage("@" + Viewer.username + " purchased a ToryTalker Vote.");
	}
}
