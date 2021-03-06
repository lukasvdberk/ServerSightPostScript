using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using Newtonsoft.Json.Linq;
using ServerSightAPI.Models.Server;

namespace ServerSightPostScript.Resources
{
    public class CpuResource: IResource
    {
        private Timer cpuInterval;
        private static List<double> _pastMinuteCpuUsage = new List<double>();
        public CpuResource()
        {
            InitializeCPUUsage();
        }

        private void AddedCpuUsage(object sender, ElapsedEventArgs e)
        {
            try
            {
                var currentCpuUsage = GetCurrentCpuUsage();
                _pastMinuteCpuUsage.Add(currentCpuUsage);
            }
            catch (Exception ignored)
            {
                Console.WriteLine(ignored.StackTrace);
            }
        }

        private void InitializeCPUUsage()
        {
            cpuInterval = new Timer();
            cpuInterval.Elapsed += AddedCpuUsage;
            cpuInterval.Interval = 1000;
            cpuInterval.Enabled = true;
        }
        public object GetResource()
        {
            var pastMinuteUsage = new List<double>(_pastMinuteCpuUsage);
            _pastMinuteCpuUsage.Clear();
            return new CpuUsage(pastMinuteUsage.Average());
        }


        private static double GetCurrentCpuUsage()
        {
            // requires mpstat
            // uses interval of 1 because else it will not fetch the right usage
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
                },
            };
            process.Start();
            process.WaitForExit();
            string result = process.StandardOutput.ReadToEnd();
        
            process.StandardOutput.Close();
            process.Kill();
        
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