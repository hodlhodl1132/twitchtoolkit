using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RimWorld;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit;

public class StorytellerComp_CustomRandomStoryTeller : StorytellerComp
{
	private readonly TwitchToolkit _twitchstories = LoadedModManager.GetMod<TwitchToolkit>();

	private IIncidentTarget incidentTarget;

	private FiringIncident singleIncident = null;

	private VoteIncidentDef incidentOptions = null;

	private bool makeIncidentOptions = false;

	private Thread thread = null;

	protected StorytellerCompProperties_CustomRandomStoryTeller Props => (StorytellerCompProperties_CustomRandomStoryTeller)(object)base.props;

	public IncidentParms parms { get; private set; }

	public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
	{
		if (VoteHandler.voteActive)
		{
			yield break;
		}
		if (VoteHelper.TimeForEventVote())
		{
			MakeRandomVoteEvent(target);
			yield break;
		}
		if (thread != null)
		{
			thread = new Thread(ThreadProc);
			thread.Start();
			yield break;
		}
		if (singleIncident != null)
		{
			thread.Join();
			thread = null;
			yield return singleIncident;
		}
		if (makeIncidentOptions && incidentOptions != null)
		{
			thread.Join();
			thread = null;
			VoteHandler.QueueVote(incidentOptions);
		}
		Thread.Sleep(0);
	}

	public void ThreadProc()
	{
		IEnumerable<FiringIncident> result = MakeIntervalIncidentsThread();
		if (result == typeof(FiringIncident))
		{
			singleIncident = (FiringIncident)((result is FiringIncident) ? result : null);
		}
		else if (incidentOptions != null)
		{
			makeIncidentOptions = true;
		}
	}

	public IEnumerable<FiringIncident> MakeIntervalIncidentsThread()
	{
		if (!Rand.MTBEventOccurs(Props.mtbDays, 60000f, 1000f) || ToolkitSettings.TimedStorytelling)
		{
			yield break;
		}
		bool targetIsRaidBeacon = incidentTarget.IncidentTargetTags().Contains(IncidentTargetTagDefOf.Map_RaidBeacon);
		List<IncidentCategoryDef> triedCategories = new List<IncidentCategoryDef>();
		List<IncidentDef> pickedoptions = new List<IncidentDef>();
		IncidentDef incDef = default(IncidentDef);
		IncidentDef picked = default(IncidentDef);
		while (true)
		{
			IncidentCategoryDef category = ChooseRandomCategory(incidentTarget, triedCategories);
			IncidentParms parms = ((StorytellerComp)this).GenerateParms(category, incidentTarget);
			Helper.Log($"Trying Category {category}");
			parms = ((StorytellerComp)this).GenerateParms(category, incidentTarget);
			IEnumerable<IncidentDef> options2 = from d in UsableIncidentsInCategory(category, parms)
				where !d.NeedsParmsPoints || parms.points >= d.minThreatPoints
				select d;
			if (!GenCollection.TryRandomElementByWeight<IncidentDef>(options2, (Func<IncidentDef, float>)base.IncidentChanceFinal, out incDef))
			{
				triedCategories.Add(category);
				if (triedCategories.Count >= Props.categoryWeights.Count)
				{
					break;
				}
				Thread.Sleep(0);
				continue;
			}
			Helper.Log($"Events Possible: {options2.Count()}");
			if (options2.Count() > 1)
			{
				options2 = options2.Where((IncidentDef k) => k != incDef);
				pickedoptions.Add(incDef);
				for (int x = 0; x < ToolkitSettings.VoteOptions - 1 && x < options2.Count(); x++)
				{
					GenCollection.TryRandomElementByWeight<IncidentDef>(options2, (Func<IncidentDef, float>)base.IncidentChanceFinal, out picked);
					if (picked != null)
					{
						options2 = options2.Where((IncidentDef k) => k != picked);
						pickedoptions.Add(picked);
						Thread.Sleep(0);
					}
				}
				Dictionary<int, IncidentDef> incidents = new Dictionary<int, IncidentDef>();
				for (int i = 0; i < pickedoptions.Count(); i++)
				{
					incidents.Add(i, pickedoptions.ToList()[i]);
				}
				incidentOptions = new VoteIncidentDef(incidents, (StorytellerComp)(object)this, this.parms);
				Helper.Log("Events created");
				yield return null;
			}
			else if (options2.Count() == 1)
			{
				yield return new FiringIncident(incDef, (StorytellerComp)(object)this, this.parms);
			}
			if (Props.skipThreatBigIfRaidBeacon && targetIsRaidBeacon && incDef.category == IncidentCategoryDefOf.ThreatBig)
			{
			}
			break;
		}
		Thread.Sleep(0);
	}

	public IncidentCategoryDef ChooseRandomCategory(IIncidentTarget target, List<IncidentCategoryDef> skipCategories)
	{
		if (!skipCategories.Contains(IncidentCategoryDefOf.ThreatBig))
		{
			int num = Find.TickManager.TicksGame - target.StoryState.LastThreatBigTick;
			if (target.StoryState.LastThreatBigTick >= 0 && (float)num > 60000f * Props.maxThreatBigIntervalDays)
			{
				return IncidentCategoryDefOf.ThreatBig;
			}
		}
		return GenCollection.RandomElementByWeight<IncidentCategoryEntry>(Props.categoryWeights.Where((IncidentCategoryEntry cw) => !skipCategories.Contains(cw.category)), (Func<IncidentCategoryEntry, float>)((IncidentCategoryEntry cw) => cw.weight)).category;
	}

	public override IncidentParms GenerateParms(IncidentCategoryDef incCat, IIncidentTarget target)
	{
		IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(incCat, target);
		incidentParms.points *= ((FloatRange)( Props.randomPointsFactorRange)).RandomInRange;
		return incidentParms;
	}

	public void MakeRandomVoteEvent(IIncidentTarget target)
	{
		Helper.Log("Forcing vote event");
		if (VoteHandler.voteActive || !ToolkitSettings.TimedStorytelling)
		{
			return;
		}
		bool targetIsRaidBeacon = target.IncidentTargetTags().Contains(IncidentTargetTagDefOf.Map_RaidBeacon);
		List<IncidentCategoryDef> triedCategories = new List<IncidentCategoryDef>();
		List<IncidentDef> pickedoptions = new List<IncidentDef>();
		IncidentCategoryDef category = ChooseRandomCategory(target, triedCategories);
		IncidentParms parms = ((StorytellerComp)this).GenerateParms(category, target);
		Helper.Log($"Trying Category{category}");
		parms = ((StorytellerComp)this).GenerateParms(category, target);
		IEnumerable<IncidentDef> options = from d in UsableIncidentsInCategory(category, parms)
			where !d.NeedsParmsPoints || parms.points >= d.minThreatPoints
			select d;
		IncidentDef incDef = default(IncidentDef);
		if (GenCollection.TryRandomElementByWeight<IncidentDef>(options, (Func<IncidentDef, float>)base.IncidentChanceFinal, out incDef))
		{
		}
		triedCategories.Add(category);
		if (triedCategories.Count >= Props.categoryWeights.Count)
		{
		}
		Helper.Log($"Events Possible: {options.Count()}");
		if (options.Count() <= 1)
		{
			return;
		}
		options = options.Where((IncidentDef k) => k != incDef);
		pickedoptions.Add(incDef);
		IncidentDef picked = default(IncidentDef);
		for (int x = 0; x < ToolkitSettings.VoteOptions - 1 && x < options.Count(); x++)
		{
			GenCollection.TryRandomElementByWeight<IncidentDef>(options, (Func<IncidentDef, float>)base.IncidentChanceFinal, out picked);
			if (picked != null)
			{
				options = options.Where((IncidentDef k) => k != picked);
				pickedoptions.Add(picked);
			}
		}
		Dictionary<int, IncidentDef> incidents = new Dictionary<int, IncidentDef>();
		for (int i = 0; i < pickedoptions.Count(); i++)
		{
			incidents.Add(i, pickedoptions.ToList()[i]);
		}
		if (incidents.Count > 1)
		{
			VoteHandler.QueueVote(new VoteIncidentDef(incidents, (StorytellerComp)(object)this, parms));
			Helper.Log("Events created");
		}
	}

	public IIncidentTarget GetRandomTarget()
	{
		List<IIncidentTarget> targets = Find.Storyteller.AllIncidentTargets;
		if (targets == null)
		{
			throw new Exception("No valid targets");
		}
		if (targets.Count() > 1)
		{
			return targets[Rand.Range(1, targets.Count()) - 1];
		}
		return targets[0];
	}
}
