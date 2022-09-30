using System;
using Cysharp.Threading.Tasks;

namespace CoolishHttp
{
    public interface IHttpRequest
    {
        IHttpRequest OnSuccess(Action<HttpResponse> onSuccess);
        IHttpRequest OnError(Action<HttpResponse> onError);
        IHttpRequest OnNetworkError(Action<HttpResponse> onNetworkError);

        UniTaskVoid Send();
    }
}