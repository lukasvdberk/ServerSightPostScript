using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerSightPostScript.Resources
{
    public interface IResource
    {
        // should return object it wants to post
        public List<object> GetResource();
        public string GetRelativeEndpoint();
    }
}