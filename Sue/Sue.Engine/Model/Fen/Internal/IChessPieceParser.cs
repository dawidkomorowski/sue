namespace Sue.Engine.Model.Fen.Internal
{
    public interface IChessPieceParser
    {
        ChessPiece Parse(char fenChessPieceCode);
    }
}