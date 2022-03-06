public class Piece18 : Piece
{
    public override int[,] disposition
    {
        get
        {
            int[,] disposition = new int[3, 3] {
                { 1, 1, 0 },
                { 0, 1, 1 },
                { 0, 0, 1 }
            };
            return disposition;
        }
    }
}