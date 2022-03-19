using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class RoomCreation : MonoBehaviour
{
    InputField nomDeLaPartieInput;
    InputField pseudoInput;
    WebSocketClient webSocketClient;
    string nomDeLaPartie = "", pseudo = "";

    public void Start()
    {

        nomDeLaPartieInput = GameObject.Find("NomDeLaPartie").GetComponent<InputField>();
        pseudoInput = GameObject.Find("Pseudo").GetComponent<InputField>();

        nomDeLaPartieInput.onValueChanged.AddListener(delegate
        {
            nomDeLaPartie = nomDeLaPartieInput.text;
        });

        pseudoInput.onValueChanged.AddListener(delegate
        {
            pseudo = pseudoInput.text;
        });

        webSocketClient.GetWebSocket().OnMessage += (sender, e) =>
        {
            Debug.Log(e.Data);
        };

    }

    public void Update()
    {
        Debug.Log(webSocketClient.GetWebSocket().ReadyState);
    }

    public void demarrerPartie()
    {
        if (nomDeLaPartie.Trim() != "" && pseudo.Trim() != "")
        {
            creationRoom();
            SceneManager.LoadScene("Ecran_de_jeu");
        }
        else
        {
            Debug.Log("Tous les champs n'ont pas été remplis");
        }
    }

    public void creationRoom()
    {

        if (nomDeLaPartie.Trim() != "" && pseudo.Trim() != "")
        {

            int indexOptions = GameObject.Find("NombreJoueurs").GetComponent<Dropdown>().value;
            List<Dropdown.OptionData> options = GameObject.Find("NombreJoueurs").GetComponent<Dropdown>().options;

            int nombreDeJoueurs = Int32.Parse(options[indexOptions].text.Substring(0, 1));

            Message.MessageCreationRoom messageCreationRoom = new Message.MessageCreationRoom("creationRoom", nomDeLaPartie, pseudo, nombreDeJoueurs);

            //Gestion de la connection
            webSocketClient = WebSocketClient.getInstance();
            webSocketClient.GetWebSocket().Connect();

            webSocketClient.GetWebSocket().Send(JsonUtility.ToJson(messageCreationRoom));

        }
        else
        {
            Debug.Log("Tous les champs n'ont pas été remplis");
        }

    }
}
