using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFMPEG.Probe.MediaInfo
{
	public interface IMediaInfo
	{
		IEnumerable<IStream> Streams
		{
			get;
		}

		string Path
		{
			get;
		}

		TimeSpan Duration
		{
			get;
		}

		long Size
		{
			get;
		}

		IEnumerable<IVideoStream> VideoStreams
		{
			get;
		}

		IEnumerable<IAudioStream> AudioStreams
		{
			get;
		}

		IEnumerable<ISubtitleStream> SubtitleStreams
		{
			get;
		}
	}
}
