using UnityEngine;

public class IAResponsePolling : MonoBehaviour
{
    [SerializeField] private TtsService voiceService;

    private bool hasAnswer = false;
    public float pollInterval = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
