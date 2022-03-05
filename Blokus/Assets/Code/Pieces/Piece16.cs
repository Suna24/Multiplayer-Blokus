public class Piece16 : Piece
{
    public override int[,] disposition
    {
        get
        {
            int[,] disposition = new int[3, 3] {
                { 0, 1, 0 },
                { 0, 1, 0 },
                { 1, 1, 1 }
            };
            return disposition;
        }
    }
}