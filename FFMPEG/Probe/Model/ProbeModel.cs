using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFMPEG.Probe.Model
{
	internal class ProbeModel
	{
		public class Stream
		{
			public string codec_name
			{
				get;
				set;
			}

			public int height
			{
				get;
				set;
			}

			public int width
			{
				get;
				set;
			}

			public string codec_type
			{
				get;
				set;
			}

			public string r_frame_rate
			{
				get;
				set;
			}

			public double duration
			{
				get;
				set;
			}

			public long bit_rate
			{
				get;
				set;
			}

			public int index
			{
				get;
				set;
			}

			public int channels
			{
				get;
				set;
			}

			public int sample_rate
			{
				get;
				set;
			}

			public string pix_fmt
			{
				get;
				set;
			}

			public Tags tags
			{
				get;
				set;
			}

			public string nb_frames
			{
				get;
				set;
			}

			public Disposition disposition
			{
				get;
				set;
			}
		}

		internal class Tags
		{
			public string language
			{
				get;
				set;
			}

			public string title
			{
				get;
				set;
			}

			public int? rotate
			{
				get;
				set;
			}
		}

		internal class Disposition
		{
			[JsonProperty("default")]
			public int _default
			{
				get;
				set;
			}

			public int forced
			{
				get;
				set;
			}
		}

		public Stream[] streams
		{
			get;
			set;
		}
	}
}
