using System;

[Serializable]
public class Joueur
{
    //Attributs
    public string nom;
    public int score;
    public string couleurJouee;

    public Joueur(string nom, int score, string couleurJouee)
    {
        this.nom = nom;
        this.score = score;
        this.couleurJouee = couleurJouee;
    }
}
