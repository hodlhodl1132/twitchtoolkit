using System.Collections.Generic;
using RimWorld;
using TwitchToolkit.Store;
using Verse;
namespace TwitchToolkit.IncidentHelpers.Weather
{
	public class ToxicFallout : IncidentHelper
	{
		private IncidentParms parms = (IncidentParms) null;
		private IncidentWorker worker = (IncidentWorker) null;

		public override bool IsPossible()
		{
			this.worker = (IncidentWorker) new IncidentWorker_MakeGameCondition();
			this.worker.def = IncidentDefOf.ToxicFallout;
			this.parms = new IncidentParms();
			List<Map> maps = Current.Game.Maps;
			maps.Shuffle<Map>();
			foreach (IIncidentTarget incidentTarget in maps)
			{
				this.parms.target = incidentTarget;
				if (this.worker.CanFireNow(this.parms))
					return true;
			}
			return false;
		}

		public override void TryExecute() => this.worker.TryExecute(this.parms);
	}
}
