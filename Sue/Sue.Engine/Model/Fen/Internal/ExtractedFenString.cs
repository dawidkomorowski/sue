using System.Collections.Generic;

namespace Sue.Engine.Model.Fen.Internal
{
    public struct ExtractedFenString
    {
        public ExtractedFenString(IEnumerable<RankLine> rankLines, Color color, string castlingAvailabilityString, string enPassantTargetFieldString, int halfmoveClock, int fullmoveNumber)
        {
            RankLines = rankLines;
            Color = color;
            CastlingAvailabilityString = castlingAvailabilityString;
            EnPassantTargetFieldString = enPassantTargetFieldString;
            HalfmoveClock = halfmoveClock;
            FullmoveNumber = fullmoveNumber;
        }

        public IEnumerable<RankLine> RankLines { get; }
        public Color Color { get; }
        public string CastlingAvailabilityString { get; }
        public string EnPassantTargetFieldString { get; }
        public int HalfmoveClock { get; }
        public int FullmoveNumber { get; }
    }
}