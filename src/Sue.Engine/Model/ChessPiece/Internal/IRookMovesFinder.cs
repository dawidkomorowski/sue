using System.Collections.Generic;

namespace Sue.Engine.Model.ChessPiece.Internal
{
    public interface IRookMovesFinder
    {
        IEnumerable<IMove> FindMoves(IChessPiece chessPiece);
    }
}