using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFMPEG.Conversion.Exceptions
{
	internal class FFmpegExceptionCatcher
	{
		private static Dictionary<ExceptionCheck, Action<string, string>> Checks;

		static FFmpegExceptionCatcher()
		{
			Checks = new Dictionary<ExceptionCheck, Action<string, string>>();
			Checks.Add(new ExceptionCheck("Invalid NAL unit size"), delegate (string output, string args)
			{
				throw new ConversionException(output, args);
			});
			Checks.Add(new ExceptionCheck("Packet mismatch", true), delegate (string output, string args)
			{
				throw new ConversionException(output, args);
			});
			Checks.Add(new ExceptionCheck("asf_read_pts failed", true), delegate (string output, string args)
			{
				throw new UnknownDecoderException(output, args);
			});
			Checks.Add(new ExceptionCheck("Missing key frame while searching for timestamp", true), delegate (string output, string args)
			{
				throw new UnknownDecoderException(output, args);
			});
			Checks.Add(new ExceptionCheck("Old interlaced mode is not supported", true), delegate (string output, string args)
			{
				throw new UnknownDecoderException(output, args);
			});
			Checks.Add(new ExceptionCheck("mpeg1video", true), delegate (string output, string args)
			{
				throw new UnknownDecoderException(output, args);
			});
			Checks.Add(new ExceptionCheck("Frame rate very high for a muxer not efficiently supporting it", true), delegate (string output, string args)
			{
				throw new UnknownDecoderException(output, args);
			});
			Checks.Add(new ExceptionCheck("multiple fourcc not supported"), delegate (string output, string args)
			{
				throw new UnknownDecoderException(output, args);
			});
			Checks.Add(new ExceptionCheck("Unknown decoder"), delegate (string output, string args)
			{
				throw new UnknownDecoderException(output, args);
			});
			Checks.Add(new ExceptionCheck("Failed to open codec in avformat_find_stream_info"), delegate (string output, string args)
			{
				throw new UnknownDecoderException(output, args);
			});
			Checks.Add(new ExceptionCheck("Unrecognized hwaccel: "), delegate (string output, string args)
			{
				throw new HardwareAcceleratorNotFoundException(output, args);
			});
			Checks.Add(new ExceptionCheck("Unable to find a suitable output format"), delegate (string output, string args)
			{
				throw new FFmpegNoSuitableOutputFormatFoundException(output, args);
			});
			Checks.Add(new ExceptionCheck("is not supported by the bitstream filter"), delegate (string output, string args)
			{
				throw new InvalidBitstreamFilterException(output, args);
			});
		}

		internal void CatchFFmpegErrors(string output, string args)
		{
			foreach (KeyValuePair<ExceptionCheck, Action<string, string>> check in Checks)
			{
				try
				{
					if (check.Key.CheckLog(output))
					{
						check.Value(output, args);
					}
				}
				catch (ConversionException ex)
				{
					throw new ConversionException(ex.Message, ex, ex.InputParameters);
				}
			}
		}
	}
}
