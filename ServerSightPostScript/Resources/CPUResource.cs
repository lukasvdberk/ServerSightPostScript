using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ServerSightPostScript.Resources
{
    public class CpuResource: IResource
    {
        public CpuResource()
        {
            new Thread( () => 
            {
                // Thread.CurrentThread.IsBackground = true;
                try
                {
                    var timer1 = new Timer(_ =>
                    {
                        var currentCpuUsage = GetCurrentCpuUsage();
                        Thread.Sleep(1000);       
                    }, null, 0, 1000);
                }
                catch (Exception ignored)
                {
                    Console.WriteLine(ignored.StackTrace);
                    // just to catch exception and still let the program run
                }
            }).Start();
        }
        public List<object> GetResource()
        {
            return null;
        }

        private double GetCurrentCpuUsage()
        {
            // TODO documente installment of mpstat sudo apt install sysstat
            // requires mpstat
            var escapedArgs = "mpstat -P ALL 1 1 -o JSON".Replace("\"", "\\\"");
            
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            var mpStatResultJObject = JObject.Parse(result);
            // mpStatResultJObject.SelectToken("$.sysstat.hosts[0].statistics.cpu-load.usr");
            var hosts = mpStatResultJObject.SelectToken("$.sysstat.hosts").Value<JArray>().ToList();

            var cpuUsages = new List<double>();
            foreach (var host in hosts)
            {
                foreach (var statistic in host.SelectToken("statistics").Value<JArray>().ToList())
                {
                    // there is 1 cpu with the already calculated average.
                    var cpuLoad = statistic.SelectToken("cpu-load").Value<JArray>().ToList()
                        .First(q => q.SelectToken("cpu").Value<string>().Equals("all"));
                    
                    cpuUsages.Add(cpuLoad.SelectToken("usr").Value<double>());
                }
            }
            return cpuUsages.Average();
        }
        public string GetRelativeEndpoint()
        {
            return "cpus";
        }
    }
}