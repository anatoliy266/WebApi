using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebApi_client
{
    public interface IWebHooker
    {
        string SendRequest(string host, int port, string target, string method);
    }


    public class WebHooker : IWebHooker
    {
        public string SendRequest(string host, int port, string target, string method)
        {
            WebClient client = new WebClient();
            client.Headers.Add(HttpRequestHeader.ContentType, "Application/json");
            client.Headers.Add(HttpRequestHeader.Allow, method);
            client.Headers.Add(HttpRequestHeader.Host, host+":"+port+"\\"+target);
            return client.DownloadString("https://localhost:44343/api/Orders");
        }
    }
}
