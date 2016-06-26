using System.Linq;
using NUnit.Framework;
using Sue.Common.Model.Chessboard;
using Sue.Common.UnitTests.Common;

namespace Sue.Common.UnitTests.Model.ChessPiece
{
    [TestFixture]
    public class BishopTests : CommonTestsBase
    {
        [Test]
        public void ShouldMovesReturn_B2_C3_D4_E5_F6_G7_H8_WhenBishopOn_A1()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/8/8/8/8/B7 w KQkq - 0 1");
            var bishop = chessboard.GetChessPiece(File.A, Rank.One);

            // Act
            var moves = bishop.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(7));
            AssertMoveExistsInMoves(File.A, Rank.One, File.B, Rank.Two, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.C, Rank.Three, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.D, Rank.Four, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.E, Rank.Five, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.F, Rank.Six, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.G, Rank.Seven, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.H, Rank.Eight, moves);
        }

        [Test]
        public void ShouldMovesReturn_B7_C6_D5_E4_F3_G2_H1_WhenBishopOn_A8()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("B7/8/8/8/8/8/8/8 w KQkq - 0 1");
            var bishop = chessboard.GetChessPiece(File.A, Rank.Eight);

            // Act
            var moves = bishop.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(7));
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.B, Rank.Seven, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.C, Rank.Six, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.D, Rank.Five, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.E, Rank.Four, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.F, Rank.Three, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.G, Rank.Two, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.H, Rank.One, moves);
        }

        [Test]
        public void ShouldMovesReturn_A1_B2_C3_D4_E5_F6_G7_WhenBishopOn_H8()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("7B/8/8/8/8/8/8/8 w KQkq - 0 1");
            var bishop = chessboard.GetChessPiece(File.H, Rank.Eight);

            // Act
            var moves = bishop.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(7));
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.A, Rank.One, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.B, Rank.Two, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.C, Rank.Three, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.D, Rank.Four, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.E, Rank.Five, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.F, Rank.Six, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.G, Rank.Seven, moves);
        }

        [Test]
        public void ShouldMovesReturn_A8_B7_C6_D5_E4_F3_G2_WhenBishopOn_H1()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/8/8/8/8/7B w KQkq - 0 1");
            var bishop = chessboard.GetChessPiece(File.H, Rank.One);

            // Act
            var moves = bishop.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(7));
            AssertMoveExistsInMoves(File.H, Rank.One, File.A, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.H, Rank.One, File.B, Rank.Seven, moves);
            AssertMoveExistsInMoves(File.H, Rank.One, File.C, Rank.Six, moves);
            AssertMoveExistsInMoves(File.H, Rank.One, File.D, Rank.Five, moves);
            AssertMoveExistsInMoves(File.H, Rank.One, File.E, Rank.Four, moves);
            AssertMoveExistsInMoves(File.H, Rank.One, File.F, Rank.Three, moves);
            AssertMoveExistsInMoves(File.H, Rank.One, File.G, Rank.Two, moves);
        }

        [Test]
        public void ShouldMovesReturn_A1_B2_D4_E5_F6_G7_H8_A5_B4_D2_E1_WhenBishopOn_C3()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/8/8/2B5/8/8 w KQkq - 0 1");
            var bishop = chessboard.GetChessPiece(File.C, Rank.Three);

            // Act
            var moves = bishop.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(11));
            AssertMoveExistsInMoves(File.C, Rank.Three, File.A, Rank.One, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.B, Rank.Two, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.D, Rank.Four, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.E, Rank.Five, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.F, Rank.Six, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.G, Rank.Seven, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.H, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.A, Rank.Five, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.B, Rank.Four, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.D, Rank.Two, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.E, Rank.One, moves);
        }

        [Test]
        public void ShouldMovesReturn_A8_B7_C6_D5_E4_G2_H1_D1_E2_G4_H5_WhenBishopOn_F3()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/8/8/5B2/8/8 w KQkq - 0 1");
            var bishop = chessboard.GetChessPiece(File.F, Rank.Three);

            // Act
            var moves = bishop.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(11));
            AssertMoveExistsInMoves(File.F, Rank.Three, File.A, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.F, Rank.Three, File.B, Rank.Seven, moves);
            AssertMoveExistsInMoves(File.F, Rank.Three, File.C, Rank.Six, moves);
            AssertMoveExistsInMoves(File.F, Rank.Three, File.D, Rank.Five, moves);
            AssertMoveExistsInMoves(File.F, Rank.Three, File.E, Rank.Four, moves);
            AssertMoveExistsInMoves(File.F, Rank.Three, File.G, Rank.Two, moves);
            AssertMoveExistsInMoves(File.F, Rank.Three, File.H, Rank.One, moves);
            AssertMoveExistsInMoves(File.F, Rank.Three, File.D, Rank.One, moves);
            AssertMoveExistsInMoves(File.F, Rank.Three, File.E, Rank.Two, moves);
            AssertMoveExistsInMoves(File.F, Rank.Three, File.G, Rank.Four, moves);
            AssertMoveExistsInMoves(File.F, Rank.Three, File.H, Rank.Five, moves);
        }

        [Test]
        public void ShouldMovesReturnNoMoves_WhenBishopOn_C3_AndNearbyFieldsOccupiedByOwnPieces()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/8/1P1P4/2B5/1P1P4/8 w KQkq - 0 1");
            var bishop = chessboard.GetChessPiece(File.C, Rank.Three);

            // Act
            var moves = bishop.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(0));
        }

        [Test]
        public void ShouldMovesReturn_B2_B4_D2_D4_WhenBishopOn_C3_AndNearbyFieldsOccupiedByEnemyPieces()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/8/1p1p4/2B5/1p1p4/8 w KQkq - 0 1");
            var bishop = chessboard.GetChessPiece(File.C, Rank.Three);

            // Act
            var moves = bishop.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(4));
            AssertMoveExistsInMoves(File.C, Rank.Three, File.B, Rank.Two, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.B, Rank.Four, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.D, Rank.Two, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.D, Rank.Four, moves);
        }

        [Test]
        public void ShouldMovesReturn_B2_D4_E5_F6_D2_E1_WhenBishopOn_C3_AndAsInGivenFenString()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/5p2/8/1P6/2B5/8/P3p3 w KQkq - 0 1");
            var bishop = chessboard.GetChessPiece(File.C, Rank.Three);

            // Act
            var moves = bishop.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(6));
            AssertMoveExistsInMoves(File.C, Rank.Three, File.B, Rank.Two, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.D, Rank.Four, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.E, Rank.Five, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.F, Rank.Six, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.D, Rank.Two, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.E, Rank.One, moves);
        }
    }
}