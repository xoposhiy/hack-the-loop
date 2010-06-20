using System;
using System.Diagnostics;

namespace WebClientNS
{
	public static class StopWatchExtensions
	{
		public static string FormatElapsedTime(this Stopwatch watch)
		{
			if (watch == null) throw new ArgumentNullException("watch");
			return string.Format(
				"{0}:{1}:{2}:{3}",
				watch.Elapsed.Hours.ToString("D2"),
				watch.Elapsed.Minutes.ToString("D2"),
				watch.Elapsed.Seconds.ToString("D2"),
				watch.Elapsed.Milliseconds.ToString("D3")
				);
		}
	}
}