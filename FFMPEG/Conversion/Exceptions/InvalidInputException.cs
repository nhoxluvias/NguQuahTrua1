using System.IO;

namespace FFMPEG.Conversion.Exceptions
{
	public class InvalidInputException : FileNotFoundException
	{
		public InvalidInputException(string msg)
			: base(msg)
		{
		}
	}
}
