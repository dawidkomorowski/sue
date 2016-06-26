using System.Collections.Generic;
using Sue.Common.Model.Chessboard;
using Sue.Common.Model.Chessboard.Internal;

namespace Sue.Common.Model.ChessPiece.Internal
{
    internal class Rook : ChessPiece
    {
        private readonly IRookMovesFinder _rookMovesFinder;

        public Rook(Color color, ChessboardField chessboardField, IRookMovesFinder rookMovesFinder) : base(color, chessboardField)
        {
            _rookMovesFinder = rookMovesFinder;
        }

        public override IEnumerable<IMove> Moves => _rookMovesFinder.FindMoves(this);
    }
}