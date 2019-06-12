using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Incidents;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers
{
    public static class IncidentHelper_PointsHelper
    {
        public static PointsWagerTarget RollNewThreatPoints(float pointsWager, IIncidentTarget target = null)
        {
            if (target == null)
            {
                target = Helper.AnyPlayerMap;
                if (target == null)
                {
                    throw new ArgumentNullException();
                }
            }

            // chance at getting a bigger raid

            float threatPoints = StorytellerUtility.DefaultThreatPointsNow(target);
            float pointsRatio = Math.Min(pointsWager / threatPoints, 5f);

            float chanceAtHigherRaid = (float)-( (5450f/10241f) * Math.Pow(pointsRatio, 2d) );
            chanceAtHigherRaid += (234705f/10241f) * pointsRatio;
            chanceAtHigherRaid -= 13175f/10241f;

            float chanceAtSmallerRaid = Math.Min(100, 100 - chanceAtHigherRaid);

            Helper.Log($"points wager: {pointsWager} threat points: {threatPoints} chanceBigRaid: {chanceAtHigherRaid} chanceSmallRaid: {chanceAtSmallerRaid}");

            float multiplier = 1;
            
            // try for big raid
            if (chanceAtHigherRaid * 10 > Verse.Rand.Range(1, 1000))
            {
                multiplier = (float)( (1f / 1078f) * Math.Pow(pointsRatio, 2d) );
                multiplier += (579f/10780f) * pointsRatio;
                multiplier += 1083f/1078f;
            }
            // try for small raid
            else if (chanceAtSmallerRaid * 10 > Verse.Rand.Range(1, 1000))
            {
                multiplier = (float)( (0.003887f) * Math.Pow(pointsRatio, 2d) );
                multiplier += (0.05977f * pointsRatio);
                multiplier += 0.594f;             
            }

            return new PointsWagerTarget(threatPoints * multiplier, target);
        }

        public static float RollProportionalGamePoints(StoreIncidentVariables incident, float pointsWager, float gamePoints)
        {
            int cost = incident.cost;
            int totalPossible = incident.maxWager - incident.minPointsToFire;
            
            float coinsPerThreatPoint = (totalPossible / 2) / gamePoints;
            float pointsPurchased = pointsWager / coinsPerThreatPoint;

            float rollToCoinRatio = totalPossible / 8;
            int totalRolls = (int) Math.Round(pointsWager / rollToCoinRatio);
        
            int highestRoll = 65;

            for (int i = 0; i < totalRolls; i++)
            {
                int roll = Verse.Rand.Range(65, 125);
                if (roll > highestRoll)
                {
                    highestRoll = roll;
                }
            }

            float finalPoints = pointsPurchased * ((float) highestRoll / 100f);

            Helper.Log($"wager: {pointsWager} gamePoints: {gamePoints} pointsPurchased: {pointsPurchased} totalRolls: {totalRolls} highestRoll: {highestRoll} finalPoints: {finalPoints}");
            Store_Logger.LogString($"wager: {pointsWager} gamePoints: {gamePoints} pointsPurchased: {pointsPurchased} totalRolls: {totalRolls} highestRoll: {highestRoll} finalPoints: {finalPoints}");

            return Math.Max(finalPoints, 35);
        }
    }

    public class PointsWagerTarget
    {
        public float points;
        public IIncidentTarget target;

        public PointsWagerTarget(float points, IIncidentTarget target)
        {
            this.points = points;
            this.target = target;
        }
    }
}
