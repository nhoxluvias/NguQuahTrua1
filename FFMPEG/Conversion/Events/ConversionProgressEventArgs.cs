using System;

namespace FFMPEG.Conversion.Events
{
	public class ConversionProgressEventArgs : EventArgs
	{
		public TimeSpan Duration
		{
			get;
		}

		public TimeSpan TotalLength
		{
			get;
		}

		public long ProcessId
		{
			get;
		}

		public int Percent
		{
			get
			{
				return (int)(Math.Round(Duration.TotalSeconds / TotalLength.TotalSeconds, 2) * 100.0);
			}
		}

		public ConversionProgressEventArgs(TimeSpan timeSpan, TimeSpan totalTime, int processId)
		{
			Duration = timeSpan;
			TotalLength = totalTime;
			ProcessId = processId;
		}
	}
}
