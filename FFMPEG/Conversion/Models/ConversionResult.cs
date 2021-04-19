using System;

namespace FFMPEG.Conversion.Models
{
	internal class ConversionResult : IConversionResult
	{
		public DateTime StartTime
		{
			get;
			internal set;
		}

		public DateTime EndTime
		{
			get;
			internal set;
		}

		public TimeSpan Duration
		{
			get
			{
				return EndTime - StartTime;
			}
		}

		public string Arguments
		{
			get;
			internal set;
		}
	}
}
