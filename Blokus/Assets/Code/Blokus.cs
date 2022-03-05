using UnityEngine;
using UnityEngine.Tilemaps;

public class Blokus : MonoBehaviour
{

    public Grid grid;
    public Tilemap map;
    public Tile sol;
    public int[,] blokus = new int[20, 20];
    Piece piece;
    bool estEnMain = false;

    // Start is called before the first frame update
    void Start()
    {

        //Création de la map
        for (int x = -10; x < 10; x++)
        {
            for (int y = 8; y > -12; y--)
            {
                Vector3Int p = new Vector3Int(x, y, 0);
                map.SetTile(p, sol);
                blokus[x + 10, 20 - (y + 12)] = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (estEnMain == true)
            {
                estEnMain = false;
            }
            else
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
                Vector3Int coordinate = grid.WorldToCell(pos);
                Debug.Log(coordinate.x + " " + coordinate.y);

                if (hit.collider != null)
                {
                    piece = hit.collider.gameObject.GetComponent<Piece>();
                    Debug.Log(hit.collider.gameObject.name);
                    Debug.Log(piece);

                    if (piece != null)
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
}
