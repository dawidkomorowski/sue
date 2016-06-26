using System;
using NUnit.Framework;
using Sue.Common.Model;
using Sue.Common.Model.Chessboard;
using Sue.Common.Model.Chessboard.Internal;
using Sue.Common.Model.ChessPiece.Internal;

namespace Sue.Common.UnitTests.Model.Chessboard
{
    [TestFixture]
    public class ChessPieceFactoryTests
    {
        [TestCase(ChessPieceKind.Pawn, Color.Black, typeof(Pawn))]
        [TestCase(ChessPieceKind.Bishop, Color.White, typeof(Bishop))]
        [TestCase(ChessPieceKind.Knight, Color.Black, typeof(Knight))]
        [TestCase(ChessPieceKind.Rook, Color.White, typeof(Rook))]
        [TestCase(ChessPieceKind.Queen, Color.Black, typeof(Queen))]
        [TestCase(ChessPieceKind.King, Color.White, typeof(King))]
        public void ShouldReturnChessPieceOfGivenKindAndColor(object chessPieceKind, Color color, Type expectedType)
        {
            // Arrange
            var chessPieceFactory = ChessPieceFactory;

            // Act
            var chessPiece = chessPieceFactory.Create((ChessPieceKind) chessPieceKind, color, null);

            // Assert
            Assert.That(chessPiece.Color, Is.EqualTo(color));
            Assert.That(chessPiece, Is.TypeOf(expectedType));
            Assert.That(chessPiece.ChessboardField, Is.Null);
        }

        [Test]
        public void ShouldReturnChessPieceWithGivenChessboardField()
        {
            // Arrange
            var chessPieceFactory = ChessPieceFactory;
            const ChessPieceKind chessPieceKind = ChessPieceKind.Pawn;
            const Color color = Color.Black;
            var chessboardField = new ChessboardField(File.A, Rank.Seven, null);

            // Act
            var chessPiece = chessPieceFactory.Create(chessPieceKind, color, chessboardField);

            // Assert
            Assert.That(chessPiece.Color, Is.EqualTo(color));
            Assert.That(chessPiece, Is.TypeOf<Pawn>());
            Assert.That(chessPiece.ChessboardField, Is.EqualTo(chessboardField));
        }

        private static IChessPieceFactory ChessPieceFactory
        {
            get
            {
                var rookMovesFinder = new RookMovesFinder();
                var bishopMovesFinder = new BishopMovesFinder();
                return new ChessPieceFactory(rookMovesFinder, bishopMovesFinder);
            }
        }
    }
}