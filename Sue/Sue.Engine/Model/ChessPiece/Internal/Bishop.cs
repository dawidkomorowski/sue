using System.Collections.Generic;
using Sue.Engine.Model.Chessboard.Internal;

namespace Sue.Engine.Model.ChessPiece.Internal
{
    public class Bishop : ChessPiece
    {
        private readonly IBishopMovesFinder _bishopMovesFinder;

        public Bishop(Color color, ChessboardField chessboardField, IBishopMovesFinder bishopMovesFinder) : base(color, chessboardField)
        {
            _bishopMovesFinder = bishopMovesFinder;
        }

        public override IEnumerable<IMove> Moves => _bishopMovesFinder.FindMoves(this);
    }
}