using System.Linq;
using NUnit.Framework;
using Sue.Engine.Model;
using Sue.Engine.OldModel.Chessboard;
using Sue.Engine.UnitTests.Common;

namespace Sue.Engine.UnitTests.OldModel.ChessPiece
{
    [TestFixture]
    public class QueenTests : CommonTestsBase
    {
        [Test]
        public void ShouldMovesReturn_A2_A3_A4_A5_A6_A7_A8_B2_C3_D4_E5_F6_G7_H8_B1_C1_D1_E1_F1_G1_H1_WhenQueenOn_A1()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/8/8/8/8/Q7 w KQkq - 0 1");
            var queen = chessboard.GetChessPiece(File.A, Rank.One);

            // Act
            var moves = queen.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(21));

            AssertMoveExistsInMoves(File.A, Rank.One, File.A, Rank.Two, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.A, Rank.Three, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.A, Rank.Four, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.A, Rank.Five, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.A, Rank.Six, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.A, Rank.Seven, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.A, Rank.Eight, moves);

            AssertMoveExistsInMoves(File.A, Rank.One, File.B, Rank.Two, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.C, Rank.Three, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.D, Rank.Four, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.E, Rank.Five, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.F, Rank.Six, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.G, Rank.Seven, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.H, Rank.Eight, moves);

            AssertMoveExistsInMoves(File.A, Rank.One, File.B, Rank.One, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.C, Rank.One, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.D, Rank.One, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.E, Rank.One, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.F, Rank.One, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.G, Rank.One, moves);
            AssertMoveExistsInMoves(File.A, Rank.One, File.H, Rank.One, moves);
        }

        [Test]
        public void ShouldMovesReturn_A1_A2_A3_A4_A5_A6_A7_B7_C6_D5_E4_F3_G2_H1_B8_C8_D8_E8_F8_G8_H8_WhenQueenOn_A8()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("Q7/8/8/8/8/8/8/8 w KQkq - 0 1");
            var queen = chessboard.GetChessPiece(File.A, Rank.Eight);

            // Act
            var moves = queen.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(21));

            AssertMoveExistsInMoves(File.A, Rank.Eight, File.A, Rank.One, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.A, Rank.Two, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.A, Rank.Three, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.A, Rank.Four, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.A, Rank.Five, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.A, Rank.Six, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.A, Rank.Seven, moves);

            AssertMoveExistsInMoves(File.A, Rank.Eight, File.B, Rank.Seven, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.C, Rank.Six, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.D, Rank.Five, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.E, Rank.Four, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.F, Rank.Three, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.G, Rank.Two, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.H, Rank.One, moves);

            AssertMoveExistsInMoves(File.A, Rank.Eight, File.B, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.C, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.D, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.E, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.F, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.G, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.A, Rank.Eight, File.H, Rank.Eight, moves);
        }

        [Test]
        public void ShouldMovesReturn_H1_H2_H3_H4_H5_H6_H7_A1_B2_C3_D4_E5_F6_G7_A8_B8_C8_D8_E8_F8_G8_WhenQueenOn_H8()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("7Q/8/8/8/8/8/8/8 w KQkq - 0 1");
            var queen = chessboard.GetChessPiece(File.H, Rank.Eight);

            // Act
            var moves = queen.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(21));

            AssertMoveExistsInMoves(File.H, Rank.Eight, File.H, Rank.One, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.H, Rank.Two, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.H, Rank.Three, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.H, Rank.Four, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.H, Rank.Five, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.H, Rank.Six, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.H, Rank.Seven, moves);

            AssertMoveExistsInMoves(File.H, Rank.Eight, File.A, Rank.One, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.B, Rank.Two, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.C, Rank.Three, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.D, Rank.Four, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.E, Rank.Five, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.F, Rank.Six, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.G, Rank.Seven, moves);

            AssertMoveExistsInMoves(File.H, Rank.Eight, File.A, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.B, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.C, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.D, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.E, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.F, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.H, Rank.Eight, File.G, Rank.Eight, moves);
        }

        [Test]
        public void ShouldMovesReturn_H2_H3_H4_H5_H6_H7_H8_A8_B7_C6_D5_E4_F3_G2_A1_B1_C1_D1_E1_F1_G1_WhenQueenOn_H1()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/8/8/8/8/7Q w KQkq - 0 1");
            var queen = chessboard.GetChessPiece(File.H, Rank.One);

            // Act
            var moves = queen.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(21));

            AssertMoveExistsInMoves(File.H, Rank.One, File.H, Rank.Two, moves);
            AssertMoveExistsInMoves(File.H, Rank.One, File.H, Rank.Three, moves);
            AssertMoveExistsInMoves(File.H, Rank.One, File.H, Rank.Four, moves);
            AssertMoveExistsInMoves(File.H, Rank.One, File.H, Rank.Five, moves);
            AssertMoveExistsInMoves(File.H, Rank.One, File.H, Rank.Six, moves);
            AssertMoveExistsInMoves(File.H, Rank.One, File.H, Rank.Seven, moves);
            AssertMoveExistsInMoves(File.H, Rank.One, File.H, Rank.Eight, moves);

            AssertMoveExistsInMoves(File.H, Rank.One, File.A, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.H, Rank.One, File.B, Rank.Seven, moves);
            AssertMoveExistsInMoves(File.H, Rank.One, File.C, Rank.Six, moves);
            AssertMoveExistsInMoves(File.H, Rank.One, File.D, Rank.Five, moves);
            AssertMoveExistsInMoves(File.H, Rank.One, File.E, Rank.Four, moves);
            AssertMoveExistsInMoves(File.H, Rank.One, File.F, Rank.Three, moves);
            AssertMoveExistsInMoves(File.H, Rank.One, File.G, Rank.Two, moves);

            AssertMoveExistsInMoves(File.H, Rank.One, File.A, Rank.One, moves);
            AssertMoveExistsInMoves(File.H, Rank.One, File.B, Rank.One, moves);
            AssertMoveExistsInMoves(File.H, Rank.One, File.C, Rank.One, moves);
            AssertMoveExistsInMoves(File.H, Rank.One, File.D, Rank.One, moves);
            AssertMoveExistsInMoves(File.H, Rank.One, File.E, Rank.One, moves);
            AssertMoveExistsInMoves(File.H, Rank.One, File.F, Rank.One, moves);
            AssertMoveExistsInMoves(File.H, Rank.One, File.G, Rank.One, moves);
        }

        [Test]
        public void ShouldMovesReturn_A5_B4_A3_B3_A1_B2_C1_C2_D2_E1_D3_E3_F3_G3_H3_C4_C5_C6_C7_C8_D4_E5_F6_G7_H8_WhenQueenOn_C3()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/8/8/2Q5/8/8 w KQkq - 0 1");
            var queen = chessboard.GetChessPiece(File.C, Rank.Three);

            // Act
            var moves = queen.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(25));

            AssertMoveExistsInMoves(File.C, Rank.Three, File.A, Rank.Five, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.B, Rank.Four, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.A, Rank.Three, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.B, Rank.Three, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.A, Rank.One, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.B, Rank.Two, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.C, Rank.One, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.C, Rank.Two, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.D, Rank.Two, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.E, Rank.One, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.D, Rank.Three, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.E, Rank.Three, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.F, Rank.Three, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.G, Rank.Three, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.H, Rank.Three, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.C, Rank.Four, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.C, Rank.Five, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.C, Rank.Six, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.C, Rank.Seven, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.C, Rank.Eight, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.D, Rank.Four, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.E, Rank.Five, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.F, Rank.Six, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.G, Rank.Seven, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.H, Rank.Eight, moves);
        }

        [Test]
        public void ShouldMovesReturn_A3_B3_C3_D3_E3_G3_H3_F1_F2_F4_F5_F6_F7_F8_A8_B7_C6_D5_E4_G2_H1_D1_E2_G4_H5_WhenQueenOn_F3()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/8/8/5Q2/8/8 w KQkq - 0 1");
            var queen = chessboard.GetChessPiece(File.F, Rank.Three);

            // Act
            var moves = queen.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(25));

            AssertMoveExistsInMoves(File.F, Rank.Three, File.A, Rank.Three, moves);
            AssertMoveExistsInMoves(File.F, Rank.Three, File.B, Rank.Three, moves);
            AssertMoveExistsInMoves(File.F, Rank.Three, File.C, Rank.Three, moves);
            AssertMoveExistsInMoves(File.F, Rank.Three, File.D, Rank.Three, moves);
            AssertMoveExistsInMoves(File.F, Rank.Three, File.E, Rank.Three, moves);
            AssertMoveExistsInMoves(File.F, Rank.Three, File.G, Rank.Three, moves);
            AssertMoveExistsInMoves(File.F, Rank.Three, File.H, Rank.Three, moves);
            AssertMoveExistsInMoves(File.F, Rank.Three, File.F, Rank.One, moves);
            AssertMoveExistsInMoves(File.F, Rank.Three, File.F, Rank.Two, moves);
            AssertMoveExistsInMoves(File.F, Rank.Three, File.F, Rank.Four, moves);
            AssertMoveExistsInMoves(File.F, Rank.Three, File.F, Rank.Five, moves);
            AssertMoveExistsInMoves(File.F, Rank.Three, File.F, Rank.Six, moves);
            AssertMoveExistsInMoves(File.F, Rank.Three, File.F, Rank.Seven, moves);
            AssertMoveExistsInMoves(File.F, Rank.Three, File.F, Rank.Eight, moves);
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
        public void ShouldMovesReturnNoMoves_WhenQueenOn_C3_AndNearbyFieldsOccupiedByOwnPieces()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/8/1PPP4/1PQP4/1PPP4/8 w KQkq - 0 1");
            var queen = chessboard.GetChessPiece(File.C, Rank.Three);

            // Act
            var moves = queen.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(0));
        }

        [Test]
        public void ShouldMovesReturn_B2_B3_B4_C2_C4_D2_D3_D4_WhenQueenOn_C3_AndNearbyFieldsOccupiedByEnemyPieces()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/8/8/8/1ppp4/1pQp4/1ppp4/8 w KQkq - 0 1");
            var queen = chessboard.GetChessPiece(File.C, Rank.Three);

            // Act
            var moves = queen.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(8));

            AssertMoveExistsInMoves(File.C, Rank.Three, File.B, Rank.Two, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.B, Rank.Three, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.B, Rank.Four, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.C, Rank.Two, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.C, Rank.Four, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.D, Rank.Two, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.D, Rank.Three, moves);
            AssertMoveExistsInMoves(File.C, Rank.Three, File.D, Rank.Four, moves);
        }

        [Test]
        public void ShouldMovesReturn_A1_B2_C3_E5_F6_B4_C4_E4_F4_A7_B6_C5_E3_D3_D5_D6_D7_WhenQueenOn_D4_AndAsInGivenFenString()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create("8/p2p2P1/8/8/P2Q1p2/8/3P1P2/p7 w KQkq - 0 1");
            var queen = chessboard.GetChessPiece(File.D, Rank.Four);

            // Act
            var moves = queen.Moves;

            // Assert
            Assert.That(moves.Count(), Is.EqualTo(17));

            AssertMoveExistsInMoves(File.D, Rank.Four, File.A, Rank.One, moves);
            AssertMoveExistsInMoves(File.D, Rank.Four, File.B, Rank.Two, moves);
            AssertMoveExistsInMoves(File.D, Rank.Four, File.C, Rank.Three, moves);
            AssertMoveExistsInMoves(File.D, Rank.Four, File.E, Rank.Five, moves);
            AssertMoveExistsInMoves(File.D, Rank.Four, File.F, Rank.Six, moves);
            AssertMoveExistsInMoves(File.D, Rank.Four, File.B, Rank.Four, moves);
            AssertMoveExistsInMoves(File.D, Rank.Four, File.C, Rank.Four, moves);
            AssertMoveExistsInMoves(File.D, Rank.Four, File.E, Rank.Four, moves);
            AssertMoveExistsInMoves(File.D, Rank.Four, File.F, Rank.Four, moves);
            AssertMoveExistsInMoves(File.D, Rank.Four, File.A, Rank.Seven, moves);
            AssertMoveExistsInMoves(File.D, Rank.Four, File.B, Rank.Six, moves);
            AssertMoveExistsInMoves(File.D, Rank.Four, File.C, Rank.Five, moves);
            AssertMoveExistsInMoves(File.D, Rank.Four, File.E, Rank.Three, moves);
            AssertMoveExistsInMoves(File.D, Rank.Four, File.D, Rank.Three, moves);
            AssertMoveExistsInMoves(File.D, Rank.Four, File.D, Rank.Five, moves);
            AssertMoveExistsInMoves(File.D, Rank.Four, File.D, Rank.Six, moves);
            AssertMoveExistsInMoves(File.D, Rank.Four, File.D, Rank.Seven, moves);
        }
    }
}