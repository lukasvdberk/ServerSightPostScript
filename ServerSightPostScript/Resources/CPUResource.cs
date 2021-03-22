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
                    while (true)
                    {
                        var currentCpuUsage = GetCurrentCpuUsage();
                        Console.WriteLine(currentCpuUsage);
                        Thread.Sleep(1000);
                    }
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
            var escapedArgs = "mpstat -P ALL -o JSON".Replace("\"", "\\\"");
            
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
                    foreach (var cpuLoad in statistic.SelectToken("cpu-load").Value<JArray>().ToList())
                    {
                        Console.WriteLine(cpuLoad.SelectToken("usr").Value<double>());
                        cpuUsages.Add(cpuLoad.SelectToken("usr").Value<double>());
                    }
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