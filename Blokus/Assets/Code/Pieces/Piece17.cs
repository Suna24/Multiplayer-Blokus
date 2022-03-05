public class Piece17 : Piece
{
    public override int[,] disposition
    {
        get
        {
            int[,] disposition = new int[3, 3] {
                { 1, 0, 0 },
                { 1, 0, 0 },
                { 1, 1, 1 }
            };
            return disposition;
        }
    }
}