using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Verse;

namespace TwitchToolkit;

public static class Extensions
{
	public static class ThreadSafeRandom
	{
		[ThreadStatic]
		private static Random Local;

		public static Random ThisThreadsRandom => Local ?? (Local = new Random(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId));
	}

	public interface IWeighted
	{
		int Weight { get; set; }
	}

	public static void Shuffle<T>(this IList<T> list)
	{
		int j = list.Count;
		while (j > 1)
		{
			j--;
			int i = ThreadSafeRandom.ThisThreadsRandom.Next(j + 1);
			T value = list[i];
			list[i] = list[j];
			list[j] = value;
		}
	}

	public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
	{
		return listToClone.Select((T item) => (T)item.Clone()).ToList();
	}

	public static IEnumerable<T> Replace<T>(this IEnumerable<T> enumerable, int index, T value)
	{
		return enumerable.Select((T x, int i) => (index == i) ? value : x);
	}

	public static T RandomElement<T>(this IEnumerable<T> enumerable, Random rand)
	{
		int index = rand.Next(0, enumerable.Count());
		return enumerable.ElementAt(index);
	}

	public static string ToReadableTimeString(this float seconds)
	{
		return ((int)seconds).ToReadableTimeString();
	}

	public static string ToReadableTimeString(this int seconds)
	{
		int days = seconds / 86400;
		seconds %= 86400;
		int hours = seconds / 3600;
		seconds %= 3600;
		int minutes = seconds / 60;
		seconds %= 60;
		string formatted = string.Format("{0}{1}{2}{3}", (days > 0) ? string.Format("{0:0} day{1}, ", days, (days > 1) ? "s" : string.Empty) : string.Empty, (hours > 0) ? string.Format("{0:0} hour{1}, ", hours, (hours > 1) ? "s" : string.Empty) : string.Empty, (minutes > 0) ? string.Format("{0:0} minute{1}, ", minutes, (minutes > 1) ? "s" : string.Empty) : string.Empty, (seconds > 0) ? string.Format("{0:0} second{1}", seconds, (seconds > 1) ? "s" : string.Empty) : string.Empty);
		if (formatted.EndsWith(", ", StringComparison.InvariantCultureIgnoreCase))
		{
			formatted = formatted.Substring(0, formatted.Length - 2);
		}
		if (string.IsNullOrEmpty(formatted))
		{
			formatted = "0 seconds";
		}
		return formatted;
	}

	public static string ToReadableRimworldTimeString(this float ticks)
	{
		return ((int)ticks).ToReadableRimworldTimeString();
	}

	public static string ToReadableRimworldTimeString(this int ticks)
	{
		int years = ticks / 3600000;
		ticks %= 3600000;
		int quadrums = ticks / 900000;
		ticks %= 900000;
		int days = ticks / 60000;
		ticks %= 60000;
		int hours = ticks / 2500;
		ticks %= 2500;
		int minutes = ticks / 90;
		ticks %= 90;
		string formatted = string.Format("{0}{1}{2}{3}{4}", (years > 0) ? string.Format("{0:0} year{1}, ", years, (years > 1) ? "s" : string.Empty) : string.Empty, (quadrums > 0) ? string.Format("{0:0} quadrum{1}, ", quadrums, (quadrums > 1) ? "s" : string.Empty) : string.Empty, (days > 0) ? string.Format("{0:0} day{1}, ", days, (days > 1) ? "s" : string.Empty) : string.Empty, (hours > 0) ? string.Format("{0:0} hour{1}, ", hours, (hours > 1) ? "s" : string.Empty) : string.Empty, (minutes > 0) ? string.Format("{0:0} minute{1}, ", minutes, (minutes > 1) ? "s" : string.Empty) : string.Empty);
		if (formatted.EndsWith(", ", StringComparison.InvariantCultureIgnoreCase))
		{
			formatted = formatted.Substring(0, formatted.Length - 2);
		}
		if (string.IsNullOrEmpty(formatted))
		{
			formatted = "0 minutes";
		}
		return formatted;
	}

	public static string Truncate(this string value, int maxLength, bool dots = false)
	{
		if (string.IsNullOrEmpty(value))
		{
			return value;
		}
		return (value.Length <= maxLength) ? value : (value.Substring(0, maxLength) + (dots ? "..." : ""));
	}

	public static bool TryChooseRandomElementByWeight<T>(this IEnumerable<T> source, Func<T, float> weightSelector, out T result)
	{
		IList<T> list = source.ToList();
		if (list == null || list.Count() == 0)
		{
			Helper.Log("list is null");
			result = default(T);
			return false;
		}
		float totalWeight = 0f;
		for (int j = 0; j < list.Count(); j++)
		{
			float weight = weightSelector(list[j]);
			if (weight < 0f)
			{
				Log.Error("Negative weight in selector: " + weight + " from " + list[j]);
				weight = 0f;
			}
			totalWeight += weight;
		}
		float choice = Rand.Range(0f, totalWeight);
		float sum = 0f;
		int iterator = 0;
		foreach (T obj in list)
		{
			float weight2 = weightSelector(list[iterator]);
			for (int i = (int)sum; (float)i < weight2 + sum; i++)
			{
				if ((float)i >= choice)
				{
					result = obj;
					return true;
				}
			}
			iterator++;
			sum += weight2;
		}
		result = list.ElementAt(0);
		return true;
	}
}
