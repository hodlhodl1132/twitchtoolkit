using System;

namespace TwitchToolkit;

public class Karma
{
	public static string GetKarmaStringFromInt(int karmaType)
	{
		return karmaType switch
		{
			0 => "Bad", 
			1 => "Good", 
			2 => "Neutral", 
			3 => "Doom", 
			_ => null, 
		};
	}

	public static KarmaType GetKarmaTypeFromInt(int karmaType)
	{
		return karmaType switch
		{
			0 => KarmaType.Bad, 
			1 => KarmaType.Good, 
			2 => KarmaType.Neutral, 
			3 => KarmaType.Doom, 
			_ => KarmaType.Good, 
		};
	}

	public static int CalculateNewKarma(int karma, KarmaType karmatype, int calculatedprice = 0)
	{
		float tier = (float)karma / (float)ToolkitSettings.KarmaCap;
		Helper.Log($"Calculating new karma with {karma}, and karma type {karmatype} for {calculatedprice} with curve {CalculateForCurve()} tier {tier}");
		double newkarma = 0.0;
		int maxkarma = 0;
		if (karmatype == KarmaType.Doom)
		{
			newkarma = (double)karma - Convert.ToDouble((double)calculatedprice / (double)ToolkitSettings.DoomBonus) * (double)(ToolkitSettings.KarmaCap / 100);
			if ((double)tier < 0.061)
			{
				maxkarma = 0;
			}
		}
		else if ((double)tier > 0.55)
		{
			switch (karmatype)
			{
			case KarmaType.Good:
				newkarma = (double)karma + Convert.ToDouble((double)calculatedprice / (double)ToolkitSettings.TierOneGoodBonus) * (double)CalculateForCurve();
				break;
			case KarmaType.Neutral:
				newkarma = (double)karma + Convert.ToDouble((double)calculatedprice / (double)ToolkitSettings.TierOneNeutralBonus) * (double)CalculateForCurve();
				break;
			case KarmaType.Bad:
				newkarma = (double)karma - Convert.ToDouble((double)calculatedprice / (double)ToolkitSettings.TierOneBadBonus) * (double)CalculateForCurve();
				break;
			}
		}
		else if ((double)tier > 0.36)
		{
			switch (karmatype)
			{
			case KarmaType.Good:
				newkarma = (double)karma + Convert.ToDouble((double)calculatedprice / (double)ToolkitSettings.TierTwoGoodBonus) * (double)CalculateForCurve();
				break;
			case KarmaType.Neutral:
				Helper.Log($"{(double)karma} + ( ({(double)calculatedprice} / {(double)ToolkitSettings.TierTwoNeutralBonus}) * ({CalculateForCurve()}))");
				newkarma = (double)karma + Convert.ToDouble((double)calculatedprice / (double)ToolkitSettings.TierTwoNeutralBonus) * (double)CalculateForCurve();
				break;
			case KarmaType.Bad:
				newkarma = (double)karma - Convert.ToDouble((double)calculatedprice / (double)ToolkitSettings.TierTwoBadBonus) * (double)CalculateForCurve();
				break;
			}
		}
		else if ((double)tier > 0.06)
		{
			switch (karmatype)
			{
			case KarmaType.Good:
				newkarma = (double)karma + Convert.ToDouble((double)calculatedprice / (double)ToolkitSettings.TierThreeGoodBonus) * (double)CalculateForCurve();
				break;
			case KarmaType.Neutral:
				newkarma = (double)karma + Convert.ToDouble((double)calculatedprice / (double)ToolkitSettings.TierThreeNeutralBonus) * (double)CalculateForCurve();
				break;
			case KarmaType.Bad:
				newkarma = (double)karma - Convert.ToDouble((double)calculatedprice / (double)ToolkitSettings.TierThreeBadBonus) * (double)CalculateForCurve();
				break;
			}
		}
		else
		{
			switch (karmatype)
			{
			case KarmaType.Good:
				newkarma = (double)karma + Convert.ToDouble((double)calculatedprice / (double)ToolkitSettings.TierFourGoodBonus) * (double)CalculateForCurve();
				break;
			case KarmaType.Neutral:
				newkarma = (double)karma + Convert.ToDouble((double)calculatedprice / (double)ToolkitSettings.TierFourNeutralBonus) * (double)CalculateForCurve();
				break;
			case KarmaType.Bad:
				newkarma = (double)karma - Convert.ToDouble((double)calculatedprice / (double)(((double)ToolkitSettings.TierFourBadBonus > 0.0) ? ToolkitSettings.TierFourBadBonus : 66)) * (double)CalculateForCurve();
				break;
			}
		}
		if (newkarma < 1.0 && ToolkitSettings.BanViewersWhoPurchaseAlwaysBad)
		{
			newkarma = 1.0;
		}
		if (newkarma < (double)ToolkitSettings.KarmaMinimum)
		{
			newkarma = ToolkitSettings.KarmaMinimum;
		}
		return (Convert.ToInt32(Math.Ceiling(newkarma)) > ToolkitSettings.KarmaCap) ? ToolkitSettings.KarmaCap : Convert.ToInt32(Math.Ceiling(newkarma));
	}

	private static float CalculateForCurve()
	{
		return 0.00116279069f * (float)ToolkitSettings.KarmaCap + 0.8372093f;
	}
}
