using System.Collections.Generic;

namespace Sue.Engine.Model.ChessPiece.Internal
{
    public interface IBishopMovesFinder
    {
        IEnumerable<IMove> FindMoves(IChessPiece chessPiece);
    }
}