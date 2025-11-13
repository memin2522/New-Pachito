using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Multiplayer.Center.Common;
using UnityEngine;

public class WebSocketService : MonoBehaviour
{
    [Header("Server Configuration")]
    [Tooltip("URL of the API")]
    [SerializeField] string serverUrl = "https://pachitoapi.onrender.com/";

    SocketIOClient.SocketIO client;
    public Action<string> OnAnswerReceived;
    public Action OnPreparedToCommunicate;
    async void Start()
    {
        client = new SocketIOClient.SocketIO(serverUrl, new SocketIOOptions
        {
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });

        client.OnConnected += async (s, e) => {
            Debug.Log("[SocketIO] Pachito Unity successfully connected to the server");
            OnPreparedToCommunicate();
            await client.EmitAsync("join_session", "unity");
        };

        client.OnDisconnected += async (s, e) => {
            Debug.LogWarning("[SocketIO] Disconnected from the server");
            await client.EmitAsync("disconnect");
        };

        client.On("answer", response =>
        {
            string json = response.ToString();

            json = json.TrimStart('[').TrimEnd(']');

            // Deserializamos
            AnswerResponse data = JsonUtility.FromJson<AnswerResponse>(json);
            Debug.Log($"Answer recevied: {data.answer}");
            OnAnswerReceived(data.answer);
        });

        try
        {
            await client.ConnectAsync();
        }
        catch (Exception ex)
        {
            Debug.LogError("[SocketIO] Error conectando: " + ex);
        }
    }

    public async Task SendQuestion(string question)
    {
        await client.EmitAsync("ask_question", question);
    }
}
