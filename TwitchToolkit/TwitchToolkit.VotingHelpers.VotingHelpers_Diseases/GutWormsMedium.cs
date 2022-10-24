using RimWorld;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Diseases;

public class GutWormsMedium : DiseaseBase
{
	public override bool IsPossible()
	{
		base.IsPossible();
		worker.def = IncidentDef.Named("Disease_GutWorms");
		parms.points /= 2f;
		return worker.CanFireNow(parms);
	}
}
