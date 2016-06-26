using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Internal;
using NUnit.Framework;
using Sue.Common.Model;
using Sue.Common.Model.Chessboard;
using Sue.Common.Model.Chessboard.Internal;
using Sue.Common.Model.ChessPiece.Internal;
using Sue.Common.Model.Fen;
using Sue.Common.UnitTests.Common;

namespace Sue.Common.UnitTests.Model.Chessboard
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
            CollectionAssert.IsEmpty(whiteChessPieces);
            CollectionAssert.IsEmpty(blackChessPieces);
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
