using System.Collections.Generic;
using Sue.Common.Model.Chessboard.Internal;

namespace Sue.Common.Model.ChessPiece.Internal
{
    internal class Bishop : Internal.ChessPiece
    {
        public Bishop(Color color, ChessboardField chessboardField) : base(color, chessboardField)
        {
        }

        public override IEnumerable<IMove> Moves { get; }
    }
}
