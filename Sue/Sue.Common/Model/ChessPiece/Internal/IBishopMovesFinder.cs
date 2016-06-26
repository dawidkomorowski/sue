using System.Collections.Generic;

namespace Sue.Common.Model.ChessPiece.Internal
{
    internal interface IBishopMovesFinder
    {
        IEnumerable<IMove> FindMoves(IChessPiece chessPiece);
    }
}