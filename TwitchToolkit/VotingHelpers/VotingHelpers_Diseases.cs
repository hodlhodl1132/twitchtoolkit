using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Diseases
{
    public class HeartAttack : VotingHelper
    {
        public override bool IsPossible()
        {
            if (target.PlayerPawnsForStoryteller != null)
            {
                return true;
            }

            return false;
        }

        public override void TryExecute()
        {
            List<Pawn> candidates = target.PlayerPawnsForStoryteller.ToList();
            candidates.Shuffle();

            candidates[0].health.AddHediff(HediffDef.Named("HeartAttack"));

            string text = candidates[0].Name + " is suffering from a heart attack.";

            Find.LetterStack.ReceiveLetter("HeartAttack", text, LetterDefOf.NegativeEvent, candidates[0]);
        }
    }

    public abstract class DiseaseBase : VotingHelper
    {
        public override bool IsPossible()
        {
            parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.DiseaseHuman, target);
            points = parms.points;
            return true;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private float points;

        public IncidentWorker worker = new IncidentWorker_DiseaseHuman();
        public IncidentDef disease;
        public IncidentParms parms;
    }

    public class PlagueEasy : DiseaseBase
    {
        public override bool IsPossible()
        {
            base.IsPossible();
            worker.def = IncidentDef.Named("Disease_Plague");
            parms.points = parms.points / 3;
            return worker.CanFireNow(parms);
        }
    }

    public class PlagueMedium : DiseaseBase
    {
        public override bool IsPossible()
        {
            base.IsPossible();
            worker.def = IncidentDef.Named("Disease_Plague");
            parms.points = parms.points / 2;
            return worker.CanFireNow(parms);
        }
    }

    public class PlagueHard : DiseaseBase
    {
        public override bool IsPossible()
        {
            base.IsPossible();
            worker.def = IncidentDef.Named("Disease_Plague");
            return worker.CanFireNow(parms);
        }
    }

    public class FluEasy : DiseaseBase
    {
        public override bool IsPossible()
        {
            base.IsPossible();
            worker.def = IncidentDef.Named("Disease_Flu");
            parms.points = parms.points / 3;
            return worker.CanFireNow(parms);
        }
    }

    public class FluMedium : DiseaseBase
    {
        public override bool IsPossible()
        {
            base.IsPossible();
            worker.def = IncidentDef.Named("Disease_Flu");
            parms.points = parms.points / 2;
            return worker.CanFireNow(parms);
        }
    }

    public class FluHard : DiseaseBase
    {
        public override bool IsPossible()
        {
            base.IsPossible();
            worker.def = IncidentDef.Named("Disease_Flu");
            return worker.CanFireNow(parms);
        }
    }

    public class MalariaEasy : DiseaseBase
    {
        public override bool IsPossible()
        {
            base.IsPossible();
            worker.def = IncidentDef.Named("Disease_Malaria");
            parms.points = parms.points / 3;
            return worker.CanFireNow(parms);
        }
    }

    public class MalariaMedium : DiseaseBase
    {
        public override bool IsPossible()
        {
            base.IsPossible();
            worker.def = IncidentDef.Named("Disease_Malaria");
            parms.points = parms.points / 2;
            return worker.CanFireNow(parms);
        }
    }

    public class MalariaHard : DiseaseBase
    {
        public override bool IsPossible()
        {
            base.IsPossible();
            worker.def = IncidentDef.Named("Disease_Malaria");
            return worker.CanFireNow(parms);
        }
    }

    public class GutWormsEasy : DiseaseBase
    {
        public override bool IsPossible()
        {
            base.IsPossible();
            worker.def = IncidentDef.Named("Disease_GutWorms");
            parms.points = parms.points / 3;
            return worker.CanFireNow(parms);
        }
    }

    public class GutWormsMedium : DiseaseBase
    {
        public override bool IsPossible()
        {
            base.IsPossible();
            worker.def = IncidentDef.Named("Disease_GutWorms");
            parms.points = parms.points / 2;
            return worker.CanFireNow(parms);
        }
    }

    public class GutWormsHard : DiseaseBase
    {
        public override bool IsPossible()
        {
            base.IsPossible();
            worker.def = IncidentDef.Named("Disease_GutWorms");
            return worker.CanFireNow(parms);
        }
    }

    public class MuscleParasitesEasy : DiseaseBase
    {
        public override bool IsPossible()
        {
            base.IsPossible();
            worker.def = IncidentDef.Named("Disease_MuscleParasites");
            parms.points = parms.points / 3;
            return worker.CanFireNow(parms);
        }
    }

    public class MuscleParasitesMedium : DiseaseBase
    {
        public override bool IsPossible()
        {
            base.IsPossible();
            worker.def = IncidentDef.Named("Disease_MuscleParasites");
            parms.points = parms.points / 2;
            return worker.CanFireNow(parms);
        }
    }

    public class MuscleParasitesHard : DiseaseBase
    {
        public override bool IsPossible()
        {
            base.IsPossible();
            worker.def = IncidentDef.Named("Disease_MuscleParasites");
            return worker.CanFireNow(parms);
        }
    }

    public abstract class Infection : VotingHelper
    {
        public override bool IsPossible()
        {
            candidates = target.PlayerPawnsForStoryteller.ToList();

            candidates.Shuffle();

            int count = (int) Math.Round((float) candidates.Count * percentAffected);

            candidates = candidates.Take(count).ToList();

            if (candidates.Count > 0)
            {
                return true;
            }

            return false;
        }

        public override void TryExecute()
        {
            string label = "";
            string text = "";

            foreach (Pawn pawn in candidates)
            {
                BodyPartRecord part = pawn.health.hediffSet.GetRandomNotMissingPart(new DamageDef());
                Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.WoundInfection, pawn, part);
                label = "LetterLabelNewDisease".Translate() + " (" + hediff.def.label + ")";
                var d = hediff.def.CompProps<HediffCompProperties_Discoverable>();
                d.sendLetterWhenDiscovered = false;
                pawn.health.hediffSet.hediffs.Add(hediff);

                if (candidates.Count <= 1)
                {
                    text = "NewPartDisease".Translate(pawn.Named("PAWN"), part.Label, pawn.LabelDefinite(), hediff.def.label).AdjustedFor(pawn, "PAWN").CapitalizeFirst();
                }
            }

            if (candidates.Count > 1)
            {
                text = "TwitchStoriesDescription18".Translate();
                Current.Game.letterStack.ReceiveLetter(label, text, LetterDefOf.NegativeEvent, candidates, null, null);
            }
            else
            {
                Current.Game.letterStack.ReceiveLetter(label, text, LetterDefOf.NegativeEvent, candidates[0], null, null);
            }
        }

        public float percentAffected;
        private List<Pawn> candidates;
    }

    public class InfectionEasy : Infection
    {
        public InfectionEasy()
        {
            this.percentAffected = 0.25f;
        }
    }

    public class InfectionMedium : Infection
    {
        public InfectionMedium()
        {
            this.percentAffected = 0.40f;
        }
    }

    public class InfectionHard : Infection
    {
        public InfectionHard()
        {
            this.percentAffected = 0.63f;
        }
    }

    public class Blindness : VotingHelper
    {
        public override bool IsPossible()
        {
            candidates = target.PlayerPawnsForStoryteller.ToList();

            if (candidates == null || candidates.Count < 1)
            {
                return false;
            }

            return true;
        }

        public override void TryExecute()
        {
            Pawn pawn = candidates[0];
            pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).FirstOrDefault((BodyPartRecord x) => x.def == BodyPartDefOf.Eye);

            string text = pawn.Name + " has experienced sudden blindness.";

            Find.LetterStack.ReceiveLetter("Blindness", text, LetterDefOf.NegativeEvent, pawn);
        }

        private List<Pawn> candidates;
    }
}
