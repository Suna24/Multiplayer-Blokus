public class Piece12 : Piece
{
    public override int[,] disposition
    {
        get
        {
            int[,] disposition = new int[4, 2] {
                { 0, 1 },
                { 0, 1 },
                { 0, 1 },
                { 1, 1}
            };
            return disposition;
        }
    }
}