public class Piece21 : Piece
{
    public override int[,] disposition
    {
        get
        {
            int[,] disposition = new int[3, 3] {
                { 0, 1, 0 },
                { 1, 1, 1 },
                { 0, 1, 0 }
            };
            return disposition;
        }
    }
}