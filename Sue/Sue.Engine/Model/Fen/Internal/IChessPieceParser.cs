namespace Sue.Common.Model.Fen.Internal
{
    public interface IChessPieceParser
    {
        ChessPiece Parse(char fenChessPieceCode);
    }
}