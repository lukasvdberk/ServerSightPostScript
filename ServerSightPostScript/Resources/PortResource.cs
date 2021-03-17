using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using ServerSightAPI.Models.Server;

namespace ServerSightPostScript.Resources
{
    public class PortResource: IResource
    {
        public List<object> GetResource()
        {
            List<object> ports = new List<object>();
            
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            var tcpListeners = properties.GetActiveTcpListeners();
            var udpListeners = properties.GetActiveUdpListeners();

            var tcpListenersFiltered = tcpListeners.Where(
                t => t.Address.ToString().Equals("127.0.0.1") || t.Address.ToString().Equals("::")
            );
            
            var udpListenersFiltered = udpListeners.Where(
                t => t.Address.ToString().Equals("127.0.0.1") || t.Address.ToString().Equals("::")
            );

            foreach (var tcpListener in tcpListenersFiltered)
            {
                ports.Add(new PortServer(tcpListener.Port));
            }
            
            foreach (var udpListener in udpListeners)
            {
                ports.Add(new PortServer(udpListener.Port));
            }
            return ports;
        }

        public string GetRelativeEndpoint()
        {
            return "ports";
        }
    }
}