using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Demo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LogMessage(string title, string message)
    {
#if UNITY_EDITOR
        EditorUtility.DisplayDialog(title, message, "Ok");
#else
		Debug.Log(message);
#endif
    }

    public void OnClickGet()
    {
        LogMessage("get", "test response");
    }
}
