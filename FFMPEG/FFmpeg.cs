using FFMPEG.Conversion.Exceptions;
using FFMPEG.Probe.MediaInfo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace FFMPEG
{
	public abstract class FFmpeg
	{
		private static string _ffmpegPath;

		private static string _ffprobePath;

		private static string _lastExecutablePath = Guid.NewGuid().ToString();

		private static readonly object _ffmpegPathLock = new object();

		private static readonly object _ffprobePathLock = new object();

		private static string _ffmpegExecutableName = "ffmpeg";

		private static string _ffprobeExecutableName = "ffprobe";

		public static Conversion.Conversion Conversions = new Conversion.Conversion();

		protected string FFmpegPath
		{
			get
			{
				lock (_ffmpegPathLock)
				{
					return _ffmpegPath;
				}
			}
			private set
			{
				lock (_ffmpegPathLock)
				{
					_ffmpegPath = value;
				}
			}
		}

		protected string FFprobePath
		{
			get
			{
				lock (_ffprobePathLock)
				{
					return _ffprobePath;
				}
			}
			private set
			{
				lock (_ffprobePathLock)
				{
					_ffprobePath = value;
				}
			}
		}

		public static string ExecutablesPath
		{
			get;
			private set;
		}

		protected FFmpeg()
		{
			FindAndValidateExecutables();
		}

		private void FindAndValidateExecutables()
		{
			if (!string.IsNullOrWhiteSpace(FFprobePath) && !string.IsNullOrWhiteSpace(FFmpegPath) && _lastExecutablePath.Equals(ExecutablesPath))
			{
				return;
			}
			if (!string.IsNullOrWhiteSpace(ExecutablesPath))
			{
				FileInfo fileInfo = new DirectoryInfo(ExecutablesPath).GetFiles().FirstOrDefault((FileInfo x) => x.Name.ToLower().Contains(_ffprobeExecutableName.ToLower()));
				FFprobePath = ((fileInfo != null) ? fileInfo.FullName : null);
				FileInfo fileInfo2 = new DirectoryInfo(ExecutablesPath).GetFiles().FirstOrDefault((FileInfo x) => x.Name.ToLower().Contains(_ffmpegExecutableName.ToLower()));
				FFmpegPath = ((fileInfo2 != null) ? fileInfo2.FullName : null);
				ValidateExecutables();
				_lastExecutablePath = ExecutablesPath;
				return;
			}
			Assembly entryAssembly = Assembly.GetEntryAssembly();
			if (entryAssembly != null)
			{
				string directoryName = Path.GetDirectoryName(entryAssembly.Location);
				FindProgramsFromPath(directoryName);
				if (FFmpegPath != null && FFprobePath != null)
				{
					return;
				}
			}
			string[] array = Environment.GetEnvironmentVariable("PATH").Split(new char[1]
			{
			Path.PathSeparator
			});
			foreach (string path in array)
			{
				FindProgramsFromPath(path);
				if (FFmpegPath != null && FFprobePath != null)
				{
					break;
				}
			}
			ValidateExecutables();
		}

		private void ValidateExecutables()
		{
			if (FFmpegPath != null && FFprobePath != null)
			{
				return;
			}
			string text = (string.IsNullOrWhiteSpace(ExecutablesPath) ? string.Empty : string.Format(ExecutablesPath + " or "));
			throw new FFmpegNotFoundException("Cannot find FFmpeg in " + text + "PATH. This package needs installed FFmpeg. Please add it to your PATH variable or specify path to DIRECTORY with FFmpeg executables in FFmpeg.ExecutablesPath");
		}

		private void FindProgramsFromPath(string path)
		{
			if (Directory.Exists(path))
			{
				IEnumerable<FileInfo> files = new DirectoryInfo(path).GetFiles();
				FFprobePath = GetFullName(files, _ffprobeExecutableName);
				FFmpegPath = GetFullName(files, _ffmpegExecutableName);
			}
		}

		internal static string GetFullName(IEnumerable<FileInfo> files, string fileName)
		{
			FileInfo fileInfo = files.FirstOrDefault((FileInfo x) => x.Name.Equals(fileName, StringComparison.InvariantCultureIgnoreCase) || x.Name.Equals(fileName + ".exe", StringComparison.InvariantCultureIgnoreCase));
			if (fileInfo == null)
			{
				return null;
			}
			return fileInfo.FullName;
		}

		protected Process RunProcess(string args, string processPath, ProcessPriorityClass? priority, bool standardInput = false, bool standardOutput = false, bool standardError = false)
		{
			Process process = new Process
			{
				StartInfo =
			{
				FileName = processPath,
				Arguments = args,
				UseShellExecute = false,
				CreateNoWindow = true,
				RedirectStandardInput = standardInput,
				RedirectStandardOutput = standardOutput,
				RedirectStandardError = standardError
			},
				EnableRaisingEvents = true
			};
			process.Start();
			try
			{
				if (priority.HasValue)
				{
					process.PriorityClass = priority.Value;
					return process;
				}
				process.PriorityClass = Process.GetCurrentProcess().PriorityClass;
				return process;
			}
			catch (Exception)
			{
				return process;
			}
		}

		public static async Task<IMediaInfo> GetMediaInfo(string fileName)
		{
			return await MediaInfo.Get(fileName);
		}

		public static async Task<IMediaInfo> GetMediaInfo(string fileName, CancellationToken token)
		{
			return await MediaInfo.Get(fileName, token);
		}

		public static void SetExecutablesPath(string directoryWithFFmpegAndFFprobe, string ffmpegExeutableName = "ffmpeg", string ffprobeExecutableName = "ffprobe")
		{
			ExecutablesPath = ((directoryWithFFmpegAndFFprobe == null) ? null : new DirectoryInfo(directoryWithFFmpegAndFFprobe).FullName);
			_ffmpegExecutableName = ffmpegExeutableName;
			_ffprobeExecutableName = ffprobeExecutableName;
		}
	}
}
