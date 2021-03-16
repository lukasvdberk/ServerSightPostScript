using System.ComponentModel.DataAnnotations.Schema;

namespace ServerSightAPI.Models.Server
{
    public class HardDiskServer
    {
        public string DiskName { get; set; }
        public float SpaceAvailable { get; set; }
        public float SpaceTotal { get; set; }
    }
}