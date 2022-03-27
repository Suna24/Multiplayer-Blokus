using WebSocketSharp;
using UnityEngine;
using UnityEngine.SceneManagement;

class WebSocketClient
{

    //Attributs
    public static WebSocketClient webSocketClient;
    private WebSocket webSocket;
    public Blokus blokus;
    public RoomJoin roomJoin;
    public ScoreUI scoreUI;

    //Constructeur
    private WebSocketClient()
    {
        webSocket = new WebSocket("ws://localhost:3000");
        creationListener();
    }

    //Méthode pour avoir l'instance unique du webSocketClient
    public static WebSocketClient getInstance()
    {
        if (webSocketClient == null)
        {
            webSocketClient = new WebSocketClient();
        }

        return webSocketClient;
    }

    //Méthode pour avoir le webSocket
    public WebSocket GetWebSocket()
    {
        return webSocket;
    }

    //Méthode pour envoyer un message à l'aide du webSocket
    public void Send(string message)
    {
        webSocket.Send(message);
    }

    //Méthode pour setup tous les listeners associés au webSocket
    private void creationListener()
    {

        //Listener quand le webSocket reçoit un message du serveur
        webSocket.OnMessage += (sender, e) =>
        {
            Debug.Log("Message du serveur : " + e.Data);

            //On récupère le type du  message
            Message message = JsonUtility.FromJson<Message>(e.Data);

            //Suivant le type du message, on réalise une action en particulier
            switch (message.type)
            {

                //Si c'est pour la création du joueur en début de partie (+ attribution couleur)
                case "joueur":
                    blokus.creationJoueur(e.Data);
                    break;

                //Si c'est pour une mise à jour du plateau de jeu
                case "plateau":
                    Debug.Log("Dans mise à jour plateau");
                    blokus.plateau(e.Data);
                    break;

                //Si c'est pour gérer les tours de jeu
                case "tour":
                    blokus.tour(e.Data);
                    break;

                //Si c'est pour afficher toutes les rooms disponibles
                case "affichageRooms":
                    roomJoin.affichageDesRooms(e.Data);
                    break;

                //Si c'est pour afficher les scores et que la partie est terminée
                case "scores":
                    SceneManager.LoadScene("Ecran_des_scores");
                    scoreUI.affichageDesScores(e.Data);
                    break;

                //Message par défaut si le message n'est pas identifié
                default:
                    Debug.Log("Message non identifié");
                    break;
            }
        };
    }

    //Méthode pour setup le blokus 
    public void setBlokus(Blokus blokus)
    {
        this.blokus = blokus;
    }

    //Méthode pour setup la roomJoin
    public void setRoomJoin(RoomJoin roomJoin)
    {
        this.roomJoin = roomJoin;
    }

    //Méthode pour setup le scoreUI
    public void setScoreUI(ScoreUI scoreUI)
    {
        this.scoreUI = scoreUI;
    }

}