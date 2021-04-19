using System;

namespace FFMPEG.Conversion.Exceptions
{
	public class ConversionException : Exception
	{
		public string InputParameters
		{
			get;
		}

		public ConversionException(string message, Exception innerException, string inputParameters)
			: base(message, innerException)
		{
			InputParameters = inputParameters;
		}

		internal ConversionException(string errorMessage, string inputParameters)
			: base(errorMessage)
		{
			InputParameters = inputParameters;
		}
	}
}
