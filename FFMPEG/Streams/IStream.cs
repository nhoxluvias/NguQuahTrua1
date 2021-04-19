using System.Collections.Generic;

namespace FFMPEG.Streams
{
	public interface IStream
	{
		string Path
		{
			get;
		}

		int Index
		{
			get;
		}

		string Codec
		{
			get;
		}

		StreamType StreamType
		{
			get;
		}

		string Build();

		string BuildInputArguments();

		IEnumerable<string> GetSource();
	}
}
