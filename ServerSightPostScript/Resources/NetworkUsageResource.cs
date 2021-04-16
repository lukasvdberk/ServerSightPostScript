using System;
using System.Net.NetworkInformation;
using ServerSightAPI.Models.Server;

namespace ServerSightPostScript.Resources
{
    public class NetworkUsageResource : IResource
    {
        private NetworkUsage _lastNetworkUsage;
        public NetworkUsageResource()
        {
            _lastNetworkUsage = GetTotalUsage();
        }

        public object GetResource()
        {
            var currentNetworkUsage = GetTotalUsage();

            var usageOfPastMinute = new NetworkUsage(
                currentNetworkUsage.DownloadInBytes - _lastNetworkUsage.DownloadInBytes, 
                    currentNetworkUsage.UploadInBytes - _lastNetworkUsage.UploadInBytes
            );

            this._lastNetworkUsage = currentNetworkUsage; 
            return usageOfPastMinute;
        }

        public string GetRelativeEndpoint()
        {
            return "network-usages";
        }

        private NetworkUsage GetTotalUsage()
        {
                        
            if (!NetworkInterface.GetIsNetworkAvailable())
                return null;

            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

            double totalBytesSent = 0;
            double totalBytesReceived = 0;
            foreach (NetworkInterface ni in interfaces)
            {
                totalBytesReceived += ni.GetIPv4Statistics().BytesReceived;
                totalBytesSent += ni.GetIPv4Statistics().BytesSent;  
            }
            
            return new NetworkUsage(totalBytesReceived,totalBytesSent);
        }
    }
}