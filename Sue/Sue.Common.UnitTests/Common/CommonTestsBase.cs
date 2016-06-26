using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Sue.Common.Model;
using Sue.Common.Model.Chessboard;
using Sue.Common.Model.Chessboard.Internal;
using Sue.Common.Model.ChessPiece.Internal;
using Sue.Common.Model.Fen;
using Sue.Common.Model.Fen.Internal;

namespace Sue.Common.UnitTests.Common
{
    public class CommonTestsBase
    {
        protected static IChessboardFactory ChessboardFactory
        {
            get
            {
                IRookMovesFinder rookMovesFinder = new RookMovesFinder();
                IBishopMovesFinder bishopMovesFinder = new BishopMovesFinder();
                IChessPieceFactory chessPieceFactory = new ChessPieceFactory(rookMovesFinder, bishopMovesFinder);
                IChessPieceParser chessPieceParser = new ChessPieceParser();
                IRankLineParser rankLineParser = new RankLineParser(chessPieceParser);
                IFenStringExtractor fenStringExtractor = new FenStringExtractor();
                ICastlingAvailabilityParser castlingAvailabilityParser = new CastlingAvailabilityParser();
                IChessFieldParser chessFieldParser = new ChessFieldParser();
                IFenStringParser fenStringParser = new FenStringParser(rankLineParser, fenStringExtractor,
                    castlingAvailabilityParser, chessFieldParser);
                return new ChessboardFactory(chessPieceFactory, fenStringParser);
            }
        }

        protected static void AssertMoveExistsInMoves(File fromFile, Rank fromRank, File toFile, Rank toRank,
            IEnumerable<IMove> moves)
        {
            Assert.DoesNotThrow(
                () =>
                    moves.Single(
                        move =>
                            move.From.File == fromFile && move.From.Rank == fromRank && move.To.File == toFile &&
                            move.To.Rank == toRank));
        }
    }
}