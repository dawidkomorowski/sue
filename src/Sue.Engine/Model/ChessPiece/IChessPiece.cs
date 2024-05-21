using System.Collections.Generic;
using Sue.Engine.Model.Chessboard;

namespace Sue.Engine.Model.ChessPiece
{
    public interface IChessPiece
    {
        Color Color { get; }
        IChessboardField ChessboardField { get; }
        IEnumerable<IMove> Moves { get; }
        IChessboard Chessboard { get; }
        void MakeMove(IMove move);
    }
}