using System;

namespace FFMPEG.Streams.VideoStream
{
	public enum Flag
	{
		mv4,
		qpel,
		loop,
		qscale,
		pass1,
		pass2,
		gray,
		emu_edge,
		psnr,
		truncated,
		drop_changed,
		ildct,
		low_delay,
		global_header,
		bitexact,
		aic,
		[Obsolete]
		cbp,
		[Obsolete]
		qprd,
		ilme,
		cgop,
		output_corrupt
	}
}
