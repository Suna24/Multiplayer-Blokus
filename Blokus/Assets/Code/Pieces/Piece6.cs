public class Piece6 : Piece
{
    public override int[,] disposition
    {
        get
        {
            int[,] disposition = new int[3, 2] {
                { 0, 1 },
                { 0, 1 },
                { 1, 1 }
            };
            return disposition;
        }
    }
}