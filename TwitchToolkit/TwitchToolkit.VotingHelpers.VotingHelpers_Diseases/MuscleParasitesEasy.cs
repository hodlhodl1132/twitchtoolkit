using RimWorld;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Diseases;

public class MuscleParasitesEasy : DiseaseBase
{
	public override bool IsPossible()
	{
		base.IsPossible();
		worker.def = IncidentDef.Named("Disease_MuscleParasites");
		parms.points /= 3f;
		return worker.CanFireNow(parms);
	}
}
