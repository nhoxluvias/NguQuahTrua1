using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.FFMPEG
{
    public class FFMPEGExecution
    {
        public void GetVideoInfo(string pathOrUrl)
        {
            string result = FFMPEGStatement.VideoInfo(pathOrUrl);

        }
    }
}
