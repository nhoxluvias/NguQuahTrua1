using System.Collections.Generic;

namespace FFMPEG.Streams
{
	internal class FilterConfiguration : IFilterConfiguration
	{
		public string FilterType
		{
			get;
			set;
		}

		public int StreamNumber
		{
			get;
			set;
		}

		public Dictionary<string, string> Filters
		{
			get;
			set;
		} = new Dictionary<string, string>();

	}
}
