using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class IAResponsePolling : MonoBehaviour
{
    [SerializeField] private TtsService voiceService;

    private bool hasAnswer = false;
    public float pollInterval = 5f;

    private async Task PollForAnswerAsync()
    {
        while (!hasAnswer)
        {
            await ApiService.GetDataAsync(OnAnswerReceived, OnErrorReceived);
            await Task.Delay(1000); 
        }
    }


    private void OnAnswerReceived(string answer)
    {
        hasAnswer = true;
    }

    private void OnErrorReceived(string error)
    {

    }
}
