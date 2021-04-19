using FFMPEG.Conversion.Enums;
using FFMPEG.Conversion.Events;
using FFMPEG.Conversion.Models;
using FFMPEG.Streams;
using FFMPEG.Streams.VideoStream;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FFMPEG.Conversion
{
	public interface IConversion
	{
		string OutputFilePath
		{
			get;
		}

		int? FFmpegProcessId
		{
			get;
		}

		event ConversionProgressEventHandler OnProgress;

		event DataReceivedEventHandler OnDataReceived;

		void Clear();

		IConversion SetPriority(ProcessPriorityClass? priority);

		IConversion ExtractEveryNthFrame(int frameNo, Func<string, string> buildOutputFileName);

		IConversion ExtractNthFrame(int frameNo, Func<string, string> buildOutputFileName);

		IConversion BuildVideoFromImages(int startNumber, Func<string, string> buildInputFileName);

		IConversion BuildVideoFromImages(IEnumerable<string> imageFiles);

		IConversion SetFrameRate(double frameRate);

		IConversion SetInputFrameRate(double frameRate);

		IConversion SetSeek(TimeSpan? seek);

		IConversion SetInputTime(TimeSpan? seek);

		IConversion SetOutputTime(TimeSpan? seek);

		IConversion SetPreset(ConversionPreset preset);

		IConversion SetHashFormat(Hash format);

		IConversion SetHashFormat(string format);

		IConversion SetVideoBitrate(long bitrate);

		IConversion SetAudioBitrate(long bitrate);

		IConversion UseMultiThread(int threadCount);

		IConversion UseMultiThread(bool multiThread);

		IConversion SetOutput(string outputPath);

		IConversion SetOverwriteOutput(bool overwrite);

		IConversion GetScreenCapture(double frameRate);

		IConversion SetInputFormat(string inputFormat);

		IConversion SetInputFormat(Format inputFormat);

		IConversion SetOutputFormat(Format outputFormat);

		IConversion SetOutputFormat(string outputFormat);

		IConversion SetPixelFormat(string pixelFormat);

		IConversion SetPixelFormat(PixelFormat pixelFormat);

		IConversion UseShortest(bool useShortest);

		string Build();

		Task<IConversionResult> Start();

		Task<IConversionResult> Start(CancellationToken cancellationToken);

		Task<IConversionResult> Start(string parameters);

		Task<IConversionResult> Start(string parameters, CancellationToken cancellationToken);

		IConversion AddParameter(string parameter, ParameterPosition parameterPosition = ParameterPosition.PostInput);

		IConversion AddStream<T>(params T[] streams) where T : IStream;

		IConversion AddStream(IEnumerable<IStream> streams);

		IConversion UseHardwareAcceleration(HardwareAccelerator hardwareAccelerator, VideoCodec decoder, VideoCodec encoder, int device = 0);

		IConversion UseHardwareAcceleration(string hardwareAccelerator, string decoder, string encoder, int device = 0);

		IConversion SetVideoSyncMethod(VideoSyncMethod method);
	}
}
