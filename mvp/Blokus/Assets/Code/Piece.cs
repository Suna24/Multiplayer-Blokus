using UnityEngine;

public class Piece : MonoBehaviour
{
    public bool estPosee = false;
    public int score;
    public float positionInitialeX;
    public float positionInitialeY;
    public Quaternion rotationInitiale;

    void Start()
    {
        positionInitialeX = transform.position.x;
        positionInitialeY = transform.position.y;
        rotationInitiale = transform.rotation;
    }

}
