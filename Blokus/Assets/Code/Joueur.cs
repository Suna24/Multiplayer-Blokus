using System;
using System.Collections.Generic;

[Serializable]
public class Joueur
{
    //Attributs
    public string nom { get; }
    public int score;
    public Couleur couleurJouee { get; }
    public bool aFaitSonPremierPlacement { get; }

    public Joueur(string nom, Couleur couleurJouee)
    {
        this.nom = nom;
        this.couleurJouee = couleurJouee;
        this.aFaitSonPremierPlacement = false;
        this.score = 89;
    }

    public void diminuerScore(int score)
    {
        this.score -= score;
    }
}
