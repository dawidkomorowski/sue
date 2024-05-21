using System.Collections.Generic;
using Sue.Common.Model.Chessboard;

namespace Sue.Common.Model.ChessPiece.Internal
{
    public interface IRookMovesFinder
    {
        IEnumerable<IMove> FindMoves(IChessPiece chessPiece);
    }
}