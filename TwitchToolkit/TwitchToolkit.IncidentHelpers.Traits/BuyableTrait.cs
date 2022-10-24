using RimWorld;

namespace TwitchToolkit.IncidentHelpers.Traits;

public class BuyableTrait
{
	public TraitDef def;

	public string label;

	public int degree;

	public BuyableTrait(TraitDef def, string label, int degree = 0)
	{
		this.def = def;
		this.label = label;
		this.degree = degree;
	}
}
