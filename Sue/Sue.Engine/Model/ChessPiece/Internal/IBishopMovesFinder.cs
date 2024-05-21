using System.Collections.Generic;

namespace Sue.Common.Model.ChessPiece.Internal
{
    public interface IBishopMovesFinder
    {
        IEnumerable<IMove> FindMoves(IChessPiece chessPiece);
    }
}