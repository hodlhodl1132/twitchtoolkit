using System;
using UnityEngine;
using Verse;
using RimWorld;

namespace TwitchToolkit.Incidents
{
  public class IncidentWorker_ThrumboPasses : IncidentWorker
  {
    readonly string Quote;

    public IncidentWorker_ThrumboPasses(string quote)
    {
      Quote = quote;
    }

    protected override bool CanFireNowSub(IncidentParms parms)
    {
      Map map = (Map)parms.target;
      IntVec3 intVec;
      return !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout) && map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(ThingDefOf.Thrumbo) && this.TryFindEntryCell(map, out intVec);
    }

    protected override bool TryExecuteWorker(IncidentParms parms)
    {
      Map map = (Map)parms.target;
      IntVec3 intVec;
      if (!this.TryFindEntryCell(map, out intVec))
      {
        return false;
      }
      PawnKindDef thrumbo = PawnKindDefOf.Thrumbo;
      float num = StorytellerUtility.DefaultThreatPointsNow(map);
      int num2 = GenMath.RoundRandom(num / thrumbo.combatPower);
      int max = Rand.RangeInclusive(2, 4);
      num2 = Mathf.Clamp(num2, 1, max);
      int num3 = Rand.RangeInclusive(90000, 150000);
      IntVec3 invalid = IntVec3.Invalid;
      if (!RCellFinder.TryFindRandomCellOutsideColonyNearTheCenterOfTheMap(intVec, map, 10f, out invalid))
      {
        invalid = IntVec3.Invalid;
      }
      Pawn pawn = null;
      for (int i = 0; i < num2; i++)
      {
        IntVec3 loc = CellFinder.RandomClosewalkCellNear(intVec, map, 10, null);
        pawn = PawnGenerator.GeneratePawn(thrumbo, null);
        GenSpawn.Spawn(pawn, loc, map, Rot4.Random, WipeMode.Vanish, false);
        pawn.mindState.exitMapAfterTick = Find.TickManager.TicksGame + num3;
        if (invalid.IsValid)
        {
          pawn.mindState.forcedGotoPosition = CellFinder.RandomClosewalkCellNear(invalid, map, 10, null);
        }
      }

      var text = "LetterThrumboPasses".Translate(thrumbo.label);

      if(Quote != null)
      {
        text += "\n\n";
        text += Quote;
      }

      Find.LetterStack.ReceiveLetter("LetterLabelThrumboPasses".Translate(thrumbo.label).CapitalizeFirst(), text, LetterDefOf.PositiveEvent, pawn, null, null);
      return true;
    }

    private bool TryFindEntryCell(Map map, out IntVec3 cell)
    {
      return RCellFinder.TryFindRandomPawnEntryCell(out cell, map, CellFinder.EdgeRoadChance_Animal + 0.2f, false, null);
    }
  }
}