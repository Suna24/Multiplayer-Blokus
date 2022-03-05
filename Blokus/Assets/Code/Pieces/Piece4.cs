public class Piece4 : Piece
{
    public override int[,] disposition
    {
        get
        {
            int[,] disposition = new int[2, 2] {
                { 1, 0 },
                { 1, 1 }
            };
            return disposition;
        }
    }
}