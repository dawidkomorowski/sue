using System.Collections.Generic;
using Sue.Engine.OldModel.Chessboard.Internal;

namespace Sue.Engine.OldModel.Fen.Internal
{
    public class FenStringParser : IFenStringParser
    {
        private readonly IRankLineParser _rankLineParser;
        private readonly IFenStringExtractor _fenStringExtractor;
        private readonly ICastlingAvailabilityParser _castlingAvailabilityParser;
        private readonly IChessFieldParser _chessFieldParser;

        public FenStringParser(IRankLineParser rankLineParser, IFenStringExtractor fenStringExtractor,
            ICastlingAvailabilityParser castlingAvailabilityParser, IChessFieldParser chessFieldParser)
        {
            _rankLineParser = rankLineParser;
            _fenStringExtractor = fenStringExtractor;
            _castlingAvailabilityParser = castlingAvailabilityParser;
            _chessFieldParser = chessFieldParser;
        }

        public void Parse(string fenString, ISettableChessboard settableChessboard)
        {
            var extractedFenString = _fenStringExtractor.Extract(fenString);

            foreach (var placedChessPiece in GetPlacedChessPieces(extractedFenString.RankLines))
            {
                settableChessboard.SetChessPiece(placedChessPiece.ChessPiece.ChessPieceKind,
                    placedChessPiece.ChessPiece.Color, placedChessPiece.File, placedChessPiece.Rank);
            }

            settableChessboard.CurrentPlayer = extractedFenString.Color;

            var castlingAvailability = _castlingAvailabilityParser.Parse(extractedFenString.CastlingAvailabilityString);
            settableChessboard.WhiteKingsideCastlingAvailable = castlingAvailability.WhiteKingsideCastlingAvailable;
            settableChessboard.WhiteQueensideCastlingAvailable = castlingAvailability.WhiteQueenssideCastlingAvailable;
            settableChessboard.BlackKingsideCastlingAvailable = castlingAvailability.BlackKingsideCastlingAvailable;
            settableChessboard.BlackQueensideCastlingAvailable = castlingAvailability.BlackQueensideCastlingAvailable;

            if (!extractedFenString.EnPassantTargetFieldString.Contains('-'))
            {
                var chessField = _chessFieldParser.Parse(extractedFenString.EnPassantTargetFieldString);
                settableChessboard.EnPassantTargetField = settableChessboard.GetChessboardField(chessField.File,
                    chessField.Rank);
            }

            settableChessboard.HalfmoveClock = extractedFenString.HalfmoveClock;
            settableChessboard.FullmoveNumber = extractedFenString.FullmoveNumber;
        }

        private IEnumerable<PlacedChessPiece> GetPlacedChessPieces(IEnumerable<RankLine> rankLines)
        {
            var allPlacedChessPieces = new List<PlacedChessPiece>();
            foreach (var rankLine in rankLines)
            {
                var placedChessPieces = _rankLineParser.Parse(rankLine);
                allPlacedChessPieces.AddRange(placedChessPieces);
            }

            return allPlacedChessPieces;
        }
    }
}