using System;

[Serializable]
public class Message
{
    public string type;

    public Message()
    {

    }

    public class MessageCreationJoueur
    {
        public string type;
        public int couleurJouee;

        public MessageCreationJoueur()
        {

        }
    }

    public class MessageMiseAJourPlateau
    {
        public string type;
        public string id;
        public int[,] plateau;

        public MessageMiseAJourPlateau()
        {

        }

        public MessageMiseAJourPlateau(string type, int[,] plateau)
        {
            this.type = type;
            this.plateau = plateau;
        }
    }

    public class MessageMiseAJourInterface
    {
        public string type;
        public bool tourCourant;
        public int couleurTour;

        public MessageMiseAJourInterface()
        {

        }
    }

    public class MessageCreationRoom
    {
        public string type;
        public string id;
        public string nom;
        public string pseudo;
        public int nombreDeJoueurs;

        public MessageCreationRoom(string type, string id, string nom, string pseudo, int nombreDeJoueurs)
        {
            this.type = type;
            this.id = id;
            this.nom = nom;
            this.pseudo = pseudo;
            this.nombreDeJoueurs = nombreDeJoueurs;
        }
    }
}