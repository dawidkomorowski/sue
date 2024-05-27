using Sue.Engine.OldModel.Chessboard;

namespace Sue.Engine.OldModel.Fen
{
    public interface IChessboardFactory
    {
        IChessboard Create(string fenString);
    }
}