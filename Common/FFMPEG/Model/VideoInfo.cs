using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.FFMPEG.Model
{
    public class VideoInfo
    {
        public int Index { get; set; }
        public string CodecName { get; set; }
        public string CodecLongName { get; set; }
        public string Profile { get; set; }
        public string CodecType { get; set; }
        public string CodecTimeBase { get; set; }
        public string CodecTagString { get; set; }
        public string CodecTag { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int CodeWidth { get; set; }
        public int CodeHeight { get; set; }
        public int HasBFrames { get; set; }
        public string SampleAspectRatio { get; set; }
        public string DisplayAspectRation { get; set; }
        public string PixFmt { get; set; }
        public int Level { get; set; }
        public string ColorRange { get; set; }
        public string ColorSpace { get; set; }
        public string ColorTransfer { get; set; }
        public string ColorPrimaries { get; set; }
        public string ChromaLocation { get; set; }
        public int Refs { get; set; }
        public bool IsAvc { get; set; }
        public string NalLengthSize { get; set; }

    }
}
