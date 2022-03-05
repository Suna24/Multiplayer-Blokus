public class Piece5 : Piece
{
    public override int[,] disposition
    {
        get
        {
            int[,] disposition = new int[4, 1] {
                { 1 },
                { 1 },
                { 1 },
                { 1 }
            };
            return disposition;
        }
    }
}