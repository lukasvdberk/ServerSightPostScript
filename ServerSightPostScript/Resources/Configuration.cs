using System.Collections.Generic;
using System.IO;

namespace ServerSightPostScript.Resources
{
    public class Configuration
    {
        public static string GetApiKey()
        {
            return System.Environment.GetEnvironmentVariable("SERVER_SIGHT_API_KEY");
        }

        public static string GetServerId()
        {
            return System.Environment.GetEnvironmentVariable("SERVER_SIGHT_SERVER_ID");            
        }
    }
}