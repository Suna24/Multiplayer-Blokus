using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class RoomJoin : MonoBehaviour
{

    WebSocketClient webSocketClient;
    [SerializeField] private Transform contenuContainer;
    [SerializeField] private GameObject prefab;
    string[] noms;
    int[] nbJoueursCourant;
    int[] nbJoueursTotal;
    bool aFini = false;
    int index = 0;
    public Button rejoindre;
    string nomDeLaRoomSelectionnee { get; set; }


    public void Start()
    {
        webSocketClient = WebSocketClient.webSocketClient;

        //Listener pour la réponse du serveur
        webSocketClient.GetWebSocket().OnMessage += (sender, e) =>
        {
            Debug.Log(e.Data);

            Message.MessageRooms messageRooms = JsonConvert.DeserializeObject<Message.MessageRooms>(e.Data);

            int taille = messageRooms.rooms.GetLength(0);

            noms = new string[taille];
            nbJoueursCourant = new int[taille];
            nbJoueursTotal = new int[taille];

            //Pour chaque room présente sur le serveur on les affiche sous forme de boutons
            foreach (Message.MessageRoom room in messageRooms.rooms)
            {
                noms[index] = room.nom;
                nbJoueursCourant[index] = room.nbJoueursCourant;
                nbJoueursTotal[index] = room.nbJoueursTotal;

                index++;
            }

            aFini = true;
        };

        rejoindre.onClick.AddListener(() =>
        {
            Message.MessageJoinRoom messageJoinRoom = new Message.MessageJoinRoom("joinRoom", nomDeLaRoomSelectionnee);
            webSocketClient.GetWebSocket().Send(JsonUtility.ToJson(messageJoinRoom));
            SceneManager.LoadScene("Ecran_de_jeu");
        });

    }

    public void Update()
    {

        if (aFini)
        {

            for (int i = 0; i < noms.Length; i++)
            {
                GameObject button = Instantiate(prefab, new Vector3(1f, 1f, 1f), Quaternion.identity);
                button.GetComponentInChildren<Text>().text = noms[i] + "        " + nbJoueursCourant[i] + "/" + nbJoueursTotal[i] + " joueurs";
                button.transform.localScale = Vector3.one;
                button.transform.SetParent(contenuContainer, false);

                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Debug.Log("Clic sur le bouton " + button.GetComponentInChildren<Text>().text);
                    nomDeLaRoomSelectionnee = noms[button.transform.GetSiblingIndex()];
                    rejoindre.interactable = true;

                });
            }

            aFini = false;
        }

    }

}