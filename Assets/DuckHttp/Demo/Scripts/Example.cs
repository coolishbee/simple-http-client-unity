using System;
using System.Collections.Generic;
using UnityEngine;
using Duck.Http;
using Duck.Http.Service;

[Serializable]
public class User
{
    [SerializeField]
    public int id;
    [SerializeField]
    public string username;
}

public class Example : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        
    }

    public void OnClickGet()
    {
        string url = "http://httpbin.org/delay/3";
        //string url = "http://localhost:8000/api/teamlist";
        Http.Get(url)
            .SetTimeout(3)
            .OnSuccess(HandleSuccess)
            .OnNetworkError(err => Debug.LogWarning(err.Error))
            .OnError(response => Debug.LogWarning(response.Error))
            .Send();
        
    }

    private void HandleSuccess(HttpResponse response)
    {
        //var user = JsonUtility.FromJson<User>(response.Text);
        Debug.Log(response.Text);
    }
}
