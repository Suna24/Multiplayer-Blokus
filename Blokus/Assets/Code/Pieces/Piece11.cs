public class Piece11 : Piece
{
    public override int[,] disposition
    {
        get
        {
            int[,] disposition = new int[3, 2] {
                { 1, 1 },
                { 1, 0 },
                { 1, 1 }
            };
            return disposition;
        }
    }
}