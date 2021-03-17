using System.ComponentModel.DataAnnotations.Schema;

namespace ServerSightAPI.Models.Server
{
    public class PortServer
    {
        public int Port { get; set; }

        public PortServer(int port)
        {
            Port = port;
        }
    }
}