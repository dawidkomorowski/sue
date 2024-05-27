using System;
using Sue.Engine.OldModel.Chessboard;
using Sue.Engine.OldModel.ChessPiece;

namespace Sue.Engine.OldModel.Internal
{
    public class Move : IMove
    {
        public Move(IChessboardField from, IChessboardField to)
        {
            if (from == to) throw new ArgumentException("From and To chessboard fields cannot be the same.");

            From = from;
            To = to;
        }

        public IChessboardField From { get; }
        public IChessboardField To { get; }
        public IChessPiece ChessPiece => From.ChessPiece;
    }
}