using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Common.SystemInformation
{
    public class SystemInfo
    {
        public string OSName { get; }
        public string MemoryUsed { get; }
        public string TotalMemory { get; set; }
        public string IpAddress { get; set; }
        public string ExtenalIpAddress { get; set; }
        public string FfmpegPath { get; set; }
        public string[] Path_EnvironmentVariable { get; set; }

        public SystemInfo()
        {
            OSName = GetOSName();
            MemoryUsed = GetMemoryUsed(MemorySizeInfo.MB) + " MB";
            TotalMemory = GetTotalMemory(MemorySizeInfo.MB) + " MB";
            IpAddress = GetIPAddress();
            ExtenalIpAddress = GetExtenalIPAddress();
            string ffmpegPath = GetFfmpegPath();
            FfmpegPath = (ffmpegPath == null) ? "null" : ffmpegPath;
            Path_EnvironmentVariable = GetPath_EnvironmentVariable();
        }

        public enum MemorySizeInfo { Byte, KB, MB, GB };

        private double GetMemoryUsed(MemorySizeInfo memorySizeInfo = MemorySizeInfo.MB)
        {
            Process process = Process.GetCurrentProcess();
            double memory = process.PrivateMemorySize64;
            process.Dispose();
            process = null;
            if (memorySizeInfo == MemorySizeInfo.Byte)
                return memory;
            else if (memorySizeInfo == MemorySizeInfo.KB)
                return memory / (double)1024;
            else if (memorySizeInfo == MemorySizeInfo.MB)
                return memory / (double)(1024 * 1024);
            else
                return memory / (double)(1024 * 1024 * 1024);
        }

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetPhysicallyInstalledSystemMemory(out long TotalMemoryInKilobytes);

        private double GetTotalMemory(MemorySizeInfo memorySizeInfo = MemorySizeInfo.MB)
        {
            long memory;
            GetPhysicallyInstalledSystemMemory(out memory);
            if (memorySizeInfo == MemorySizeInfo.Byte)
                return memory * 1024;
            else if (memorySizeInfo == MemorySizeInfo.KB)
                return memory;
            else if (memorySizeInfo == MemorySizeInfo.MB)
                return memory / (double)1024;
            else
                return memory / (double)(1024 * 1024);
        }

        private string GetOSName()
        {
            return System.Environment.OSVersion.VersionString;
        }

        private string GetFfmpegPath()
        {
            return System.Environment.GetEnvironmentVariable("ffmpeg");
        }

        private string[] GetPath_EnvironmentVariable()
        {
            return System.Environment.GetEnvironmentVariable("PATH").Split(';');
        }

        private string GetIPAddress()
        {
            string IPAddress = string.Empty;
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                }
            }
            return IPAddress;
        }

        private string GetExtenalIPAddress()
        {
            WebClient webClient = new WebClient();
            string raw = webClient.DownloadString("http://checkip.dyndns.org");
            webClient.Dispose();
            webClient = null;
            return Regex.Replace(raw, "<.*?>", String.Empty)
                .Replace("Current IP Check", String.Empty)
                .Replace("Current IP Address: ", String.Empty);
        }
    }
}
