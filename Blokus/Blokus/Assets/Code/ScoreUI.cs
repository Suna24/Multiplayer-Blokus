using UnityEngine.UI;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Linq;

public class ScoreUI : MonoBehaviour
{
    WebSocketClient webSocketClient;

    public Text premier, second, troisieme, quatrieme;
    Text[] classement;
    int[] scores;
    List<Joueur> joueurs = new List<Joueur>();
    bool aTermine = false;

    // Start is called before the first frame update
    void Start()
    {
        webSocketClient = WebSocketClient.getInstance();
        webSocketClient.setScoreUI(this);

        webSocketClient.GetWebSocket().Send(JsonUtility.ToJson(new Message("scores")));

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
            joueurs.Add(new Joueur(messageScores.scores[i], (Couleur)Enum.Parse(typeof(Couleur), i.ToString())));
        }

        aTermine = true;

        joueurs.Sort(delegate (Joueur x, Joueur y)
        {
            return x.score.CompareTo(y.score);
        });
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log("Here");

        if (aTermine)
        {

            for (int i = 0; i < joueurs.Count; i++)
            {
                Debug.Log("Dans la boucle");
                Couleur couleur = (Couleur)joueurs.ElementAt(i).couleurJouee + 1;
                classement[i].text = "Joueur " + couleur + " : " + joueurs[i].score + " blocs restants";
            }

            aTermine = false;

        }
    }

}
