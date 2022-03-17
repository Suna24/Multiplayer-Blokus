using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using System.Collections.Generic;
using System;
public class SceneLoader : MonoBehaviour
{
    InputField nomDeLaPartieInput;
    InputField pseudoInput;
    public static WebSocket webSocket;
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

    }

    public void demarrerPartie()
    {
        if (nomDeLaPartie.Trim() != "" && pseudo.Trim() != "")
        {
            SceneManager.LoadScene("Ecran_de_jeu");
        }
        else
        {
            Debug.Log("Tous les champs n'ont pas été remplis");
        }
    }

    public void quitter()
    {
        Application.Quit();
    }

    public void commentJouer()
    {
        SceneManager.LoadScene("Ecran_de_regles");
    }

    public void rejoindreUnePartie()
    {
        SceneManager.LoadScene("Ecran_rejoindre_partie");
    }

    public void retourAccueil()
    {
        SceneManager.LoadScene("Ecran_d_accueil");
    }

    public void ouvrirReglesDuJeu()
    {
        Application.OpenURL("https://www.jeuxavolonte.asso.fr/regles/blokus.pdf");
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
            webSocket = new WebSocket("ws://localhost:3000");
            webSocket.Connect();

            webSocket.Send(JsonUtility.ToJson(messageCreationRoom));
        }
        else
        {
            Debug.Log("Tous les champs n'ont pas été remplis");
        }

    }
}
