
namespace FFMPEG.Conversion.Exceptions
{
	public class HardwareAcceleratorNotFoundException : ConversionException
	{
		internal HardwareAcceleratorNotFoundException(string errorMessage, string inputParameters)
			: base(errorMessage, inputParameters)
		{
		}
	}
}
