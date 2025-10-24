using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class ApiService
{

    private static string apiUrl= "https://pachitoapi.onrender.com/answer";
    public static string ApiUrl
    {
        get { return apiUrl; }
        set { apiUrl = value; }
    }

    public static async Task PostDataAsync(string jsonData)
    {
        using (UnityWebRequest webRequest = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            var operation = webRequest.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"POST Error: {webRequest.error}");
                Debug.LogError($"Response: {webRequest.downloadHandler.text}");
            }
            else
            {
                Debug.Log($"POST Success: {webRequest.downloadHandler.text}");
            }
        }
    }

    public static async Task GetDataAsync(Action<string> onSuccess, Action<string> onError)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            var operation = webRequest.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"GET Error: {webRequest.error}");
                Debug.LogError($"Response: {webRequest.downloadHandler.text}");
                onError?.Invoke($"Error receiving data: {webRequest.downloadHandler.text}");
            }
            else
            {
                Debug.Log($"GET Success: {webRequest.downloadHandler.text}");
                onSuccess?.Invoke(webRequest.downloadHandler.text);
            }
        }
    }
}
