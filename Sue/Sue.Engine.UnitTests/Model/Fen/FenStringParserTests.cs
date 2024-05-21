using NUnit.Framework;
using Sue.Engine.Model;
using Sue.Engine.Model.Chessboard;
using Sue.Engine.Model.Chessboard.Internal;
using Sue.Engine.Model.ChessPiece.Internal;
using Sue.Engine.Model.Fen.Internal;

namespace Sue.Engine.UnitTests.Model.Fen
{
    [TestFixture]
    public class FenStringParserTests
    {
        [Test]
        public void ShouldSetupInitialChessboardState()
        {
            // Arrange
            var fenStringParser = FenStringParser;
            var chessboard = Chessboard;

            const string fenString = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

            // Act
            fenStringParser.Parse(fenString, chessboard);

            // Assert

            #region Chess Pieces Placement

            Assert.That(chessboard.GetChessPiece(File.A, Rank.Eight), Is.TypeOf<Rook>());
            Assert.That(chessboard.GetChessPiece(File.A, Rank.Eight).Color, Is.EqualTo(Color.Black));
            Assert.That(chessboard.GetChessPiece(File.B, Rank.Eight), Is.TypeOf<Knight>());
            Assert.That(chessboard.GetChessPiece(File.B, Rank.Eight).Color, Is.EqualTo(Color.Black));
            Assert.That(chessboard.GetChessPiece(File.C, Rank.Eight), Is.TypeOf<Bishop>());
            Assert.That(chessboard.GetChessPiece(File.C, Rank.Eight).Color, Is.EqualTo(Color.Black));
            Assert.That(chessboard.GetChessPiece(File.D, Rank.Eight), Is.TypeOf<Queen>());
            Assert.That(chessboard.GetChessPiece(File.D, Rank.Eight).Color, Is.EqualTo(Color.Black));
            Assert.That(chessboard.GetChessPiece(File.E, Rank.Eight), Is.TypeOf<King>());
            Assert.That(chessboard.GetChessPiece(File.E, Rank.Eight).Color, Is.EqualTo(Color.Black));
            Assert.That(chessboard.GetChessPiece(File.F, Rank.Eight), Is.TypeOf<Bishop>());
            Assert.That(chessboard.GetChessPiece(File.F, Rank.Eight).Color, Is.EqualTo(Color.Black));
            Assert.That(chessboard.GetChessPiece(File.G, Rank.Eight), Is.TypeOf<Knight>());
            Assert.That(chessboard.GetChessPiece(File.G, Rank.Eight).Color, Is.EqualTo(Color.Black));
            Assert.That(chessboard.GetChessPiece(File.H, Rank.Eight), Is.TypeOf<Rook>());
            Assert.That(chessboard.GetChessPiece(File.H, Rank.Eight).Color, Is.EqualTo(Color.Black));

            foreach (var file in FileExtensions.Enumerable())
            {
                Assert.That(chessboard.GetChessPiece(file, Rank.Seven), Is.TypeOf<Pawn>());
                Assert.That(chessboard.GetChessPiece(file, Rank.Seven).Color, Is.EqualTo(Color.Black));
            }

            foreach (var file in FileExtensions.Enumerable())
            {
                for (var i = 2; i < 6; i++)
                {
                    Assert.That(chessboard.GetChessPiece(file, i.ToRank()), Is.Null);
                }
            }

            foreach (var file in FileExtensions.Enumerable())
            {
                Assert.That(chessboard.GetChessPiece(file, Rank.Two), Is.TypeOf<Pawn>());
                Assert.That(chessboard.GetChessPiece(file, Rank.Two).Color, Is.EqualTo(Color.White));
            }

            Assert.That(chessboard.GetChessPiece(File.A, Rank.One), Is.TypeOf<Rook>());
            Assert.That(chessboard.GetChessPiece(File.A, Rank.One).Color, Is.EqualTo(Color.White));
            Assert.That(chessboard.GetChessPiece(File.B, Rank.One), Is.TypeOf<Knight>());
            Assert.That(chessboard.GetChessPiece(File.B, Rank.One).Color, Is.EqualTo(Color.White));
            Assert.That(chessboard.GetChessPiece(File.C, Rank.One), Is.TypeOf<Bishop>());
            Assert.That(chessboard.GetChessPiece(File.C, Rank.One).Color, Is.EqualTo(Color.White));
            Assert.That(chessboard.GetChessPiece(File.D, Rank.One), Is.TypeOf<Queen>());
            Assert.That(chessboard.GetChessPiece(File.D, Rank.One).Color, Is.EqualTo(Color.White));
            Assert.That(chessboard.GetChessPiece(File.E, Rank.One), Is.TypeOf<King>());
            Assert.That(chessboard.GetChessPiece(File.E, Rank.One).Color, Is.EqualTo(Color.White));
            Assert.That(chessboard.GetChessPiece(File.F, Rank.One), Is.TypeOf<Bishop>());
            Assert.That(chessboard.GetChessPiece(File.F, Rank.One).Color, Is.EqualTo(Color.White));
            Assert.That(chessboard.GetChessPiece(File.G, Rank.One), Is.TypeOf<Knight>());
            Assert.That(chessboard.GetChessPiece(File.G, Rank.One).Color, Is.EqualTo(Color.White));
            Assert.That(chessboard.GetChessPiece(File.H, Rank.One), Is.TypeOf<Rook>());
            Assert.That(chessboard.GetChessPiece(File.H, Rank.One).Color, Is.EqualTo(Color.White));

            #endregion

            Assert.That(chessboard.CurrentPlayer, Is.EqualTo(Color.White));
            Assert.That(chessboard.WhiteKingsideCastlingAvailable, Is.True);
            Assert.That(chessboard.WhiteQueensideCastlingAvailable, Is.True);
            Assert.That(chessboard.BlackKingsideCastlingAvailable, Is.True);
            Assert.That(chessboard.BlackQueensideCastlingAvailable, Is.True);
            Assert.That(chessboard.EnPassantTargetField, Is.Null);
            Assert.That(chessboard.HalfmoveClock, Is.EqualTo(0));
            Assert.That(chessboard.FullmoveNumber, Is.EqualTo(1));
        }

        [Test]
        public void ShouldSetup_CurrentPlayerBlack_WhiteKingsideCastlingOnly_EnPassant_C4_HalfMove12_FullMove37()
        {
            // Arrange
            var fenStringParser = FenStringParser;
            var chessboard = Chessboard;

            const string fenString = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b K c4 12 37";

            // Act
            fenStringParser.Parse(fenString, chessboard);

            // Assert
            Assert.That(chessboard.CurrentPlayer, Is.EqualTo(Color.Black));
            Assert.That(chessboard.WhiteKingsideCastlingAvailable, Is.True);
            Assert.That(chessboard.WhiteQueensideCastlingAvailable, Is.False);
            Assert.That(chessboard.BlackKingsideCastlingAvailable, Is.False);
            Assert.That(chessboard.BlackQueensideCastlingAvailable, Is.False);
            Assert.That(chessboard.EnPassantTargetField.File, Is.EqualTo(File.C));
            Assert.That(chessboard.EnPassantTargetField.Rank, Is.EqualTo(Rank.Four));
            Assert.That(chessboard.HalfmoveClock, Is.EqualTo(12));
            Assert.That(chessboard.FullmoveNumber, Is.EqualTo(37));
        }

        [Test]
        public void ShouldSetupGivenChessboardState()
        {
            // Arrange
            var fenStringParser = FenStringParser;
            var chessboard = Chessboard;

            const string fenString = "r1b2r2/1p1nq1b1/1np1p1kp/p7/3PN3/1P2B3/2Q1BPPP/2RR2K1 w - - 4 23";

            // Act
            fenStringParser.Parse(fenString, chessboard);

            // Assert

            #region Chess Pieces Placement

            Assert.That(chessboard.GetChessPiece(File.A, Rank.Eight), Is.TypeOf<Rook>());
            Assert.That(chessboard.GetChessPiece(File.A, Rank.Eight).Color, Is.EqualTo(Color.Black));
            Assert.That(chessboard.GetChessPiece(File.B, Rank.Eight), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.C, Rank.Eight), Is.TypeOf<Bishop>());
            Assert.That(chessboard.GetChessPiece(File.C, Rank.Eight).Color, Is.EqualTo(Color.Black));
            Assert.That(chessboard.GetChessPiece(File.D, Rank.Eight), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.E, Rank.Eight), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.F, Rank.Eight), Is.TypeOf<Rook>());
            Assert.That(chessboard.GetChessPiece(File.F, Rank.Eight).Color, Is.EqualTo(Color.Black));
            Assert.That(chessboard.GetChessPiece(File.G, Rank.Eight), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.H, Rank.Eight), Is.Null);

            Assert.That(chessboard.GetChessPiece(File.A, Rank.Seven), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.B, Rank.Seven), Is.TypeOf<Pawn>());
            Assert.That(chessboard.GetChessPiece(File.B, Rank.Seven).Color, Is.EqualTo(Color.Black));
            Assert.That(chessboard.GetChessPiece(File.C, Rank.Seven), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.D, Rank.Seven), Is.TypeOf<Knight>());
            Assert.That(chessboard.GetChessPiece(File.D, Rank.Seven).Color, Is.EqualTo(Color.Black));
            Assert.That(chessboard.GetChessPiece(File.E, Rank.Seven), Is.TypeOf<Queen>());
            Assert.That(chessboard.GetChessPiece(File.E, Rank.Seven).Color, Is.EqualTo(Color.Black));
            Assert.That(chessboard.GetChessPiece(File.F, Rank.Seven), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.G, Rank.Seven), Is.TypeOf<Bishop>());
            Assert.That(chessboard.GetChessPiece(File.G, Rank.Seven).Color, Is.EqualTo(Color.Black));
            Assert.That(chessboard.GetChessPiece(File.H, Rank.Seven), Is.Null);

            Assert.That(chessboard.GetChessPiece(File.A, Rank.Six), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.B, Rank.Six), Is.TypeOf<Knight>());
            Assert.That(chessboard.GetChessPiece(File.B, Rank.Six).Color, Is.EqualTo(Color.Black));
            Assert.That(chessboard.GetChessPiece(File.C, Rank.Six), Is.TypeOf<Pawn>());
            Assert.That(chessboard.GetChessPiece(File.C, Rank.Six).Color, Is.EqualTo(Color.Black));
            Assert.That(chessboard.GetChessPiece(File.D, Rank.Six), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.E, Rank.Six), Is.TypeOf<Pawn>());
            Assert.That(chessboard.GetChessPiece(File.E, Rank.Six).Color, Is.EqualTo(Color.Black));
            Assert.That(chessboard.GetChessPiece(File.F, Rank.Six), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.G, Rank.Six), Is.TypeOf<King>());
            Assert.That(chessboard.GetChessPiece(File.G, Rank.Six).Color, Is.EqualTo(Color.Black));
            Assert.That(chessboard.GetChessPiece(File.H, Rank.Six), Is.TypeOf<Pawn>());
            Assert.That(chessboard.GetChessPiece(File.H, Rank.Six).Color, Is.EqualTo(Color.Black));

            Assert.That(chessboard.GetChessPiece(File.A, Rank.Five), Is.TypeOf<Pawn>());
            Assert.That(chessboard.GetChessPiece(File.A, Rank.Five).Color, Is.EqualTo(Color.Black));
            Assert.That(chessboard.GetChessPiece(File.B, Rank.Five), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.C, Rank.Five), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.D, Rank.Five), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.E, Rank.Five), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.F, Rank.Five), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.G, Rank.Five), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.H, Rank.Five), Is.Null);

            Assert.That(chessboard.GetChessPiece(File.A, Rank.Four), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.B, Rank.Four), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.C, Rank.Four), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.D, Rank.Four), Is.TypeOf<Pawn>());
            Assert.That(chessboard.GetChessPiece(File.D, Rank.Four).Color, Is.EqualTo(Color.White));
            Assert.That(chessboard.GetChessPiece(File.E, Rank.Four), Is.TypeOf<Knight>());
            Assert.That(chessboard.GetChessPiece(File.E, Rank.Four).Color, Is.EqualTo(Color.White));
            Assert.That(chessboard.GetChessPiece(File.F, Rank.Four), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.G, Rank.Four), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.H, Rank.Four), Is.Null);

            Assert.That(chessboard.GetChessPiece(File.A, Rank.Three), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.B, Rank.Three), Is.TypeOf<Pawn>());
            Assert.That(chessboard.GetChessPiece(File.B, Rank.Three).Color, Is.EqualTo(Color.White));
            Assert.That(chessboard.GetChessPiece(File.C, Rank.Three), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.D, Rank.Three), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.E, Rank.Three), Is.TypeOf<Bishop>());
            Assert.That(chessboard.GetChessPiece(File.E, Rank.Three).Color, Is.EqualTo(Color.White));
            Assert.That(chessboard.GetChessPiece(File.F, Rank.Three), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.G, Rank.Three), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.H, Rank.Three), Is.Null);

            Assert.That(chessboard.GetChessPiece(File.A, Rank.Two), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.B, Rank.Two), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.C, Rank.Two), Is.TypeOf<Queen>());
            Assert.That(chessboard.GetChessPiece(File.C, Rank.Two).Color, Is.EqualTo(Color.White));
            Assert.That(chessboard.GetChessPiece(File.D, Rank.Two), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.E, Rank.Two), Is.TypeOf<Bishop>());
            Assert.That(chessboard.GetChessPiece(File.E, Rank.Two).Color, Is.EqualTo(Color.White));
            Assert.That(chessboard.GetChessPiece(File.F, Rank.Two), Is.TypeOf<Pawn>());
            Assert.That(chessboard.GetChessPiece(File.F, Rank.Two).Color, Is.EqualTo(Color.White));
            Assert.That(chessboard.GetChessPiece(File.G, Rank.Two), Is.TypeOf<Pawn>());
            Assert.That(chessboard.GetChessPiece(File.G, Rank.Two).Color, Is.EqualTo(Color.White));
            Assert.That(chessboard.GetChessPiece(File.H, Rank.Two), Is.TypeOf<Pawn>());
            Assert.That(chessboard.GetChessPiece(File.H, Rank.Two).Color, Is.EqualTo(Color.White));

            Assert.That(chessboard.GetChessPiece(File.A, Rank.One), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.B, Rank.One), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.C, Rank.One), Is.TypeOf<Rook>());
            Assert.That(chessboard.GetChessPiece(File.C, Rank.One).Color, Is.EqualTo(Color.White));
            Assert.That(chessboard.GetChessPiece(File.D, Rank.One), Is.TypeOf<Rook>());
            Assert.That(chessboard.GetChessPiece(File.D, Rank.One).Color, Is.EqualTo(Color.White));
            Assert.That(chessboard.GetChessPiece(File.E, Rank.One), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.F, Rank.One), Is.Null);
            Assert.That(chessboard.GetChessPiece(File.G, Rank.One), Is.TypeOf<King>());
            Assert.That(chessboard.GetChessPiece(File.G, Rank.One).Color, Is.EqualTo(Color.White));
            Assert.That(chessboard.GetChessPiece(File.H, Rank.One), Is.Null);

            #endregion

            Assert.That(chessboard.CurrentPlayer, Is.EqualTo(Color.White));
            Assert.That(chessboard.WhiteKingsideCastlingAvailable, Is.False);
            Assert.That(chessboard.WhiteQueensideCastlingAvailable, Is.False);
            Assert.That(chessboard.BlackKingsideCastlingAvailable, Is.False);
            Assert.That(chessboard.BlackQueensideCastlingAvailable, Is.False);
            Assert.That(chessboard.EnPassantTargetField, Is.Null);
            Assert.That(chessboard.HalfmoveClock, Is.EqualTo(4));
            Assert.That(chessboard.FullmoveNumber, Is.EqualTo(23));
        }


        private static IFenStringParser FenStringParser
        {
            get
            {
                IChessPieceParser chessPieceParser = new ChessPieceParser();
                IRankLineParser rankLineParser = new RankLineParser(chessPieceParser);
                IFenStringExtractor fenStringExtractor = new FenStringExtractor();
                ICastlingAvailabilityParser castlingAvailabilityParser = new CastlingAvailabilityParser();
                IChessFieldParser chessFieldParser = new ChessFieldParser();
                return new FenStringParser(rankLineParser, fenStringExtractor, castlingAvailabilityParser,
                    chessFieldParser);
            }
        }

        private static ArrayChessboard Chessboard
        {
            get
            {
                IRookMovesFinder rookMovesFinder = new RookMovesFinder();
                IBishopMovesFinder bishopMovesFinder = new BishopMovesFinder();
                IChessPieceFactory chessPieceFactory = new ChessPieceFactory(rookMovesFinder, bishopMovesFinder);
                return new ArrayChessboard(chessPieceFactory);
            }
        }
    }
}