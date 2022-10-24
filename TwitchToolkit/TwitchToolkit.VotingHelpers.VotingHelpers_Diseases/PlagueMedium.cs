using RimWorld;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Diseases;

public class PlagueMedium : DiseaseBase
{
	public override bool IsPossible()
	{
		base.IsPossible();
		worker.def = IncidentDef.Named("Disease_Plague");
		parms.points /= 2f;
		return worker.CanFireNow(parms);
	}
}
