using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitchToolkit
{
    public class Karma
    {

        public static int CalculateNewKarma(int karma, int karmatype, int calculatedprice = 0)
        {
            Helper.Log($"Calculating new karma with {karma}, and karma type {karmatype}");
            double newkarma = 0;

            if (calculatedprice > 0)
            {
                newkarma = (double)karma + (calculatedprice / 25);
                return Convert.ToInt32(Math.Round(newkarma));
            }


            switch (karmatype)
            {
                //bad event
                case 0:

                    if (karma >= 100)
                    {
                        newkarma = (double)karma * 0.5d;
                    }
                    else if (karma >= 80 && karma < 100)
                    {
                        newkarma = (double)karma * 0.3d;
                    }
                    else if (karma >= 60 && karma < 80)
                    {
                        newkarma = (double)karma * 0.35d;
                    }
                    else if (karma >= 40 && karma < 60)
                    {
                        newkarma = (double)karma * 0.4d;
                    }
                    else if (karma >= 20 && karma < 40)
                    {
                        newkarma = (double)karma * 0.45d;
                    }
                    else
                    {
                        newkarma = (double)karma * 0.5d;
                    }

                    break;
                //good event
                case 1:

                    if (karma >= 100)
                    {
                        newkarma = (double)karma * 1.05d;
                    }
                    else if (karma >= 82 && karma < 100)
                    {
                        // hard stop at 100 karma.
                        newkarma = karma * 1.18;
                        if (karma > 100)
                        {
                            newkarma = 100;
                        }
                    }
                    else if (karma >= 67 && karma < 82)
                    {
                        newkarma = (double)karma * 1.22d;
                    }
                    else if (karma >= 50 && karma < 67)
                    {
                        newkarma = (double)karma * 1.34d;
                    }
                    else if (karma >= 6 && karma < 50)
                    {
                        newkarma = (double)karma * 1.38d;
                    }
                    else
                    {
                        newkarma = (double)karma * 1.45d;
                    }

                    break;
                //neutral event
                case 2:

                    newkarma = karma * 1.02;

                    break;
                //doom event
                case 3:

                    newkarma = karma - 80;

                    break;
                default:

                    break;
            }
            Helper.Log($"New Karma is {Convert.ToInt32(Math.Round(newkarma))}");

            return Convert.ToInt32(Math.Round(newkarma));
        }
    }
}