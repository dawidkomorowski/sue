using System.Linq;
using NUnit.Framework;
using Sue.Engine.Model;
using Sue.Engine.OldModel.Chessboard;
using Sue.Engine.UnitTests.Common;

namespace Sue.Engine.UnitTests.OldModel.ChessPiece
{
    [TestFixture]
    public class KnightTests : CommonTestsBase
    {
        [Test]
        public void ShouldMovesReturn_B3_C2_WhenKnightOn_A1()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/8/8/8/8/N7 w KQkq - 0 1");
            var knight = chessboard.GetChessPiece(File.A, Rank.One);

            // Act
            var moves = knight.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(2));
            AssertMoveExistsInMoves(File.A, Rank.One, File.B, Rank.Three, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.C, Rank.Two, moves);
        }

        [Test]
        public void ShouldMovesReturn_B6_C7_WhenKnightOn_A8()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("N7/8/8/8/8/8/8/8 w KQkq - 0 1");
            var knight = chessboard.GetChessPiece(File.A, Rank.Eight);

            // Act
            var moves = knight.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(2));
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.B, Rank.Six, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.C, Rank.Seven, moves);
        }

        [Test]
        public void ShouldMovesReturn_G6_F7_WhenKnightOn_H8()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("7N/8/8/8/8/8/8/8 w KQkq - 0 1");
            var knight = chessboard.GetChessPiece(File.H, Rank.Eight);

            // Act
            var moves = knight.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(2));
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.G, Rank.Six, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.F, Rank.Seven, moves);
        }

        [Test]
        public void ShouldMovesReturn_G3_F2_WhenKnightOn_H1()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/8/8/8/8/7N w KQkq - 0 1");
            var knight = chessboard.GetChessPiece(File.H, Rank.One);

            // Act
            var moves = knight.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(2));
            AssertMoveExistsInMoves(File.H, Rank.One, File.G, Rank.Three, moves);
            AssertMoveExistsInMoves(File.H, Rank.One, File.F, Rank.Two, moves);
        }

        [Test]
        public void ShouldMovesReturn_D6_F6_C3_C5_G3_G5_D2_F2_WhenKnightOn_E4()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/8/4N3/8/8/7N w KQkq - 0 1");
            var knight = chessboard.GetChessPiece(File.E, Rank.Four);

            // Act
            var moves = knight.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(8));
            AssertMoveExistsInMoves(File.E, Rank.Four, File.D, Rank.Six, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.F, Rank.Six, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.C, Rank.Three, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.C, Rank.Five, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.G, Rank.Three, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.G, Rank.Five, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.D, Rank.Two, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.F, Rank.Two, moves);
        }

        [Test]
        public void ShouldMovesReturn_D6_F6_C3_C5_G3_G5_D2_F2_WhenKnightOn_E4_AndNearbyFieldsOccupied()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/3PPP2/3PNP2/3PPP2/8/8 w KQkq - 0 1");
            var knight = chessboard.GetChessPiece(File.E, Rank.Four);

            // Act
            var moves = knight.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(8));
            AssertMoveExistsInMoves(File.E, Rank.Four, File.D, Rank.Six, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.F, Rank.Six, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.C, Rank.Three, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.C, Rank.Five, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.G, Rank.Three, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.G, Rank.Five, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.D, Rank.Two, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.F, Rank.Two, moves);
        }

        [Test]
        public void ShouldMovesReturn_D6_F6_C3_C5_G3_G5_D2_F2_WhenKnightOn_E4_AndTargetFieldsOccupiedByEnemyPieces()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/3p1p2/2p3p1/4N3/2p3p1/3p1p2/8 w KQkq - 0 1");
            var knight = chessboard.GetChessPiece(File.E, Rank.Four);

            // Act
            var moves = knight.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(8));
            AssertMoveExistsInMoves(File.E, Rank.Four, File.D, Rank.Six, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.F, Rank.Six, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.C, Rank.Three, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.C, Rank.Five, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.G, Rank.Three, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.G, Rank.Five, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.D, Rank.Two, moves);
            AssertMoveExistsInMoves(File.E, Rank.Four, File.F, Rank.Two, moves);
        }

        [Test]
        public void ShouldMovesReturnNoMoves_WhenKnightOn_E4_AndTargetFieldsOccupiedByOwnPieces()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/3P1P2/2P3P1/4N3/2P3P1/3P1P2/8 w KQkq - 0 1");
            var knight = chessboard.GetChessPiece(File.E, Rank.Four);

            // Act
            var moves = knight.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(0));
        }

        [TestCase("8/8/8/8/8/3N4/8/8 w - - 0 1", "8/8/8/8/5N2/8/8/8 b - - 1 1", File.D, Rank.Three, File.F, Rank.Four)]
        [TestCase("8/8/8/8/5N2/8/8/8 w - - 3 5", "8/8/4N3/8/8/8/8/8 b - - 4 5", File.F, Rank.Four, File.E, Rank.Six)]
        [TestCase("8/2p5/4N3/8/8/8/8/8 w - - 3 5", "8/2N5/8/8/8/8/8/8 b - - 0 5", File.E, Rank.Six, File.C, Rank.Seven)]
        [TestCase("8/2n5/8/8/8/8/8/8 b - - 3 5", "8/8/8/1n6/8/8/8/8 w - - 4 6", File.C, Rank.Seven, File.B, Rank.Five)]
        [TestCase("8/8/8/1n6/3B4/8/8/8 b - - 3 5", "8/8/8/8/3n4/8/8/8 w - - 0 6", File.B, Rank.Five, File.D, Rank.Four)]
        [TestCase("8/8/8/1n6/3B4/8/8/8 b KQkq - 3 5", "8/8/8/8/3n4/8/8/8 w KQkq - 0 6", File.B, Rank.Five, File.D, Rank.Four)]
        public void ShouldMoveFromGivenPositionToGivenPosition(string initialFenString, string expectedFenString,
            File fromFile, Rank fromRank, File toFile, Rank toRank)
        {
            // Arrange
            var chessboard = ChessboardFactory.Create(initialFenString);
            var knight = chessboard.GetChessPiece(fromFile, fromRank);
            var move = CreateMove(knight, toFile, toRank);

            // Act
            knight.MakeMove(move);

            // Assert
            var expectedChessboard = ChessboardFactory.Create(expectedFenString);
            Assert.That(chessboard.EqualsTo(expectedChessboard), Is.True);
        }
    }
}