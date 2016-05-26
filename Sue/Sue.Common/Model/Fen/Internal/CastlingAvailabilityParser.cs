using System;
using System.Linq;

namespace Sue.Common.Model.Fen.Internal
{
    internal class CastlingAvailabilityParser : ICastlingAvailabilityParser
    {
        public CastlingAvailability Parse(string castlingAvailabilityString)
        {
            var whiteKingsideCastlingAvailable = castlingAvailabilityString.Contains('K');
            var whiteQueenssideCastlingAvailable = castlingAvailabilityString.Contains('Q');
            var blackKingsideCastlingAvailable = castlingAvailabilityString.Contains('k');
            var blackQueensideCastlingAvailable = castlingAvailabilityString.Contains('q');
            return new CastlingAvailability(whiteKingsideCastlingAvailable, whiteQueenssideCastlingAvailable,
                blackKingsideCastlingAvailable, blackQueensideCastlingAvailable);
        }
    }
}