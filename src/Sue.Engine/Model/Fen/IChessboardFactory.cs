using Sue.Engine.Model.Chessboard;

namespace Sue.Engine.Model.Fen
{
    public interface IChessboardFactory
    {
        IChessboard Create(string fenString);
    }
}