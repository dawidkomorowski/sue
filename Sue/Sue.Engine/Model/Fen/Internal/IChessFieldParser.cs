namespace Sue.Engine.Model.Fen.Internal
{
    public interface IChessFieldParser
    {
        ChessField Parse(string chessFieldString);
    }
}