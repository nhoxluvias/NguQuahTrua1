using FFMPEG.Conversion.Enums;

namespace FFMPEG.Conversion.Models
{
	internal class ConversionParameter
	{
		public string Parameter
		{
			get;
			set;
		}

		public ParameterPosition Position
		{
			get;
			set;
		} = ParameterPosition.PostInput;

	}
}
