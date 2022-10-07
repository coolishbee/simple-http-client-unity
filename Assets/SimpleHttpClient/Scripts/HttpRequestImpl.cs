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
        internal UnityWebRequest UnityWebRequest => unityWebRequest;

        private readonly UnityWebRequest unityWebRequest;

        private event Action<HttpResponse> onSuccess;
        private event Action<HttpResponse> onError;
        private event Action<HttpResponse> onNetworkError;

        public HttpRequestImpl(UnityWebRequest request)
        {
            unityWebRequest = request;
            unityWebRequest.timeout = 3;
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

        public async UniTaskVoid Send()
        {
            //var cts = new CancellationTokenSource();
            //cts.CancelAfterSlim(TimeSpan.FromSeconds(3));            

            try
            {
                await unityWebRequest.SendWebRequest();
                var response = CreateResponse(unityWebRequest);                
                onSuccess?.Invoke(response);
            }
            //catch (OperationCanceledException ex)
            //{
            //    if (ex.CancellationToken == cts.Token)
            //    {
            //        var response = CreateResponse(unityWebRequest);
            //        onNetworkError?.Invoke(response);
            //    }
            //}
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
                StatusCode = unityWebRequest.responseCode
            };
        }
    }
}

