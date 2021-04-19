using System.Collections.Generic;

namespace FFMPEG.Streams
{
    internal interface IFilterable
    {
        IEnumerable<IFilterConfiguration> GetFilters();
    }
}
