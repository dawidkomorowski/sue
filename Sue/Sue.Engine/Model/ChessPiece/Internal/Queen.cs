using System.Collections.Generic;
using System.Linq;
using Sue.Common.Model.Chessboard.Internal;

namespace Sue.Common.Model.ChessPiece.Internal
{
    public class Queen : ChessPiece
    {
        private readonly IRookMovesFinder _rookMovesFinder;
        private readonly IBishopMovesFinder _bishopMovesFinder;

        public Queen(Color color, ChessboardField chessboardField, IRookMovesFinder rookMovesFinder, IBishopMovesFinder bishopMovesFinder) : base(color, chessboardField)
        {
            _rookMovesFinder = rookMovesFinder;
            _bishopMovesFinder = bishopMovesFinder;
        }

        public override IEnumerable<IMove> Moves => _rookMovesFinder.FindMoves(this).Concat(_bishopMovesFinder.FindMoves(this));
    }
}
