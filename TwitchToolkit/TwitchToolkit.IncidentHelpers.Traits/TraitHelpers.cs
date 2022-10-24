using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Traits;

public static class TraitHelpers
{
	private static readonly SimpleCurve AgeSkillMaxFactorCurve;

	private static readonly SimpleCurve LevelFinalAdjustmentCurve;

	private static readonly SimpleCurve LevelRandomCurve;

	public static int FinalLevelOfSkill(Pawn pawn, SkillDef sk)
	{
		float num = ((!sk.usuallyDefinedInBackstories) ? Rand.ByCurve(LevelRandomCurve) : ((float)Rand.RangeInclusive(0, 4)));
		foreach (BackstoryDef backstory in from bs in pawn.story.AllBackstories
			where bs != null
			select bs)
		{
			foreach (KeyValuePair<SkillDef, int> keyValuePair in backstory.skillGains)
			{
				if (keyValuePair.Key == sk)
				{
					num += (float)keyValuePair.Value * Rand.Range(1f, 1.4f);
				}
			}
		}
		for (int i = 0; i < pawn.story.traits.allTraits.Count; i++)
		{
			int num2 = 0;
			if (pawn.story.traits.allTraits[i].CurrentData.skillGains.TryGetValue(sk, out num2))
			{
				num += (float)num2;
			}
		}
		float num3 = Rand.Range(1f, AgeSkillMaxFactorCurve.Evaluate((float)pawn.ageTracker.AgeBiologicalYears));
		num *= num3;
		num = LevelFinalAdjustmentCurve.Evaluate(num);
		return Mathf.Clamp(Mathf.RoundToInt(num), 0, 20);
	}

	static TraitHelpers()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0005: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0010: Unknown result type (might be due to invalid IL or missing erences)
		//IL_001c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0027: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0033: Unknown result type (might be due to invalid IL or missing erences)
		//IL_003e: Unknown result type (might be due to invalid IL or missing erences)
		//IL_004a: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0055: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0066: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing erences)
		//IL_006b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0076: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0082: Unknown result type (might be due to invalid IL or missing erences)
		//IL_008d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0099: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00cc: Expected O, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing erences)
		//IL_010a: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0116: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0121: Unknown result type (might be due to invalid IL or missing erences)
		//IL_012d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0138: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0144: Unknown result type (might be due to invalid IL or missing erences)
		//IL_014f: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0160: Expected O, but got Unknown
		SimpleCurve val = new SimpleCurve();
		val.Add(new CurvePoint(0f, 0f), true);
		val.Add(new CurvePoint(10f, 0.7f), true);
		val.Add(new CurvePoint(35f, 1f), true);
		val.Add(new CurvePoint(60f, 1.6f), true);
		AgeSkillMaxFactorCurve = val;
		SimpleCurve val2 = new SimpleCurve();
		val2.Add(new CurvePoint(0f, 0f), true);
		val2.Add(new CurvePoint(10f, 10f), true);
		val2.Add(new CurvePoint(20f, 16f), true);
		val2.Add(new CurvePoint(27f, 20f), true);
		LevelFinalAdjustmentCurve = val2;
		SimpleCurve val3 = new SimpleCurve();
		val3.Add(new CurvePoint(0f, 0f), true);
		val3.Add(new CurvePoint(0.5f, 150f), true);
		val3.Add(new CurvePoint(4f, 150f), true);
		val3.Add(new CurvePoint(5f, 25f), true);
		val3.Add(new CurvePoint(10f, 5f), true);
		val3.Add(new CurvePoint(15f, 0f), true);
		LevelRandomCurve = val3;
	}
}
