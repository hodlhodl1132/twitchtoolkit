using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Utilities;

namespace TwitchToolkit.Votes
{
    public static class VoteHelper
    {
        public static bool TimeForEventVote()
        {
            return (
                        ToolkitSettings.TimedStorytelling && 
                        TimeHelper.MinutesElapsed(Ticker.lastEvent) >= ToolkitSettings.TimeBetweenStorytellingEvents
                    );
        }
    }
}
