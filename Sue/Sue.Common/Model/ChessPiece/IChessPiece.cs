using System.Collections.Generic;
using Sue.Common.Model.Chessboard;

namespace Sue.Common.Model.ChessPiece
{
    public interface IChessPiece
    {
        Color Color { get; }
        IChessboardField ChessboardField { get; }
        IEnumerable<IMove> Moves { get; }
        void MakeMove(IMove move);
        IChessboard Chessboard { get; }
    }
}
