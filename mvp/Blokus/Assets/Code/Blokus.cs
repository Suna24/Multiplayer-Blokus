using UnityEngine;
using UnityEngine.Tilemaps;

public class Blokus : MonoBehaviour
{
    public Grid grid;
    public Tilemap map;
    public Tile bordure;
    public Tile sol;
    public int[,] blokus = new int[22, 22];
    Piece piece;
    bool estEnMain = false, premierPlacement = false;
    Vector3Int coordinate;
    public Joueur joueur;
    float positionInitialeX, positionInitialeY;
    Quaternion rotationInitiale;

    // Start is called before the first frame update
    void Start()
    {

        //Création de la map
        for (int x = -11; x < 11; x++)
        {
            for (int y = 9; y > -13; y--)
            {
                Vector3Int p = new Vector3Int(x, y, 0);

                if (x == -11 || x == 10 || y == 9 || y == -12)
                {
                    map.SetTile(p, bordure);
                    blokus[x + 11, 22 - (y + 13)] = 10;
                }
                else
                {
                    map.SetTile(p, sol);
                    blokus[x + 11, 22 - (y + 13)] = 0;
                }
            }
        }

        //Création d'un joueur
        joueur = new Joueur("Joueur1", Couleur.ROUGE);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            coordinate = grid.WorldToCell(pos);

            if (estEnMain == true)
            {
                if (coordinate.x >= -10 && coordinate.x <= 10 && coordinate.y >= -12 && coordinate.y <= 8 && placementCorrect(piece, coordinate))
                {
                    estEnMain = false;
                    piece.estPosee = true;
                    joueur.aFaitSonPremierPlacement = true;
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

                    if (piece != null && !piece.estPosee)
                    {
                        estEnMain = true;
                    }

                }
            }
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
            if (blokus[coord.x + 11, 22 - (coord.y + 12)] != 0)
            {
                Debug.Log("Deux pièces se chevauchent");
                return false;
            }

            //Vérifie s'il y a une pièce de la même couleur directement à côté
            if (blokus[coord.x + 11 + 1, 22 - (coord.y + 12)] == ((int)joueur.couleurJouee) ||
                blokus[coord.x + 11 - 1, 22 - (coord.y + 12)] == ((int)joueur.couleurJouee) ||
                blokus[coord.x + 11, 22 - (coord.y + 12) + 1] == ((int)joueur.couleurJouee) ||
                blokus[coord.x + 11, 22 - (coord.y + 12) - 1] == ((int)joueur.couleurJouee))
            {
                Debug.Log("Une pièce de la même couleur est directement à côté");
                return false;
            }

            //Vérifie s'il y a une piece de la même couleur en diagonale
            if (blokus[coord.x + 11 + 1, 22 - (coord.y + 12) + 1] == ((int)joueur.couleurJouee) ||
                blokus[coord.x + 11 - 1, 22 - (coord.y + 12) - 1] == ((int)joueur.couleurJouee) ||
                blokus[coord.x + 11 + 1, 22 - (coord.y + 12) - 1] == ((int)joueur.couleurJouee) ||
                blokus[coord.x + 11 - 1, 22 - (coord.y + 12) + 1] == ((int)joueur.couleurJouee))
            {
                aUneDiagonale = true;
                Debug.Log("Diagonale trouvée");
            }

            //TODO refaire le premier placement
            if (joueur.aFaitSonPremierPlacement == false)
            {
                if ((coord.x == -10 && coord.y == 8) || (coord.x == -10 && coord.y == -11)
                    || (coord.x == 10 && coord.y == 8) || (coord.x == 10 && coord.y == -11))
                {
                    premierPlacement = true;
                    aUneDiagonale = true;
                    joueur.aFaitSonPremierPlacement = true;
                }
            }

        }

        if (aUneDiagonale == false)
        {
            Debug.Log("N'est pas connecté en diagonale avec une pièce de votre couleur");
            return false;
        }

        if (premierPlacement == false)
        {
            Debug.Log("Votre premier placement doit toucher un coin du plateau");
            return false;
        }

        foreach (Transform enfant in piece.transform)
        {
            Vector3Int coord = grid.WorldToCell(enfant.transform.position);

            blokus[coord.x + 11, 22 - (coord.y + 12)] = ((int)joueur.couleurJouee);
            Debug.Log(blokus[coord.x + 11, 22 - (coord.y + 12)]);
        }

        return true;
    }
}
