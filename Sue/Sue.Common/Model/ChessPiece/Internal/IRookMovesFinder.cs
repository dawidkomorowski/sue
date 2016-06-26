using System.Collections.Generic;
using Sue.Common.Model.Chessboard;

namespace Sue.Common.Model.ChessPiece.Internal
{
    internal interface IRookMovesFinder
    {
        IEnumerable<IMove> FindMoves(IChessPiece chessPiece);
    }
}