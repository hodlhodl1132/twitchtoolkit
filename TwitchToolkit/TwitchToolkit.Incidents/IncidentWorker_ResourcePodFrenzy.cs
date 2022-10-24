using System.Collections.Generic;
using RimWorld;
using Verse;

namespace TwitchToolkit.Incidents;

public class IncidentWorker_ResourcePodFrenzy : IncidentWorker
{
	private readonly string Quote;

	public IncidentWorker_ResourcePodFrenzy(string quote)
	{
		Quote = quote;
	}

	protected override bool TryExecuteWorker(IncidentParms parms)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing erences)
		//IL_000d: Expected O, but got Unknown
		//IL_0024: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0029: Unknown result type (might be due to invalid IL or missing erences)
		//IL_002b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0050: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0055: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0066: Unknown result type (might be due to invalid IL or missing erences)
		//IL_006c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0071: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0072: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0090: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0095: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing erences)
		Map map = (Map)parms.target;
		for (int x = 0; x < 10; x++)
		{
			List<Thing> things = ThingSetMakerDefOf.ResourcePod.root.Generate();
			IntVec3 intVec = DropCellFinder.RandomDropSpot(map, true);
			DropPodUtility.DropThingsNear(intVec, map, (IEnumerable<Thing>)things, 110, false, true, true, true);
		}
		TaggedString text = Translator.Translate("TwitchToolkitCargoPodFrenzyInc");
		if (Quote != null)
		{
			text += "\n\n";
			text += Helper.ReplacePlaceholder(Quote);
		}
		Find.LetterStack.ReceiveLetter(Translator.Translate("TwitchToolkitCargoPodFrenzyInc"), text, LetterDefOf.PositiveEvent, (LookTargets)null, (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
		return true;
	}
}
