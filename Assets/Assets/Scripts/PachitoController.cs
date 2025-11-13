using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class PachitoController : MonoBehaviour
{
    [SerializeField] private WebSocketService _websocketService;
    [SerializeField] private  TtsService _ttsService;

    [SerializeField] private TMP_InputField inputQuestion;
    [SerializeField] private TextMeshProUGUI textLabel;
    void Start()
    {
        _websocketService.OnAnswerReceived += OnAnswerReceived;
        _websocketService.OnPreparedToCommunicate += OnPreparedToSpeak;
        _ttsService.Speak("Hola soy pachito, puedes preguntarme cosas sobre la Sanbuena");
    }

    private void OnPreparedToSpeak()
    {
        _ttsService.Speak("Ya estoy listo para responder tus preguntas");
    }

    private void OnAnswerReceived(string answer)
    {
        textLabel.text += $"Received from server: {answer}\n";
        _ttsService.Speak(answer);
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
