namespace Sue.Common.Model.Fen.Internal
{
    internal interface IChessFieldParser
    {
        ChessField Parse(string chessFieldString);
    }
}