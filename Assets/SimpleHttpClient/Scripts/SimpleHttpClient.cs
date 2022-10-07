using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace CoolishHttp
{
    public enum Method
    {
        GET,
        POST
    }

    public class SimpleHttpClient
    {        
        public static IHttpRequest Get(string uri)
        {
            return new HttpRequestImpl(UnityWebRequest.Get(uri));
        }

        public static IHttpRequest PostJson(string uri, string json)
        {
            Debug.Log(json);
            return new HttpRequestImpl(uri,
                                       Encoding.UTF8.GetBytes(json),
                                       "application/json");
        }
    }
}