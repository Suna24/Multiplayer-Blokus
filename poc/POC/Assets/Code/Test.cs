using UnityEngine;
using KyleDulce.SocketIo;
public class Test : MonoBehaviour
{
    private Socket socket;

    // Start is called before the first frame update
    void Start()
    {
        socket = SocketIo.establishSocketConnection("ws://localhost:3000");
        socket.connect();
    }

    void message(string chaineEnvoyee)
    {

    }
}
