using UnityEngine;

public class Mouvement : MonoBehaviour
{

    bool estEnMain = false;
    int n = 0;
    float xPositionInitiale, yPositionInitiale;
    Piece piece = new Piece();

    // Start is called before the first frame update
    void Start()
    {
        xPositionInitiale = transform.position.x;
        yPositionInitiale = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {

        if (estEnMain == true)
        {
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(Mathf.Round(cursorPos.x) - 0.5f, Mathf.Round(cursorPos.y) - 0.5f);


            if (Input.GetMouseButtonDown(1))
            {
                transform.Rotate(0, 0, 90, Space.World);
            }

        }
    }

    void OnMouseDown()
    {
        if (piece.estPosee == false)
        {
            n++;

            if (n % 2 == 0)
            {
                estEnMain = false;
                piece.estPosee = true;
            }
            else
            {
                estEnMain = true;
            }
        }
    }
}
