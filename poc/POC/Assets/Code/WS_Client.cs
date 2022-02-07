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
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnApplicationQuit()
    {
        webSocket.Close();
    }

}
