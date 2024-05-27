using Sue.Engine.OldModel.Chessboard;
using Sue.Engine.OldModel.ChessPiece;

namespace Sue.Engine.OldModel
{
    public interface IMove
    {
        IChessboardField From { get; }
        IChessboardField To { get; }
        IChessPiece ChessPiece { get; }
    }
}