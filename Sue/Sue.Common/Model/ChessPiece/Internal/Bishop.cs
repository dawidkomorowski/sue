using System.Collections.Generic;
using Sue.Common.Model.Chessboard.Internal;

namespace Sue.Common.Model.ChessPiece.Internal
{
    internal class Bishop : ChessPiece
    {
        private readonly IBishopMovesFinder _bishopMovesFinder;

        public Bishop(Color color, ChessboardField chessboardField, IBishopMovesFinder bishopMovesFinder) : base(color, chessboardField)
        {
            _bishopMovesFinder = bishopMovesFinder;
        }

        public override IEnumerable<IMove> Moves => _bishopMovesFinder.FindMoves(this);
    }
}