using UnityEngine;
using System;

[Serializable]
public class Room
{
    public string id;
    public string nom;
    public int nombreDeJoueurs;

    public Room(string id, string nom, int nombreDeJoueurs)
    {
        this.id = id;
        this.nom = nom;
        this.nombreDeJoueurs = nombreDeJoueurs;
    }

}
