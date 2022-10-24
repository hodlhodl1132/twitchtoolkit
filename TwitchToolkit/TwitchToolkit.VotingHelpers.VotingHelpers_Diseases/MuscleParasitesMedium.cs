using RimWorld;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Diseases;

public class MuscleParasitesMedium : DiseaseBase
{
	public override bool IsPossible()
	{
		base.IsPossible();
		worker.def = IncidentDef.Named("Disease_MuscleParasites");
		parms.points /= 2f;
		return worker.CanFireNow(parms);
	}
}
