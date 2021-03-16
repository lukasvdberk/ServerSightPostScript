using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerSightAPI.Models.Server
{
    public class NetworkAdapterServer
    {
        public string AdapterName { get; set; }
        public string Ip { get; set; }
    }
}