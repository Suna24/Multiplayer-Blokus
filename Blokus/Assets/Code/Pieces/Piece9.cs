public class Piece9 : Piece
{
    public override int[,] disposition
    {
        get
        {
            int[,] disposition = new int[2, 3] {
                { 1, 1, 0 },
                { 0, 1, 1 }
            };
            return disposition;
        }
    }
}