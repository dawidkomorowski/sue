using System.Collections.Generic;
using Sue.Common.Model.Chessboard;

namespace Sue.Common.Model.Fen.Internal
{
    public interface IRankLineParser
    {
        IEnumerable<PlacedChessPiece> Parse(RankLine rankLine);
    }
}