using Sue.Engine.Model.ChessPiece;

namespace Sue.Engine.Model.Chessboard
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
