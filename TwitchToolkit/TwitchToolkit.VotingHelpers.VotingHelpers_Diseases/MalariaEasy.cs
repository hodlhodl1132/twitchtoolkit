using RimWorld;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Diseases;

public class MalariaEasy : DiseaseBase
{
	public override bool IsPossible()
	{
		base.IsPossible();
		worker.def = IncidentDef.Named("Disease_Malaria");
		parms.points /= 3f;
		return worker.CanFireNow(parms);
	}
}
