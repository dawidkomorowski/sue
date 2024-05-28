using Sue.Engine.Model;
using Sue.Engine.OldModel.ChessPiece;

namespace Sue.Engine.OldModel.Chessboard
{
    public interface IChessboardField
    {
        File File { get; }
        Rank Rank { get; }
        IChessPiece ChessPiece { get; }
        bool Empty { get; }
        IChessboard Chessboard { get; }
    }
}