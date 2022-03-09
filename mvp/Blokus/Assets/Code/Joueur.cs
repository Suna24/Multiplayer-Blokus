using System;
using System.Collections.Generic;

[Serializable]
public class Joueur
{
    //Attributs
    public string nom { get; set; }
    public int score;
    public Couleur couleurJouee { get; }
    public bool aFaitSonPremierPlacement { get; set; }

    public bool tour { get; set; }
    public Joueur(string nom, Couleur couleurJouee)
    {
        this.nom = nom;
        this.couleurJouee = couleurJouee;
        this.aFaitSonPremierPlacement = false;
        this.score = 89;
        this.tour = false;
    }

    public void diminuerScore(int score)
    {
        this.score -= score;
    }
}
