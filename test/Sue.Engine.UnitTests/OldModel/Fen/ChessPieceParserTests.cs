using System;
using NUnit.Framework;
using Sue.Engine.Model;
using Sue.Engine.OldModel.Chessboard.Internal;
using Sue.Engine.OldModel.Fen.Internal;

namespace Sue.Engine.UnitTests.OldModel.Fen
{
    [TestFixture]
    public class ChessPieceParserTests
    {
        [TestCase('P', Color.White, ChessPieceKind.Pawn)]
        [TestCase('B', Color.White, ChessPieceKind.Bishop)]
        [TestCase('N', Color.White, ChessPieceKind.Knight)]
        [TestCase('R', Color.White, ChessPieceKind.Rook)]
        [TestCase('Q', Color.White, ChessPieceKind.Queen)]
        [TestCase('K', Color.White, ChessPieceKind.King)]
        [TestCase('p', Color.Black, ChessPieceKind.Pawn)]
        [TestCase('b', Color.Black, ChessPieceKind.Bishop)]
        [TestCase('n', Color.Black, ChessPieceKind.Knight)]
        [TestCase('r', Color.Black, ChessPieceKind.Rook)]
        [TestCase('q', Color.Black, ChessPieceKind.Queen)]
        [TestCase('k', Color.Black, ChessPieceKind.King)]
        public void ShouldReturn_ExpectedPieceKindOfExpectedColor_GivenFenChessPieceCode(char fenChessPieceCode, Color expectedColor,
            object expectedChessPieceKind)
        {
            // Arrange
            IChessPieceParser chessPieceParser = new ChessPieceParser();

            // Act
            var chessPiece = chessPieceParser.Parse(fenChessPieceCode);

            // Assert
            Assert.That(chessPiece.Color, Is.EqualTo(expectedColor));
            Assert.That(chessPiece.ChessPieceKind, Is.EqualTo(expectedChessPieceKind));
        }

        [Test]
        public void ShouldThrowException_GivenIncorrectFenChessPieceCode()
        {
            // Arrange
            IChessPieceParser chessPieceParser = new ChessPieceParser();

            char fenChessPieceCode = 'J';

            // Act
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => chessPieceParser.Parse(fenChessPieceCode));
        }
    }
}