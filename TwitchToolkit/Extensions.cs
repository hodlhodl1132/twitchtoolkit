using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace TwitchToolkit
{
    public static class Extensions
    {
        public static class ThreadSafeRandom
        {
            [ThreadStatic] private static Random Local;

            public static Random ThisThreadsRandom
            {
                get { return Local ?? (Local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
            }
        }


        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        public static IEnumerable<T> Replace<T>(this IEnumerable<T> enumerable, int index, T value)
        {
            return enumerable.Select((x, i) => index == i ? value : x);
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
            seconds = seconds % 86400;
            int hours = seconds / 3600;
            seconds = seconds % 3600;
            int minutes = seconds / 60;
            seconds = seconds % 60;

            string formatted = string.Format("{0}{1}{2}{3}",
              days > 0 ? string.Format("{0:0} day{1}, ", days, days > 1 ? "s" : string.Empty) : string.Empty,
              hours > 0 ? string.Format("{0:0} hour{1}, ", hours, hours > 1 ? "s" : string.Empty) : string.Empty,
              minutes > 0 ? string.Format("{0:0} minute{1}, ", minutes, minutes > 1 ? "s" : string.Empty) : string.Empty,
              seconds > 0 ? string.Format("{0:0} second{1}", seconds, seconds > 1 ? "s" : string.Empty) : string.Empty);

            if (formatted.EndsWith(", ", StringComparison.InvariantCultureIgnoreCase)) formatted = formatted.Substring(0, formatted.Length - 2);

            if (string.IsNullOrEmpty(formatted)) formatted = "0 seconds";

            return formatted;
        }

        public static string ToReadableRimworldTimeString(this float ticks)
        {
            return ((int)ticks).ToReadableRimworldTimeString();
        }

        public static string ToReadableRimworldTimeString(this int ticks)
        {
            int years = ticks / 3600000;
            ticks = ticks % 3600000;
            int quadrums = ticks / 900000;
            ticks = ticks % 900000;
            int days = ticks / 60000;
            ticks = ticks % 60000;
            int hours = ticks / 2500;
            ticks = ticks % 2500;
            int minutes = ticks / 90;
            ticks = ticks % 90;

            string formatted = string.Format("{0}{1}{2}{3}{4}",
                years > 0 ? string.Format("{0:0} year{1}, ", years, years > 1 ? "s" : string.Empty) : string.Empty,
                quadrums > 0 ? string.Format("{0:0} quadrum{1}, ", quadrums, quadrums > 1 ? "s" : string.Empty) : string.Empty,
                days > 0 ? string.Format("{0:0} day{1}, ", days, days > 1 ? "s" : string.Empty) : string.Empty,
                hours > 0 ? string.Format("{0:0} hour{1}, ", hours, hours > 1 ? "s" : string.Empty) : string.Empty,
                minutes > 0 ? string.Format("{0:0} minute{1}, ", minutes, minutes > 1 ? "s" : string.Empty) : string.Empty);

            if (formatted.EndsWith(", ", StringComparison.InvariantCultureIgnoreCase)) formatted = formatted.Substring(0, formatted.Length - 2);

            if (string.IsNullOrEmpty(formatted)) formatted = "0 minutes";

            return formatted;
        }
    }
}
