using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFMPEG.Streams.AudioStream
{
	public class AudioStream : IAudioStream, IStream, IFilterable
	{
		private readonly Dictionary<string, string> _audioFilters = new Dictionary<string, string>();

		private string _bitsreamFilter;

		private string _reverse;

		private string _seek;

		private string _split;

		private string _sampleRate;

		private string _channels;

		private string _bitrate;

		private string _codec;

		public StreamType StreamType
		{
			get
			{
				return StreamType.Audio;
			}
		}

		public int Index
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

		public long Bitrate
		{
			get;
			internal set;
		}

		public int Channels
		{
			get;
			internal set;
		}

		public int SampleRate
		{
			get;
			internal set;
		}

		public string Language
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

		public string Path
		{
			get;
			set;
		}

		internal AudioStream()
		{
		}

		public IAudioStream Reverse()
		{
			_reverse = "-af areverse ";
			return this;
		}

		public string Build()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(BuildAudioCodec());
			stringBuilder.Append(_bitsreamFilter);
			stringBuilder.Append(_sampleRate);
			stringBuilder.Append(_channels);
			stringBuilder.Append(_bitrate);
			stringBuilder.Append(_reverse);
			stringBuilder.Append(_split);
			return stringBuilder.ToString();
		}

		public string BuildInputArguments()
		{
			return _seek;
		}

		public string BuildAudioCodec()
		{
			if (_codec != null)
			{
				return "-c:a " + _codec.ToString() + " ";
			}
			return string.Empty;
		}

		public IAudioStream Split(TimeSpan startTime, TimeSpan duration)
		{
			_split = "-ss " + startTime.ToFFmpeg() + " -t " + duration.ToFFmpeg() + " ";
			return this;
		}

		public IAudioStream CopyStream()
		{
			return SetCodec(AudioCodec.copy);
		}

		public IAudioStream SetChannels(int channels)
		{
			_channels = string.Format("-ac:{0} {1} ", Index, channels);
			return this;
		}

		public IAudioStream SetBitstreamFilter(BitstreamFilter filter)
		{
			return SetBitstreamFilter(string.Format("{0}", filter));
		}

		public IAudioStream SetBitstreamFilter(string filter)
		{
			_bitsreamFilter = "-bsf:a " + filter + " ";
			return this;
		}

		public IAudioStream SetBitrate(long bitRate)
		{
			_bitrate = string.Format("-b:a:{0} {1} ", Index, bitRate);
			return this;
		}

		public IAudioStream SetBitrate(long minBitrate, long maxBitrate, long bufferSize)
		{
			_bitrate = string.Format("-b:a:{0} {1} -maxrate {2} -bufsize {3} ", Index, minBitrate, maxBitrate, bufferSize);
			return this;
		}

		public IAudioStream SetSampleRate(int sampleRate)
		{
			_sampleRate = string.Format("-ar:{0} {1} ", Index, sampleRate);
			return this;
		}

		public IAudioStream ChangeSpeed(double multiplication)
		{
			_audioFilters["atempo"] = GetAudioSpeed(multiplication) ?? "";
			return this;
		}

		private string GetAudioSpeed(double multiplication)
		{
			if (multiplication < 0.5 || multiplication > 2.0)
			{
				throw new ArgumentOutOfRangeException("multiplication", "Value has to be greater than 0.5 and less than 2.0.");
			}
			return multiplication.ToFFmpegFormat() + " ";
		}

		public IAudioStream SetCodec(AudioCodec codec)
		{
			string text = codec.ToString();
			switch (codec)
			{
				case AudioCodec._4gv:
					text = "4gv";
					break;
				case AudioCodec._8svx_exp:
					text = "8svx_exp";
					break;
				case AudioCodec._8svx_fib:
					text = "8svx_fib";
					break;
			}
			return SetCodec(text ?? "");
		}

		public IAudioStream SetCodec(string codec)
		{
			_codec = codec;
			return this;
		}

		public IEnumerable<string> GetSource()
		{
			return new string[1]
			{
			Path
			};
		}

		public IAudioStream SetSeek(TimeSpan? seek)
		{
			if (seek.HasValue)
			{
				_seek = "-ss " + seek.Value.ToFFmpeg() + " ";
			}
			return this;
		}

		public IEnumerable<IFilterConfiguration> GetFilters()
		{
			if (_audioFilters.Any())
			{
				yield return new FilterConfiguration
				{
					FilterType = "-filter:a",
					StreamNumber = Index,
					Filters = _audioFilters
				};
			}
		}
	}
}
