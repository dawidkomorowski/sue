using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Sue.Common.Model;
using Sue.Common.Model.Chessboard.Internal;
using Sue.Common.Model.Fen.Internal;

namespace Sue.Common.UnitTests.Model.Fen
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