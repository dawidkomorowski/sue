using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Sue.Engine.Model;
using Sue.Engine.Model.Chessboard;
using Sue.Engine.Model.Chessboard.Internal;
using Sue.Engine.Model.ChessPiece;
using Sue.Engine.Model.ChessPiece.Internal;
using Sue.Engine.Model.Fen;
using Sue.Engine.Model.Fen.Internal;
using Sue.Engine.Model.Internal;

namespace Sue.Engine.UnitTests.Common
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

        protected static IMove CreateMove(IChessPiece chessPiece, File toFile, Rank toRank)
        {
            var from = chessPiece.ChessboardField;
            var to = chessPiece.Chessboard.GetChessboardField(toFile, toRank);
            return new Move(from, to);
        }
    }
}