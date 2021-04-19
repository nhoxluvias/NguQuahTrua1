using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFMPEG.Streams.VideoStream
{
	public class VideoStream : IVideoStream, IStream, IFilterable
	{
		private readonly List<string> _parameters = new List<string>();

		private readonly Dictionary<string, string> _videoFilters = new Dictionary<string, string>();

		private string _watermarkSource;

		private string _bitrate;

		private string _bitsreamFilter;

		private string _frameCount;

		private string _framerate;

		private string _loop;

		private string _reverse;

		private string _rotate;

		private string _seek;

		private string _size;

		private string _split;

		private string _flags;

		private string _codec;

		public int Width
		{
			get;
			internal set;
		}

		public int Height
		{
			get;
			internal set;
		}

		public double Framerate
		{
			get;
			internal set;
		}

		public string Ratio
		{
			get;
			internal set;
		}

		public TimeSpan Duration
		{
			get;
			internal set;
		}

		public string Codec
		{
			get;
			internal set;
		}

		public int Index
		{
			get;
			internal set;
		}

		public string Path
		{
			get;
			internal set;
		}

		public int? Default
		{
			get;
			internal set;
		}

		public int? Forced
		{
			get;
			internal set;
		}

		public string PixelFormat
		{
			get;
			internal set;
		}

		public int? Rotation
		{
			get;
			internal set;
		}

		public StreamType StreamType
		{
			get
			{
				return StreamType.Video;
			}
		}

		public long Bitrate
		{
			get;
			internal set;
		}

		internal VideoStream()
		{
		}

		public IEnumerable<IFilterConfiguration> GetFilters()
		{
			if (_videoFilters.Any())
			{
				yield return new FilterConfiguration
				{
					FilterType = "-filter_complex",
					StreamNumber = Index,
					Filters = _videoFilters
				};
			}
		}

		public string Build()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Join(" ", _parameters));
			stringBuilder.Append(BuildVideoCodec());
			stringBuilder.Append(_bitsreamFilter);
			stringBuilder.Append(_bitrate);
			stringBuilder.Append(_framerate);
			stringBuilder.Append(_frameCount);
			stringBuilder.Append(_loop);
			stringBuilder.Append(_split);
			stringBuilder.Append(_reverse);
			stringBuilder.Append(_rotate);
			stringBuilder.Append(_size);
			stringBuilder.Append(_flags);
			return stringBuilder.ToString();
		}

		public string BuildInputArguments()
		{
			return _seek;
		}

		public string BuildVideoCodec()
		{
			if (_codec != null)
			{
				return "-c:v " + _codec.ToString() + " ";
			}
			return string.Empty;
		}

		public IVideoStream ChangeSpeed(double multiplication)
		{
			_videoFilters["setpts"] = GetVideoSpeedFilter(multiplication);
			return this;
		}

		private string GetVideoSpeedFilter(double multiplication)
		{
			if (multiplication < 0.5 || multiplication > 2.0)
			{
				throw new ArgumentOutOfRangeException("multiplication", "Value has to be greater than 0.5 and less than 2.0.");
			}
			double num = 1.0;
			num = ((!(multiplication >= 1.0)) ? (1.0 + (multiplication - 1.0) * -2.0) : (1.0 - (multiplication - 1.0) / 2.0));
			return num.ToFFmpegFormat() + "*PTS ";
		}

		public IVideoStream Rotate(RotateDegrees rotateDegrees)
		{
			_rotate = ((rotateDegrees == RotateDegrees.Invert) ? "-vf \"transpose=2,transpose=2\" " : string.Format("-vf \"transpose={0}\" ", (int)rotateDegrees));
			return this;
		}

		public IVideoStream CopyStream()
		{
			return SetCodec(VideoCodec.copy);
		}

		public IVideoStream SetLoop(int count, int delay)
		{
			_loop = string.Format("-loop {0} ", count);
			if (delay > 0)
			{
				_loop += string.Format("-final_delay {0} ", delay / 100);
			}
			return this;
		}

		public IVideoStream AddSubtitles(string subtitlePath, VideoSize originalSize, string encode, string style)
		{
			return BuildSubtitleFilter(subtitlePath, originalSize, encode, style);
		}

		public IVideoStream AddSubtitles(string subtitlePath, string encode, string style)
		{
			return BuildSubtitleFilter(subtitlePath, null, encode, style);
		}

		private IVideoStream BuildSubtitleFilter(string subtitlePath, VideoSize? originalSize, string encode, string style)
		{
			string text = ("'" + subtitlePath + "'").Replace("\\", "\\\\").Replace(":", "\\:");
			if (!string.IsNullOrEmpty(encode))
			{
				text = text + ":charenc=" + encode;
			}
			if (!string.IsNullOrEmpty(style))
			{
				text = text + ":force_style='" + style + "'";
			}
			if (originalSize.HasValue)
			{
				text = text + ":original_size=" + originalSize.Value.ToFFmpegFormat();
			}
			_videoFilters.Add("subtitles", text);
			return this;
		}

		public IVideoStream Reverse()
		{
			_reverse = "-vf reverse ";
			return this;
		}

		public IVideoStream SetBitrate(long bitrate)
		{
			_bitrate = string.Format("-b:v {0} ", bitrate);
			return this;
		}

		public IVideoStream SetBitrate(long minBitrate, long maxBitrate, long bufferSize)
		{
			_bitrate = string.Format("-b:v {0} -maxrate {1} -bufsize {2} ", minBitrate, maxBitrate, bufferSize);
			return this;
		}

		public IVideoStream SetFlags(params Flag[] flags)
		{
			return SetFlags(flags.Select((Flag x) => x.ToString()).ToArray());
		}

		public IVideoStream SetFlags(params string[] flags)
		{
			string text = string.Join("+", flags);
			if (text[0] != '+')
			{
				text = "+" + text;
			}
			_flags = "-flags " + text + " ";
			return this;
		}

		public IVideoStream SetFramerate(double framerate)
		{
			_framerate = "-r " + framerate.ToFFmpegFormat(3) + " ";
			return this;
		}

		public IVideoStream SetSize(VideoSize size)
		{
			_size = "-s " + size.ToFFmpegFormat() + " ";
			return this;
		}

		public IVideoStream SetSize(int width, int height)
		{
			_size = string.Format("-s {0}x{1} ", width, height);
			return this;
		}

		public IVideoStream SetCodec(VideoCodec codec)
		{
			string text = codec.ToString();
			switch (codec)
			{
				case VideoCodec._8bps:
					text = "8bps";
					break;
				case VideoCodec._4xm:
					text = "4xm";
					break;
				case VideoCodec._012v:
					text = "012v";
					break;
			}
			return SetCodec(text ?? "");
		}

		public IVideoStream SetCodec(string codec)
		{
			_codec = codec;
			return this;
		}

		public IVideoStream SetBitstreamFilter(BitstreamFilter filter)
		{
			return SetBitstreamFilter(string.Format("{0}", filter));
		}

		public IVideoStream SetBitstreamFilter(string filter)
		{
			_bitsreamFilter = "-bsf:v " + filter + " ";
			return this;
		}

		public IVideoStream SetSeek(TimeSpan seek)
		{
			if (seek > Duration)
			{
				throw new ArgumentException("Seek can not be greater than video duration. Seek: " + seek.TotalSeconds + " Duration: " + Duration.TotalSeconds);
			}
			_seek = string.Format("-ss {0} ", seek);
			return this;
		}

		public IVideoStream SetOutputFramesCount(int number)
		{
			_frameCount = string.Format("-frames:v {0} ", number);
			return this;
		}

		public IVideoStream SetWatermark(string imagePath, Position position)
		{
			_watermarkSource = imagePath;
			string text = string.Empty;
			switch (position)
			{
				case Position.Bottom:
					text += "(main_w-overlay_w)/2:main_h-overlay_h";
					break;
				case Position.Center:
					text += "x=(main_w-overlay_w)/2:y=(main_h-overlay_h)/2";
					break;
				case Position.BottomLeft:
					text += "5:main_h-overlay_h";
					break;
				case Position.UpperLeft:
					text += "5:5";
					break;
				case Position.BottomRight:
					text += "(main_w-overlay_w):main_h-overlay_h";
					break;
				case Position.UpperRight:
					text += "(main_w-overlay_w):5";
					break;
				case Position.Left:
					text += "5:(main_h-overlay_h)/2";
					break;
				case Position.Right:
					text += "(main_w-overlay_w-5):(main_h-overlay_h)/2";
					break;
				case Position.Up:
					text += "(main_w-overlay_w)/2:5";
					break;
			}
			_videoFilters["overlay"] = text;
			return this;
		}

		public IVideoStream Split(TimeSpan startTime, TimeSpan duration)
		{
			_split = "-ss " + startTime.ToFFmpeg() + " -t " + duration.ToFFmpeg() + " ";
			return this;
		}

		public IEnumerable<string> GetSource()
		{
			if (string.IsNullOrWhiteSpace(_watermarkSource))
			{
				return new string[1]
				{
				Path
				};
			}
			return new string[2]
			{
			Path,
			_watermarkSource
			};
		}
	}
}
