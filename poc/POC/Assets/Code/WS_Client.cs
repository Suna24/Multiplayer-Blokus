using UnityEngine;
using WebSocketSharp;
public class WS_Client : MonoBehaviour
{
    WebSocket webSocket;
    Joueur joueur;
    string joueurJson;

    // Start is called before the first frame update
    void Start()
    {

        //Gestion de la connection
        webSocket = new WebSocket("ws://localhost:3000");
        webSocket.Connect();

        webSocket.OnMessage += (sender, e) =>
        {
            Debug.Log("Message du serveur " + e.Data);
        };

        //Gestion du joueur
        joueur = new Joueur("Suna", 0, "rouge");
        joueurJson = JsonUtility.ToJson(joueur);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            webSocket.Send("Bonjour !");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            webSocket.Send(joueurJson);
        }
    }

    private void OnApplicationQuit()
    {
        webSocket.Close();
    }

}
