using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Command
{
    public class Cmd
    {
        public static string Run(string cmdText)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            //p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = cmdText;
            p.Start();

            //p.StandardInput.WriteLine(cmdText);
            //p.StandardInput.Flush();
            //p.StandardInput.Close();

            
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            p.Close();

            return output;
        }
    }
}
