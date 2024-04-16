using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public static class Utils
{
    public static IEnumerator GetTexture(string url, Action<Texture> onComplete)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            onComplete?.Invoke(myTexture);
        }
    }
}