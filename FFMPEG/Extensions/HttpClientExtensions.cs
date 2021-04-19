using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FFMPEG;
using FFMPEG.Extensions;

namespace FFMPEG.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task DownloadAsync
        (
            this HttpClient client,
            string requestUri,
            Stream destination,
            IProgress<ProgressInfo> progress = null,
            CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            using (HttpResponseMessage response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                long? contentLength = response.Content.Headers.ContentLength;
                using (Stream download = await response.Content.ReadAsStreamAsync())
                {
                    if (progress == null || !contentLength.HasValue)
                    {
                        await download.CopyToAsync(destination);
                        return;
                    }
                    Progress<ProgressInfo> progress2 = new Progress<ProgressInfo>(delegate (ProgressInfo totalBytes)
                    {
                        progress.Report(totalBytes);
                    });
                    await download.CopyToAsync(destination, contentLength.Value, 81920, progress2, cancellationToken);
                    progress.Report(new ProgressInfo(1L, 1L));
                }
            }
        }
    }
}