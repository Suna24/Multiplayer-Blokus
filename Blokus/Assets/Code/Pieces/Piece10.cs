public class Piece10 : Piece
{
    public override int[,] disposition
    {
        get
        {
            int[,] disposition = new int[5, 1] {
                { 1 },
                { 1 },
                { 1 },
                { 1 },
                { 1 }
            };
            return disposition;
        }
    }
}