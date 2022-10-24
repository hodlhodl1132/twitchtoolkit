using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace TwitchToolkit.Storytellers;

public class StorytellerComp_TwitchToolkit : StorytellerComp
{
	protected StorytellerCompProperties_TwitchToolkit Props => (StorytellerCompProperties_TwitchToolkit)(object)base.props;

	public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
	{
		CheckIfPacksAreEnabled();
		StoryTellerVoteTracker voteTracker = Current.Game.GetComponent<StoryTellerVoteTracker>();
		List<StorytellerPack> allPacks = (from s in DefDatabase<StorytellerPack>.AllDefs
			where s.enabled && voteTracker.HaveMinimumDaysBetweenEventsPassed(s)
			select s).ToList();
		if (allPacks == null || allPacks.Count < 1)
		{
			yield break;
		}
		allPacks.Shuffle();
		StorytellerPack chosen = allPacks[0];
		foreach (FiringIncident item in chosen.StorytellerComp.MakeIntervalIncidents(target))
		{
			yield return item;
		}
	}

	private void CheckIfPacksAreEnabled()
	{
		List<StorytellerComp> comps = Current.Game.storyteller.storytellerComps;
		foreach (StorytellerPack pack in DefDatabase<StorytellerPack>.AllDefs)
		{
			if (((Def)pack).defName == "HodlBot")
			{
				pack.enabled = ToolkitSettings.HodlBotEnabled;
			}
			else if (((Def)pack).defName == "ToryTalker")
			{
				pack.enabled = ToolkitSettings.ToryTalkerEnabled;
			}
			else if (((Def)pack).defName == "UristBot")
			{
				pack.enabled = ToolkitSettings.UristBotEnabled;
			}
			else if (((Def)pack).defName == "Milasandra")
			{
				pack.enabled = ToolkitSettings.MilasandraEnabled;
			}
			else if (((Def)pack).defName == "Mercurius")
			{
				pack.enabled = ToolkitSettings.MercuriusEnabled;
			}
		}
	}
}
