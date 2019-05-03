using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Incidents;
using TwitchToolkit.Votes;
using UnityEngine;
using Verse;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Animals
{
    public class MadAnimal : VotingHelper
    {
        public override bool IsPossible()
        {
            if (target is Map)
            {
                map = target as Map;

                worker = new IncidentWorker_AnimalInsanitySingle();
                worker.def = IncidentDef.Named("AnimalInsanitySingle");

                parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatSmall, map);

                return worker.CanFireNow(parms);
            }

            return false;
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private Map map;
        private IncidentWorker worker;
        private IncidentParms parms;
    }

    public class HerdMigration : VotingHelper
    {
        public override bool IsPossible()
        {
            worker = new IncidentWorker_HerdMigration();
            worker.def = IncidentDef.Named("HerdMigration");

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

    public class FarmAnimalsWanderIn : VotingHelper
    {
        public override bool IsPossible()
        {
            worker = new IncidentWorker_FarmAnimalsWanderIn();
            worker.def = IncidentDef.Named("FarmAnimalsWanderIn");

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

    public class ThrumboPasses : VotingHelper
    {
        public override bool IsPossible()
        {
            worker = new IncidentWorker_ThrumboPasses();
            worker.def = IncidentDef.Named("ThrumboPasses");

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

    public class SelfTame : VotingHelper
    {
        public override bool IsPossible()
        {
            worker = new IncidentWorker_SelfTame();
            worker.def = IncidentDef.Named("SelfTame");

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

    public class YorkshireTerriers : VotingHelper
    {
        public override bool IsPossible()
        {
            PawnKindDef animalKind = PawnKindDef.Named("YorkshireTerrier");
            int num = AnimalsCount.RandomInRange;

            worker = new IncidentWorker_SpecificAnimalsWanderIn(null, animalKind, true, num, false, true);
            worker.def = IncidentDef.Named("SelfTame");

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
        private static readonly IntRange AnimalsCount = new IntRange(3, 7);
    }

    public class ManhunterPack : VotingHelper
    {
        public override bool IsPossible()
        {
            worker = new IncidentWorker_ManhunterPack();
            worker.def = IncidentDefOf.RaidEnemy;

            parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, target);

            return worker.CanFireNow(parms);
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentWorker worker;
        private IncidentParms parms;
    }

    public class Predators : VotingHelper
    {
        public override bool IsPossible()
        {
            List<string> animals = new List<string>()
            { "Bear_Grizzly", "Bear_Polar", "Rhinoceros", "Elephant", "Megasloth", "Thrumbo" };

            animals.Shuffle();

            ThingDef def = ThingDef.Named(animals[0]);
            float averagePower = 0;
            if (def != null && def.race != null)
            {
                foreach (Tool t in def.tools)
                {
                    averagePower += t.power;
                }
                averagePower = averagePower / def.tools.Count;
            }

            float animalCount = 2.5f;
            if (averagePower > 18)
            {
                animalCount = 2.0f;
            }

            worker = new IncidentWorker_SpecificAnimalsWanderIn("TwitchStoriesLetterLabelPredators", PawnKindDef.Named(animals[0]), false, (int)animalCount, true);
            worker.def = IncidentDef.Named("HerdMigration");

            parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, target);

            return worker.CanFireNow(parms);
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentWorker worker;
        private IncidentParms parms;
    }
}
