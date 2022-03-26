using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitWs : MonoBehaviour
{

    WebSocketClient webSocketClient;

    // Start is called before the first frame update
    void Start()
    {
        webSocketClient = WebSocketClient.getInstance();
        webSocketClient.GetWebSocket().Connect();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
