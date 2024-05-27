using System.Collections.Generic;

namespace Sue.Engine.OldModel.ChessPiece.Internal
{
    public interface IRookMovesFinder
    {
        IEnumerable<IMove> FindMoves(IChessPiece chessPiece);
    }
}