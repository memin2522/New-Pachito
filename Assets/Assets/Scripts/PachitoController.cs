using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using System.Collections.Concurrent;

public class PachitoController : MonoBehaviour
{
    [SerializeField] private WebSocketService _websocketService;
    [SerializeField] private  TtsService _ttsService;

    [SerializeField] private TMP_InputField inputQuestion;
    [SerializeField] private TextMeshProUGUI textLabel;

    private ConcurrentQueue<string> AnswerQueue = new ConcurrentQueue<string>();
    void Start()
    {
        _websocketService.OnAnswerReceived += OnAnswerReceived;
        _websocketService.OnPreparedToCommunicate += OnPreparedToSpeak;
        _ttsService.Speak("Hola soy pachito, puedes preguntarme cosas sobre la Sanbuena");
    }

    private void Update()
    {
        if(AnswerQueue.TryDequeue(out string answer))
        {
            textLabel.text += $"Received from server: {answer}\n";
            _ttsService.Speak(answer);
        }
    }

    private void OnPreparedToSpeak()
    {
        _ttsService.Speak("Ya estoy listo para responder tus preguntas");
    }

    private void OnAnswerReceived(string answer)
    {
        AnswerQueue.Enqueue(answer);
    }

    public void OnSendButtonClicked()
    {
        string question = inputQuestion.text;
        inputQuestion.text = "";

        if (string.IsNullOrEmpty(question))
            return;

        
        _ = SendQuestionToServer(question);
    }

    private async Task SendQuestionToServer(string question)
    {
        textLabel.text += $"Sended from client: {question}\n";
        _ttsService.Speak($"Me preguntaste {question}");
        await _websocketService.SendQuestion(question);
    }
}
