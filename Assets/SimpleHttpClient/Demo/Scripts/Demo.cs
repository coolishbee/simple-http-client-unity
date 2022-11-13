using UnityEngine;
using CoolishDemo;
using CoolishDemo.WWWResponse;

public class Demo : MonoBehaviour
{
    public HttpManager httpManager;    

    public void OnClickGet1()
    {        
        httpManager.SendPacket<GetPosts_Res>(ePacketType.GetPosts, res =>
        {            
            Debug.Log(JsonHelper.ArrayToJsonString<Post>(res.Items, true));
        });
    }

    public void OnClickGet2()
    {
        httpManager.SendPacket<GetTodos_Res>(ePacketType.GetPosts, res =>
        {
            Debug.Log(JsonHelper.ArrayToJsonString<Todo>(res.Items, true));
        });        
    }

    public void OnClickPost()
    {
        httpManager.SendPacket<Post>(ePacketType.PostPosts, res =>
        {
            Debug.Log(JsonUtility.ToJson(res, true));
        });
    }
}
