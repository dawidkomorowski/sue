namespace Sue.Engine.OldModel.Fen.Internal
{
    public interface IChessPieceParser
    {
        ChessPiece Parse(char fenChessPieceCode);
    }
}