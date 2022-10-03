using System;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
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
        protected static double timeout = 3;

        public static async UniTask<T> SendToServer<T>(string url, Method method, string jsonBody = null)
        {
            await CheckNetwork();

            string requestURL = "http://localhost:8000" + url;
            //string requestURL = "http://httpbin.org/delay/3";

            var cts = new CancellationTokenSource();
            cts.CancelAfterSlim(TimeSpan.FromSeconds(timeout));

            UnityWebRequest request = new UnityWebRequest(requestURL, method.ToString());

            request.downloadHandler = new DownloadHandlerBuffer();
            if (!string.IsNullOrEmpty(jsonBody))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }

            SetHeaders(request);
            try
            {
                var res = await request.SendWebRequest().WithCancellation(cts.Token);
                if (res.result == UnityWebRequest.Result.ConnectionError)
                    Debug.Log(res.error);
                T result = JsonUtility.FromJson<T>(res.downloadHandler.text);
                return result;
            }
            catch (OperationCanceledException ex)
            {
                if (ex.CancellationToken == cts.Token)
                {
                    Debug.Log("Timeout");

                    return default;
                    //return await SendToServer<T>(url, sendType, jsonBody); //retry
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return default;
            }
            return default;
        }

        private static async UniTask CheckNetwork()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.LogError("no connected");
                await UniTask.WaitUntil(() => Application.internetReachability != NetworkReachability.NotReachable);
                Debug.Log("network is connected");
            }
        }

        private static void SetHeaders(UnityWebRequest request)
        {
            request.SetRequestHeader("Content-Type", "application/json");
        }

        public static IHttpRequest Request(string url, Method method, string jsonBody = null)
        {
            switch (method)
            {
                case Method.GET:
                    return new HttpRequestImpl(UnityWebRequest.Get(url));
                    //break;
                case Method.POST:
                    return new HttpRequestImpl(url, method, jsonBody);
                //break;
                default:
                    return null;
            }
        }
    }
}