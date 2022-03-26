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
    bool roomExiste = false;

    public void Start()
    {

        webSocketClient = WebSocketClient.webSocketClient;

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
            if (e.Data == "Room")
            {
                roomExiste = true;
            }
        };

    }

    public void demarrerPartie()
    {
        if (nomDeLaPartie.Trim() != "" && pseudo.Trim() != "" && roomExiste == false)
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
            webSocketClient.GetWebSocket().Send(JsonUtility.ToJson(messageCreationRoom));

        }
        else
        {
            Debug.Log("Tous les champs n'ont pas été remplis");
        }

    }
}
