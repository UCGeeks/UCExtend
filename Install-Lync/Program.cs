using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace Install_Lync
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get normal filepath of this assembly's permanent directory
            var apppath = new Uri(
                    System.IO.Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
                ).LocalPath;

            //Console.WriteLine(apppath + @"\Lync2013_Client_x86_x64\");
            //Console.WriteLine(@"setup.exe /adminfile " + apppath + @"\Lync2013_Client_x86_x64\LexelLyncOnlineSetup.MSP");

            Process process = new System.Diagnostics.Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();          
            startInfo.UseShellExecute = true;
            startInfo.WorkingDirectory = apppath + @"\Install_Lync2013Client";
            startInfo.FileName = "cmd.exe";
            //startInfo.Verb = "runas";
            startInfo.Arguments = @"/c " + @"OCT_Lync2013Client.MSP";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;            
            process.StartInfo = startInfo;
            process.Start();
            //process.WaitForExit();
        }
    }
}
