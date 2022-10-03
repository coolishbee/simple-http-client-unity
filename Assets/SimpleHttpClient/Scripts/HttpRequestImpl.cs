using System;
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
        private event Action<HttpResponse> onSuccess;
        private event Action<HttpResponse> onError;
        private event Action<HttpResponse> onNetworkError;

        public HttpRequestImpl(string url, Method method, string jsonBody = null)
        {
            string requestURL = "http://localhost:8000" + url;
            //string requestURL = "http://httpbin.org/delay/3";

            UnityWebRequest request = new UnityWebRequest(requestURL, method.ToString());
            //UnityWebRequest testReq = UnityWebRequest.Get(requestURL);
            //UnityWebRequest postTest = UnityWebRequest.Post(requestURL, jsonBody);

            request.downloadHandler = new DownloadHandlerBuffer();
            if (!string.IsNullOrEmpty(jsonBody))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }

            request.SetRequestHeader("Content-Type", "application/json");
            request.timeout = 3;
            unityWebRequest = request;
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

