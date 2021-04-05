namespace ServerSightAPI.Models.Server
{
    public class RamUsage
    {
        public double UsageInBytes { get; set; }
        public double TotalAvailableInBytes { get; set; }

        public RamUsage(double usageInBytes, double totalAvailableInBytes)
        {
            UsageInBytes = usageInBytes;
            TotalAvailableInBytes = totalAvailableInBytes;
        }
    }
}