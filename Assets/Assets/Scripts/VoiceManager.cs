using Meta.WitAi.TTS.Utilities;
using UnityEngine;

public class VoiceManager : MonoBehaviour
{
    [SerializeField] TTSSpeaker voice;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        voice.Speak("Que coma mierda la militar");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
