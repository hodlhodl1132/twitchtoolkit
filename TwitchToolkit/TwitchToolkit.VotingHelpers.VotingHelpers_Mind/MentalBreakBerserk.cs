using Verse;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Mind;

public class MentalBreakBerserk : MentalBreakBase
{
	public override void TryExecute()
	{
		MentalBreakDef mentalBreakDef = DefDatabase<MentalBreakDef>.GetNamed("Berserk", true);
		mentalBreakDef.Worker.TryStart(pawn, "Upset by Chat", true);
	}
}
