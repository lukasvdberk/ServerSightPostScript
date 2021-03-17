using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using ServerSightAPI.Models.Server;

namespace ServerSightPostScript.Resources
{
    public class NetworkAdapterResource: IResource
    {
        public List<object> GetResource()
        {
            List<object> networkAdapters = new List<object>();
            foreach(NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                try
                {
                    string ip = ni.GetIPProperties().UnicastAddresses[0].Address.ToString();
                    networkAdapters.Add(new NetworkAdapter(ni.Name, ip));
                }
                catch
                {
                    continue;
                }
            }

            return networkAdapters.ToList();   
        }

        public string GetRelativeEndpoint()
        {
            return "ips";
        }
    }
}