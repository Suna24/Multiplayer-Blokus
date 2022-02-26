using UnityEngine;

public class Piece
{
    public int[,] disposition { get; }
    public bool estPosee { get; set; }

    public Piece(int[,] disposition)
    {
        this.disposition = disposition;
        estPosee = false;
    }

    public Piece()
    {
        estPosee = false;
    }

    //public abstract void tourner();
}
