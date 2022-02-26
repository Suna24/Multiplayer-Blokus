using UnityEngine;

public class Mouvement : MonoBehaviour
{

    bool estEnMain = false;
    bool doitRevenirASaPositionDOrigine = false;
    int n = 0;
    float xPositionInitiale, yPositionInitiale;
    Piece piece = new Piece();
    Quaternion rotationOriginale;

    // Start is called before the first frame update
    void Start()
    {
        xPositionInitiale = transform.position.x;
        yPositionInitiale = transform.position.y;
        rotationOriginale = transform.rotation;
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

        if (doitRevenirASaPositionDOrigine == true)
        {
            transform.position = new Vector2(xPositionInitiale, yPositionInitiale);
            estEnMain = false;
            piece.estPosee = false;
            doitRevenirASaPositionDOrigine = false;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationOriginale, Time.time * 1.0f);
        }
    }

    void OnMouseDown()
    {
        if (piece.estPosee == false)
        {
            n++;
            if (n % 2 == 0)
            {
                if (transform.position.x >= -10 && transform.position.x <= 10 && transform.position.y >= -12 && transform.position.y <= 8)
                {
                    estEnMain = false;
                    piece.estPosee = true;
                }
                else
                {
                    Debug.Log("Ne peut pas être placée ici");
                    doitRevenirASaPositionDOrigine = true;
                }
            }
            else
            {
                estEnMain = true;
            }
        }
    }
}
