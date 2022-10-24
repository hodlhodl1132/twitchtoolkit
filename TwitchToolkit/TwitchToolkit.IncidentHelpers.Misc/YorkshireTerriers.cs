using RimWorld;
using TwitchToolkit.Incidents;
using TwitchToolkit.Store;
using UnityEngine;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Misc;

public class YorkshireTerriers : IncidentHelper
{
	private IncidentParms parms = null;

	private IncidentWorker worker = null;

	private static readonly IntRange AnimalsCount = new IntRange(3, 5);

	public override bool IsPossible()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0011: Unknown result type (might be due to invalid IL or missing erences)
		PawnKindDef animalKind = PawnKindDef.Named("YorkshireTerrier");
		IntRange animalsCount = AnimalsCount;
		int num = ((IntRange)(animalsCount)).RandomInRange;
		num = Mathf.Max(num, Mathf.CeilToInt(4f / animalKind.RaceProps.baseBodySize));
		worker = (IncidentWorker)(object)new IncidentWorker_SpecificAnimalsWanderIn(null, animalKind, joinColony: true, num, manhunter: false, defaultText: true);
		worker.def = IncidentDef.Named("FarmAnimalsWanderIn");
		Map map = Helper.AnyPlayerMap;
		if (map != null)
		{
			parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, (IIncidentTarget)(object)map);
			return worker.CanFireNow(parms);
		}
		return false;
	}

	public override void TryExecute()
	{
		worker.TryExecute(parms);
	}
}
