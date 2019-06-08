using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Hazards
{
    public class Blight : VotingHelper
    {
        public override bool IsPossible()
        {
            worker = new IncidentWorker_CropBlight();
            worker.def = IncidentDef.Named("CropBlight");

            parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, target);

            return worker.CanFireNow(parms);
        }

        public override void TryExecute()
        {
            worker.TryExecute(parms);
        }

        private IncidentWorker worker;
        private IncidentParms parms;
    }

    public class SolarFlare : VotingHelper_GameCondition
    {
        public SolarFlare()
        {
            incidentDefName = "SolarFlare";
        }
    }

    public class VolcanicWinter : VotingHelper_GameCondition
    {
        public VolcanicWinter()
        {
            incidentDefName = "VolcanicWinter";
        }
    }

    public class ToxicFallout : VotingHelper_GameCondition
    {
        public ToxicFallout()
        {
            incidentDefName = "ToxicFallout";
        }
    }

    public class HeatWave : VotingHelper_GameCondition
    {
        public HeatWave()
        {
            incidentDefName = "HeatWave";
        }
    }

    public class ColdSnap : VotingHelper_GameCondition
    {
        public ColdSnap()
        {
            incidentDefName = "ColdSnap";
        }
    }

    public class Tornado : VotingHelper
    {
        public Tornado()
        {
            map = Helper.AnyPlayerMap;
        }

        public override bool IsPossible()
        {
            cellRect = CellRect.WholeMap(map).ContractedBy(30);
            if (cellRect.IsEmpty)
            {
                cellRect = CellRect.WholeMap(map);
            }

            if (!CellFinder.TryFindRandomCellInsideWith(cellRect, (IntVec3 x) => this.CanSpawnTornadoAt(x, map), out loc))
            {
                return false;
            }

            return true;
        }

        public override void TryExecute()
        {
            RimWorld.Tornado tornado = (RimWorld.Tornado)GenSpawn.Spawn(ThingDefOf.Tornado, loc, map);

            string text = "A  mobile, destructive vortex of violently rotating winds have appeard. Seek safe shelter!";

            Find.LetterStack.ReceiveLetter("Tornado", text, LetterDefOf.NegativeEvent, tornado);
        }

        protected bool CanSpawnTornadoAt(IntVec3 c, Map map)
        {
            if (c.Fogged(map))
            {
                return false;
            }
            int num = GenRadial.NumCellsInRadius(7f);
            for (int i = 0; i < num; i++)
            {
                IntVec3 c2 = c + GenRadial.RadialPattern[i];
                if (c2.InBounds(map))
                {
                    if (this.AnyPawnOfPlayerFactionAt(c2, map))
                    {
                        return false;
                    }

                    // Added to eliminate spawning inside areas that will immediate despawn
                    RoofDef roofDef = map.roofGrid.RoofAt(c2);
                    if (roofDef != null && roofDef.isThickRoof)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        protected bool AnyPawnOfPlayerFactionAt(IntVec3 c, Map map)
        {
            List<Thing> thingList = c.GetThingList(map);
            for (int i = 0; i < thingList.Count; i++)
            {
                Pawn pawn = thingList[i] as Pawn;
                if (pawn != null && pawn.Faction == Faction.OfPlayer)
                {
                    return true;
                }
            }
            return false;
        }

        internal IntVec3 loc;
        internal CellRect cellRect;
        internal readonly Map map;
    }

    public class Tornados : Tornado
    {
        public override void TryExecute()
        {
            int count = 0;
            List<Thing> tornados = new List<Thing>();

            while (CellFinder.TryFindRandomCellInsideWith(cellRect, (IntVec3 x) => this.CanSpawnTornadoAt(x, map), out loc) && count < 3)
            {
                count++;
                RimWorld.Tornado tornado = (RimWorld.Tornado)GenSpawn.Spawn(ThingDefOf.Tornado, loc, map);
                tornados.Add(tornado);
            }

            string text = "A  mobile, destructive vortex of violently rotating winds have appeard. Seek safe shelter!";

            Find.LetterStack.ReceiveLetter("Tornados", text, LetterDefOf.NegativeEvent, tornados);
        }
    }
}
