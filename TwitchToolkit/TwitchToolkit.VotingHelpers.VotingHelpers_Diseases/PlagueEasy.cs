using RimWorld;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Diseases;

public class PlagueEasy : DiseaseBase
{
	public override bool IsPossible()
	{
		base.IsPossible();
		worker.def = IncidentDef.Named("Disease_Plague");
		parms.points /= 3f;
		return worker.CanFireNow(parms);
	}
}
