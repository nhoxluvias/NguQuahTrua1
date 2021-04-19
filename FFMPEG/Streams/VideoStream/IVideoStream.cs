using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFMPEG.Streams.VideoStream
{
	public interface IVideoStream : IStream
	{
		TimeSpan Duration
		{
			get;
		}

		int Width
		{
			get;
		}

		int Height
		{
			get;
		}

		double Framerate
		{
			get;
		}

		string Ratio
		{
			get;
		}

		long Bitrate
		{
			get;
		}

		int? Default
		{
			get;
		}

		int? Forced
		{
			get;
		}

		string PixelFormat
		{
			get;
		}

		int? Rotation
		{
			get;
		}

		IVideoStream Rotate(RotateDegrees rotateDegrees);

		IVideoStream ChangeSpeed(double multiplicator);

		IVideoStream SetWatermark(string imagePath, Position position);

		IVideoStream Reverse();

		IVideoStream SetFlags(params Flag[] flags);

		IVideoStream SetFlags(params string[] flags);

		IVideoStream SetFramerate(double framerate);

		IVideoStream SetBitrate(long minBitrate, long maxBitrate, long bufferSize);

		IVideoStream SetBitrate(long bitrate);

		IVideoStream SetSize(VideoSize size);

		IVideoStream SetSize(int width, int height);

		IVideoStream SetCodec(VideoCodec codec);

		IVideoStream SetCodec(string codec);

		IVideoStream CopyStream();

		IVideoStream SetBitstreamFilter(BitstreamFilter filter);

		IVideoStream SetLoop(int count, int delay = 0);

		IVideoStream SetOutputFramesCount(int number);

		IVideoStream SetSeek(TimeSpan seek);

		IVideoStream AddSubtitles(string subtitlePath, string encode = null, string style = null);

		IVideoStream AddSubtitles(string subtitlePath, VideoSize originalSize, string encode = null, string style = null);

		IVideoStream Split(TimeSpan startTime, TimeSpan duration);

		IVideoStream SetBitstreamFilter(string filter);
	}
}
