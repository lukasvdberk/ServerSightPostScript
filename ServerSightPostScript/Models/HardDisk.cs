using System.ComponentModel.DataAnnotations.Schema;

namespace ServerSightAPI.Models.Server
{
    public class HardDisk
    {
        public string DiskName { get; set; }
        public float SpaceAvailable { get; set; }
        public float SpaceTotal { get; set; }

        public HardDisk(string diskName, float spaceAvailable, float spaceTotal)
        {
            DiskName = diskName;
            SpaceAvailable = spaceAvailable;
            SpaceTotal = spaceTotal;
        }
    }
}