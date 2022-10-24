using System;
using RimWorld;
using TwitchToolkit.Incidents;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers;

public static class IncidentHelper_PointsHelper
{
	public static PointsWagerTarget RollNewThreatPoints(float pointsWager, IIncidentTarget target = null)
	{
		if (target == null)
		{
			target = (IIncidentTarget)(object)Helper.AnyPlayerMap;
			if (target == null)
			{
				throw new ArgumentNullException();
			}
		}
		float threatPoints = StorytellerUtility.DefaultThreatPointsNow(target);
		float pointsRatio = Math.Min(pointsWager / threatPoints, 5f);
		float chanceAtHigherRaid = (float)(0.0 - 0.53217458724975586 * Math.Pow(pointsRatio, 2.0));
		chanceAtHigherRaid += 22.9181728f * pointsRatio;
		chanceAtHigherRaid -= 1.28649545f;
		float chanceAtSmallerRaid = Math.Min(100f, 100f - chanceAtHigherRaid);
		Helper.Log($"points wager: {pointsWager} threat points: {threatPoints} chanceBigRaid: {chanceAtHigherRaid} chanceSmallRaid: {chanceAtSmallerRaid}");
		float multiplier = 1f;
		if (chanceAtHigherRaid * 10f > (float)Rand.Range(1, 1000))
		{
			multiplier = (float)(0.00092764379223808646 * Math.Pow(pointsRatio, 2.0));
			multiplier += 0.0537105761f * pointsRatio;
			multiplier += 1.0046382f;
		}
		else if (chanceAtSmallerRaid * 10f > (float)Rand.Range(1, 1000))
		{
			multiplier = (float)(0.0038870000280439854 * Math.Pow(pointsRatio, 2.0));
			multiplier += 0.05977f * pointsRatio;
			multiplier += 0.594f;
		}
		return new PointsWagerTarget(threatPoints * multiplier, target);
	}

	public static float RollProportionalGamePoints(StoreIncidentVariables incident, float pointsWager, float gamePoints)
	{
		int cost = incident.cost;
		int totalPossible = incident.maxWager - incident.minPointsToFire;
		float coinsPerThreatPoint = (float)(totalPossible / 2) / gamePoints;
		float pointsPurchased = pointsWager / coinsPerThreatPoint;
		float rollToCoinRatio = totalPossible / 8;
		int totalRolls = (int)Math.Round(pointsWager / rollToCoinRatio);
		int highestRoll = 65;
		for (int i = 0; i < totalRolls; i++)
		{
			int roll = Rand.Range(65, 125);
			if (roll > highestRoll)
			{
				highestRoll = roll;
			}
		}
		float finalPoints = pointsPurchased * ((float)highestRoll / 100f);
		Helper.Log($"wager: {pointsWager} gamePoints: {gamePoints} pointsPurchased: {pointsPurchased} totalRolls: {totalRolls} highestRoll: {highestRoll} finalPoints: {finalPoints}");
		Store_Logger.LogString($"wager: {pointsWager} gamePoints: {gamePoints} pointsPurchased: {pointsPurchased} totalRolls: {totalRolls} highestRoll: {highestRoll} finalPoints: {finalPoints}");
		return Math.Max(finalPoints, 35f);
	}
}
