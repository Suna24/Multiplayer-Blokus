public class Piece7 : Piece
{
    public override int[,] disposition
    {
        get
        {
            int[,] disposition = new int[3, 2] {
                { 1, 0 },
                { 1, 1 },
                { 1, 0 }
            };
            return disposition;
        }
    }
}