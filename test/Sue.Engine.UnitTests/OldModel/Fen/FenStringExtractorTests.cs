using System.Linq;
using NUnit.Framework;
using Sue.Engine.Model;
using Sue.Engine.OldModel;
using Sue.Engine.OldModel.Fen;
using Sue.Engine.OldModel.Fen.Internal;

namespace Sue.Engine.UnitTests.OldModel.Fen
{
    [TestFixture]
    public class FenStringExtractorTests
    {
        [Test]
        public void ShouldReturnExtractedFenStringWith8RankLines()
        {
            // Arrange
            IFenStringExtractor fenStringExtractor = new FenStringExtractor();
            const string fenString = FenString.StartPos;

            // Act
            var extractedFenString = fenStringExtractor.Extract(fenString);

            // Assert
            Assert.That(extractedFenString.RankLines.Count(), Is.EqualTo(8));
            Assert.That(extractedFenString.RankLines.Select(r => r.Rank),
                Is.EqualTo(RankExtensions.Ranks().Reverse()));
            Assert.That(extractedFenString.RankLines.Select(r => r.String),
                Is.EqualTo(new[] { "rnbqkbnr", "pppppppp", "8", "8", "8", "8", "PPPPPPPP", "RNBQKBNR" }));
        }

        [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", Color.White)]
        [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b KQkq - 0 1", Color.Black)]
        public void ShouldReturnExtractedFenStringWithColor(string fenString, Color color)
        {
            // Arrange
            IFenStringExtractor fenStringExtractor = new FenStringExtractor();

            // Act
            var extractedFenString = fenStringExtractor.Extract(fenString);

            // Assert
            Assert.That(extractedFenString.Color, Is.EqualTo(color));
        }

        [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", "KQkq")]
        [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w Kq - 0 1", "Kq")]
        [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w - - 0 1", "-")]
        public void ShouldReturnExtractedFenStringWithCastlingAvailabilityString(string fenString,
            string castlingAvailabilityString)
        {
            // Arrange
            IFenStringExtractor fenStringExtractor = new FenStringExtractor();

            // Act
            var extractedFenString = fenStringExtractor.Extract(fenString);

            // Assert
            Assert.That(extractedFenString.CastlingAvailabilityString, Is.EqualTo(castlingAvailabilityString));
        }

        [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", "-")]
        [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq e3 0 1", "e3")]
        public void ShouldReturnExtractedFenStringWithEnPassantTargetFieldString(string fenString,
            string enPassantTargetFieldString)
        {
            // Arrange
            IFenStringExtractor fenStringExtractor = new FenStringExtractor();

            // Act
            var extractedFenString = fenStringExtractor.Extract(fenString);

            // Assert
            Assert.That(extractedFenString.EnPassantTargetFieldString, Is.EqualTo(enPassantTargetFieldString));
        }

        [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", 0)]
        [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b KQkq - 37 1", 37)]
        public void ShouldReturnExtractedFenStringWithHalfmoveClock(string fenString, int halfmoveClock)
        {
            // Arrange
            IFenStringExtractor fenStringExtractor = new FenStringExtractor();

            // Act
            var extractedFenString = fenStringExtractor.Extract(fenString);

            // Assert
            Assert.That(extractedFenString.HalfmoveClock, Is.EqualTo(halfmoveClock));
        }

        [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", 1)]
        [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b KQkq - 0 29", 29)]
        public void ShouldReturnExtractedFenStringWithFullmoveNumber(string fenString, int fullmoveNumber)
        {
            // Arrange
            IFenStringExtractor fenStringExtractor = new FenStringExtractor();

            // Act
            var extractedFenString = fenStringExtractor.Extract(fenString);

            // Assert
            Assert.That(extractedFenString.FullmoveNumber, Is.EqualTo(fullmoveNumber));
        }
    }
}