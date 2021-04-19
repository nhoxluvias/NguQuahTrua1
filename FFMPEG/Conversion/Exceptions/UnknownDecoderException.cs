
namespace FFMPEG.Conversion.Exceptions
{
	public class UnknownDecoderException : ConversionException
	{
		internal UnknownDecoderException(string errorMessage, string inputParameters)
			: base(errorMessage, inputParameters)
		{
		}
	}
}
