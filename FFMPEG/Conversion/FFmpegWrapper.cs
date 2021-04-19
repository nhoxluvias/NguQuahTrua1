using FFMPEG.Conversion.Events;
using FFMPEG.Conversion.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace FFMPEG.Conversion
{

	internal class FFmpegWrapper : FFmpeg
	{
		private const string TimeFormatPattern = "\\w\\w:\\w\\w:\\w\\w";

		private static readonly Regex s_timeFormatRegex = new Regex("\\w\\w:\\w\\w:\\w\\w", RegexOptions.Compiled);

		private List<string> _outputLog;

		private TimeSpan _totalTime;

		private bool _wasKilled;

		internal event ConversionProgressEventHandler OnProgress;

		internal event DataReceivedEventHandler OnDataReceived;

		internal Task<bool> RunProcess(string args, CancellationToken cancellationToken, ProcessPriorityClass? priority)
		{
			return Task.Factory.StartNew(delegate
			{
				_outputLog = new List<string>();
				Process process = RunProcess(args, base.FFmpegPath, priority, true, false, true);
				using (process)
				{
					process.ErrorDataReceived += delegate (object sender, DataReceivedEventArgs e)
					{
						ProcessOutputData(e, args, process.Id);
					};
					process.BeginErrorReadLine();
					using (cancellationToken.Register(delegate
					{
						if (Environment.OSVersion.Platform != PlatformID.Win32NT)
						{
							try
							{
								process.StandardInput.Write("q");
								Task.Delay(5000).GetAwaiter().GetResult();
							}
							catch (InvalidOperationException)
							{
							}
							finally
							{
								if (!process.HasExited)
								{
									process.CloseMainWindow();
									process.Kill();
									_wasKilled = true;
								}
							}
						}
					}))
					{
						using (ManualResetEvent manualResetEvent = new ManualResetEvent(false))
						{
							WaitHandleExtensions.SetSafeWaitHandle(manualResetEvent, new SafeWaitHandle(process.Handle, false));
							int num = WaitHandle.WaitAny(new WaitHandle[2]
							{
							manualResetEvent,
							cancellationToken.WaitHandle
							});
							if (num == 1 && !process.HasExited)
							{
								process.CloseMainWindow();
								process.Kill();
								_wasKilled = true;
							}
							else if (num == 0 && !process.HasExited)
							{
								process.WaitForExit();
							}
						}
						cancellationToken.ThrowIfCancellationRequested();
						if (_wasKilled)
						{
							throw new ConversionException("Cannot stop process. Killed it.", args);
						}
						if (cancellationToken.IsCancellationRequested)
						{
							return false;
						}
						string text = string.Join(Environment.NewLine, _outputLog.ToArray());
						new FFmpegExceptionCatcher().CatchFFmpegErrors(text, args);
						if (process.ExitCode != 0)
						{
							throw new ConversionException(text, args);
						}
					}
				}
				return true;
			}, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
		}

		private void ProcessOutputData(DataReceivedEventArgs e, string args, int processId)
		{
			if (e.Data != null)
			{
				DataReceivedEventHandler onDataReceived = this.OnDataReceived;
				if (onDataReceived != null)
				{
					onDataReceived(this, e);
				}
				_outputLog.Add(e.Data);
				if (this.OnProgress != null)
				{
					CalculateTime(e, args, processId);
				}
			}
		}

		private void CalculateTime(DataReceivedEventArgs e, string args, int processId)
		{
			if (e.Data.Contains("Duration: N/A"))
			{
				return;
			}
			if (e.Data.Contains("Duration"))
			{
				GetDuration(e, s_timeFormatRegex, args);
			}
			else if (e.Data.Contains("size"))
			{
				Match match = s_timeFormatRegex.Match(e.Data);
				if (match.Success)
				{
					this.OnProgress(this, new ConversionProgressEventArgs(TimeSpan.Parse(match.Value), _totalTime, processId));
				}
			}
		}

		private void GetDuration(DataReceivedEventArgs e, Regex regex, string args)
		{
			string argumentValue = GetArgumentValue("-t", args);
			if (!string.IsNullOrWhiteSpace(argumentValue))
			{
				_totalTime = TimeSpan.Parse(argumentValue);
				return;
			}
			Match match = regex.Match(e.Data);
			_totalTime = TimeSpan.Parse(match.Value);
			string argumentValue2 = GetArgumentValue("-ss", args);
			if (!string.IsNullOrWhiteSpace(argumentValue2))
			{
				_totalTime -= TimeSpan.Parse(argumentValue2);
			}
		}

		private string GetArgumentValue(string option, string args)
		{
			List<string> list = args.Split(new char[1]
			{
			' '
			}).ToList();
			int num = list.IndexOf(option);
			if (num >= 0)
			{
				return list[num + 1];
			}
			return string.Empty;
		}
	}
}
