using System.Collections.Generic;
using Sue.Engine.Model;
using Sue.Engine.OldModel.Chessboard;

namespace Sue.Engine.OldModel.ChessPiece
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