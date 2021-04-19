using System;

namespace FFMPEG.Streams.AudioStream
{
	public interface IAudioStream : IStream
	{
		TimeSpan Duration
		{
			get;
		}

		long Bitrate
		{
			get;
		}

		int SampleRate
		{
			get;
		}

		int Channels
		{
			get;
		}

		string Language
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

		IAudioStream CopyStream();

		IAudioStream Reverse();

		IAudioStream SetChannels(int channels);

		IAudioStream SetCodec(AudioCodec codec);

		IAudioStream SetCodec(string codec);

		IAudioStream SetBitstreamFilter(BitstreamFilter filter);

		IAudioStream SetBitrate(long bitRate);

		IAudioStream SetBitrate(long minBitrate, long maxBitrate, long bufferSize);

		IAudioStream SetSampleRate(int sampleRate);

		IAudioStream ChangeSpeed(double multiplicator);

		IAudioStream Split(TimeSpan startTime, TimeSpan duration);

		IAudioStream SetSeek(TimeSpan? seek);

		IAudioStream SetBitstreamFilter(string filter);
	}
}
