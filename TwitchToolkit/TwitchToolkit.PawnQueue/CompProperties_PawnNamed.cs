using Verse;

namespace TwitchToolkit.PawnQueue;

public class CompProperties_PawnNamed : CompProperties
{
	public bool isNamed;

	public CompProperties_PawnNamed()
	{
		base.compClass = typeof(CompPawnNamed);
	}
}
