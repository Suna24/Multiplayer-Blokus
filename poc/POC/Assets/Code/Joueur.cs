using System;
using UnityEngine;

[Serializable]
public class Joueur : MonoBehaviour
{
    //Attributs
    private string nom { get; }
    private int score { get; }
    private string couleurJouee { get; }

    public Joueur(string nom, int score, string couleurJouee)
    {
        this.nom = nom;
        this.score = score;
        this.couleurJouee = couleurJouee;
    }
}
