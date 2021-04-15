namespace ServerSightAPI.Models.Server
{
    public class NetworkUsage
    {
        public double DownloadInBytes { get; set; }
        public double UploadInBytes { get; set; }

        public NetworkUsage(double downloadInBytes, double uploadInBytes)
        {
            DownloadInBytes = downloadInBytes;
            UploadInBytes = uploadInBytes;
        }
    }
}