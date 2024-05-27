using System.Collections.Generic;

namespace Sue.Engine.OldModel.ChessPiece.Internal
{
    public interface IBishopMovesFinder
    {
        IEnumerable<IMove> FindMoves(IChessPiece chessPiece);
    }
}