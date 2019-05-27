using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitchToolkit
{
    [Flags]
    public enum KarmaType
    {
        Bad = 0,
        Good = 1,
        Neutral = 2,
        Doom = 3
    }
    public class Karma
    {
        public static string GetKarmaStringFromInt(int karmaType)
        {
            switch(karmaType)
            {
                case 0:
                    return "Bad";
                case 1:
                    return "Good";
                case 2:
                    return "Neutral";
                case 3:
                    return "Doom";
                default:
                    return null;
            }
        }

        public static KarmaType GetKarmaTypeFromInt(int karmaType)
        {
            switch(karmaType)
            {
                case 0:
                    return KarmaType.Bad;
                case 1:
                    return KarmaType.Good;
                case 2:
                    return KarmaType.Neutral;
                case 3:
                    return KarmaType.Doom;
                default:
                    return KarmaType.Good;
            }
        }

        public static int CalculateNewKarma(int karma, KarmaType karmatype, int calculatedprice = 0)
        {
            float tier = ( (float)karma / ( (float)ToolkitSettings.KarmaCap ) );
            Helper.Log($"Calculating new karma with {karma}, and karma type {karmatype} for {calculatedprice} with curve {CalculateForCurve()} tier {tier}");
            double newkarma = 0;
            int maxkarma = 0;

            

            if (karmatype == KarmaType.Doom)
            {
                
                newkarma = (double)karma - (Convert.ToDouble((double)calculatedprice / (double)ToolkitSettings.DoomBonus) * (ToolkitSettings.KarmaCap / 100) );
                //possibly ban?
                if (tier < 0.061)
                {
                    //ban viewer
                    maxkarma = 0;
                }
            }
            else
            {
                if (tier > 0.55)
                {
                    switch(karmatype)
                    {
                        //small bonus for good
                        case KarmaType.Good:
                            newkarma = (double)karma + (Convert.ToDouble((double)calculatedprice / (double)ToolkitSettings.TierOneGoodBonus) * CalculateForCurve());
                            break;
                        //minute bonus for neutral
                        case KarmaType.Neutral:
                            newkarma = (double)karma + (Convert.ToDouble((double)calculatedprice / (double)ToolkitSettings.TierOneNeutralBonus) * CalculateForCurve());
                            break;
                        //small punishment for bad
                        case KarmaType.Bad:
                            newkarma = (double)karma - (Convert.ToDouble((double)calculatedprice / (double)ToolkitSettings.TierOneBadBonus) * CalculateForCurve());
                            break;
                    }

                }
                else if (tier > 0.36)
                {
                    switch(karmatype)
                    {
                        //medium bonus for good
                        case KarmaType.Good:
                            newkarma = (double)karma + (Convert.ToDouble((double)calculatedprice / (double)ToolkitSettings.TierTwoGoodBonus) * CalculateForCurve());
                            break;

                        //minute bonus for neutral
                        case KarmaType.Neutral:
                            Helper.Log($"{(double)karma} + ( ({(double)calculatedprice} / {(double)ToolkitSettings.TierTwoNeutralBonus}) * ({CalculateForCurve()}))");
                            newkarma = (double)karma + (Convert.ToDouble((double)calculatedprice / (double)ToolkitSettings.TierTwoNeutralBonus) * CalculateForCurve());
                            break;

                        //medium punishment for bad
                        case KarmaType.Bad:
                            newkarma = (double)karma - (Convert.ToDouble((double)calculatedprice / (double)ToolkitSettings.TierTwoBadBonus) * CalculateForCurve());
                            break;
                    }
                }
                else if (tier > 0.06)
                {
                    switch(karmatype)
                    {
                        //small bonus for good
                        case KarmaType.Good:
                            newkarma = (double)karma + (Convert.ToDouble((double)calculatedprice / (double)ToolkitSettings.TierThreeGoodBonus) * CalculateForCurve());
                            break;
                        //small bonus for neutral
                        case KarmaType.Neutral:
                            newkarma = (double)karma + (Convert.ToDouble((double)calculatedprice / (double)ToolkitSettings.TierThreeNeutralBonus) * CalculateForCurve());
                            break;
                        //big punishment for bad
                        case KarmaType.Bad:
                            newkarma = (double)karma - (Convert.ToDouble((double)calculatedprice / (double)ToolkitSettings.TierThreeBadBonus) * CalculateForCurve());
                            break;
                    }
                }
                else
                {
                    switch(karmatype)
                    {
                        //medium bonus for good
                        case KarmaType.Good:
                            newkarma = (double)karma + (Convert.ToDouble((double)calculatedprice / (double)ToolkitSettings.TierFourGoodBonus) * CalculateForCurve());
                            break;
                        //small bonus for neutral
                        case KarmaType.Neutral:
                            newkarma = (double)karma + (Convert.ToDouble((double)calculatedprice / (double)ToolkitSettings.TierFourNeutralBonus) * CalculateForCurve());
                            break;
                        //banned for bad
                        case KarmaType.Bad:
                            newkarma = (double)karma - (Convert.ToDouble((double)calculatedprice / ((double)ToolkitSettings.TierFourBadBonus > 0 ? ToolkitSettings.TierFourBadBonus : 66)) * CalculateForCurve());
                            break;
                    }
                }
            }

            if (newkarma < 1 && ToolkitSettings.BanViewersWhoPurchaseAlwaysBad)
            {
                newkarma = 1;
            }
            
            if (newkarma < ToolkitSettings.KarmaMinimum)
            {
                newkarma = ToolkitSettings.KarmaMinimum;
            }

            maxkarma = Convert.ToInt32(Math.Round(newkarma)) > ToolkitSettings.KarmaCap ? ToolkitSettings.KarmaCap : Convert.ToInt32(Math.Round(newkarma));


            // if banning veiwers who always purchase bad is off, protect them from 0 karma

            return maxkarma;
        }

        private static float CalculateForCurve()
        {
            return ((1f/860f) * (float)ToolkitSettings.KarmaCap) + (36f/43f);
        }
    }
}