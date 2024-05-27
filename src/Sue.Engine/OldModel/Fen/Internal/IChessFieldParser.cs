namespace Sue.Engine.OldModel.Fen.Internal
{
    public interface IChessFieldParser
    {
        ChessField Parse(string chessFieldString);
    }
}