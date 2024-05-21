using System;
using NUnit.Framework;
using Sue.Common.Model.Chessboard;
using Sue.Common.Model.Fen.Internal;

namespace Sue.Common.UnitTests.Model.Fen
{
    [TestFixture]
    public class ChessFieldParserTests
    {
        [TestCase("a1", File.A, Rank.One)]
        [TestCase("d4", File.D, Rank.Four)]
        [TestCase("h8", File.H, Rank.Eight)]
        public void ShouldReturnChessFieldGivenChessFieldString(string chessFieldString, File file, Rank rank)
        {
            // Arrange
            var chessFieldParser = ChessFieldParser;

            // Act
            var chessField = chessFieldParser.Parse(chessFieldString);

            // Assert
            Assert.That(chessField.File, Is.EqualTo(file));
            Assert.That(chessField.Rank, Is.EqualTo(rank));
        }

        [TestCase("c2e4")]
        [TestCase("c2p")]
        [TestCase("c")]
        [TestCase("")]
        public void ShouldRaiseExceptionGivenInvalidChessFieldString(string chessFieldString)
        {
            // Arrange
            var chessFieldParser = ChessFieldParser;

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => chessFieldParser.Parse(chessFieldString));
        }

        private static IChessFieldParser ChessFieldParser => new ChessFieldParser();
    }
}