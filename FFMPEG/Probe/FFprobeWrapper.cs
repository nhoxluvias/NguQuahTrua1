using FFMPEG.Probe.Model;
using FFMPEG.Streams.AudioStream;
using FFMPEG.Streams.VideoStream;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FFMPEG.Probe
{
	internal sealed class FFprobeWrapper : FFmpeg
	{
		private async Task<ProbeModel.Stream[]> GetStreams(string videoPath, CancellationToken cancellationToken)
		{
			string value = await Start("-v panic -print_format json -show_streams \"" + videoPath + "\"", cancellationToken);
			if (string.IsNullOrEmpty(value))
			{
				return new ProbeModel.Stream[0];
			}
			return JsonConvert.DeserializeObject<ProbeModel>(value).streams ?? new ProbeModel.Stream[0];
		}

		private double GetVideoFramerate(ProbeModel.Stream vid)
		{
			long frameCount = GetFrameCount(vid);
			double duration = vid.duration;
			string[] array = vid.r_frame_rate.Split(new char[1]
			{
			'/'
			});
			if (frameCount > 0)
			{
				return Math.Round((double)frameCount / duration, 3);
			}
			return Math.Round(double.Parse(array[0]) / double.Parse(array[1]), 3);
		}

		private long GetFrameCount(ProbeModel.Stream vid)
		{
			long result = 0L;
			if (!long.TryParse(vid.nb_frames, out result))
			{
				return 0L;
			}
			return result;
		}

		private string GetVideoAspectRatio(int width, int height)
		{
			int gcd = GetGcd(width, height);
			if (gcd <= 0)
			{
				return "0:0";
			}
			return width / gcd + ":" + height / gcd;
		}

		private async Task<FormatModel.Format> GetFormat(string videoPath, CancellationToken cancellationToken)
		{
			return JsonConvert.DeserializeObject<FormatModel.Root>(await Start("-v panic -print_format json -show_entries format=size,duration,bit_rate \"" + videoPath + "\"", cancellationToken)).format;
		}

		private TimeSpan GetAudioDuration(ProbeModel.Stream audio)
		{
			return TimeSpan.FromSeconds(audio.duration);
		}

		private TimeSpan GetVideoDuration(ProbeModel.Stream video, FormatModel.Format format)
		{
			return TimeSpan.FromSeconds((video.duration > 0.01) ? video.duration : format.duration);
		}

		private int GetGcd(int width, int height)
		{
			while (width != 0 && height != 0)
			{
				if (width > height)
				{
					width -= height;
				}
				else
				{
					height -= width;
				}
			}
			if (width != 0)
			{
				return width;
			}
			return height;
		}

		public Task<string> Start(string args, CancellationToken cancellationToken)
		{
			return RunProcess(args, cancellationToken);
		}

		private async Task<string> RunProcess(string args, CancellationToken cancellationToken)
		{
			return await Task.Factory.StartNew(delegate
			{
				Process process = RunProcess(args, base.FFprobePath, null, false, true);
				try
				{
					cancellationToken.Register(delegate
					{
						try
						{
							if (!process.HasExited)
							{
								process.CloseMainWindow();
								process.Kill();
							}
						}
						catch
						{
						}
					});
					string result = process.StandardOutput.ReadToEnd();
					process.WaitForExit();
					return result;
				}
				finally
				{
					if (process != null)
					{
						((IDisposable)process).Dispose();
					}
				}
			}, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
		}

		public async Task<MediaInfo.MediaInfo> SetProperties(MediaInfo.MediaInfo mediaInfo, CancellationToken cancellationToken)
		{
			string path = mediaInfo.Path;
			ProbeModel.Stream[] streams = await GetStreams(path, cancellationToken);
			if (!streams.Any())
			{
				throw new ArgumentException("Invalid file. Cannot load file " + path);
			}
			FormatModel.Format format = await GetFormat(path, cancellationToken);
			if (format.size != null)
			{
				mediaInfo.Size = long.Parse(format.size);
			}
			mediaInfo.VideoStreams = PrepareVideoStreams(path, streams.Where((ProbeModel.Stream x) => x.codec_type == "video"), format);
			mediaInfo.AudioStreams = PrepareAudioStreams(path, streams.Where((ProbeModel.Stream x) => x.codec_type == "audio"));
			mediaInfo.SubtitleStreams = PrepareSubtitleStreams(path, streams.Where((ProbeModel.Stream x) => x.codec_type == "subtitle"));
			mediaInfo.Duration = CalculateDuration(mediaInfo.VideoStreams, mediaInfo.AudioStreams);
			return mediaInfo;
		}

		private static TimeSpan CalculateDuration(IEnumerable<IVideoStream> videoStreams, IEnumerable<IAudioStream> audioStreams)
		{
			double val = (audioStreams.Any() ? audioStreams.Max((IAudioStream x) => x.Duration.TotalSeconds) : 0.0);
			double val2 = (videoStreams.Any() ? videoStreams.Max((IVideoStream x) => x.Duration.TotalSeconds) : 0.0);
			return TimeSpan.FromSeconds(Math.Max(val, val2));
		}

		private IEnumerable<IAudioStream> PrepareAudioStreams(string path, IEnumerable<ProbeModel.Stream> audioStreamModels)
		{
			return audioStreamModels.Select(delegate (ProbeModel.Stream model)
			{
				AudioStream obj = new AudioStream
				{
					Codec = model.codec_name,
					Duration = GetAudioDuration(model),
					Path = path,
					Index = model.index,
					Bitrate = Math.Abs(model.bit_rate),
					Channels = model.channels,
					SampleRate = model.sample_rate
				};
				ProbeModel.Tags tags = model.tags;
				obj.Language = ((tags != null) ? tags.language : null);
				ProbeModel.Disposition disposition = model.disposition;
				obj.Default = ((disposition != null) ? new int?(disposition._default) : null);
				ProbeModel.Disposition disposition2 = model.disposition;
				obj.Forced = ((disposition2 != null) ? new int?(disposition2.forced) : null);
				return obj;
			});
		}

		private static IEnumerable<ISubtitleStream> PrepareSubtitleStreams(string path, IEnumerable<ProbeModel.Stream> subtitleStreamModels)
		{
			return subtitleStreamModels.Select(delegate (ProbeModel.Stream model)
			{
				SubtitleStream obj = new SubtitleStream
				{
					Codec = model.codec_name,
					Path = path,
					Index = model.index
				};
				ProbeModel.Tags tags = model.tags;
				obj.Language = ((tags != null) ? tags.language : null);
				ProbeModel.Tags tags2 = model.tags;
				obj.Title = ((tags2 != null) ? tags2.title : null);
				ProbeModel.Disposition disposition = model.disposition;
				obj.Default = ((disposition != null) ? new int?(disposition._default) : null);
				ProbeModel.Disposition disposition2 = model.disposition;
				obj.Forced = ((disposition2 != null) ? new int?(disposition2.forced) : null);
				return obj;
			});
		}

		private IEnumerable<IVideoStream> PrepareVideoStreams(string path, IEnumerable<ProbeModel.Stream> videoStreamModels, FormatModel.Format format)
		{
			return videoStreamModels.Select(delegate (ProbeModel.Stream model)
			{
				VideoStream obj = new VideoStream
				{
					Codec = model.codec_name,
					Duration = GetVideoDuration(model, format),
					Width = model.width,
					Height = model.height,
					Framerate = GetVideoFramerate(model),
					Ratio = GetVideoAspectRatio(model.width, model.height),
					Path = path,
					Index = model.index,
					Bitrate = (((double)Math.Abs(model.bit_rate) > 0.01) ? model.bit_rate : format.bit_Rate),
					PixelFormat = model.pix_fmt
				};
				ProbeModel.Disposition disposition = model.disposition;
				obj.Default = ((disposition != null) ? new int?(disposition._default) : null);
				ProbeModel.Disposition disposition2 = model.disposition;
				obj.Forced = ((disposition2 != null) ? new int?(disposition2.forced) : null);
				ProbeModel.Tags tags = model.tags;
				obj.Rotation = ((tags != null) ? tags.rotate : null);
				return obj;
			});
		}
	}

}
