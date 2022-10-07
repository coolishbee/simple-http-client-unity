# simple-http-client-unity


## Features

* using Unitask
* Success, error and network error events

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


## GET Example

```
string requestURL = "http://localhost:8000" + "/api/path";

var req = SimpleHttpClient.Get(requestURL)
    .OnSuccess(res => Debug.Log(res.Text))
    .OnError(err => Debug.LogWarning(err.Error))
    .OnNetworkError(netErr => Debug.LogError(netErr.Error))
    .Send();
```

## POSTJson Example

```
var team = new TeamInfo_Req("Chelsea", "Graham Potter", "3:4:2:1");

string requestURL = "http://localhost:8000" + "/api/path";

var req = SimpleHttpClient.PostJson(requestURL, JsonUtility.ToJson(team))
    .OnSuccess(res => Debug.Log(res.Text))
    .OnError(err => Debug.Log(err.Error))
    .OnNetworkError(netErr => Debug.LogError(netErr.Error))
    .Send();
```