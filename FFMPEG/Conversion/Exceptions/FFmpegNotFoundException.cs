using System.IO;

namespace FFMPEG.Conversion.Exceptions
{
	public class FFmpegNotFoundException : FileNotFoundException
	{
		internal FFmpegNotFoundException(string errorMessage)
			: base(errorMessage)
		{
		}
	}
}
