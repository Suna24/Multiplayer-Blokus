using UnityEngine.SceneManagement;
using UnityEngine;
public class SceneLoader : MonoBehaviour
{

    WebSocketClient webSocketClient = WebSocketClient.getInstance();

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
        //Envoi d'une requête pour connaître toutes les rooms existantes
        webSocketClient.GetWebSocket().Send(JsonUtility.ToJson(new Message("majRoom")));
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

    public void creerPartie()
    {
        SceneManager.LoadScene("Ecran_creation_partie");
    }

}
