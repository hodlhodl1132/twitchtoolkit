using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Traits;

[StaticConstructorOnStartup]
public static class AllTraits
{
	public static List<BuyableTrait> buyableTraits;

	static AllTraits()
	{
		buyableTraits = new List<BuyableTrait>();
		List<TraitDef> traitDefs = DefDatabase<TraitDef>.AllDefs.ToList();
		foreach (TraitDef def in traitDefs)
		{
			if (def.degreeDatas != null)
			{
				foreach (TraitDegreeData degree in def.degreeDatas)
				{
					buyableTraits.Add(new BuyableTrait(def, string.Join("", degree.label.Split(' ')).ToLower(), degree.degree));
				}
			}
			else
			{
				buyableTraits.Add(new BuyableTrait(def, string.Join("", ((Def)def).label.Split(' ')).ToLower()));
			}
		}
	}
}
