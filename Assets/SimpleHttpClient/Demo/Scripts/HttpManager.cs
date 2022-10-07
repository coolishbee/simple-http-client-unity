using System;
using System.Collections.Generic;
using UnityEngine;
using CoolishHttp;
using CoolishDemo.WWWRequest;
using CoolishDemo.WWWResponse;

public enum ePacketType
{
    GetAllTeams,
}

public class HttpManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
