using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ServerSightAPI.Models.Server;
using ServerSightPostScript.Resources;

namespace ServerSightPostScript
{
    class Program
    {
        // TODO id en api key to .env or json file
        private static readonly string SERVER_ID = Configuration.GetServerId();
        private static readonly string API_KEY = Configuration.GetApiKey();
        
        // TODO change back to main host url
        static readonly string BASE_URL = "https://serversight.lukas.sh/api/";
        
        static void Main(string[] args)
        {
            new NetworkUsageResource().GetResource();
            List<IResource> resources = new List<IResource>()
            {
                new CpuResource(),
                new RamResource(),
                new NetworkUsageResource(),
                new HardDiskResource(),
                new NetworkAdapterResource(),
                new PortResource(),
            };

            if (string.IsNullOrWhiteSpace(SERVER_ID) || string.IsNullOrWhiteSpace(API_KEY))
            {
                Console.WriteLine("You either did not supply a SERVER_SIGHT_API_KEY or SERVER_SIGHT_SERVER_ID key in your environment");
                return;
            }
            
            // interval every minute and exactly starts at the minute.
            var timer = new System.Threading.Timer(async (e) =>
            {
                try
                {
                    Console.WriteLine($"Posted new results at {DateTime.Now}");
                    foreach (var resource in resources)
                    {
                        // cpu, networkusage and ram resource use post method
                        var httpMethod = ReferenceEquals(typeof(CpuResource), resource.GetType()) || 
                                         ReferenceEquals(typeof(NetworkUsageResource), resource.GetType()) || 
                                         ReferenceEquals(typeof(RamResource), resource.GetType())
                            ? HttpMethod.Post
                            : HttpMethod.Put;
                        await PostResults(
                            string.Concat("servers/", SERVER_ID, "/", resource.GetRelativeEndpoint()),
                            httpMethod,
                            resource.GetResource() 
                        );
                    }
                }
                catch(Exception exception)
                {
                    // TODO setup a logger of some sort
                    Console.WriteLine(exception.StackTrace);
                }
            }, null, GetStartTime(), 60000);
            while (true)
            {
                Thread.Sleep(60000);
            }
        }

        public static async Task PostResults(string endpoint, HttpMethod httpMethod, object data)
        {
            // for untrusted certificates (like development)
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            
            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("X-Api-Key", API_KEY);
            client.BaseAddress = new Uri(BASE_URL);
            
            var request = new HttpRequestMessage(httpMethod, endpoint)
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(data),
                    Encoding.UTF8,
                    "application/json"
                )
            };

            var result = await client.SendAsync(request);
            if ( result.StatusCode != HttpStatusCode.OK && result.StatusCode != HttpStatusCode.NoContent)
            {
                // TODO use logger of some sorts
                Console.WriteLine($"Http request failed for {endpoint} {httpMethod} {data} api key {API_KEY} server id {SERVER_ID} content {result.Content}");
            }
        }

        private static int GetStartTime()
        {
            // so it starts exactly at the beginning of the minute
            return (60 - DateTime.Now.Second) * 1000;
        }
    }
}