namespace ServerSightAPI.Models.Server
{
    public class CpuUsage
    {
        public double AverageCpuUsagePastMinute { get; set; }

        public CpuUsage(double averageCpuUsagePastMinute)
        {
            AverageCpuUsagePastMinute = averageCpuUsagePastMinute;
        }
    }
}