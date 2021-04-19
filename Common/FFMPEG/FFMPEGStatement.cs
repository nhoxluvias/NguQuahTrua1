using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.FFMPEG
{
    public class FFMPEGStatement
    {
        public static string VideoInfo(string pathOrUrl)
        {
            return "ffprobe -v error -print_format json -show_streams " + pathOrUrl;
        }

        public static string PictureToVideo()
        {
            return null;
        }
    }
}
