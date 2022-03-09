using System;

[Serializable]
public class Message
{
    public string id;

    public Message()
    {

    }

    public class MessageCreationJoueur
    {
        public string id;
        public int couleurJouee;

        public MessageCreationJoueur()
        {

        }
    }

    public class MessageMiseAJourPlateau
    {
        public string id;
        public int[,] plateau;

        public MessageMiseAJourPlateau()
        {

        }
    }

    public class MessageMiseAJourInterface
    {
        public string id;
        public bool tourCourant;
        public int couleurTour;

        public MessageMiseAJourInterface()
        {

        }
    }
}