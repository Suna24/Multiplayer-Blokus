public class Piece1 : Piece
{
    public override int[,] disposition
    {
        get
        {
            int[,] disposition = new int[1, 1] { { 1 } };
            return disposition;
        }
    }
}