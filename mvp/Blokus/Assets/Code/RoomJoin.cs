using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class RoomJoin : MonoBehaviour
{

    WebSocketClient webSocketClient;

    public void Start()
    {
        webSocketClient = WebSocketClient.getInstance();

        //Listener pour la réponse du serveur
        webSocketClient.GetWebSocket().OnMessage += (sender, e) =>
        {
            Debug.Log("dans Listener");
            Debug.Log(e.Data);

            Message.MessageRooms messageRooms = JsonConvert.DeserializeObject<Message.MessageRooms>(e.Data);

            //Pour chaque room présente sur le serveur on les affiche sous forme de boutons
            foreach (Message.MessageRoom room in messageRooms.rooms)
            {
                GameObject button = new GameObject();
                button.AddComponent<Button>();
                button.transform.position = new Vector3(192f, 122f, 0f);
                button.GetComponent<Button>().GetComponent<Text>().text = room.nom + "      " + room.nbJoueursCourant + "/" + room.nbJoueursTotal + "joueurs";
            }
        };

    }

}