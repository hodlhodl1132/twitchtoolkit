using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Mind
{
    public class Inspiration : VotingHelper
    {
        public override bool IsPossible()
        {
            if (target is Map)
            {
                map = target as Map;
                candidates = map.mapPawns.FreeColonistsSpawned.ToList();

                return true;
            }

            return false;
        }

        public override void TryExecute()
        {
            foreach (Pawn pawn in candidates)
            {
                if (pawn.Inspired) continue;

                randomAvailableInspirationDef = (
                    from x in DefDatabase<InspirationDef>.AllDefsListForReading
                    where true
                    select x).RandomElementByWeight((InspirationDef x) => x.Worker.CommonalityFor(pawn));

                if (randomAvailableInspirationDef != null)
                {
                    successfulInspiration = pawn.mindState.inspirationHandler.TryStartInspiration(randomAvailableInspirationDef);
                    if (successfulInspiration)
                    {
                        string text = pawn.Name + " has experienced an inspiration: " + randomAvailableInspirationDef.LabelCap;
                        Find.LetterStack.ReceiveLetter("Inspiration", text, LetterDefOf.PositiveEvent, pawn);
                        break;
                    }
                }
            }

            if (!successfulInspiration)
            {
                Log.Error("No pawn was available for inspiration.");
            }
        }

        private Map map;
        List<Pawn> candidates;
        private bool successfulInspiration = false;
        private InspirationDef randomAvailableInspirationDef = null;
    }

    public class PsychicWave : VotingHelper
    {
        public override bool IsPossible()
        {
            worker = new IncidentWorker_AnimalInsanityMass();
            worker.def = IncidentDef.Named("AnimalInsanityMass");

            parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, target);

            return worker.CanFireNow(parms);
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private Map map;
        private IncidentWorker worker;
        private IncidentParms parms;
    }

    public class PsychicDrone : VotingHelper
    {
        public override bool IsPossible()
        {
            worker = new IncidentWorker_PsychicDrone(); ;
            worker.def = IncidentDef.Named("PsychicDrone");

            parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, target);

            return worker.CanFireNow(parms);
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private Map map;
        private IncidentWorker worker;
        private IncidentParms parms;
    }

    public class PsychicSoothe : VotingHelper
    {
        public override bool IsPossible()
        {
            worker = new IncidentWorker_PsychicSoothe(); ;
            worker.def = IncidentDef.Named("PsychicSoothe");

            parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, target);

            return worker.CanFireNow(parms);
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private Map map;
        private IncidentWorker worker;
        private IncidentParms parms;
    }

    // TODO
    public class MentalBreakBase : VotingHelper
    {
        public override bool IsPossible()
        {
            if (target is Map)
            {
                map = target as Map;
                List<Pawn> candidates = map.mapPawns.FreeColonistsSpawned.Where(s =>
                    !s.mindState.mentalStateHandler.InMentalState
                ).OrderBy(s => s.mindState.mentalBreaker.CurMood).Take(3).ToList();

                if (candidates != null && candidates.Count > 0)
                {
                    candidates.Shuffle();

                    pawn = candidates[0];
                }
            }

            return false;
        }

        public override void TryExecute()
        {
            List<MentalBreakDef> mentalBreakDefs = DefDatabase<MentalBreakDef>.AllDefs.Where(s =>
                s.intensity == intensity
            ).ToList();

            mentalBreakDefs.Shuffle();

            mentalBreakDefs[0].Worker.TryStart(pawn, "Upset by chat", true);
        }

        private Map map;
        public Pawn pawn;
        public MentalBreakIntensity intensity;
    }

    public class MentalBreakMinor : MentalBreakBase
    {
        public MentalBreakMinor()
        {
            intensity = MentalBreakIntensity.Minor;
        }
    }

    public class MentalBreakMajor : MentalBreakBase
    {
        public MentalBreakMajor()
        {
            intensity = MentalBreakIntensity.Major;
        }
    }

    public class MentalBreakExtreme : MentalBreakBase
    {
        public MentalBreakExtreme()
        {
            intensity = MentalBreakIntensity.Extreme;
        }
    }

    public class MentalBreakBerserk : MentalBreakBase
    {
        public override void TryExecute()
        {
            MentalBreakDef mentalBreakDef = DefDatabase<MentalBreakDef>.GetNamed("Berserk");

            mentalBreakDef.Worker.TryStart(pawn, "Upset by Chat", true);
        }
    }
}
