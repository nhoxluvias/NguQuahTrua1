using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FFMPEG;

namespace FFMPEG.Extensions
{
	public static class StreamExtensions
	{
		public static async Task CopyToAsync(this Stream source, Stream destination, long contentLength, int bufferSize, IProgress<ProgressInfo> progress = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (!source.CanRead)
			{
				throw new ArgumentException("Has to be readable", "source");
			}
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			if (!destination.CanWrite)
			{
				throw new ArgumentException("Has to be writable", "destination");
			}
			if (bufferSize < 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize");
			}
			byte[] buffer = new byte[bufferSize];
			long totalBytesRead = 0L;
			while (true)
			{
				int num;
				int bytesRead = (num = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false));
				if (num == 0)
				{
					break;
				}
				await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);
				totalBytesRead += bytesRead;
				if (progress != null)
				{
					progress.Report(new ProgressInfo(totalBytesRead, contentLength));
				}
			}
		}
	}
}
