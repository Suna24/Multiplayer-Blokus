public class Piece8 : Piece
{
    public override int[,] disposition
    {
        get
        {
            int[,] disposition = new int[2, 2] {
                { 1, 1 },
                { 1, 1 }
            };
            return disposition;
        }
    }
}