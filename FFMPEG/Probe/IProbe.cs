using System.Threading;
using System.Threading.Tasks;

namespace FFMPEG.Probe
{
    public interface IProbe
    {
        Task<string> Start(string args, CancellationToken cancellationToken = default(CancellationToken));
    }
}
