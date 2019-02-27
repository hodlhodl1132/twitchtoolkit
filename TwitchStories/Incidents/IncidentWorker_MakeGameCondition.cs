using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;

namespace TwitchToolkit.Incidents
{
  public class IncidentWorker_MakeGameCondition : IncidentWorker_Quote
  {
    readonly int Ticks;

    public IncidentWorker_MakeGameCondition(string quote, int ticks) : base(quote) 
    {
      Ticks = ticks;
    }

    protected override bool CanFireNowSub(IncidentParms parms)
    {
      GameConditionManager gameConditionManager = parms.target.GameConditionManager;
      if (gameConditionManager == null)
      {
        Log.ErrorOnce(string.Format("Couldn't find condition manager for incident target {0}", parms.target), 70849667, false);
        return false;
      }
      if (gameConditionManager.ConditionIsActive(this.def.gameCondition))
      {
        return false;
      }
      List<GameCondition> activeConditions = gameConditionManager.ActiveConditions;
      for (int i = 0; i < activeConditions.Count; i++)
      {
        if (!this.def.gameCondition.CanCoexistWith(activeConditions[i].def))
        {
          return false;
        }
      }
      return true;
    }

    protected override bool TryExecuteWorker(IncidentParms parms)
    {
      GameConditionManager gameConditionManager = parms.target.GameConditionManager;
      GameCondition cond = GameConditionMaker.MakeCondition(this.def.gameCondition, Ticks, 0);
      gameConditionManager.RegisterCondition(cond);
      base.SendStandardLetter();
      return true;
    }
  }
}
