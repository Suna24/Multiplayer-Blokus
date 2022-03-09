using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Tilemaps;
using WebSocketSharp;
using System;
using Newtonsoft.Json;

public class Blokus : MonoBehaviour
{
    public Grid grid;
    public Tilemap map;
    public Tile bordure, sol, rouge, bleu, jaune, vert;
    public int[,] blokus = new int[22, 22];
    Piece piece;
    bool estEnMain = false;
    Vector3Int coordinate;
    public Joueur joueur;
    float positionInitialeX, positionInitialeY;
    Quaternion rotationInitiale;
    WebSocket webSocket;
    public Text tourCourant;

    // Start is called before the first frame update
    void Start()
    {
        //Gestion de la connection
        webSocket = new WebSocket("ws://localhost:3000");
        webSocket.Connect();

        webSocket.OnMessage += (sender, e) =>
        {
            Debug.Log("Message du serveur : " + e.Data);

            Message message = JsonUtility.FromJson<Message>(e.Data);

            switch (message.id)
            {
                case "joueur":

                    Message.MessageCreationJoueur messageCreationJoueur = JsonUtility.FromJson<Message.MessageCreationJoueur>(e.Data);

                    //Création du joueur
                    Couleur couleur = (Couleur)Enum.Parse(typeof(Couleur), messageCreationJoueur.couleurJouee.ToString());
                    joueur = new Joueur("Joueur " + e.Data, couleur);

                    //Si c'est le premier joueur, alors c'est son tour
                    if (couleur.Equals(Couleur.ROUGE))
                    {
                        joueur.tour = true;
                    }

                    break;

                case "plateau":

                    Message.MessageMiseAJourPlateau messageMiseAJourPlateau = JsonConvert.DeserializeObject<Message.MessageMiseAJourPlateau>(e.Data);

                    for (int x = 0; x < 22; x++)
                    {
                        for (int y = 0; y < 22; y++)
                        {
                            Debug.Log(messageMiseAJourPlateau.plateau[x, y]);
                            blokus[x, y] = messageMiseAJourPlateau.plateau[x, y];
                        }
                    }
                    miseAJourDuPlateau();
                    break;

                case "tour":

                    Message.MessageMiseAJourInterface majInterface = JsonUtility.FromJson<Message.MessageMiseAJourInterface>(e.Data);

                    if (majInterface.tourCourant == true)
                    {
                        joueur.tour = true;
                    }
                    tourCourant.text = "Tour de " + (Couleur)Enum.Parse(typeof(Couleur), majInterface.couleurTour.ToString());
                    break;

                default:
                    Debug.Log("Message inconnu");
                    break;
            }
        };

        //Création de la map
        creationDuPlateau();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && joueur.tour == true)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            coordinate = grid.WorldToCell(pos);

            if (estEnMain == true)
            {
                if (coordinate.x >= -10 && coordinate.x <= 9 && coordinate.y >= -11 && coordinate.y <= 8 && placementCorrect(piece, coordinate))
                {
                    estEnMain = false;
                    piece.estPosee = true;
                    joueur.aFaitSonPremierPlacement = true;
                    joueur.tour = false;
                    string json = JsonConvert.SerializeObject(blokus, Formatting.Indented);
                    Debug.Log(json);
                    webSocket.Send(json);
                }
                else
                {
                    Debug.Log("Ne peut pas être placée ici");
                    estEnMain = false;
                    piece.transform.position = new Vector2(positionInitialeX, positionInitialeY);
                    piece.transform.rotation = rotationInitiale;
                }
            }
            else
            {

                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

                if (hit.collider != null)
                {
                    piece = hit.collider.gameObject.GetComponent<Piece>();

                    positionInitialeX = piece.transform.position.x;
                    positionInitialeY = piece.transform.position.y;
                    rotationInitiale = piece.transform.rotation;

                    //On vérifie que la piece existe, qu'elle n'est pas posée définitivement sur le plateau et qu'elle fait partie
                    //du set de pieces du joueur (autrement dit sa couleur)
                    if (piece != null && !piece.estPosee && piece.transform.tag.CompareTo(joueur.couleurJouee.ToString()) == 0)
                    {
                        estEnMain = true;
                    }

                }
            }
        }

        if (Input.GetMouseButtonDown(0) && joueur.tour == false)
        {
            Debug.Log("Ce n'est pas votre tour");
        }

        if (estEnMain == true)
        {
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            piece.transform.position = new Vector2(Mathf.Round(cursorPos.x) - 0.5f, Mathf.Round(cursorPos.y) - 0.5f);

            if (Input.GetMouseButtonDown(1))
            {
                piece.transform.Rotate(0, 0, 90);
            }
        }

    }

    public void creationDuPlateau()
    {
        for (int x = -11; x < 11; x++)
        {
            for (int y = -12; y < 10; y++)
            {
                Vector3Int p = new Vector3Int(x, y, 0);

                if (x == -11 || x == 10 || y == 9 || y == -12)
                {
                    map.SetTile(p, bordure);
                    blokus[x + 11, y + 12] = 10;
                }
                else
                {
                    map.SetTile(p, sol);
                    blokus[x + 11, y + 12] = 0;
                }
            }
        }
    }

    public bool placementCorrect(Piece piece, Vector3Int coordonnes)
    {

        bool aUneDiagonale = false;

        foreach (Transform enfant in piece.transform)
        {
            Vector3Int coord = grid.WorldToCell(enfant.transform.position);

            Debug.Log(coord.x + " " + coord.y);

            if (coord.x < -10 || coord.x >= 10 || coord.y <= -12 || coord.y > 8)
            {
                return false;
            }

            //Vérifie si deux pièces se chevauchent
            if (blokus[coord.x + 11, coord.y + 12] != 0)
            {
                Debug.Log("Deux pièces se chevauchent");
                return false;
            }

            //Vérifie s'il y a une pièce de la même couleur directement à côté
            if (blokus[coord.x + 11 + 1, coord.y + 12] == ((int)joueur.couleurJouee) ||
                blokus[coord.x + 11 - 1, coord.y + 12] == ((int)joueur.couleurJouee) ||
                blokus[coord.x + 11, (coord.y + 12) + 1] == ((int)joueur.couleurJouee) ||
                blokus[coord.x + 11, (coord.y + 12) - 1] == ((int)joueur.couleurJouee))
            {
                Debug.Log("Une pièce de la même couleur est directement à côté");
                return false;
            }

            //Vérifie s'il y a une piece de la même couleur en diagonale
            if (blokus[coord.x + 11 + 1, coord.y + 12 + 1] == ((int)joueur.couleurJouee) ||
                blokus[coord.x + 11 - 1, coord.y + 12 - 1] == ((int)joueur.couleurJouee) ||
                blokus[coord.x + 11 + 1, coord.y + 12 - 1] == ((int)joueur.couleurJouee) ||
                blokus[coord.x + 11 - 1, coord.y + 12 + 1] == ((int)joueur.couleurJouee))
            {
                aUneDiagonale = true;
                Debug.Log("Diagonale trouvée");
            }

            ///Premier placement
            if (joueur.aFaitSonPremierPlacement == false)
            {
                switch (joueur.couleurJouee)
                {
                    case Couleur.ROUGE:
                        if (coord.x == -10 && coord.y == 8)
                        {
                            aUneDiagonale = true;
                            joueur.aFaitSonPremierPlacement = true;
                        }
                        else
                        {
                            Debug.Log("Le premier placement n'est pas bon");
                        }
                        break;

                    case Couleur.BLEU:
                        if (coord.x == 9 && coord.y == 8)
                        {
                            aUneDiagonale = true;
                            joueur.aFaitSonPremierPlacement = true;
                        }
                        else
                        {
                            Debug.Log("Le premier placement n'est pas bon");
                        }
                        break;

                    case Couleur.JAUNE:
                        if (coord.x == -10 && coord.y == -11)
                        {
                            aUneDiagonale = true;
                            joueur.aFaitSonPremierPlacement = true;
                        }
                        else
                        {
                            Debug.Log("Le premier placement n'est pas bon");
                        }
                        break;

                    case Couleur.VERT:
                        if (coord.x == 9 && coord.y == -11)
                        {
                            aUneDiagonale = true;
                            joueur.aFaitSonPremierPlacement = true;
                        }
                        else
                        {
                            Debug.Log("Le premier placement n'est pas bon");
                        }
                        break;
                }
            }

        }

        if (aUneDiagonale == false)
        {
            Debug.Log("N'est pas connecté en diagonale avec une pièce de votre couleur");
            return false;
        }

        foreach (Transform enfant in piece.transform)
        {
            Vector3Int coord = grid.WorldToCell(enfant.transform.position);

            blokus[coord.x + 11, coord.y + 12] = ((int)joueur.couleurJouee);
            Debug.Log(blokus[coord.x + 11, coord.y + 12]);
        }

        return true;
    }

    public void miseAJourDuPlateau()
    {
        //On parcourt tout le tableau tout en enlevant au fur et à mesure l'ancien
        for (int x = -11; x < 11; x++)
        {
            for (int y = -12; y < 10; y++)
            {
                Vector3Int p = new Vector3Int(x, y, 0);
                map.RefreshTile(p);

                //On regarde quel nombre est stocké dans le tableau
                switch (blokus[x + 11, y + 12])
                {
                    //Dans le cas où il s'agit d'une bordure
                    case 10:
                        map.SetTile(p, bordure);
                        break;

                    //Dans le cas où il s'agit du sol
                    case 0:
                        map.SetTile(p, sol);
                        break;

                    //Dans le cas où il s'agit d'une pièce rouge
                    case 1:
                        map.SetTile(p, rouge);
                        break;

                    //Dans le cas où il s'agit d'une pièce bleu
                    case 2:
                        map.SetTile(p, bleu);
                        break;

                    //Dans le cas où il s'agit d'une pièce jaune
                    case 3:
                        map.SetTile(p, jaune);
                        break;

                    //Dans le cas où il s'agit d'une pièce vert
                    case 4:
                        map.SetTile(p, vert);
                        break;
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (webSocket != null)
        {
            webSocket.Close();
        }
    }
}
