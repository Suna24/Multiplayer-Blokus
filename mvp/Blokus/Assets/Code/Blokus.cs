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
    WebSocketClient webSocketClient;
    public Text tourCourant;

    // Start is called before the first frame update
    void Start()
    {

        webSocketClient = WebSocketClient.getInstance();

        webSocketClient.GetWebSocket().OnMessage += (sender, e) =>
        {
            Debug.Log("Message du serveur : " + e.Data);

            Message message = JsonUtility.FromJson<Message>(e.Data);

            switch (message.type)
            {
                case "joueur":

                    Message.MessageCreationJoueur messageCreationJoueur = JsonUtility.FromJson<Message.MessageCreationJoueur>(e.Data);

                    //Création du joueur
                    Couleur couleur = (Couleur)Enum.Parse(typeof(Couleur), messageCreationJoueur.couleurJouee.ToString());
                    joueur = new Joueur("Joueur " + e.Data, couleur);

                    Debug.Log(couleur.ToString());
                    Debug.Log(GameObject.FindGameObjectsWithTag(couleur.ToString()));

                    //On initialise sa liste de pièces
                    foreach (GameObject g in GameObject.FindGameObjectsWithTag(couleur.ToString()))
                    {
                        joueur.setDePieces.Add(g);
                    }

                    Debug.Log(joueur.setDePieces);

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
                        tourCourant.text = "C'est mon tour";
                    }
                    else
                    {
                        tourCourant.text = "Tour de " + (Couleur)Enum.Parse(typeof(Couleur), majInterface.couleurTour.ToString());
                    }
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
                    joueur.diminuerScore(piece.score);
                    Debug.Log(joueur.score);
                    piece.estPosee = true;
                    joueur.aFaitSonPremierPlacement = true;
                    joueur.tour = false;
                    Message.MessageMiseAJourPlateau message = new Message.MessageMiseAJourPlateau("plateau", ""/*NOM DELA ROOM*/, blokus);
                    string json = JsonConvert.SerializeObject(message, Formatting.Indented);
                    Debug.Log(json);
                    webSocketClient.GetWebSocket().Send(json);
                }
                else
                {
                    Debug.Log("Ne peut pas être placée ici");
                    estEnMain = false;
                    piece.transform.position = new Vector2(piece.positionInitialeX, piece.positionInitialeY);
                    piece.transform.rotation = piece.rotationInitiale;
                }
            }
            else
            {

                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

                if (hit.collider != null)
                {
                    piece = hit.collider.gameObject.GetComponent<Piece>();

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
                    choixQuartierDeCouleur(x, y, p);
                }
                else
                {
                    map.SetTile(p, sol);
                    blokus[x + 11, y + 12] = 0;
                }
            }
        }
    }

    public void choixQuartierDeCouleur(int x, int y, Vector3Int p)
    {
        if (x == -11 && y == 9)
        {
            map.SetTile(p, rouge);
            blokus[x + 11, y + 12] = 1;
        }
        else if (x == 10 && y == 9)
        {
            map.SetTile(p, bleu);
            blokus[x + 11, y + 12] = 2;
        }
        else if (x == -11 && y == -12)
        {
            map.SetTile(p, jaune);
            blokus[x + 11, y + 12] = 3;
        }
        else if (x == 10 && y == -12)
        {
            map.SetTile(p, vert);
            blokus[x + 11, y + 12] = 4;
        }
    }

    public bool placementCorrect(Piece piece, Vector3Int coordonnes)
    {

        bool aUneDiagonale = false;

        foreach (Transform enfant in piece.transform)
        {
            Vector3Int coord = grid.WorldToCell(enfant.transform.position);

            Debug.Log(coord.x + " " + coord.y);

            if (verificationPlacementHorsPlateau(coord) == true) return false;

            if (verificationChevauchementPiece(coord) == true) return false;

            if (verificationMemeCouleurACote(coord) == true) return false;

            if (verificationDiagonale(coord) == true)
            {
                aUneDiagonale = true;
            }

            ///Premier placement
            if (joueur.aFaitSonPremierPlacement == false)
            {
                switch (joueur.couleurJouee)
                {
                    case Couleur.ROUGE:
                        if (verificationPremierPlacement(coord, -10, 8) == true)
                        {
                            aUneDiagonale = true;
                        }
                        break;

                    case Couleur.BLEU:
                        if (verificationPremierPlacement(coord, 9, 8) == true)
                        {
                            aUneDiagonale = true;
                        }
                        break;

                    case Couleur.JAUNE:
                        if (verificationPremierPlacement(coord, -10, -11) == true)
                        {
                            aUneDiagonale = true;
                        }
                        break;

                    case Couleur.VERT:
                        if (verificationPremierPlacement(coord, 9, -11) == true)
                        {
                            aUneDiagonale = true;
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
        }

        return true;
    }

    public bool verificationPlacementHorsPlateau(Vector3Int coordonnees)
    {
        if (coordonnees.x < -10 || coordonnees.x >= 10 || coordonnees.y <= -12 || coordonnees.y > 8)
        {
            Debug.Log("Votre pièce est en dehors du plateau");
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool verificationChevauchementPiece(Vector3Int coordonnees)
    {
        if (blokus[coordonnees.x + 11, coordonnees.y + 12] != 0)
        {
            Debug.Log("Deux pièces se chevauchent");
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool verificationMemeCouleurACote(Vector3Int coordonnees)
    {
        if (blokus[coordonnees.x + 11 + 1, coordonnees.y + 12] == ((int)joueur.couleurJouee) ||
                blokus[coordonnees.x + 11 - 1, coordonnees.y + 12] == ((int)joueur.couleurJouee) ||
                blokus[coordonnees.x + 11, (coordonnees.y + 12) + 1] == ((int)joueur.couleurJouee) ||
                blokus[coordonnees.x + 11, (coordonnees.y + 12) - 1] == ((int)joueur.couleurJouee))
        {
            Debug.Log("Une pièce de la même couleur est directement à côté");
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool verificationDiagonale(Vector3Int coordonnees)
    {
        if (blokus[coordonnees.x + 11 + 1, coordonnees.y + 12 + 1] == ((int)joueur.couleurJouee) ||
                blokus[coordonnees.x + 11 - 1, coordonnees.y + 12 - 1] == ((int)joueur.couleurJouee) ||
                blokus[coordonnees.x + 11 + 1, coordonnees.y + 12 - 1] == ((int)joueur.couleurJouee) ||
                blokus[coordonnees.x + 11 - 1, coordonnees.y + 12 + 1] == ((int)joueur.couleurJouee))
        {
            Debug.Log("Diagonale trouvée");
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool verificationPremierPlacement(Vector3Int coordonnees, int x, int y)
    {
        if (coordonnees.x == x && coordonnees.y == y)
        {
            joueur.aFaitSonPremierPlacement = true;
            return true;
        }
        else
        {
            Debug.Log("Le premier placement n'est pas bon");
            return false;
        }
    }

    public void miseAJourDuPlateau()
    {
        //On parcourt tout le tableau tout en enlevant au fur et à mesure l'ancien
        for (int x = -11; x < 11; x++)
        {
            for (int y = -12; y < 10; y++)
            {
                Vector3Int p = new Vector3Int(x, y, 0);

                //On refresh toutes les tles du plateau
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
        if (webSocketClient.GetWebSocket() != null)
        {
            webSocketClient.GetWebSocket().Close();
        }
    }
}
