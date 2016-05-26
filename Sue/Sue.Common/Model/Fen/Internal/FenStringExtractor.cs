using System;
using System.Linq;
using Sue.Common.Model.Chessboard;

namespace Sue.Common.Model.Fen.Internal
{
    internal class FenStringExtractor : IFenStringExtractor
    {
        public ExtractedFenString Extract(string fenString)
        {
            var fenStringParts = fenString.Split(' ');

            var rankLinesStrings = fenStringParts[0].Split('/');
            var rankLines = rankLinesStrings.Select((s, i) => new RankLine(s, (7 - i).ToRank())).ToArray();
            var color = ParseColor(fenStringParts[1].First());
            var castlingAvailabilityString = fenStringParts[2];
            var enPassantTargetFieldString = fenStringParts[3];
            var halfmoveClock = int.Parse(fenStringParts[4]);
            var fullmoveNumber = int.Parse(fenStringParts[5]);

            return new ExtractedFenString(rankLines, color, castlingAvailabilityString, enPassantTargetFieldString,
                halfmoveClock, fullmoveNumber);
        }

        private Color ParseColor(char fenColorCode)
        {
            switch (fenColorCode)
            {
                case 'b':
                    return Color.Black;
                case 'w':
                    return Color.White;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fenColorCode), fenColorCode, null);
            }
        }
    }
}