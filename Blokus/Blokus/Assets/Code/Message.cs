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
        public string nomRoom;

        public MessageCreationJoueur()
        {

        }
    }

    public class MessageMiseAJourPlateau
    {
        public string type;
        public string nomRoom;

        public int score;
        public int[,] plateau;

        public MessageMiseAJourPlateau()
        {

        }

        public MessageMiseAJourPlateau(string type, int[,] plateau)
        {
            this.type = type;
            this.plateau = plateau;
        }

        public MessageMiseAJourPlateau(string type, int score, string nomRoom, int[,] plateau)
        {
            this.type = type;
            this.score = score;
            this.nomRoom = nomRoom;
            this.plateau = plateau;
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
        public string type;
        public MessageRoom[] rooms;

        public MessageRooms()
        {

        }
    }

    public class MessageJoinRoom
    {
        public string type;
        public string nomRoom;

        public MessageJoinRoom(string type, string nomRoom)
        {
            this.type = type;
            this.nomRoom = nomRoom;
        }
    }

    public class MessageScores
    {
        public string type;
        public string nomRoom;
        public string scores;

        public MessageScores()
        {

        }

        public MessageScores(string type, string nomRoom)
        {
            this.type = type;
            this.nomRoom = nomRoom;
        }
    }
}