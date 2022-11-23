using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace CoolishHttp
{
    public class HttpRequestImpl : IHttpRequest
    {
        private readonly UnityWebRequest unityWebRequest;
        private readonly Dictionary<string, string> headers;

        private event Action<HttpResponse> onSuccess;
        private event Action<HttpResponse> onError;
        private event Action<HttpResponse> onNetworkError;

        public HttpRequestImpl(UnityWebRequest request)
        {
            unityWebRequest = request;
            unityWebRequest.timeout = 3;
            headers = new Dictionary<string, string>();
        }        

        public HttpRequestImpl(string uri, byte[] bytes, string contentType)
        {
            var request = new UnityWebRequest(uri, UnityWebRequest.kHttpVerbPOST)
            {
                uploadHandler = new UploadHandlerRaw(bytes)
                {
                    contentType = contentType
                },
                downloadHandler = new DownloadHandlerBuffer()
            };
            unityWebRequest = request;
            unityWebRequest.timeout = 3;
            headers = new Dictionary<string, string>();
        }        

        public IHttpRequest OnSuccess(Action<HttpResponse> onSuccess)
        {
            this.onSuccess += onSuccess;
            return this;
        }

        public IHttpRequest OnError(Action<HttpResponse> onError)
        {
            this.onError += onError;
            return this;
        }

        public IHttpRequest OnNetworkError(Action<HttpResponse> onNetworkError)
        {
            this.onNetworkError += onNetworkError;
            return this;
        }

        public IHttpRequest SetTimeout(int duration)
        {
            unityWebRequest.timeout = duration;
            return this;
        }

        public IHttpRequest SetHeader(string key, string value)
        {
            headers.Add(key, value);
            return this;
        }

        public IHttpRequest SetHeaders(IEnumerable<KeyValuePair<string, string>> headers)
        {
            foreach(var item in headers)
            {
                SetHeader(item.Key, item.Value);
            }
            return this;
        }

        public bool RemoveHeader(string key)
        {
            return headers.Remove(key);
        }

        public IHttpRequest SetRedirectLimit(int redirectLimit)
        {
            unityWebRequest.redirectLimit = redirectLimit;
            return this;
        }

        public void Abort()
        {
            unityWebRequest.Abort();
        }

        public async UniTaskVoid Send()
        {
            try
            {
                foreach(var header in headers)
                {
                    unityWebRequest.SetRequestHeader(header.Key, header.Value);
                }

                await unityWebRequest.SendWebRequest();
                var response = CreateResponse(unityWebRequest);                
                onSuccess?.Invoke(response);
            }            
            catch (Exception e)
            {
                var response = CreateResponse(unityWebRequest);
                if (response.IsNetworkError)
                {
                    onNetworkError?.Invoke(response);
                }
                else if(response.IsHttpError)
                {
                    onError?.Invoke(response);
                }
                Debug.Log(e.Message);
            }

            if (unityWebRequest != null)
            {
                unityWebRequest.Dispose();
            }
        }

        private HttpResponse CreateResponse(UnityWebRequest unityWebRequest)
        {
            return new HttpResponse
            {
                Url = unityWebRequest.url,
                Bytes = unityWebRequest.downloadHandler?.data,
                Text = unityWebRequest.downloadHandler?.text,
                IsSuccessful = unityWebRequest.result == UnityWebRequest.Result.Success,
                IsHttpError = unityWebRequest.result == UnityWebRequest.Result.ProtocolError,
                IsNetworkError = unityWebRequest.result == UnityWebRequest.Result.ConnectionError,
                Error = unityWebRequest.error,
                StatusCode = unityWebRequest.responseCode,
                ResponseHeaders = unityWebRequest.GetResponseHeaders(),
                Texture = (unityWebRequest.downloadHandler as DownloadHandlerTexture)?.texture
            };
        }
    }
}

