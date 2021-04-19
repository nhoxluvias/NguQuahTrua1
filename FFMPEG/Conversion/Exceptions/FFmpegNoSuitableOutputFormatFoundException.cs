
namespace FFMPEG.Conversion.Exceptions
{
	public class FFmpegNoSuitableOutputFormatFoundException : ConversionException
	{
		internal FFmpegNoSuitableOutputFormatFoundException(string errorMessage, string inputParameters)
			: base(errorMessage, inputParameters)
		{
		}
	}
}
