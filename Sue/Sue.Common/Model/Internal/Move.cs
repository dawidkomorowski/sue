using Sue.Common.Model.Chessboard;
using Sue.Common.Model.ChessPiece;

namespace Sue.Common.Model.Internal
{
    internal class Move : IMove
    {
        public Move(IChessboardField from, IChessboardField to)
        {
            From = from;
            To = to;
        }

        public IChessboardField From { get; }
        public IChessboardField To { get; }
        public IChessPiece ChessPiece => From.ChessPiece;
    }
}