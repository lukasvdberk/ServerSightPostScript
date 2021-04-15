using System;
using System.Net.NetworkInformation;
using ServerSightAPI.Models.Server;

namespace ServerSightPostScript.Resources
{
    public class NetworkUsageResource : IResource
    {
        public object GetResource()
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

        public string GetRelativeEndpoint()
        {
            return "network-usage";
        }
    }
}