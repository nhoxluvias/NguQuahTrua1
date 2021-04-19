using System.Collections.Generic;

namespace FFMPEG.Streams
{
	public interface IFilterConfiguration
	{
		string FilterType
		{
			get;
		}

		int StreamNumber
		{
			get;
		}

		Dictionary<string, string> Filters
		{
			get;
		}
	}
}
