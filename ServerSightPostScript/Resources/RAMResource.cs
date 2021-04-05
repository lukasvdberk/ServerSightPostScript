using System;
using System.Diagnostics;
using ServerSightAPI.Models.Server;

namespace ServerSightPostScript.Resources
{
    public class RamResource : IResource
    {
        public object GetResource()
        {
            // https://dzone.com/articles/system-memory-health-check-for-aspnet-core
            var output = "";         
            var info = new ProcessStartInfo("free -m");        
            info.FileName = "/bin/bash";        
            info.Arguments = "-c \"free -b\"";        
            info.RedirectStandardOutput = true;         

            using (var process = Process.Start(info))        
            {            
                output = process.StandardOutput.ReadToEnd();            
            }         

            var lines = output.Split("\n");        
            var memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            
            double usedMemory = double.Parse(memory[2]);
            double totalMemory = double.Parse(memory[1]);

            return new RamUsage(usedMemory,totalMemory);
        }

        public string GetRelativeEndpoint()
        {
            return "ram";
        }
    }
}