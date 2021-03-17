using System;
using System.Collections.Generic;
using System.IO;
using ServerSightAPI.Models.Server;

namespace ServerSightPostScript.Resources
{
    public class HardDiskResource : IResource
    {
        public List<object> GetResource()
        {
            List<object> harddisks = new List<object>();
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady && d.DriveType == DriveType.Fixed)
                {
                    harddisks.Add(new HardDisk(d.VolumeLabel, d.TotalFreeSpace, d.TotalSize));
                }
            }

            return harddisks;
        }

        public string GetRelativeEndpoint()
        {
            return "hard-disks";
        }
    }
}