using Meta.WitAi.TTS.Utilities;
using UnityEngine;

public class TtsService : MonoBehaviour
{
    [SerializeField] private TTSSpeaker voice;
    
    public void Speak(string message)
    {
        voice.Speak(message);
    }
}
