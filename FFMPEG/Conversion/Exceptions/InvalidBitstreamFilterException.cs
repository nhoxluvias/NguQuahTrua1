
namespace FFMPEG.Conversion.Exceptions
{
	public class InvalidBitstreamFilterException : ConversionException
	{
		internal InvalidBitstreamFilterException(string errorMessage, string inputParameters)
			: base(errorMessage, inputParameters)
		{
		}
	}
}
