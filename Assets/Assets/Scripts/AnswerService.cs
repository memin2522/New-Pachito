using Meta.WitAi.TTS.Utilities;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class AnswerService : MonoBehaviour
{
    [SerializeField] private string url = "https://pachitoapi.onrender.com/answer";
    [SerializeField] TTSSpeaker voice;

    private bool hasAnswer = false; 
    private float pollInterval = 5f; // Intervalo de polling en segundos
    void Start()
    {
        StartCoroutine(PollForAnswer());
    }

    public void SendAnswer(string content)
    {
        // Iniciar la corrutina para enviar la respuesta
        StartCoroutine(PostData(content));
    }

    public IEnumerator PostData(string content)
    {
        
        string jsonData = JsonUtility.ToJson(new AnswerResponse { answer = content});

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

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


    IEnumerator PollForAnswer()
    {
        while (!hasAnswer)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                    webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"GET Error: {webRequest.error}");
                    Debug.LogError($"Response: {webRequest.downloadHandler.text}");
                }
                else
                {
                    Debug.Log($"GET Success: {webRequest.downloadHandler.text}");
                    var data = JsonUtility.FromJson<AnswerResponse>(webRequest.downloadHandler.text);
                    voice.Speak(data.answer); // Reproducir la respuesta con TTSSpeaker
                    hasAnswer = true; // cortamos el bucle
                }
            }
            
            if (!hasAnswer)
                yield return new WaitForSeconds(pollInterval);
        }
    }
}
