using RimWorld;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Diseases;

public class GutWormsEasy : DiseaseBase
{
	public override bool IsPossible()
	{
		base.IsPossible();
		worker.def = IncidentDef.Named("Disease_GutWorms");
		parms.points /= 3f;
		return worker.CanFireNow(parms);
	}
}
