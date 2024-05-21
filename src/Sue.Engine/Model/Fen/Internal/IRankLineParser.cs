using System.Collections.Generic;

namespace Sue.Engine.Model.Fen.Internal
{
    public interface IRankLineParser
    {
        IEnumerable<PlacedChessPiece> Parse(RankLine rankLine);
    }
}