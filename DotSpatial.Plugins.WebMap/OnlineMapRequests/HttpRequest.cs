using System.Collections;
using System.Net;

namespace DotSpatial.Plugins.WebMap.OnlineMapRequests
{
    internal abstract class HttpRequest
    {
        public HttpRequest(string endpointUrl)
        {
            this.EndpointUrl = endpointUrl;
        }

        public string EndpointUrl { get; set; }

        public Hashtable paramTable { get; set; }

        public abstract HttpWebResponse IssueRequest();
    }
}