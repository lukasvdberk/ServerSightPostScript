using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ServerSightPostScript.Resources;

namespace ServerSightPostScript
{
    class Program
    {
        // TODO id en api key to .env or json file
        private static readonly string SERVER_ID = Configuration.GetServerId();
        private static readonly string API_KEY = Configuration.GetApiKey();
        
        static readonly string BASE_URL = "https://127.0.0.1:5001/api/";
        
        static void Main(string[] args)
        {
            List<IResource> resources = new List<IResource>()
            {
                new CpuResource(),
                new HardDiskResource(),
                new NetworkAdapterResource(),
                new PortResource(),
            };

            if (string.IsNullOrWhiteSpace(SERVER_ID) || string.IsNullOrWhiteSpace(API_KEY))
            {
                Console.WriteLine("You either did not supply a SERVER_SIGHT_API_KEY or SERVER_SIGHT_SERVER_ID key in your environment");
                return;
            }

            while (true)
            {
                foreach (var resource in resources)
                {
                    // Console.WriteLine(resource.GetResource());
                    // TODO put in a job that does it every minute
                    // PostResults(
                    //         string.Concat("servers/", SERVER_ID, "/", resource.GetRelativeEndpoint()),
                    //     resource.GetResource()
                    // ).Wait();
                    // Console.WriteLine("Done!");
                }   
            }
        }

        public static async Task PostResults(string endpoint, object data)
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

            var result = await client.PutAsync(endpoint, new StringContent(
                    JsonSerializer.Serialize(data),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            Console.WriteLine(await result.Content.ReadAsStringAsync());
            if (result.StatusCode != HttpStatusCode.NoContent)
            {
                throw new Exception("Data not saved");
            }
        }
    }
}