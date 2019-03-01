using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using UnityEngine;

namespace TwitchToolkit.Incidents
{
    public class IncidentWorker_ShipPartCrash : IncidentWorker_Quote
    {
        const float ShipPointsFactor = 0.9f;

        const int IncidentMinimumPoints = 300;

        public IncidentWorker_ShipPartCrash(string quote) : base(quote)
        {
        }

        protected virtual int CountToSpawn
        {
            get
            {
                return 1;
            }
        }

        protected override bool CanFireNowSub(IncidentParms parms)
        {
            var map = (Map)parms.target;
            if (map.listerThings.ThingsOfDef(def.shipPart).Count > 0)
            {
                return false;
            }
            return true;
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            var map = (Map)parms.target;
            int num = 0;
            int countToSpawn = this.CountToSpawn;
            var list = new List<TargetInfo>();
            var shrapnelDirection = Rand.Range(0f, 360f);
            for (int i = 0; i < countToSpawn; i++)
            {
                IntVec3 intVec = default(IntVec3);
                if (!CellFinderLoose.TryFindSkyfallerCell(ThingDefOf.CrashedShipPartIncoming, map, out intVec, 14, default(IntVec3), -1, false, true, true, true, true, false, null))
                {
                    break;
                }
                var building_CrashedShipPart = (Building_CrashedShipPart)ThingMaker.MakeThing(def.shipPart, null);
                building_CrashedShipPart.SetFaction(Faction.OfMechanoids, null);
                building_CrashedShipPart.GetComp<CompSpawnerMechanoidsOnDamaged>().pointsLeft = Mathf.Max(parms.points * ShipPointsFactor, 300f);
                var skyfaller = SkyfallerMaker.MakeSkyfaller(ThingDefOf.CrashedShipPartIncoming, building_CrashedShipPart);
                skyfaller.shrapnelDirection = shrapnelDirection;
                GenSpawn.Spawn(skyfaller, intVec, map, WipeMode.Vanish);
                num++;
                list.Add(new TargetInfo(intVec, map, false));
            }
            if (num > 0)
            {
                SendStandardLetter(list, null);
            }
            return num > 0;
        }
    }
}