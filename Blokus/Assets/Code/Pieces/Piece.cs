using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public abstract int[,] disposition { get; }

    public bool estPosee = false;

}
