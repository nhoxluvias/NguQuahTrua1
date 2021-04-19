using System;
using System.Collections.Generic;
using System.Linq;

namespace FFMPEG
{
	public static class TimeExtensions
	{
		public static string ToFFmpeg(this TimeSpan ts)
		{
			int milliseconds = ts.Milliseconds;
			int seconds = ts.Seconds;
			int minutes = ts.Minutes;
			int num = (int)ts.TotalHours;
			return string.Format("{0:D}:{1:D2}:{2:D2}.{3:D3}", num, minutes, seconds, milliseconds);
		}

		public static TimeSpan ParseFFmpegTime(this string text)
		{
			List<string> list = text.Split(new char[1]
			{
			':'
			}).Reverse().ToList();
			int milliseconds = 0;
			int num = 0;
			if (Enumerable.Contains(list[0], '.'))
			{
				string[] array = list[0].Split(new char[1]
				{
				'.'
				});
				num = int.Parse(array[0]);
				milliseconds = int.Parse(array[1]);
			}
			else
			{
				num = int.Parse(list[0]);
			}
			int minutes = int.Parse(list[1]);
			int hours = int.Parse(list[2]);
			return new TimeSpan(0, hours, minutes, num, milliseconds);
		}
	}
}
