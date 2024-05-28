using System.Collections.Generic;
using Sue.Engine.Model;

namespace Sue.Engine.OldModel.Fen.Internal
{
    public class RankLineParser : IRankLineParser
    {
        private readonly IChessPieceParser _chessPieceParser;

        public RankLineParser(IChessPieceParser chessPieceParser)
        {
            _chessPieceParser = chessPieceParser;
        }

        public IEnumerable<PlacedChessPiece> Parse(RankLine rankLine)
        {
            IList<PlacedChessPiece> placedChessPieces = new List<PlacedChessPiece>();

            var index = 0;

            foreach (var character in rankLine.String)
            {
                if (char.IsDigit(character))
                {
                    var leapSize = (int)char.GetNumericValue(character);
                    index += leapSize;
                }
                else
                {
                    var chessPiece = _chessPieceParser.Parse(character);
                    placedChessPieces.Add(new PlacedChessPiece(chessPiece, index.ToFile(), rankLine.Rank));
                    index++;
                }
            }

            return placedChessPieces;
        }
    }
}