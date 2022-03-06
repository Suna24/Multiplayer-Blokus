public class Piece3 : Piece
{
    public override int[,] disposition
    {
        get
        {
            int[,] disposition = new int[3, 1] {
                { 1 },
                { 1 },
                { 1 }
            };
            return disposition;
        }
    }
}