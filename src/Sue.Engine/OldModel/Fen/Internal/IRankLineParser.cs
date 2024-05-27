using System.Collections.Generic;

namespace Sue.Engine.OldModel.Fen.Internal
{
    public interface IRankLineParser
    {
        IEnumerable<PlacedChessPiece> Parse(RankLine rankLine);
    }
}