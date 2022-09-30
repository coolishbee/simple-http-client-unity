using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEditor;
using Cysharp.Threading.Tasks;
using CoolishHttp;

public class Demo : MonoBehaviour
{
    public Button getBtn;
    Action act;
    UnityAction unityact;

    void Start()
    {
        getBtn.onClick.AddListener(() => OnClickGet().Forget());

        act += UniTask.Action(async () =>
        {
            var info = await getAllTeams();
            if(info != null)
            {
                if (info.code == 200)
                {
                    foreach (var item in info.data.list)
                    {
                        Debug.Log(item.ToString());
                    }
                }
            }
            else
            {
                Debug.Log("is null");
            }                    
        });
        unityact += UniTask.UnityAction(async () =>
        {
            var info = await getAllTeams();            
        });
    }

    private void LogMessage(string title, string message)
    {
#if UNITY_EDITOR
        EditorUtility.DisplayDialog(title, message, "Ok");
#else
		Debug.Log(message);
#endif
    }

    public async UniTaskVoid OnClickGet()
    {
        //var info = await SimpleHttpClient.SendToServer<string>("/api/teamlist", SENDTYPE.GET);
        //LogMessage("get", info);

        var info = await getAllTeams();
        LogMessage("get", info.msg);
    }

    public void OnClickTest()
    {
        //getTests().Forget();

        var req = SimpleHttpClient.Request("/api/teamlist", Method.GET)
            .OnSuccess(res => Debug.Log(res.Text))
            .OnError(err => Debug.LogWarning(err.Error))
            .OnNetworkError(netErr => Debug.LogError(netErr.Error))
            .Send();
    }

    public void OnClickPost()
    {
        act.Invoke();
        //unityact.Invoke();
    }

    public async UniTask<GetAllTeams_Response> getAllTeams()
    {
        var info = await SimpleHttpClient.SendToServer<GetAllTeams_Response>("/api/teamlist", Method.GET);
        return info;        
    }

    public async UniTaskVoid getTests()
    {
        var info = await SimpleHttpClient.SendToServer<GetAllTeams_Response>("/api/teamlist", Method.GET);
        foreach(var item in info.data.list)
        {
            Debug.Log(item.team_name);
        }
    }
}
