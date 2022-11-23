using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace CoolishHttp
{
    public class SimpleHttpClient
    {        
        public static IHttpRequest Get(string uri)
        {
            return new HttpRequestImpl(UnityWebRequest.Get(uri));
        }

        public static IHttpRequest GetTexture(string uri)
        {
            return new HttpRequestImpl(UnityWebRequestTexture.GetTexture(uri));
        }

        public static IHttpRequest Post(string uri, WWWForm formData)
        {
            return new HttpRequestImpl(UnityWebRequest.Post(uri, formData));
        }

        public static IHttpRequest Post(string uri, Dictionary<string, string> formData)
        {
            return new HttpRequestImpl(UnityWebRequest.Post(uri, formData));
        }

        public static IHttpRequest Post(string uri, List<IMultipartFormSection> multipartForms)
        {
            return new HttpRequestImpl(UnityWebRequest.Post(uri, multipartForms));
        }

        public static IHttpRequest Post(string uri, byte[] bytes, string contentType)
        {
            var unityWebRequest = new UnityWebRequest(uri, UnityWebRequest.kHttpVerbPOST)
            {
                uploadHandler = new UploadHandlerRaw(bytes)
                {
                    contentType = contentType
                },
                downloadHandler = new DownloadHandlerBuffer()
            };
            return new HttpRequestImpl(unityWebRequest);
        }

        public static IHttpRequest PostJson(string uri, string json)
        {            
            return new HttpRequestImpl(uri,
                                       Encoding.UTF8.GetBytes(json),
                                       "application/json");
        }

        public static IHttpRequest Put(string uri, byte[] bodyData)
        {
            return new HttpRequestImpl(UnityWebRequest.Put(uri, bodyData));
        }

        public static IHttpRequest Put(string uri, string bodyData)
        {
            return new HttpRequestImpl(UnityWebRequest.Post(uri, bodyData));
        }

        public static IHttpRequest Delete(string uri)
        {
            return new HttpRequestImpl(UnityWebRequest.Delete(uri));
        }

        public static IHttpRequest Head(string uri)
        {
            return new HttpRequestImpl(UnityWebRequest.Head(uri));
        }
    }
}