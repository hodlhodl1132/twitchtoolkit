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

        public static int CalculateNewKarma(int karma, KarmaType karmatype, int calculatedprice = 0)
        {
            float tier = ( (float)karma / ( (float)Settings.KarmaCap ) );
            Helper.Log($"Calculating new karma with {karma}, and karma type {karmatype} for {calculatedprice} with curve {CalculateForCurve()} tier {tier}");
            double newkarma = 0;
            int maxkarma = 0;

            

            if (karmatype == KarmaType.Doom)
            {
                
                newkarma = (double)karma - (Convert.ToDouble((double)calculatedprice / (double)Settings.DoomBonus) * (Settings.KarmaCap / 100) );
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
                            newkarma = (double)karma + (Convert.ToDouble((double)calculatedprice / (double)Settings.TierOneGoodBonus) * CalculateForCurve());
                            break;
                        //minute bonus for neutral
                        case KarmaType.Neutral:
                            newkarma = (double)karma + (Convert.ToDouble((double)calculatedprice / (double)Settings.TierOneNeutralBonus) * CalculateForCurve());
                            break;
                        //small punishment for bad
                        case KarmaType.Bad:
                            newkarma = (double)karma - (Convert.ToDouble((double)calculatedprice / (double)Settings.TierOneBadBonus) * CalculateForCurve());
                            break;
                    }

                }
                else if (tier > 0.36)
                {
                    switch(karmatype)
                    {
                        //medium bonus for good
                        case KarmaType.Good:
                            newkarma = (double)karma + (Convert.ToDouble((double)calculatedprice / (double)Settings.TierTwoGoodBonus) * CalculateForCurve());
                            break;

                        //minute bonus for neutral
                        case KarmaType.Neutral:
                            Helper.Log($"{(double)karma} + ( ({(double)calculatedprice} / {(double)Settings.TierTwoNeutralBonus}) * ({CalculateForCurve()}))");
                            newkarma = (double)karma + (Convert.ToDouble((double)calculatedprice / (double)Settings.TierTwoNeutralBonus) * CalculateForCurve());
                            break;

                        //medium punishment for bad
                        case KarmaType.Bad:
                            newkarma = (double)karma - (Convert.ToDouble((double)calculatedprice / (double)Settings.TierTwoBadBonus) * CalculateForCurve());
                            break;
                    }
                }
                else if (tier > 0.06)
                {
                    switch(karmatype)
                    {
                        //small bonus for good
                        case KarmaType.Good:
                            newkarma = (double)karma + (Convert.ToDouble((double)calculatedprice / (double)Settings.TierThreeGoodBonus) * CalculateForCurve());
                            break;
                        //small bonus for neutral
                        case KarmaType.Neutral:
                            newkarma = (double)karma + (Convert.ToDouble((double)calculatedprice / (double)Settings.TierThreeNeutralBonus) * CalculateForCurve());
                            break;
                        //big punishment for bad
                        case KarmaType.Bad:
                            newkarma = (double)karma - (Convert.ToDouble((double)calculatedprice / (double)Settings.TierThreeBadBonus) * CalculateForCurve());
                            break;
                    }
                }
                else
                {
                    switch(karmatype)
                    {
                        //medium bonus for good
                        case KarmaType.Good:
                            newkarma = (double)karma + (Convert.ToDouble((double)calculatedprice / (double)Settings.TierFourGoodBonus) * CalculateForCurve());
                            break;
                        //small bonus for neutral
                        case KarmaType.Neutral:
                            newkarma = (double)karma + (Convert.ToDouble((double)calculatedprice / (double)Settings.TierFourNeutralBonus) * CalculateForCurve());
                            break;
                        //banned for bad
                        case KarmaType.Bad:
                            newkarma = (double)karma - (Convert.ToDouble((double)calculatedprice / ((double)Settings.TierFourBadBonus > 0 ? Settings.TierFourBadBonus : 66)) * CalculateForCurve());
                            break;
                    }
                }
            }
            if (newkarma < 1 && !Settings.BanViewersWhoPurchaseAlwaysBad)
            {
                newkarma = 1;
            }

            maxkarma = Convert.ToInt32(Math.Round(newkarma)) > Settings.KarmaCap ? Settings.KarmaCap : Convert.ToInt32(Math.Round(newkarma));


            // if banning veiwers who always purchase bad is off, protect them from 0 karma

            return maxkarma;
        }

        private static float CalculateForCurve()
        {
            return ((1f/860f) * (float)Settings.KarmaCap) + (36f/43f);
        }
    }
}