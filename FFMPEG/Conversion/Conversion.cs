using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FFMPEG;
using FFMPEG.Conversion.Enums;
using FFMPEG.Conversion.Events;
using FFMPEG.Conversion.Models;
using FFMPEG.Probe.MediaInfo;
using FFMPEG.Streams;
using FFMPEG.Streams.AudioStream;
using FFMPEG.Streams.VideoStream;

namespace FFMPEG.Conversion
{
	public class Conversion : IConversion
	{
		private readonly object _builderLock = new object();

		private readonly Dictionary<string, int> _inputFileMap = new Dictionary<string, int>();

		private readonly IList<ConversionParameter> _parameters = new List<ConversionParameter>();

		private readonly List<IStream> _streams = new List<IStream>();

		private IEnumerable<FieldInfo> _fields;

		private string _output;

		private string _preset;

		private string _hashFormat;

		private string _hardwareAcceleration;

		private bool _overwriteOutput;

		private string _shortestInput;

		private string _seek;

		private bool _useMultiThreads = true;

		private bool _capturing;

		private bool _hasInputBuilder;

		private int? _threadsCount;

		private string _inputTime;

		private string _outputTime;

		private string _outputFormat;

		private string _inputFormat;

		private string _outputPixelFormat;

		private string _framerate;

		private string _inputFramerate;

		private string _vsyncMode;

		private ProcessPriorityClass? _priority;

		private FFmpegWrapper _ffmpeg;

		private Func<string, string> _buildInputFileName;

		private Func<string, string> _buildOutputFileName;

		private int? _processId;

		public int? FFmpegProcessId
		{
			get
			{
				return _processId;
			}
		}

		public string OutputFilePath
		{
			get;
			private set;
		}

		public event ConversionProgressEventHandler OnProgress;

		public event DataReceivedEventHandler OnDataReceived;

		public string Build()
		{
			lock (_builderLock)
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (_buildOutputFileName == null)
				{
					_buildOutputFileName = (string number) => _output;
				}
				stringBuilder.Append(_hardwareAcceleration);
				stringBuilder.Append(_inputFormat);
				stringBuilder.Append(_inputTime);
				stringBuilder.Append(_inputFramerate);
				stringBuilder.Append(BuildParameters(ParameterPosition.PreInput));
				if (!_capturing)
				{
					stringBuilder.Append(BuildInputParameters());
					if (_buildInputFileName == null)
					{
						stringBuilder.Append(BuildInput());
					}
					else
					{
						_hasInputBuilder = true;
						stringBuilder.Append(_buildInputFileName("_%03d"));
						stringBuilder.Append(BuildInput());
					}
				}
				stringBuilder.Append(BuildOverwriteOutputParameter(_overwriteOutput));
				stringBuilder.Append(BuildThreadsArgument(_useMultiThreads));
				stringBuilder.Append(BuildConversionParameters());
				stringBuilder.Append(BuildStreamParameters());
				stringBuilder.Append(BuildFilters());
				stringBuilder.Append(BuildMap());
				stringBuilder.Append(_framerate);
				stringBuilder.Append(BuildParameters(ParameterPosition.PostInput));
				stringBuilder.Append(_vsyncMode);
				stringBuilder.Append(_outputTime);
				stringBuilder.Append(_outputPixelFormat);
				stringBuilder.Append(_outputFormat);
				stringBuilder.Append(_hashFormat);
				stringBuilder.Append(_buildOutputFileName("_%03d"));
				return stringBuilder.ToString();
			}
		}

		public Task<IConversionResult> Start()
		{
			return Start(Build());
		}

		public Task<IConversionResult> Start(CancellationToken cancellationToken)
		{
			return Start(Build(), cancellationToken);
		}

		public Task<IConversionResult> Start(string parameters)
		{
			return Start(parameters, default(CancellationToken));
		}

		public async Task<IConversionResult> Start(string parameters, CancellationToken cancellationToken)
		{
			if (_ffmpeg != null)
			{
				throw new InvalidOperationException("Conversion has already been started. ");
			}
			_ffmpeg = new FFmpegWrapper();
			_ffmpeg.OnProgress += this.OnProgress;
			_ffmpeg.OnDataReceived += this.OnDataReceived;
			DateTime startTime = DateTime.Now;
			await _ffmpeg.RunProcess(parameters, cancellationToken, _priority);
			ConversionResult result = new ConversionResult
			{
				StartTime = startTime,
				EndTime = DateTime.Now,
				Arguments = parameters
			};
			_processId = null;
			return result;
		}

		public void Clear()
		{
			_parameters.Clear();
			if (_fields == null)
			{
				_fields = from x in GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
						  where x.FieldType == typeof(string)
						  select x;
			}
			foreach (FieldInfo field in _fields)
			{
				field.SetValue(this, null);
			}
		}

		public IConversion AddParameter(string parameter, ParameterPosition parameterPosition = ParameterPosition.PostInput)
		{
			_parameters.Add(new ConversionParameter
			{
				Parameter = parameter.Trim() + " ",
				Position = parameterPosition
			});
			return this;
		}

		public IConversion AddStream<T>(params T[] streams) where T : IStream
		{
			foreach (T val in streams)
			{
				if (val != null)
				{
					_streams.Add(val);
				}
			}
			return this;
		}

		public IConversion AddStream(IEnumerable<IStream> streams)
		{
			foreach (IStream stream in streams)
			{
				AddStream<IStream>(stream);
			}
			return this;
		}

		public IConversion SetHashFormat(Hash hashFormat)
		{
			string hashFormat2 = hashFormat.ToString();
			switch (hashFormat)
			{
				case Hash.SHA512_256:
					hashFormat2 = "SHA512/256";
					break;
				case Hash.SHA512_224:
					hashFormat2 = "SHA512/224";
					break;
			}
			return SetHashFormat(hashFormat2);
		}

		public IConversion SetHashFormat(string hashFormat)
		{
			_hashFormat = "-hash " + hashFormat + " ";
			return this;
		}

		public IConversion SetPreset(ConversionPreset preset)
		{
			_preset = "-preset " + preset.ToString().ToLower() + " ";
			return this;
		}

		public IConversion SetSeek(TimeSpan? seek)
		{
			if (seek.HasValue)
			{
				_seek = "-ss " + seek.Value.ToFFmpeg() + " ";
			}
			return this;
		}

		public IConversion SetInputTime(TimeSpan? time)
		{
			if (time.HasValue)
			{
				_inputTime = "-t " + time.Value.ToFFmpeg() + " ";
			}
			return this;
		}

		public IConversion SetOutputTime(TimeSpan? time)
		{
			if (time.HasValue)
			{
				_outputTime = "-t " + time.Value.ToFFmpeg() + " ";
			}
			return this;
		}

		public IConversion UseMultiThread(bool multiThread)
		{
			_useMultiThreads = multiThread;
			return this;
		}

		public IConversion UseMultiThread(int threadsCount)
		{
			UseMultiThread(true);
			_threadsCount = threadsCount;
			return this;
		}

		public IConversion SetOutput(string outputPath)
		{
			OutputFilePath = outputPath;
			_output = "\"" + outputPath + "\"";
			return this;
		}

		public IConversion SetVideoBitrate(long bitrate)
		{
			AddParameter(string.Format("-b:v {0}", bitrate));
			AddParameter(string.Format("-minrate {0}", bitrate));
			AddParameter(string.Format("-maxrate {0}", bitrate));
			AddParameter(string.Format("-bufsize {0}", bitrate));
			if (HasH264Stream())
			{
				AddParameter("-x264opts nal-hrd=cbr:force-cfr=1");
			}
			return this;
		}

		public IConversion SetAudioBitrate(long bitrate)
		{
			AddParameter(string.Format("-b:a {0} ", bitrate));
			return this;
		}

		public IConversion UseShortest(bool useShortest)
		{
			_shortestInput = ((!useShortest) ? string.Empty : "-shortest ");
			return this;
		}

		public IConversion SetPriority(ProcessPriorityClass? priority)
		{
			_priority = priority;
			return this;
		}

		public IConversion ExtractEveryNthFrame(int frameNo, Func<string, string> buildOutputFileName)
		{
			_buildOutputFileName = buildOutputFileName;
			AddParameter(string.Format("-vf select='not(mod(n\\,{0}))'", frameNo));
			SetVideoSyncMethod(VideoSyncMethod.vfr);
			return this;
		}

		public IConversion ExtractNthFrame(int frameNo, Func<string, string> buildOutputFileName)
		{
			_buildOutputFileName = buildOutputFileName;
			AddParameter(string.Format("-vf select='eq(n\\,{0})'", frameNo));
			SetVideoSyncMethod(VideoSyncMethod.passthrough);
			return this;
		}

		public IConversion BuildVideoFromImages(int startNumber, Func<string, string> buildInputFileName)
		{
			_buildInputFileName = buildInputFileName;
			AddParameter(string.Format("-start_number {0}", startNumber), ParameterPosition.PreInput);
			return this;
		}

		public IConversion BuildVideoFromImages(IEnumerable<string> imageFiles)
		{
			InputBuilder inputBuilder = new InputBuilder();
			string directory = string.Empty;
			_buildInputFileName = inputBuilder.PrepareInputFiles(imageFiles.ToList(), out directory);
			return this;
		}

		public IConversion SetInputFrameRate(double frameRate)
		{
			_inputFramerate = string.Format("-framerate {0} -r {1} ", frameRate, frameRate);
			return this;
		}

		public IConversion SetFrameRate(double frameRate)
		{
			_framerate = "-framerate " + frameRate.ToFFmpegFormat(3) + " -r " + frameRate.ToFFmpegFormat(3) + " ";
			return this;
		}

		public IConversion GetScreenCapture(double frameRate)
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				_capturing = true;
				SetInputFormat(Format.gdigrab);
				SetFrameRate(frameRate);
				AddParameter("-i desktop ", ParameterPosition.PreInput);
				SetPixelFormat(PixelFormat.yuv420p);
				AddParameter("-preset ultrafast");
				return this;
			}
			if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				_capturing = true;
				SetInputFormat(Format.avfoundation);
				SetFrameRate(frameRate);
				AddParameter("-i 1:1 ", ParameterPosition.PreInput);
				SetPixelFormat(PixelFormat.yuv420p);
				AddParameter("-preset ultrafast");
				return this;
			}
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				_capturing = true;
				SetInputFormat(Format.x11grab);
				SetFrameRate(frameRate);
				AddParameter("-i :0.0+0,0 ", ParameterPosition.PreInput);
				SetPixelFormat(PixelFormat.yuv420p);
				AddParameter("-preset ultrafast");
				return this;
			}
			_capturing = false;
			return this;
		}

		private string BuildConversionParameters()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(_preset);
			stringBuilder.Append(_shortestInput);
			stringBuilder.Append(_seek);
			return stringBuilder.ToString();
		}

		private string BuildStreamParameters()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (IStream stream in _streams)
			{
				stringBuilder.Append(stream.Build());
			}
			return stringBuilder.ToString();
		}

		private string BuildInputParameters()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (IStream stream in _streams)
			{
				stringBuilder.Append(stream.BuildInputArguments());
			}
			return stringBuilder.ToString();
		}

		private string BuildOverwriteOutputParameter(bool overwriteOutput)
		{
			if (!overwriteOutput)
			{
				return "-n ";
			}
			return "-y ";
		}

		private string BuildFilters()
		{
			StringBuilder stringBuilder = new StringBuilder();
			List<IFilterConfiguration> list = new List<IFilterConfiguration>();
			foreach (IStream stream in _streams)
			{
				IFilterable filterable = stream as IFilterable;
				if (filterable != null)
				{
					list.AddRange(filterable.GetFilters());
				}
			}
			foreach (IGrouping<string, IFilterConfiguration> filterGroup in from configuration in list
																			group configuration by configuration.FilterType)
			{
				stringBuilder.Append(filterGroup.Key + " \"");
				foreach (IFilterConfiguration item in list.Where((IFilterConfiguration x) => x.FilterType == filterGroup.Key))
				{
					List<string> list2 = new List<string>();
					foreach (KeyValuePair<string, string> filter in item.Filters)
					{
						string text = string.Format("[{0}]", item.StreamNumber);
						string text2 = (string.IsNullOrEmpty(filter.Value) ? (filter.Key + " ") : (filter.Key + "=" + filter.Value));
						list2.Add(text + " " + text2 + " ");
					}
					stringBuilder.Append(string.Join(";", list2));
				}
				stringBuilder.Append("\" ");
			}
			return stringBuilder.ToString();
		}

		private string BuildThreadsArgument(bool multiThread)
		{
			string text = "";
			text = (_threadsCount.HasValue ? _threadsCount.ToString() : (multiThread ? Environment.ProcessorCount.ToString() : "1"));
			return "-threads " + text + " ";
		}

		private string BuildMap()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (IStream stream in _streams)
			{
				if (_hasInputBuilder)
				{
					stringBuilder.Append("-map 0:0 ");
				}
				foreach (string item in stream.GetSource())
				{
					if (_hasInputBuilder)
					{
						stringBuilder.Append(string.Format("-map {0}:{1} ", _inputFileMap[item] + 1, stream.Index));
					}
					else
					{
						stringBuilder.Append(string.Format("-map {0}:{1} ", _inputFileMap[item], stream.Index));
					}
				}
			}
			return stringBuilder.ToString();
		}

		private string BuildParameters(ParameterPosition forPosition)
		{
			IList<ConversionParameter> parameters = _parameters;
			IEnumerable<ConversionParameter> enumerable = ((parameters != null) ? parameters.Where((ConversionParameter x) => x.Position == forPosition) : null);
			if (enumerable != null && enumerable.Any())
			{
				return string.Join(string.Empty, enumerable.Select((ConversionParameter x) => x.Parameter));
			}
			return string.Empty;
		}

		private string BuildInput()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string item in _streams.SelectMany((IStream x) => x.GetSource()).Distinct())
			{
				_inputFileMap[item] = num++;
				stringBuilder.Append("-i \"" + item + "\" ");
			}
			return stringBuilder.ToString();
		}

		private bool HasH264Stream()
		{
			foreach (IStream stream in _streams)
			{
				if (stream is IVideoStream)
				{
					IVideoStream videoStream = (IVideoStream)stream;
					if (videoStream.Codec == "libx264" || videoStream.Codec == VideoCodec.h264.ToString())
					{
						return true;
					}
				}
			}
			return false;
		}

		[Obsolete("This will be deleted in next major version. Please use FFmpeg.Conversions.New() instead of that.")]
		public static IConversion New()
		{
			return new Conversion();
		}

		public IConversion UseHardwareAcceleration(HardwareAccelerator hardwareAccelerator, VideoCodec decoder, VideoCodec encoder, int device = 0)
		{
			return UseHardwareAcceleration(string.Format("{0}", hardwareAccelerator), decoder.ToString(), encoder.ToString(), device);
		}

		public IConversion UseHardwareAcceleration(string hardwareAccelerator, string decoder, string encoder, int device = 0)
		{
			_hardwareAcceleration = "-hwaccel " + hardwareAccelerator + " -c:v " + decoder + " ";
			AddParameter("-c:v " + ((encoder != null) ? encoder.ToString() : null) + " ");
			if (device != 0)
			{
				_hardwareAcceleration += string.Format("-hwaccel_device {0} ", device);
			}
			UseMultiThread(false);
			return this;
		}

		public IConversion SetOverwriteOutput(bool overwrite)
		{
			_overwriteOutput = overwrite;
			return this;
		}

		public IConversion SetInputFormat(Format inputFormat)
		{
			string inputFormat2 = inputFormat.ToString();
			switch (inputFormat)
			{
				case Format._3dostr:
					inputFormat2 = "3dostr";
					break;
				case Format._3g2:
					inputFormat2 = "3g2";
					break;
				case Format._3gp:
					inputFormat2 = "3gp";
					break;
				case Format._4xm:
					inputFormat2 = "4xm";
					break;
			}
			return SetInputFormat(inputFormat2);
		}

		public IConversion SetInputFormat(string format)
		{
			if (format != null)
			{
				_inputFormat = "-f " + format + " ";
			}
			return this;
		}

		public IConversion SetOutputFormat(Format outputFormat)
		{
			string outputFormat2 = outputFormat.ToString();
			switch (outputFormat)
			{
				case Format._3dostr:
					outputFormat2 = "3dostr";
					break;
				case Format._3g2:
					outputFormat2 = "3g2";
					break;
				case Format._3gp:
					outputFormat2 = "3gp";
					break;
				case Format._4xm:
					outputFormat2 = "4xm";
					break;
			}
			return SetOutputFormat(outputFormat2);
		}

		public IConversion SetOutputFormat(string format)
		{
			if (format != null)
			{
				_outputFormat = "-f " + format + " ";
			}
			return this;
		}

		public IConversion SetPixelFormat(PixelFormat pixelFormat)
		{
			string pixelFormat2 = pixelFormat.ToString();
			switch (pixelFormat)
			{
				case PixelFormat._0bgr:
					pixelFormat2 = "0bgr";
					break;
				case PixelFormat._0rgb:
					pixelFormat2 = "0rgb";
					break;
			}
			return SetPixelFormat(pixelFormat2);
		}

		public IConversion SetPixelFormat(string pixelFormat)
		{
			if (pixelFormat != null)
			{
				_outputPixelFormat = "-pix_fmt " + pixelFormat + " ";
			}
			return this;
		}

		public IConversion SetVideoSyncMethod(VideoSyncMethod method)
		{
			if (method == VideoSyncMethod.auto)
			{
				_vsyncMode = "-vsync -1 ";
			}
			else
			{
				_vsyncMode = string.Format("-vsync {0} ", method);
			}
			return this;
		}

		[Obsolete("This will be deleted in next major version. Please use FFmpeg.Conversions.FromSnippet instead of that.")]
		public static IConversion ExtractAudio(string inputPath, string outputPath)
		{
			IAudioStream audioStream = FFmpeg.GetMediaInfo(inputPath).GetAwaiter().GetResult()
				.AudioStreams.FirstOrDefault();
			return New().AddStream<IAudioStream>(audioStream).SetAudioBitrate(audioStream.Bitrate).SetOutput(outputPath);
		}

		[Obsolete("This will be deleted in next major version. Please use FFmpeg.Conversions.FromSnippet instead of that.")]
		public static IConversion AddAudio(string videoPath, string audioPath, string outputPath)
		{
			IMediaInfo result = FFmpeg.GetMediaInfo(videoPath).GetAwaiter().GetResult();
			IMediaInfo result2 = FFmpeg.GetMediaInfo(audioPath).GetAwaiter().GetResult();
			return New().AddStream<IVideoStream>(result.VideoStreams.FirstOrDefault()).AddStream<IAudioStream>(result2.AudioStreams.FirstOrDefault()).AddStream(result.SubtitleStreams.ToArray())
				.SetOutput(outputPath);
		}

		[Obsolete("This will be deleted in next major version. Please use FFmpeg.Conversions.FromSnippet instead of that.")]
		public static IConversion VisualiseAudio(string inputPath, string outputPath, VideoSize size, PixelFormat pixelFormat = PixelFormat.yuv420p, VisualisationMode mode = VisualisationMode.bar, AmplitudeScale amplitudeScale = AmplitudeScale.lin, FrequencyScale frequencyScale = FrequencyScale.log)
		{
			IMediaInfo result = FFmpeg.GetMediaInfo(inputPath).GetAwaiter().GetResult();
			IAudioStream audioStream = result.AudioStreams.FirstOrDefault();
			IVideoStream videoStream = result.VideoStreams.FirstOrDefault();
			string text = string.Format("\"[0:a]showfreqs=mode={0}:fscale={1}:ascale={2},format={3},scale={4} [v]\"", mode, frequencyScale, amplitudeScale, pixelFormat, size.ToFFmpegFormat());
			return New().AddStream<IAudioStream>(audioStream).AddParameter("-filter_complex " + text).AddParameter("-map [v]")
				.SetFrameRate((videoStream != null) ? videoStream.Framerate : 30.0)
				.SetOutput(outputPath);
		}

		[Obsolete("This will be deleted in next major version. Please use FFmpeg.Conversions.FromSnippet instead of that.")]
		public static IConversion ToMp4(string inputPath, string outputPath)
		{
			IMediaInfo result = FFmpeg.GetMediaInfo(inputPath).GetAwaiter().GetResult();
			IVideoStream videoStream = result.VideoStreams.FirstOrDefault();
			IStream stream = ((videoStream != null) ? videoStream.SetCodec(VideoCodec.h264) : null);
			IAudioStream audioStream = result.AudioStreams.FirstOrDefault();
			IStream stream2 = ((audioStream != null) ? audioStream.SetCodec(AudioCodec.aac) : null);
			return New().AddStream<IStream>(stream, stream2).SetOutput(outputPath);
		}

		[Obsolete("This will be deleted in next major version. Please use FFmpeg.Conversions.FromSnippet instead of that.")]
		public static IConversion ToTs(string inputPath, string outputPath)
		{
			IMediaInfo result = FFmpeg.GetMediaInfo(inputPath).GetAwaiter().GetResult();
			IVideoStream videoStream = result.VideoStreams.FirstOrDefault();
			IStream stream = ((videoStream != null) ? videoStream.SetCodec(VideoCodec.mpeg2video) : null);
			IAudioStream audioStream = result.AudioStreams.FirstOrDefault();
			IStream stream2 = ((audioStream != null) ? audioStream.SetCodec(AudioCodec.mp2) : null);
			return New().AddStream<IStream>(stream, stream2).SetOutput(outputPath);
		}

		[Obsolete("This will be deleted in next major version. Please use FFmpeg.Conversions.FromSnippet instead of that.")]
		public static IConversion ToOgv(string inputPath, string outputPath)
		{
			IMediaInfo result = FFmpeg.GetMediaInfo(inputPath).GetAwaiter().GetResult();
			IVideoStream videoStream = result.VideoStreams.FirstOrDefault();
			IStream stream = ((videoStream != null) ? videoStream.SetCodec(VideoCodec.theora) : null);
			IAudioStream audioStream = result.AudioStreams.FirstOrDefault();
			IStream stream2 = ((audioStream != null) ? audioStream.SetCodec(AudioCodec.libvorbis) : null);
			return New().AddStream<IStream>(stream, stream2).SetOutput(outputPath);
		}

		[Obsolete("This will be deleted in next major version. Please use FFmpeg.Conversions.FromSnippet instead of that.")]
		public static IConversion ToWebM(string inputPath, string outputPath)
		{
			IMediaInfo result = FFmpeg.GetMediaInfo(inputPath).GetAwaiter().GetResult();
			IVideoStream videoStream = result.VideoStreams.FirstOrDefault();
			IStream stream = ((videoStream != null) ? videoStream.SetCodec(VideoCodec.vp8) : null);
			IAudioStream audioStream = result.AudioStreams.FirstOrDefault();
			IStream stream2 = ((audioStream != null) ? audioStream.SetCodec(AudioCodec.libvorbis) : null);
			return New().AddStream<IStream>(stream, stream2).SetOutput(outputPath);
		}

		[Obsolete("This will be deleted in next major version. Please use FFmpeg.Conversions.FromSnippet instead of that.")]
		public static IConversion ToGif(string inputPath, string outputPath, int loop, int delay = 0)
		{
			IVideoStream videoStream = FFmpeg.GetMediaInfo(inputPath).GetAwaiter().GetResult()
				.VideoStreams.FirstOrDefault();
			IVideoStream videoStream2 = ((videoStream != null) ? videoStream.SetLoop(loop, delay) : null);
			return New().AddStream<IVideoStream>(videoStream2).SetOutput(outputPath);
		}

		[Obsolete("This will be deleted in next major version. Please use FFmpeg.Conversions.FromSnippet instead of that.")]
		public static IConversion ConvertWithHardware(string inputFilePath, string outputFilePath, HardwareAccelerator hardwareAccelerator, VideoCodec decoder, VideoCodec encoder, int device = 0)
		{
			return Convert(inputFilePath, outputFilePath).UseHardwareAcceleration(hardwareAccelerator, decoder, encoder, device);
		}

		[Obsolete("This will be deleted in next major version. Please use FFmpeg.Conversions.FromSnippet instead of that.")]
		public static IConversion AddSubtitles(string inputPath, string outputPath, string subtitlesPath)
		{
			IMediaInfo result = FFmpeg.GetMediaInfo(inputPath).GetAwaiter().GetResult();
			IVideoStream videoStream = result.VideoStreams.FirstOrDefault();
			IVideoStream videoStream2 = ((videoStream != null) ? videoStream.AddSubtitles(subtitlesPath) : null);
			return New().AddStream<IVideoStream>(videoStream2).AddStream<IAudioStream>(result.AudioStreams.FirstOrDefault()).SetOutput(outputPath);
		}

		[Obsolete("This will be deleted in next major version. Please use FFmpeg.Conversions.FromSnippet instead of that.")]
		public static IConversion AddSubtitle(string inputPath, string outputPath, string subtitlePath, string language = null)
		{
			IMediaInfo result = FFmpeg.GetMediaInfo(inputPath).GetAwaiter().GetResult();
			ISubtitleStream subtitleStream = FFmpeg.GetMediaInfo(subtitlePath).GetAwaiter().GetResult()
				.SubtitleStreams.First().SetLanguage(language);
			return New().AddStream((IEnumerable<IStream>)result.VideoStreams).AddStream((IEnumerable<IStream>)result.AudioStreams).AddStream<ISubtitleStream>(subtitleStream.SetCodec(SubtitleCodec.copy))
				.SetOutput(outputPath);
		}

		[Obsolete("This will be deleted in next major version. Please use FFmpeg.Conversions.FromSnippet instead of that.")]
		public static IConversion AddSubtitle(string inputPath, string outputPath, string subtitlePath, SubtitleCodec subtitleCodec, string language = null)
		{
			IMediaInfo result = FFmpeg.GetMediaInfo(inputPath).GetAwaiter().GetResult();
			ISubtitleStream subtitleStream = FFmpeg.GetMediaInfo(subtitlePath).GetAwaiter().GetResult()
				.SubtitleStreams.First().SetLanguage(language);
			return New().AddStream((IEnumerable<IStream>)result.VideoStreams).AddStream((IEnumerable<IStream>)result.AudioStreams).AddStream<ISubtitleStream>(subtitleStream.SetCodec(subtitleCodec))
				.SetOutput(outputPath);
		}

		[Obsolete("This will be deleted in next major version. Please use FFmpeg.Conversions.FromSnippet instead of that.")]
		public static IConversion SetWatermark(string inputPath, string outputPath, string inputImage, Position position)
		{
			IMediaInfo result = FFmpeg.GetMediaInfo(inputPath).GetAwaiter().GetResult();
			IVideoStream videoStream = result.VideoStreams.FirstOrDefault().SetWatermark(inputImage, position);
			return New().AddStream<IVideoStream>(videoStream).AddStream(result.AudioStreams.ToArray()).SetOutput(outputPath);
		}

		[Obsolete("This will be deleted in next major version. Please use FFmpeg.Conversions.FromSnippet instead of that.")]
		public static IConversion ExtractVideo(string inputPath, string outputPath)
		{
			IVideoStream videoStream = FFmpeg.GetMediaInfo(inputPath).GetAwaiter().GetResult()
				.VideoStreams.FirstOrDefault();
			return New().AddStream<IVideoStream>(videoStream).SetOutput(outputPath);
		}

		[Obsolete("This will be deleted in next major version. Please use FFmpeg.Conversions.FromSnippet instead of that.")]
		public static IConversion Snapshot(string inputPath, string outputPath, TimeSpan captureTime)
		{
			IVideoStream videoStream = FFmpeg.GetMediaInfo(inputPath).GetAwaiter().GetResult()
				.VideoStreams.FirstOrDefault().SetOutputFramesCount(1).SetSeek(captureTime);
			return New().AddStream<IVideoStream>(videoStream).SetOutput(outputPath);
		}

		[Obsolete("This will be deleted in next major version. Please use FFmpeg.Conversions.FromSnippet instead of that.")]
		public static IConversion ChangeSize(string inputPath, string outputPath, int width, int height)
		{
			IMediaInfo result = FFmpeg.GetMediaInfo(inputPath).GetAwaiter().GetResult();
			IVideoStream videoStream = result.VideoStreams.FirstOrDefault().SetSize(width, height);
			return New().AddStream<IVideoStream>(videoStream).AddStream(result.AudioStreams.ToArray()).AddStream(result.SubtitleStreams.ToArray())
				.SetOutput(outputPath);
		}

		[Obsolete("This will be deleted in next major version. Please use FFmpeg.Conversions.FromSnippet instead of that.")]
		public static IConversion ChangeSize(string inputPath, string outputPath, VideoSize size)
		{
			IMediaInfo result = FFmpeg.GetMediaInfo(inputPath).GetAwaiter().GetResult();
			IVideoStream videoStream = result.VideoStreams.FirstOrDefault().SetSize(size);
			return New().AddStream<IVideoStream>(videoStream).AddStream(result.AudioStreams.ToArray()).AddStream(result.SubtitleStreams.ToArray())
				.SetOutput(outputPath);
		}

		[Obsolete("This will be deleted in next major version. Please use FFmpeg.Conversions.FromSnippet instead of that.")]
		public static IConversion Split(string inputPath, string outputPath, TimeSpan startTime, TimeSpan duration)
		{
			IMediaInfo result = FFmpeg.GetMediaInfo(inputPath).GetAwaiter().GetResult();
			List<IStream> list = new List<IStream>();
			foreach (IVideoStream videoStream in result.VideoStreams)
			{
				list.Add(videoStream.Split(startTime, duration));
			}
			foreach (IAudioStream audioStream in result.AudioStreams)
			{
				list.Add(audioStream.Split(startTime, duration));
			}
			return New().AddStream((IEnumerable<IStream>)list).SetOutput(outputPath);
		}

		[Obsolete("This will be deleted in next major version. Please use FFmpeg.Conversions.FromSnippet instead of that.")]
		public static IConversion SaveM3U8Stream(Uri uri, string outputPath, TimeSpan? duration = null)
		{
			IMediaInfo result = FFmpeg.GetMediaInfo(uri.ToString()).GetAwaiter().GetResult();
			return New().AddStream(result.Streams).SetInputTime(duration).SetOutput(outputPath);
		}

		[Obsolete("This will be deleted in next major version. Please use FFmpeg.Conversions.FromSnippet instead of that.")]
		public static async Task<IConversion> Concatenate(string output, params string[] inputVideos)
		{
			if (inputVideos.Length <= 1)
			{
				throw new ArgumentException("You must provide at least 2 files for the concatenation to work", "inputVideos");
			}
			List<IMediaInfo> mediaInfos = new List<IMediaInfo>();
			IConversion conversion = New();
			foreach (string inputVideo in inputVideos)
			{
				mediaInfos.Add(await FFmpeg.GetMediaInfo(inputVideo));
				conversion.AddParameter("-i \"" + inputVideo + "\" ");
			}
			conversion.AddParameter("-t 1 -f lavfi -i anullsrc=r=48000:cl=stereo");
			conversion.AddParameter("-filter_complex \"");
			IVideoStream videoStream = (from x in mediaInfos
										select x.VideoStreams.OrderByDescending((IVideoStream z) => z.Width).First() into x
										orderby x.Width descending
										select x).First();
			for (int j = 0; j < mediaInfos.Count; j++)
			{
				conversion.AddParameter(string.Format("[{0}:v]scale={1}:{2},setdar=dar={3},setpts=PTS-STARTPTS[v{4}]; ", j, videoStream.Width, videoStream.Height, videoStream.Ratio, j));
			}
			for (int k = 0; k < mediaInfos.Count; k++)
			{
				conversion.AddParameter((!mediaInfos[k].AudioStreams.Any()) ? string.Format("[v{0}]", k) : string.Format("[v{0}][{1}:a]", k, k));
			}
			conversion.AddParameter(string.Format("concat=n={0}:v=1:a=1 [v] [a]\" -map \"[v]\" -map \"[a]\"", inputVideos.Length));
			conversion.AddParameter("-aspect " + videoStream.Ratio);
			return conversion.SetOutput(output);
		}

		[Obsolete("This will be deleted in next major version. Please use FFmpeg.Conversions.FromSnippet instead of that.")]
		public static IConversion Convert(string inputFilePath, string outputFilePath, bool keepSubtitles = false)
		{
			IMediaInfo result = FFmpeg.GetMediaInfo(inputFilePath).GetAwaiter().GetResult();
			IConversion conversion = New().SetOutput(outputFilePath);
			foreach (IStream stream in result.Streams)
			{
				IVideoStream videoStream = stream as IVideoStream;
				if (videoStream != null)
				{
					conversion.AddStream<IVideoStream>(videoStream.SetFramerate(videoStream.Framerate));
					continue;
				}
				IAudioStream audioStream = stream as IAudioStream;
				if (audioStream != null)
				{
					conversion.AddStream<IAudioStream>(audioStream);
					continue;
				}
				ISubtitleStream subtitleStream = stream as ISubtitleStream;
				if (subtitleStream != null && keepSubtitles)
				{
					conversion.AddStream<ISubtitleStream>(subtitleStream.SetCodec(SubtitleCodec.mov_text));
				}
			}
			return conversion;
		}

		[Obsolete("This will be deleted in next major version. Please use FFmpeg.Conversions.FromSnippet instead of that.")]
		public static IConversion Transcode(string inputFilePath, string outputFilePath, VideoCodec videoCodec, AudioCodec audioCodec, SubtitleCodec subtitleCodec, bool keepSubtitles = false)
		{
			IMediaInfo result = FFmpeg.GetMediaInfo(inputFilePath).GetAwaiter().GetResult();
			IConversion conversion = New().SetOutput(outputFilePath);
			foreach (IStream stream in result.Streams)
			{
				IVideoStream videoStream = stream as IVideoStream;
				if (videoStream != null)
				{
					conversion.AddStream<IVideoStream>(videoStream.SetCodec(videoCodec).SetFramerate(videoStream.Framerate));
					continue;
				}
				IAudioStream audioStream = stream as IAudioStream;
				if (audioStream != null)
				{
					conversion.AddStream<IAudioStream>(audioStream.SetCodec(audioCodec));
					continue;
				}
				ISubtitleStream subtitleStream = stream as ISubtitleStream;
				if (subtitleStream != null && keepSubtitles)
				{
					conversion.AddStream<ISubtitleStream>(subtitleStream.SetCodec(subtitleCodec));
				}
			}
			return conversion;
		}
	}
}
