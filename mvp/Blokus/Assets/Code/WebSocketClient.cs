using WebSocketSharp;

class WebSocketClient
{

    public static WebSocketClient webSocketClient;
    private WebSocket webSocket;

    private WebSocketClient()
    {
        webSocket = new WebSocket("ws://localhost:3000");
    }

    public static WebSocketClient getInstance()
    {
        if (webSocketClient == null)
        {
            webSocketClient = new WebSocketClient();
        }

        return webSocketClient;
    }

    public WebSocket GetWebSocket()
    {
        return webSocket;
    }

}