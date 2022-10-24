using System;

namespace TwitchToolkit.Utilities;

public class TimeHelper
{
	public static int SecondsElapsed(DateTime startTime)
	{
		TimeSpan span = DateTime.Now - startTime;
		return span.Seconds + (span.Hours * 60 + span.Minutes) * 60;
	}

	public static int MinutesElapsed(DateTime startTime)
	{
		TimeSpan span = DateTime.Now - startTime;
		return span.Hours * 60 + span.Minutes;
	}
}
