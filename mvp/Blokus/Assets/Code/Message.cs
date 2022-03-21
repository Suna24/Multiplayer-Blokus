using System;

[Serializable]
public class Message
{
    public string type;

    public Message()
    {
    }

    public Message(string type)
    {
        this.type = type;
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
        public string nomRoom;
        public int[,] plateau;

        public MessageMiseAJourPlateau()
        {

        }

        public MessageMiseAJourPlateau(string type, string nomRoom, int[,] plateau)
        {
            this.type = type;
            this.nomRoom = nomRoom;
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
        public string nom;
        public string pseudo;
        public int nombreDeJoueurs;

        public MessageCreationRoom(string type, string nom, string pseudo, int nombreDeJoueurs)
        {
            this.type = type;
            this.nom = nom;
            this.pseudo = pseudo;
            this.nombreDeJoueurs = nombreDeJoueurs;
        }
    }

    public class MessageRoom
    {
        public string nom;
        public int nbJoueursTotal;
        public int nbJoueursCourant;

        public MessageRoom()
        {

        }
    }

    public class MessageRooms
    {
        public MessageRoom[] rooms;

        public MessageRooms()
        {

        }
    }
}