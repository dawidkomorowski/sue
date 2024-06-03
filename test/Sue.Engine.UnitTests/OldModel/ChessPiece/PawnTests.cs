using System.Linq;
using NUnit.Framework;
using Sue.Engine.Model;
using Sue.Engine.OldModel.Chessboard;
using Sue.Engine.UnitTests.Common;

namespace Sue.Engine.UnitTests.OldModel.ChessPiece
{
    [TestFixture]
    public class PawnTests : CommonTestsBase
    {
        [Test]
        public void ShouldMovesReturn_F3_F4_WhenWhitePawnOn_F2()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/8/8/8/5P2/8 w KQkq - 0 1");
            var pawn = chessboard.GetChessPiece(File.F, Rank.Two);

            // Act
            var moves = pawn.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(2));
            AssertMoveExistsInMoves(File.F, Rank.Two, File.F, Rank.Three, moves);
            AssertMoveExistsInMoves(File.F, Rank.Two, File.F, Rank.Four, moves);
        }

        [Test]
        public void ShouldMovesReturn_F3_WhenWhitePawnOn_F2_AndNextFrontFieldOccupied()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/8/5P2/8/5P2/8 w KQkq - 0 1");
            var pawn = chessboard.GetChessPiece(File.F, Rank.Two);

            // Act
            var moves = pawn.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(1));
            AssertMoveExistsInMoves(File.F, Rank.Two, File.F, Rank.Three, moves);
        }

        [Test]
        public void ShouldMovesReturnNoMoves_WhenWhitePawnOn_F2_AndFrontFieldOccupied()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/8/8/5P2/5P2/8 w KQkq - 0 1");
            var pawn = chessboard.GetChessPiece(File.F, Rank.Two);

            // Act
            var moves = pawn.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(0));
        }

        [Test]
        public void ShouldMovesReturn_F3_F4_E3_G3_WhenWhitePawnOn_F2_AndEnemyPieceOnLeftAndOnRight()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/8/8/4p1p1/5P2/8 w KQkq - 0 1");
            var pawn = chessboard.GetChessPiece(File.F, Rank.Two);

            // Act
            var moves = pawn.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(4));
            AssertMoveExistsInMoves(File.F, Rank.Two, File.F, Rank.Three, moves);
            AssertMoveExistsInMoves(File.F, Rank.Two, File.F, Rank.Four, moves);
            AssertMoveExistsInMoves(File.F, Rank.Two, File.E, Rank.Three, moves);
            AssertMoveExistsInMoves(File.F, Rank.Two, File.G, Rank.Three, moves);
        }

        [Test]
        public void ShouldMovesReturn_B5_WhenWhitePawnOn_B4()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/8/1P6/8/8/8 w KQkq - 0 1");
            var pawn = chessboard.GetChessPiece(File.B, Rank.Four);

            // Act
            var moves = pawn.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(1));
            AssertMoveExistsInMoves(File.B, Rank.Four, File.B, Rank.Five, moves);
        }

        [Test]
        public void ShouldMovesReturn_B5_A5_C5_WhenWhitePawnOn_B4_AndEnemyPieceOnLeftAndOnRight()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/p1p5/1P6/8/8/8 w KQkq - 0 1");
            var pawn = chessboard.GetChessPiece(File.B, Rank.Four);

            // Act
            var moves = pawn.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(3));
            AssertMoveExistsInMoves(File.B, Rank.Four, File.B, Rank.Five, moves);
            AssertMoveExistsInMoves(File.B, Rank.Four, File.A, Rank.Five, moves);
            AssertMoveExistsInMoves(File.B, Rank.Four, File.C, Rank.Five, moves);
        }

        [Test]
        public void ShouldMovesReturn_F6_F5_WhenBlackPawnOn_F7()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/5p2/8/8/8/8/8/8 w KQkq - 0 1");
            var pawn = chessboard.GetChessPiece(File.F, Rank.Seven);

            // Act
            var moves = pawn.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(2));
            AssertMoveExistsInMoves(File.F, Rank.Seven, File.F, Rank.Six, moves);
            AssertMoveExistsInMoves(File.F, Rank.Seven, File.F, Rank.Five, moves);
        }

        [Test]
        public void ShouldMovesReturn_F6_WhenBlackPawnOn_F7_AndNextFrontFieldOccupied()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/5p2/8/5p2/8/8/8/8 w KQkq - 0 1");
            var pawn = chessboard.GetChessPiece(File.F, Rank.Seven);

            // Act
            var moves = pawn.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(1));
            AssertMoveExistsInMoves(File.F, Rank.Seven, File.F, Rank.Six, moves);
        }

        [Test]
        public void ShouldMovesReturnNoMoves_WhenBlackPawnOn_F7_AndFrontFieldOccupied()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/5p2/5p2/8/8/8/8/8 w KQkq - 0 1");
            var pawn = chessboard.GetChessPiece(File.F, Rank.Seven);

            // Act
            var moves = pawn.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(0));
        }

        [Test]
        public void ShouldMovesReturn_F6_F5_E6_G6_WhenBlackPawnOn_F7_AndEnemyPieceOnLeftAndOnRight()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/5p2/4P1P1/8/8/8/8/8 w KQkq - 0 1");
            var pawn = chessboard.GetChessPiece(File.F, Rank.Seven);

            // Act
            var moves = pawn.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(4));
            AssertMoveExistsInMoves(File.F, Rank.Seven, File.F, Rank.Six, moves);
            AssertMoveExistsInMoves(File.F, Rank.Seven, File.F, Rank.Five, moves);
            AssertMoveExistsInMoves(File.F, Rank.Seven, File.E, Rank.Six, moves);
            AssertMoveExistsInMoves(File.F, Rank.Seven, File.G, Rank.Six, moves);
        }

        [Test]
        public void ShouldMovesReturn_B4_WhenBlackPawnOn_B5()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/1p6/8/8/8/8 w KQkq - 0 1");
            var pawn = chessboard.GetChessPiece(File.B, Rank.Five);

            // Act
            var moves = pawn.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(1));
            AssertMoveExistsInMoves(File.B, Rank.Five, File.B, Rank.Four, moves);
        }

        [Test]
        public void ShouldMovesReturn_B4_A4_C4_WhenBlackPawnOn_B5_AndEnemyPieceOnLeftAndOnRight()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/1p6/P1P5/8/8/8 w KQkq - 0 1");
            var pawn = chessboard.GetChessPiece(File.B, Rank.Five);

            // Act
            var moves = pawn.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(3));
            AssertMoveExistsInMoves(File.B, Rank.Five, File.B, Rank.Four, moves);
            AssertMoveExistsInMoves(File.B, Rank.Five, File.A, Rank.Four, moves);
            AssertMoveExistsInMoves(File.B, Rank.Five, File.C, Rank.Four, moves);
        }
    }
}