using Sue.Common.Model.Chessboard;

namespace Sue.Common.Model.Fen
{
    public interface IChessboardFactory
    {
        IChessboard Create(string fenString);
    }
}