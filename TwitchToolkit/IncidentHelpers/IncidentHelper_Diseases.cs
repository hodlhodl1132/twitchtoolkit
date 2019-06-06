using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Incidents;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Diseases
{

    public class RandomDisease : IncidentHelperVariables
    {
        public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
        {
            this.separateChannel = separateChannel;
            this.viewer = viewer;
            string[] command = message.Split(' ');
            if (command.Length < 3)
            {
                Toolkit.client.SendMessage($"@{viewer.username} syntax is {this.storeIncident.syntax}", separateChannel);
                return false;
            }

            if (!VariablesHelpers.PointsWagerIsValid(
                    command[2],
                    viewer,
                    ref pointsWager,
                    ref storeIncident,
                    separateChannel
                ))
            {
                return false;
            }

            worker = new IncidentWorker_DiseaseHuman();
            List<IncidentDef> allDiseases = DefDatabase<IncidentDef>.AllDefs.Where(s => s.workerClass == typeof(IncidentWorker_DiseaseHuman)).ToList();
            allDiseases.Shuffle();
            worker.def = allDiseases[0];

            target = Current.Game.AnyPlayerHomeMap;
            if (target == null)
            {
                return false;
            }

            float defaultThreatPoints = StorytellerUtility.DefaultSiteThreatPointsNow();
            parms = StorytellerUtility.DefaultParmsNow(worker.def.category, target);
            parms.points = IncidentHelper_PointsHelper.RollProportionalGamePoints(storeIncident, pointsWager, StorytellerUtility.DefaultThreatPointsNow(target));
            return worker.CanFireNow(parms);
        
        }

        public override void TryExecute()
        {
            if (worker.TryExecute(parms))
            {
                viewer.TakeViewerCoins(pointsWager);
                viewer.CalculateNewKarma(this.storeIncident.karmaType, pointsWager);
                VariablesHelpers.SendPurchaseMessage($"Starting {worker.def.LabelCap} with {pointsWager} points wagered and {(int)parms.points} disease points purchased by {viewer.username}", separateChannel);
                return;
            }
            Toolkit.client.SendMessage($"@{viewer.username} not enough points spent for diseases.", separateChannel);
        }

        private int pointsWager = 0;
        private IncidentWorker worker = null;
        private IncidentParms parms = null;
        private IIncidentTarget target = null;
        private bool separateChannel = false;

        public override Viewer viewer { get; set; }
    }

    public class SpecificDisease : IncidentHelperVariables
    {
        public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
        {
            this.separateChannel = separateChannel;
            this.viewer = viewer;
            string[] command = message.Split(' ');
            if (command.Length < 4)
            {
                Toolkit.client.SendMessage($"@{viewer.username} syntax is {this.storeIncident.syntax}", separateChannel);
                return false;
            }

            if (!VariablesHelpers.PointsWagerIsValid(
                    command[3],
                    viewer,
                    ref pointsWager,
                    ref storeIncident,
                    separateChannel
                ))
            {
                return false;
            }

            string diseaseLabel = command[2].ToLower();

            worker = new IncidentWorker_DiseaseHuman();
            List<IncidentDef> allDiseases = DefDatabase<IncidentDef>.AllDefs.Where(s =>
                    s.category == IncidentCategoryDefOf.DiseaseHuman &&
                    string.Join("", s.LabelCap.Split(' ')).ToLower() == diseaseLabel
                ).ToList();

            if (allDiseases.Count < 1)
            {
                Toolkit.client.SendMessage($"@{viewer.username} no disease {diseaseLabel} found.", separateChannel);
                return false;
            }

            allDiseases.Shuffle();
            worker.def = allDiseases[0];

            target = Current.Game.AnyPlayerHomeMap;
            if (target == null)
            {
                return false;
            }

            float defaultThreatPoints = StorytellerUtility.DefaultSiteThreatPointsNow();
            parms = StorytellerUtility.DefaultParmsNow(worker.def.category, target);
            parms.points = IncidentHelper_PointsHelper.RollProportionalGamePoints(storeIncident, pointsWager, StorytellerUtility.DefaultThreatPointsNow(target));
            return worker.CanFireNow(parms);
        
        }

        public override void TryExecute()
        {
            if (worker.TryExecute(parms))
            {
                viewer.TakeViewerCoins(pointsWager);
                viewer.CalculateNewKarma(this.storeIncident.karmaType, pointsWager);
                VariablesHelpers.SendPurchaseMessage($"Starting {worker.def.LabelCap} with {pointsWager} points wagered and {(int)parms.points} disease points purchased by {viewer.username}", separateChannel);
                return;
            }
            Toolkit.client.SendMessage($"@{viewer.username} not enough points spent for diseases.", separateChannel);
        }

        private int pointsWager = 0;
        private IncidentWorker worker = null;
        private IncidentParms parms = null;
        private IIncidentTarget target = null;
        private bool separateChannel = false;

        public override Viewer viewer { get; set; }
    }

}
