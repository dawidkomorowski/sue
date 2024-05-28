using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Sue.Engine.Model;
using Sue.Engine.OldModel;
using Sue.Engine.OldModel.Chessboard;
using Sue.Engine.OldModel.Chessboard.Internal;
using Sue.Engine.OldModel.ChessPiece;
using Sue.Engine.OldModel.ChessPiece.Internal;
using Sue.Engine.OldModel.Fen;
using Sue.Engine.OldModel.Fen.Internal;
using Sue.Engine.OldModel.Internal;

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
            return new Engine.OldModel.Internal.Move(from, to);
        }
    }
}