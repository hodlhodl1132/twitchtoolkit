using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitchToolkit.Utilities
{
    public class TimeHelper
    {
        public static int SecondsElapsed(DateTime startTime)
        {
            TimeSpan span = DateTime.Now - startTime;
            return span.Seconds + (((span.Hours * 60) + span.Minutes) * 60);
        }
    }
}
