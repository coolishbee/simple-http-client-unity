using UnityEngine;
using UnityEngine.UI;
using CoolishHttp;
using CoolishDemo;
using CoolishDemo.WWWResponse;

public class Demo : MonoBehaviour
{
    public string basePath = "https://jsonplaceholder.typicode.com";
    public string timeoutPath = "http://httpbin.org/delay/3";
    public HttpManager httpManager;
    public Text responseText;

    public void OnClickGetPosts()
    {        
        httpManager.SendPacket<GetPosts_Res>(ePacketType.GetPosts, res =>
        {            
            Debug.Log(JsonHelper.ArrayToJsonString<Post>(res.Items, true));
            responseText.text = JsonHelper.ArrayToJsonString<Post>(res.Items, true);
        });
    }

    public void OnClickGetTodos()
    {
        httpManager.SendPacket<GetTodos_Res>(ePacketType.GetTodos, res =>
        {            
            Debug.Log(JsonHelper.ArrayToJsonString<Todo>(res.Items, true));
            responseText.text = JsonHelper.ArrayToJsonString<Todo>(res.Items, true);
        });
    }

    public void OnClickGetUsers()
    {
        httpManager.SendPacket<GetUsers_Res>(ePacketType.GetUsers, res =>
        {
            responseText.text = JsonHelper.ArrayToJsonString<User>(res.Items, true);
        });        
    }

    public void OnClickPostPosts()
    {
        httpManager.SendPacket<Post>(ePacketType.PostPosts, res =>
        {
            Debug.Log(JsonUtility.ToJson(res, true));
            responseText.text = JsonUtility.ToJson(res, true);
        });
    }

    public void OnClickGetInvalidURL()
    {
        string requestURL = "https://invalidurl.com" + "/test";

        var req = SimpleHttpClient.Get(requestURL)
            .OnSuccess(res =>
            {
                responseText.text = res.Text;
            })
            .OnError(err => Debug.LogWarning(err.Error))
            .OnNetworkError(netErr => {
                Debug.LogError(netErr.Error);
                responseText.text = netErr.Error;
            })
            .Send();
    }

    public void OnClickGetTimeout()
    {
        var req = SimpleHttpClient.Get(timeoutPath)            
            .OnSuccess(res => Debug.Log(res.Text))
            .OnError(err => Debug.LogWarning(err.Error))
            .OnNetworkError(netErr => {
                Debug.LogError(netErr.Error);
                responseText.text = netErr.Error;
            })
            .Send();
    }

    public void OnClickGetNotFound()
    {
        string requestURL = basePath + "/posts/999";

        var req = SimpleHttpClient.Get(requestURL)
            .OnSuccess(res => Debug.Log(res.Text))
            .OnError(err => {
                Debug.LogWarning(err.Error);
                responseText.text = err.Error;
            })
            .OnNetworkError(netErr => Debug.LogError(netErr.Error))
            .Send();
    }

    public void OnClickPostFormData()
    {
        string requestURL = basePath + "/posts";

        WWWForm form = new WWWForm();
        form.AddField("player", "son");
        form.AddField("number", "7");
        form.AddField("team", "Tottenham");
        form.AddField("country", "Korea Republic");

        var req = SimpleHttpClient.Post(requestURL, form)
            .OnSuccess(res =>
            {
                responseText.text = res.Text;
            })
            .OnError(err => Debug.LogWarning(err.Error))
            .OnNetworkError(netErr => Debug.LogError(netErr.Error))
            .Send();
    }    
}
