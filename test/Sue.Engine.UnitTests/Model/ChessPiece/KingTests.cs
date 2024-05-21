using System.Linq;
using NUnit.Framework;
using Sue.Engine.Model.Chessboard;
using Sue.Engine.UnitTests.Common;

namespace Sue.Engine.UnitTests.Model.ChessPiece
{
    [TestFixture]
    public class KingTests : CommonTestsBase
    {
        [Test]
        public void ShouldMovesReturn_A2_B2_B1_WhenKingOn_A1()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/8/8/8/8/K7 w KQkq - 0 1");
            var king = chessboard.GetChessPiece(File.A, Rank.One);

            // Act
            var moves = king.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(3));
            AssertMoveExistsInMoves(File.A, Rank.One, File.A, Rank.Two, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.B, Rank.Two, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.B, Rank.One, moves);
        }

        [Test]
        public void ShouldMovesReturn_A7_B7_B8_WhenKingOn_A8()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("K7/8/8/8/8/8/8/8 w KQkq - 0 1");
            var king = chessboard.GetChessPiece(File.A, Rank.Eight);

            // Act
            var moves = king.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(3));
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.A, Rank.Seven, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.B, Rank.Seven, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.B, Rank.Eight, moves);
        }

        [Test]
        public void ShouldMovesReturn_G8_G7_H7_WhenKingOn_H8()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("7K/8/8/8/8/8/8/8 w KQkq - 0 1");
            var king = chessboard.GetChessPiece(File.H, Rank.Eight);

            // Act
            var moves = king.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(3));
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.G, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.G, Rank.Seven, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.H, Rank.Seven, moves);
        }

        [Test]
        public void ShouldMovesReturn_G1_G2_H2_WhenKingOn_H1()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/8/8/8/8/7K w KQkq - 0 1");
            var king = chessboard.GetChessPiece(File.H, Rank.One);

            // Act
            var moves = king.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(3));
            AssertMoveExistsInMoves(File.H, Rank.One, File.G, Rank.One, moves);
            AssertMoveExistsInMoves(File.H, Rank.One, File.G, Rank.Two, moves);
            AssertMoveExistsInMoves(File.H, Rank.One, File.H, Rank.Two, moves);
        }

        [Test]
        public void ShouldMovesReturn_D3_D4_D5_E3_E5_F3_F4_F5_WhenKingOn_E4()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/8/4K3/8/8/8 w KQkq - 0 1");
            var king = chessboard.GetChessPiece(File.E, Rank.Four);

            // Act
            var moves = king.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(8));
            AssertMoveExistsInMoves(File.E, Rank.Four, File.D, Rank.Three, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.D, Rank.Four, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.D, Rank.Five, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.E, Rank.Three, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.E, Rank.Five, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.F, Rank.Three, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.F, Rank.Four, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.F, Rank.Five, moves);
        }

        [Test]
        public void ShouldMovesReturnNoMoves_WhenKingOn_E4_AndNearbyFieldsOccupiedByOwnPieces()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/3PPP2/3PKP2/3PPP2/8/8 w KQkq - 0 1");
            var king = chessboard.GetChessPiece(File.E, Rank.Four);

            // Act
            var moves = king.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(0));
        }

        [Test]
        public void ShouldMovesReturn_D3_D4_D5_E3_E5_F3_F4_F5_WhenKingOn_E4_AndNearbyFieldsOccupiedByEnemyPieces()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/3ppp2/3pKp2/3ppp2/8/8 w KQkq - 0 1");
            var king = chessboard.GetChessPiece(File.E, Rank.Four);

            // Act
            var moves = king.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(8));
            AssertMoveExistsInMoves(File.E, Rank.Four, File.D, Rank.Three, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.D, Rank.Four, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.D, Rank.Five, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.E, Rank.Three, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.E, Rank.Five, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.F, Rank.Three, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.F, Rank.Four, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.F, Rank.Five, moves);
        }

        [TestCase("8/8/8/8/4K3/8/8/8 w - - 0 1", "8/8/8/8/3K4/8/8/8 b - - 1 1", File.E, Rank.Four, File.D, Rank.Four)]
        [TestCase("8/8/8/8/4K3/8/8/8 w - - 0 1", "8/8/8/5K2/8/8/8/8 b - - 1 1", File.E, Rank.Four, File.F, Rank.Five)]
        [TestCase("8/8/8/8/4K3/4n3/8/8 w - - 5 3", "8/8/8/8/8/4K3/8/8 b - - 0 3", File.E, Rank.Four, File.E, Rank.Three)]
        [TestCase("8/8/8/8/4k3/8/8/8 b - - 0 1", "8/8/8/5k2/8/8/8/8 w - - 1 2", File.E, Rank.Four, File.F, Rank.Five)]
        [TestCase("8/8/8/8/4k3/4N3/8/8 b - - 5 3", "8/8/8/8/8/4k3/8/8 w - - 0 4", File.E, Rank.Four, File.E, Rank.Three)]
        // Castling availability
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R w KQkq - 0 1", "r3k2r/8/8/8/8/8/4K3/R6R b kq - 1 1", File.E, Rank.One, File.E, Rank.Two)]
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R b KQkq - 0 1", "r6r/4k3/8/8/8/8/8/R3K2R w KQ - 1 2", File.E, Rank.Eight, File.E, Rank.Seven)]
        public void ShouldMoveFromGivenPositionToGivenPosition(string initialFenString, string expectedFenString,
            File fromFile, Rank fromRank, File toFile, Rank toRank)
        {
            // Arrange
            var chessboard = ChessboardFactory.Create(initialFenString);
            var king = chessboard.GetChessPiece(fromFile, fromRank);
            var move = CreateMove(king, toFile, toRank);

            // Act
            king.MakeMove(move);

            // Assert
            var expectedChessboard = ChessboardFactory.Create(expectedFenString);
            Assert.That(chessboard.EqualsTo(expectedChessboard), Is.True);
        }
    }
}