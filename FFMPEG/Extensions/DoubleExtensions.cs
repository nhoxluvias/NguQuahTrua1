using System.Globalization;

namespace FFMPEG
{
	public static class DoubleExtensions
	{
		public static string ToFFmpegFormat(this double number, int decimalPlaces = 1)
		{
			return string.Format(CultureInfo.GetCultureInfo("en-US"), string.Format("{{0:N{0}}}", decimalPlaces), number);
		}
	}
}
