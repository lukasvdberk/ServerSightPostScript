using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            List<double> cpuUsages = new List<double>();

            var processorCount = Environment.ProcessorCount;
            foreach (var process in Process.GetProcesses())
            {
                var startTime = DateTime.Now;
                var startCpuUsage = process.TotalProcessorTime;
                var endTime = DateTime.UtcNow;
                var endCpuUsage = process.TotalProcessorTime;

                var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
                var totalMsPassed = (endTime - startTime).TotalMilliseconds;
                var cpuUsageTotal = cpuUsedMs / (processorCount * totalMsPassed);
                cpuUsages.Add(cpuUsageTotal * 100);
            }

            return cpuUsages.Average();
        }
        public string GetRelativeEndpoint()
        {
            return "cpus";
        }
    }
}