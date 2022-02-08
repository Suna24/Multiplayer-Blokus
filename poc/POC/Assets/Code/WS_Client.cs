using UnityEngine;
using WebSocketSharp;
public class WS_Client : MonoBehaviour
{
    WebSocket webSocket;

    // Start is called before the first frame update
    void Start()
    {
        webSocket = new WebSocket("ws://localhost:3000");
        webSocket.Connect();

        webSocket.OnMessage += (sender, e) =>
        {
            Debug.Log("Message du serveur " + e.Data);
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            webSocket.Send("Bonjour !");
        }
    }

    private void OnApplicationQuit()
    {
        webSocket.Close();
    }

}
