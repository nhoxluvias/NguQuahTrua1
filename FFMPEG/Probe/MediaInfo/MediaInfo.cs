using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFMPEG.Probe.MediaInfo
{
	public class MediaInfo : IMediaInfo
	{
		public IEnumerable<IStream> Streams
		{
			get
			{
				return ((IEnumerable<IStream>)VideoStreams).Concat((IEnumerable<IStream>)AudioStreams).Concat(SubtitleStreams);
			}
		}

		public TimeSpan Duration
		{
			get;
			internal set;
		}

		public long Size
		{
			get;
			internal set;
		}

		public IEnumerable<IVideoStream> VideoStreams
		{
			get;
			internal set;
		}

		public IEnumerable<IAudioStream> AudioStreams
		{
			get;
			internal set;
		}

		public IEnumerable<ISubtitleStream> SubtitleStreams
		{
			get;
			internal set;
		}

		public string Path
		{
			get;
		}

		private MediaInfo(string path)
		{
			Path = path;
		}

		[Obsolete("This will be deleted in next major version. Please use FFmpeg.GetMediaInfo instead of that.")]
		public static async Task<IMediaInfo> Get(string filePath)
		{
			CancellationToken token = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			return await Get(filePath, token);
		}

		[Obsolete("This will be deleted in next major version. Please use FFmpeg.GetMediaInfo instead of that.")]
		public static async Task<IMediaInfo> Get(string filePath, CancellationToken cancellationToken)
		{
			MediaInfo mediaInfo = new MediaInfo(filePath);
			return await new FFprobeWrapper().SetProperties(mediaInfo, cancellationToken);
		}

		[Obsolete("This will be deleted in next major version. Please use FFmpeg.GetMediaInfo instead of that.")]
		public static async Task<IMediaInfo> Get(FileInfo fileInfo)
		{
			if (!File.Exists(fileInfo.FullName))
			{
				throw new InvalidInputException("Input file " + fileInfo.FullName + " doesn't exists.");
			}
			return await Get(fileInfo.FullName);
		}
	}
}
