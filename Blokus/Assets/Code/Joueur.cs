using System;
using System.Collections.Generic;

[Serializable]
public class Joueur
{
    //Attributs
    public string nom { get; }
    public int score;
    public Couleur couleurJouee { get; }
    public List<Piece> setDePieces;
    public bool aFaitSonPremierPlacement { get; }

    public Joueur(string nom, Couleur couleurJouee, List<Piece> setDePieces)
    {
        this.nom = nom;
        this.couleurJouee = couleurJouee;
        this.aFaitSonPremierPlacement = false;
        this.score = 89;

        this.setDePieces = new List<Piece>();
        this.initialiserSetDePieces();
    }

    public void diminuerScore(int score)
    {
        this.score -= score;
    }

    public void initialiserSetDePieces()
    {
        //TODO Add les Pieces à la liste
    }
}
