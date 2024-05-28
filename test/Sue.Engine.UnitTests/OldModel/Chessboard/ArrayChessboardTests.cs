using System.Linq;
using NUnit.Framework;
using Sue.Engine.Model;
using Sue.Engine.OldModel;
using Sue.Engine.OldModel.Chessboard;
using Sue.Engine.OldModel.Chessboard.Internal;
using Sue.Engine.OldModel.ChessPiece.Internal;
using Sue.Engine.OldModel.Fen;
using Sue.Engine.UnitTests.Common;

namespace Sue.Engine.UnitTests.OldModel.Chessboard
{
    [TestFixture]
    public class ArrayChessboardTests : CommonTestsBase
    {
        [Test]
        public void ShouldGetChessPieceReturnNull_WhenChessboardEmpty()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create(FenString.Empty);

            // Act
            var chessPiece = chessboard.GetChessPiece(File.A, Rank.One);

            // Assert
            Assert.That(chessPiece, Is.Null);
        }

        [Test]
        public void ShouldSetAndGetChessPiece()
        {
            // Arrange
            var rookMovesFinder = new RookMovesFinder();
            var bishopMovesFinder = new BishopMovesFinder();
            var chessPieceFactory = new ChessPieceFactory(rookMovesFinder, bishopMovesFinder);
            var chessboard = new ArrayChessboard(chessPieceFactory);

            // Act
            chessboard.SetChessPiece(ChessPieceKind.Pawn, Color.White, File.A, Rank.One);
            var chessPiece = chessboard.GetChessPiece(File.A, Rank.One);

            // Assert
            Assert.That(chessPiece, Is.Not.Null);
            Assert.That(chessPiece.Color, Is.EqualTo(Color.White));
            Assert.That(chessPiece.ChessboardField.File, Is.EqualTo(File.A));
            Assert.That(chessPiece.ChessboardField.Rank, Is.EqualTo(Rank.One));
        }

        [Test]
        public void ShouldGetChessboardFieldReturnTheSameField_LikeFieldOfChessPiece()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create(FenString.StartPos);
            var expectedChessboardField = chessboard.GetChessPiece(File.A, Rank.One).ChessboardField;

            // Act
            var chessboardField = chessboard.GetChessboardField(File.A, Rank.One);

            // Assert
            Assert.That(chessboardField, Is.EqualTo(expectedChessboardField));
        }

        [Test]
        public void ShouldReturnNoChessPieces_WhenChessboardEmpty()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create(FenString.Empty);

            // Act
            var whiteChessPieces = chessboard.GetChessPieces(Color.White);
            var blackChessPieces = chessboard.GetChessPieces(Color.Black);

            // Assert
            Assert.That(whiteChessPieces, Is.Empty);
            Assert.That(blackChessPieces, Is.Empty);
        }

        [Test]
        public void ShouldReturnAllWhiteChessPieces_WhenChessboardInInitialState()
        {
            // Arrange
            var chessboard = ChessboardFactory.Create(FenString.StartPos);

            // Act
            var whiteChessPieces = chessboard.GetChessPieces(Color.White);

            // Assert
            Assert.That(whiteChessPieces.Count(), Is.EqualTo(16));
        }
    }
}