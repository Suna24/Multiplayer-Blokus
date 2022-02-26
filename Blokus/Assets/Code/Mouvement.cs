using UnityEngine;

public class Mouvement : MonoBehaviour
{

    bool estEnMain = false;
    bool doitRevenirASaPositionDOrigine = false;
    int n = 0;
    float xPositionInitiale, yPositionInitiale;
    Piece piece = new Piece();
    Quaternion rotationOriginale;

    PolygonCollider2D m_Collider;
    Vector3 m_Center;
    Vector3 m_Size, m_Min, m_Max;

    // Start is called before the first frame update
    void Start()
    {
        xPositionInitiale = transform.position.x;
        yPositionInitiale = transform.position.y;
        rotationOriginale = transform.rotation;

        //Fetch the Collider from the GameObject
        m_Collider = GetComponent<PolygonCollider2D>();
        //Fetch the center of the Collider volume
        m_Center = m_Collider.bounds.center;
        //Fetch the size of the Collider volume
        m_Size = m_Collider.bounds.size;
    }

    // Update is called once per frame
    void Update()
    {

        //Recalcul de la size et du centre lors de la rotation (x et y inversés)
        m_Center = m_Collider.bounds.center;
        m_Size = m_Collider.bounds.size;

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
                if (m_Center.x + (m_Size.x / 2) >= -10 && m_Center.x + (m_Size.x / 2) <= 10
                && m_Center.x - (m_Size.x / 2) >= -10 && m_Center.x - (m_Size.x / 2) <= 10
                && m_Center.y + (m_Size.y / 2) >= -12 && m_Center.y + (m_Size.y / 2) <= 8
                && m_Center.y - (m_Size.y / 2) >= -12 && m_Center.y - (m_Size.y / 2) <= 8)
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
