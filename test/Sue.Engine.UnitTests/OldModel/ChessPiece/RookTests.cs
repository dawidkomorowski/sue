using System.Linq;
using NUnit.Framework;
using Sue.Engine.Model;
using Sue.Engine.OldModel.Chessboard;
using Sue.Engine.UnitTests.Common;

namespace Sue.Engine.UnitTests.OldModel.ChessPiece
{
    [TestFixture]
    public class RookTests : CommonTestsBase
    {
        [Test]
        public void ShouldMovesReturn_B1_C1_D1_E1_F1_G1_H1_A2_A3_A4_A5_A6_A7_A8_WhenRookOn_A1()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/8/8/8/8/R7 w KQkq - 0 1");
            var rook = chessboard.GetChessPiece(File.A, Rank.One);

            // Act
            var moves = rook.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(14));
            AssertMoveExistsInMoves(File.A, Rank.One, File.B, Rank.One, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.C, Rank.One, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.D, Rank.One, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.E, Rank.One, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.F, Rank.One, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.G, Rank.One, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.H, Rank.One, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.A, Rank.Two, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.A, Rank.Three, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.A, Rank.Four, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.A, Rank.Five, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.A, Rank.Six, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.A, Rank.Seven, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.A, Rank.Eight, moves);
        }

        [Test]
        public void ShouldMovesReturn_B8_C8_D8_E8_F8_G8_H8_A1_A2_A3_A4_A5_A6_A7_WhenRookOn_A8()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("R7/8/8/8/8/8/8/8 w KQkq - 0 1");
            var rook = chessboard.GetChessPiece(File.A, Rank.Eight);

            // Act
            var moves = rook.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(14));
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.B, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.C, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.D, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.E, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.F, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.G, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.H, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.A, Rank.One, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.A, Rank.Two, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.A, Rank.Three, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.A, Rank.Four, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.A, Rank.Five, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.A, Rank.Six, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.A, Rank.Seven, moves);
        }

        [Test]
        public void ShouldMovesReturn__A8_B8_C8_D8_E8_F8_G8_H1_H2_H3_H4_H5_H6_H7_WhenRookOn_H8()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("7R/8/8/8/8/8/8/8 w KQkq - 0 1");
            var rook = chessboard.GetChessPiece(File.H, Rank.Eight);

            // Act
            var moves = rook.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(14));
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.A, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.B, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.C, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.D, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.E, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.F, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.G, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.H, Rank.One, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.H, Rank.Two, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.H, Rank.Three, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.H, Rank.Four, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.H, Rank.Five, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.H, Rank.Six, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.H, Rank.Seven, moves);
        }

        [Test]
        public void ShouldMovesReturn__A4_B4_C4_D4_F4_G4_H4_E1_E2_E3_E5_E6_E7_E8_WhenRookOn_E4()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/8/4R3/8/8/8 w KQkq - 0 1");
            var rook = chessboard.GetChessPiece(File.E, Rank.Four);

            // Act
            var moves = rook.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(14));
            AssertMoveExistsInMoves(File.E, Rank.Four, File.A, Rank.Four, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.B, Rank.Four, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.C, Rank.Four, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.D, Rank.Four, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.F, Rank.Four, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.G, Rank.Four, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.H, Rank.Four, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.E, Rank.One, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.E, Rank.Two, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.E, Rank.Three, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.E, Rank.Five, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.E, Rank.Six, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.E, Rank.Seven, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.E, Rank.Eight, moves);
        }

        [Test]
        public void ShouldMovesReturnNoMoves_WhenRookOn_E4_AndNearbyFieldsOccupiedByOwnPieces()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/4P3/3PRP2/4P3/8/8 w KQkq - 0 1");
            var rook = chessboard.GetChessPiece(File.E, Rank.Four);

            // Act
            var moves = rook.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(0));
        }

        [Test]
        public void ShouldMovesReturn_D4_F4_E3_E5_WhenRookOn_E4_AndNearbyFieldsOccupiedByEnemyPieces()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/4p3/3pRp2/4p3/8/8 w KQkq - 0 1");
            var rook = chessboard.GetChessPiece(File.E, Rank.Four);

            // Act
            var moves = rook.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(4));
            AssertMoveExistsInMoves(File.E, Rank.Four, File.D, Rank.Four, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.F, Rank.Four, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.E, Rank.Three, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.E, Rank.Five, moves);
        }

        [Test]
        public void ShouldMovesReturn_C4_D4_F4_G4_E3_E5_E6_E7_WhenRookOn_E4_AndAsInGivenFenString()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/4p3/8/8/2p1R2P/8/4P3/8 w KQkq - 0 1");
            var rook = chessboard.GetChessPiece(File.E, Rank.Four);

            // Act
            var moves = rook.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(8));
            AssertMoveExistsInMoves(File.E, Rank.Four, File.C, Rank.Four, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.D, Rank.Four, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.F, Rank.Four, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.G, Rank.Four, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.E, Rank.Three, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.E, Rank.Five, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.E, Rank.Six, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.E, Rank.Seven, moves);
        }

        [TestCase("8/8/8/8/3R4/8/8/8 w - - 0 1", "8/8/8/8/6R1/8/8/8 b - - 1 1", File.D, Rank.Four, File.G, Rank.Four)]
        [TestCase("8/8/8/8/3R4/8/8/8 w - - 0 1", "3R4/8/8/8/8/8/8/8 b - - 1 1", File.D, Rank.Four, File.D, Rank.Eight)]
        [TestCase("3n4/8/8/8/3R4/8/8/8 w - - 0 1", "3R4/8/8/8/8/8/8/8 b - - 0 1", File.D, Rank.Four, File.D, Rank.Eight)]
        [TestCase("3n4/8/8/8/3R4/8/8/8 w - - 5 10", "3R4/8/8/8/8/8/8/8 b - - 0 10", File.D, Rank.Four, File.D, Rank.Eight)]
        [TestCase("8/8/8/8/3r4/8/8/8 b - - 0 1", "3r4/8/8/8/8/8/8/8 w - - 1 2", File.D, Rank.Four, File.D, Rank.Eight)]
        [TestCase("3B4/8/8/8/3r4/8/8/8 b - - 1 1", "3r4/8/8/8/8/8/8/8 w - - 0 2", File.D, Rank.Four, File.D, Rank.Eight)]
        // Castling availability
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R w KQkq - 0 1", "r3k2r/8/8/8/8/8/7R/R3K3 b Qkq - 1 1", File.H, Rank.One, File.H, Rank.Two)]
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R w KQkq - 0 1", "r3k2r/8/8/8/8/8/R7/4K2R b Kkq - 1 1", File.A, Rank.One, File.A, Rank.Two)]
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R b KQkq - 0 1", "r3k3/7r/8/8/8/8/8/R3K2R w KQq - 1 2", File.H, Rank.Eight, File.H, Rank.Seven)]
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R b KQkq - 0 1", "4k2r/r7/8/8/8/8/8/R3K2R w KQk - 1 2", File.A, Rank.Eight, File.A, Rank.Seven)]
        public void ShouldMoveFromGivenPositionToGivenPosition(string initialFenString, string expectedFenString,
            File fromFile, Rank fromRank, File toFile, Rank toRank)
        {
            // Arrange
            var chessboard = ChessboardFactory.Create(initialFenString);
            var rook = chessboard.GetChessPiece(fromFile, fromRank);
            var move = CreateMove(rook, toFile, toRank);

            // Act
            rook.MakeMove(move);

            // Assert
            var expectedChessboard = ChessboardFactory.Create(expectedFenString);
            Assert.That(chessboard.EqualsTo(expectedChessboard), Is.True);
        }
    }
}