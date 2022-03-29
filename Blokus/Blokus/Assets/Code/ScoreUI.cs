using UnityEngine.UI;
using UnityEngine;
using Newtonsoft.Json;
using System;

public class ScoreUI : MonoBehaviour
{
    WebSocketClient webSocketClient;

    public Text premier, second, troisieme, quatrieme;
    Text[] classement;
    int[] scores;
    bool aTermine = false;

    // Start is called before the first frame update
    void Start()
    {
        webSocketClient = WebSocketClient.getInstance();
        webSocketClient.setScoreUI(this);

        classement = new Text[4] { premier, second, troisieme, quatrieme };
    }

    public void affichageDesScores(string data)
    {

        Debug.Log("Dans affichage Score");

        Message.MessageScores messageScores = JsonConvert.DeserializeObject<Message.MessageScores>(data);

        Debug.Log(messageScores.scores[0]);
        scores = new int[messageScores.scores.Length];

        for (int i = 0; i < messageScores.scores.Length; i++)
        {
            scores[i] = messageScores.scores[i];
        }

        aTermine = true;
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log("Here");

        if (aTermine)
        {

            for (int i = 0; i < scores.Length; i++)
            {
                Debug.Log("Dans la boucle");
                classement[i].text = "" + scores[i] + " blocs restants";
            }

        }

    }
}
