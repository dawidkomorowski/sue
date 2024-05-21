using Sue.Engine.Model.Chessboard;
using Sue.Engine.Model.ChessPiece;

namespace Sue.Engine.Model
{
    public interface IMove
    {
        IChessboardField From { get; }
        IChessboardField To { get; }
        IChessPiece ChessPiece { get; }
    }
}
