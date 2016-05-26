namespace Sue.Common.Model.Fen.Internal
{
    internal interface IChessPieceParser
    {
        ChessPiece Parse(char fenChessPieceCode);
    }
}