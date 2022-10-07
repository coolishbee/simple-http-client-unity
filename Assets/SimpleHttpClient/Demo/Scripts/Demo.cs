using UnityEngine;
using UnityEngine.UI;
using CoolishHttp;
using CoolishDemo.WWWRequest;
using CoolishDemo.WWWResponse;

public class Demo : MonoBehaviour
{
    public HttpManager httpManager;

    void Start()
    {
    }

    public void OnClickGet()
    {        
        //string requestURL = "http://localhost:8000" + "/api/teamlist";

        //var req = SimpleHttpClient.Get(requestURL)
        //    .OnSuccess(res => Debug.Log(res.Text))
        //    .OnError(err => Debug.LogWarning(err.Error))
        //    .OnNetworkError(netErr => Debug.LogError(netErr.Error))
        //    .Send();

        //httpManager.GetAllTeamsSendPacket(success =>
        //{
        //    Debug.Log(success.code);
        //    Debug.Log(success.msg);
        //    foreach (var item in success.data.list)
        //    {
        //        Debug.Log(item.ToString());
        //    }
        //});

        httpManager.SendPacket<GetAllTeams_Res>(ePacketType.GetAllTeams, res =>
        {
            Debug.Log(res.code);
            Debug.Log(res.msg);
            foreach (var item in res.data.list)
            {
                Debug.Log(item.ToString());
            }
        });
    }

    public void OnClickPost()
    {        
        var team = new TeamInfo_Req("Chelsea", "Graham Potter", "3:4:2:1");
        
        string requestURL = "http://localhost:8000" + "/api/team";

        var req = SimpleHttpClient.PostJson(requestURL, JsonUtility.ToJson(team))
            .OnSuccess(res => Debug.Log(res.Text))
            .OnError(err => Debug.Log(err.Error))
            .OnNetworkError(netErr => Debug.LogError(netErr.Error))
            .Send();
    }    
}
