using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFMPEG.Conversion
{
	public class InputBuilder : IInputBuilder
	{
		public List<FileInfo> FileList
		{
			get;
		}

		public InputBuilder()
		{
			FileList = new List<FileInfo>();
		}

		public Func<string, string> PrepareInputFiles(List<string> files, out string directory)
		{
			Guid guid = Guid.NewGuid();
			for (int i = 0; i < files.Count; i++)
			{
				string text = Path.Combine(Path.GetTempPath(), guid.ToString(), BuildFileName(i + 1, Path.GetExtension(files[i])));
				if (!Directory.Exists(Path.Combine(Path.GetTempPath(), guid.ToString())))
				{
					Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), guid.ToString()));
				}
				File.Copy(files[i], text);
				FileList.Add(new FileInfo(text));
			}
			directory = Path.Combine(Path.GetTempPath(), guid.ToString());
			return (string number) => " -i " + Path.Combine(FileList[0].DirectoryName, "img" + number + FileList[0].Extension) + " ";
		}

		private string BuildFileName(int fileIndex, string extension)
		{
			string text = "img_";
			if (fileIndex < 10)
			{
				return text + string.Format("00{0}", fileIndex) + extension;
			}
			if (fileIndex < 100)
			{
				return text + string.Format("0{0}", fileIndex) + extension;
			}
			return text + string.Format("{0}", fileIndex) + extension;
		}
	}
}
