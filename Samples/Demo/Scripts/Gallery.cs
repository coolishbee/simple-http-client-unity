using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using CoolishHttp;
using CoolishDemo.WWWResponse;
using CoolishDemo;

public class Gallery : MonoBehaviour
{
    public InputField textInput;
    public Button searchButton;
    public Transform content;
    public GameObject photoContainer;

    // Start is called before the first frame update
    void Start()
    {
        searchButton.onClick.AddListener(() => OnClickSearch(textInput.text));
    }    

    public void OnClickSearch(string term)
    {        
        foreach (Transform photo in content)
        {
            Destroy(photo.gameObject);
        }

        string requestURL = $"https://api.unsplash.com/search/photos?query={term}";

        var req = SimpleHttpClient.Get(requestURL)
            .AddHeader("Authorization", "Client-ID S2F1QMER5i40nOj6vV_JUrGfK7e2l7Ue0f_MLUX5STQ")
            .OnSuccess(res => {                
                var data = JsonUtility.FromJson<GetPhotos_Res>(res.Text);
                foreach(Photo item in data.results)
                {                    
                    LoadAndShowPhoto(item.urls.regular);
                }
            })
            .OnError(err => Debug.LogWarning(err.Error))
            .OnNetworkError(netErr => Debug.LogError(netErr.Error))
            .Send();
    }

    private void LoadAndShowPhoto(string url)
    {
        var req = SimpleHttpClient.GetTexture(url)
            .OnSuccess(res =>
            {
                StartCoroutine(GetPhotoContainerInstance(res.Texture));
            })
            .OnError(err => Debug.LogWarning(err.Error))
            .OnNetworkError(netErr => Debug.LogError(netErr.Error))
            .Send();
    }

    IEnumerator GetPhotoContainerInstance(Texture texture)
    {
        ResourceRequest request = Resources.LoadAsync("PhotoContainer");
        yield return request;
        GameObject gameObject = request.asset as GameObject;
                
        var photoContainer = Instantiate(gameObject, content);
        photoContainer.GetComponent<RawImage>().texture = texture;
    }
}
