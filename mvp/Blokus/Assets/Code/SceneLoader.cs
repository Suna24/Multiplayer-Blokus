using UnityEngine.SceneManagement;
using UnityEngine;
public class SceneLoader : MonoBehaviour
{
    public void demarrerPartie()
    {
        SceneManager.LoadScene("Ecran_de_jeu");
    }

    public void quitter()
    {
        if (UnityEditor.EditorApplication.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }

    }

    public void commentJouer()
    {
        SceneManager.LoadScene("Ecran_de_regles");
    }

    public void rejoindreUnePartie()
    {
        Debug.Log("Pas implémenté pour le moment");
    }

    public void retourAccueil()
    {
        SceneManager.LoadScene("Ecran_d_accueil");
    }

    public void ouvrirReglesDuJeu()
    {
        Application.OpenURL("https://www.jeuxavolonte.asso.fr/regles/blokus.pdf");
    }
}
