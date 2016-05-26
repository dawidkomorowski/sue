using System.Collections.Generic;
using Sue.Common.Model.Chessboard;

namespace Sue.Common.Model.Fen.Internal
{
    internal interface IRankLineParser
    {
        IEnumerable<PlacedChessPiece> Parse(RankLine rankLine);
    }
}