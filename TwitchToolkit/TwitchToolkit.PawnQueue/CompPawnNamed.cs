using Verse;

namespace TwitchToolkit.PawnQueue;

public class CompPawnNamed : ThingComp
{
	public CompProperties_PawnNamed PropsName => (CompProperties_PawnNamed)(object)base.props;

	public override void PostExposeData()
	{
		Scribe_Values.Look<bool>(ref PropsName.isNamed, "isNamed", false, false);
	}
}
