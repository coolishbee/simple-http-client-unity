# simple-http-client-unity

Provide efficient HttpClient functions using UniTask in Unity.

## Features

* using [UniTask](https://github.com/Cysharp/UniTask)
* Success, error and network error events

## UPM Package
### Install via git URL

**step 1.** Add [UniTask](https://github.com/Cysharp/UniTask#install-via-git-url) package

**step 2.** Add SimpleHttpClient Package:

Package Manager > Add package from git URL > Add `https://github.com/coolishbee/simple-http-client-unity.git`

## Project Tree

```
.
├── SimpleHttpClient
│   ├── Demo
│   │   ├── Scene
│   │   │   ├── Demo.unity
│   │   │   └── Demo.unity.meta
│   │   ├── Scene.meta
│   │   ├── Scripts
│   │   │   ├── Demo.cs
│   │   │   ├── Demo.cs.meta
│   │   │   ├── HttpManager.cs
│   │   │   ├── HttpManager.cs.meta
│   │   │   ├── Model
│   │   │   │   ├── WWWRequest.cs
│   │   │   │   ├── WWWRequest.cs.meta
│   │   │   │   ├── WWWResponse.cs
│   │   │   │   └── WWWResponse.cs.meta
│   │   │   └── Model.meta
│   │   └── Scripts.meta
│   ├── Demo.meta
│   ├── Scripts
│   │   ├── HttpRequestImpl.cs
│   │   ├── HttpRequestImpl.cs.meta
│   │   ├── HttpResponse.cs
│   │   ├── HttpResponse.cs.meta
│   │   ├── IHttpRequest.cs
│   │   ├── IHttpRequest.cs.meta
│   │   ├── SimpleHttpClient.cs
│   │   └── SimpleHttpClient.cs.meta
│   └── Scripts.meta
└── SimpleHttpClient.meta
```

## Example
### GET Example

```c#
string requestURL = "http://localhost:8000" + "/api/path";

var req = SimpleHttpClient.Get(requestURL)
    .OnSuccess(res => Debug.Log(res.Text))
    .OnError(err => Debug.LogWarning(err.Error))
    .OnNetworkError(netErr => Debug.LogError(netErr.Error))
    .Send();
```

### POSTJson Example

```c#
var team = new TeamInfo_Req("Chelsea", "Graham Potter", "3:4:2:1");

string requestURL = "http://localhost:8000" + "/api/path";

var req = SimpleHttpClient.PostJson(requestURL, JsonUtility.ToJson(team))
    .OnSuccess(res => Debug.Log(res.Text))
    .OnError(err => Debug.Log(err.Error))
    .OnNetworkError(netErr => Debug.LogError(netErr.Error))
    .Send();
```

### Custom HttpManager Sample

```c#
public enum ePacketType
{
    GetAllTeams,
}

public class HttpManager : MonoBehaviour
{
    //example.1
    public void SendPacket<T>(ePacketType packetType, Action<T> action)
    {
        switch (packetType)
        {
            case ePacketType.GetAllTeams: GetAllTeams(action); break;
        }
    }

    private void GetAllTeams<T>(Action<T> action)
    {
        string requestURL = "http://localhost:8000" + "/api/teamlist";

        var req = SimpleHttpClient.Get(requestURL)
            .OnSuccess(res =>
            {
                T data = JsonUtility.FromJson<T>(res.Text);
                action.Invoke(data);
            })
            .OnError(err => Debug.LogWarning(err.Error))
            .OnNetworkError(netErr => Debug.LogError(netErr.Error))
            .Send();
    }

    //example.2
    public void GetAllTeamsSendPacket(Action<GetAllTeams_Res> action)
    {
        string requestURL = "http://localhost:8000" + "/api/teamlist";
        //string requestURL = "http://httpbin.org/delay/3"; //timeout test

        var req = SimpleHttpClient.Get(requestURL)
            .OnSuccess(res =>
            {
                var data = JsonUtility.FromJson<GetAllTeams_Res>(res.Text);
                action.Invoke(data);
            })
            .OnError(err => Debug.LogWarning(err.Error))
            .OnNetworkError(netErr => Debug.LogError(netErr.Error))
            .Send();
    }    
}

//usage
//ex1
httpManager.SendPacket<GetAllTeams_Res>(ePacketType.GetAllTeams, res =>
{
    Debug.Log(res.code);
    Debug.Log(res.msg);
    foreach (var item in res.data.list)
    {
        Debug.Log(item.ToString());
    }
});
//ex2
httpManager.GetAllTeamsSendPacket(success =>
{
   Debug.Log(success.code);
   Debug.Log(success.msg);
   foreach (var item in success.data.list)
   {
       Debug.Log(item.ToString());
   }
});
```