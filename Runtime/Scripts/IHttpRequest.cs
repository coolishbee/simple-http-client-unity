using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace CoolishHttp
{
    public interface IHttpRequest
    {
        IHttpRequest OnSuccess(Action<HttpResponse> onSuccess);
        IHttpRequest OnError(Action<HttpResponse> onError);
        IHttpRequest OnNetworkError(Action<HttpResponse> onNetworkError);
        IHttpRequest SetTimeout(int duration);
        IHttpRequest AddHeader(string key, string value);
        IHttpRequest SetHeaders(IEnumerable<KeyValuePair<string, string>> headers);
        IHttpRequest SetRedirectLimit(int redirectLimit);
        bool RemoveHeader(string key);
        UniTaskVoid Send();
        void Abort();
    }
}