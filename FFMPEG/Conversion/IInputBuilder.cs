using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFMPEG.Conversion
{
	public interface IInputBuilder
	{
		List<FileInfo> FileList
		{
			get;
		}

		Func<string, string> PrepareInputFiles(List<string> files, out string directory);
	}
}
