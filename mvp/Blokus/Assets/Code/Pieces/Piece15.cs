public class Piece15 : Piece
{
    public override int[,] disposition
    {
        get
        {
            int[,] disposition = new int[4, 2] {
                { 1, 0 },
                { 1, 1 },
                { 1, 0 },
                { 1, 0}
            };
            return disposition;
        }
    }
}