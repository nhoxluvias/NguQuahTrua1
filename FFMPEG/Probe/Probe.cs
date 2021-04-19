using System.Threading;
using System.Threading.Tasks;

namespace FFMPEG.Probe
{
	public class Probe : IProbe
	{
		public static IProbe New()
		{
			return new Probe();
		}

		public Task<string> Start(string args, CancellationToken cancellationToken = default(CancellationToken))
		{
			return new FFprobeWrapper().Start(args, cancellationToken);
		}
	}
}
