using Sue.Common.Model.ChessPiece;

namespace Sue.Common.Model.Chessboard
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
