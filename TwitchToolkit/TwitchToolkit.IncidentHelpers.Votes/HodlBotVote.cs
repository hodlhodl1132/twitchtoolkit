using System.Collections.Generic;
using RimWorld;
using TwitchToolkit.Store;
using TwitchToolkit.Storytellers;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Votes;

public class HodlBotVote : IncidentHelperVariables
{
	private StorytellerPack hodlBot;

	public override Viewer Viewer { get; set; }

	public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
	{
		Viewer = viewer;
		hodlBot = DefDatabase<StorytellerPack>.GetNamed("HodlBot", true);
		if (hodlBot != null && hodlBot.enabled)
		{
			return true;
		}
		return false;
	}

	public override void TryExecute()
	{
		Map map = Helper.AnyPlayerMap;
		StorytellerComp_HodlBot storytellerComp = new StorytellerComp_HodlBot();
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
		VariablesHelpers.SendPurchaseMessage("@" + Viewer.username + " purchased a HodlBot Vote.");
	}
}
