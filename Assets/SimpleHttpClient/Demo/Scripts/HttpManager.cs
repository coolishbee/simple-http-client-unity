using System;
using System.Collections.Generic;
using UnityEngine;
using CoolishHttp;
using CoolishDemo;
using CoolishDemo.WWWResponse;

public enum ePacketType
{    
    GetPosts,
    GetTodos,
    GetUsers,
    PostPosts,
}

public class HttpManager : MonoBehaviour
{
    private readonly string basePath = "https://jsonplaceholder.typicode.com";

    //example.1
    public void SendPacket<T>(ePacketType packetType, Action<T> action)
    {
        switch (packetType)
        {
            case ePacketType.GetTodos: GetTodos(action); break;
            case ePacketType.GetPosts: GetPosts(action); break;
            case ePacketType.GetUsers: GetUsers(action); break;
            case ePacketType.PostPosts: PostPosts(action); break;
        }
    }

    private void GetTodos<T>(Action<T> action)
    {
        string requestURL = basePath + "/todos";

        var req = SimpleHttpClient.Get(requestURL)
            .OnSuccess(res =>
            {
                string newJson = "{ \"Items\": " + res.Text + "}";
                T data = JsonUtility.FromJson<T>(newJson);
                action.Invoke(data);
            })
            .OnError(err => Debug.LogWarning(err.Error))
            .OnNetworkError(netErr => Debug.LogError(netErr.Error))
            .Send();
    }

    private void GetPosts<T>(Action<T> action)
    {
        string requestURL = basePath + "/posts";

        var req = SimpleHttpClient.Get(requestURL)
            .OnSuccess(res =>
            {
                string newJson = "{ \"Items\": " + res.Text + "}";
                T data = JsonUtility.FromJson<T>(newJson);
                action.Invoke(data);
            })
            .OnError(err => Debug.LogWarning(err.Error))
            .OnNetworkError(netErr => Debug.LogError(netErr.Error))
            .Send();
    }

    private void GetUsers<T>(Action<T> action)
    {
        string requestURL = basePath + "/users";

        var req = SimpleHttpClient.Get(requestURL)
            .OnSuccess(res =>
            {
                string newJson = "{ \"Items\": " + res.Text + "}";
                T data = JsonUtility.FromJson<T>(newJson);
                action.Invoke(data);
            })
            .OnError(err => Debug.LogWarning(err.Error))
            .OnNetworkError(netErr => Debug.LogError(netErr.Error))
            .Send();
    }

    private void PostPosts<T>(Action<T> action)
    {
        var postBody = new Post {
            title = "foo",
            body = "bar",
            userId = 1
        };

        string requestURL = basePath + "/posts";

        var req = SimpleHttpClient.PostJson(requestURL, JsonUtility.ToJson(postBody))
            .OnSuccess(res =>
            {
                T data = JsonUtility.FromJson<T>(res.Text);
                action.Invoke(data);                
            })
            .OnError(err => Debug.Log(err.Error))
            .OnNetworkError(netErr => Debug.LogError(netErr.Error))
            .Send();
    }

    //example.2
    public void GetPostsSendPacket(Action<GetPosts_Res> action)
    {
        string requestURL = basePath + "/posts";

        var req = SimpleHttpClient.Get(requestURL)
            .OnSuccess(res =>
            {
                string newJson = "{ \"Items\": " + res.Text + "}";
                var data = JsonUtility.FromJson<GetPosts_Res>(newJson);
                action.Invoke(data);
            })
            .OnError(err => Debug.LogWarning(err.Error))
            .OnNetworkError(netErr => Debug.LogError(netErr.Error))            
            .Send();
    }    
}
