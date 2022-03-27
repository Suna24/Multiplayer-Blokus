using UnityEngine.UI;
using UnityEngine;
using Newtonsoft.Json;

public class ScoreUI : MonoBehaviour
{

    WebSocketClient webSocketClient;

    public Text premier, second, troisieme, quatrieme;
    Text[] classement;

    // Start is called before the first frame update
    void Start()
    {
        webSocketClient = WebSocketClient.getInstance();
        webSocketClient.setScoreUI(this);

        classement = new Text[4] { premier, second, troisieme, quatrieme };
    }

    public void affichageDesScores(string data)
    {
        Message.MessageScores messageScores = JsonConvert.DeserializeObject<Message.MessageScores>(data);

        for (int i = 0; i < messageScores.scores.Length; i++)
        {
            classement[i].text = messageScores.scores;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
