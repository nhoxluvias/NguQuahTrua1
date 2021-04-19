using System;

namespace FFMPEG.Conversion.Models
{
	public interface IConversionResult
	{
		DateTime StartTime
		{
			get;
		}

		DateTime EndTime
		{
			get;
		}

		TimeSpan Duration
		{
			get;
		}

		string Arguments
		{
			get;
		}
	}
}
