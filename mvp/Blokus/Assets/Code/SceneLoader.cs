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
}
