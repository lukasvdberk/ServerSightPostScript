using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerSightAPI.Models.Server
{
    public class NetworkAdapter
    {
        public string AdapterName { get; set; }
        public string Ip { get; set; }

        public NetworkAdapter(string adapterName, string ip)
        {
            AdapterName = adapterName;
            Ip = ip;
        }
    }
}