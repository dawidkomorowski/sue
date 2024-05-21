namespace Sue.Common.Model.Fen.Internal
{
    public interface IChessFieldParser
    {
        ChessField Parse(string chessFieldString);
    }
}