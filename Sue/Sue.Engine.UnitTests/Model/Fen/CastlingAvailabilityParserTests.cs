using NUnit.Framework;
using Sue.Engine.Model.Fen.Internal;

namespace Sue.Engine.UnitTests.Model.Fen
{
    [TestFixture]
    public class CastlingAvailabilityParserTests
    {
        [TestCase("-", false, false, false, false)]
        [TestCase("KQkq", true, true, true, true)]
        [TestCase("Kq", true, false, false, true)]
        [TestCase("Qk", false, true, true, false)]
        public void ShouldReturnCastlingAvailability_GivenCastlingAvailabilityString(string castlingAvailabilityString,
            bool whiteKingsideCastlingAvailable, bool whiteQueenssideCastlingAvailable,
            bool blackKingsideCastlingAvailable, bool blackQueensideCastlingAvailable)
        {
            // Arrange
            var castlingAvailabilityParser = CastlingAvailabilityParser;

            // Act
            var castlingAvailability = castlingAvailabilityParser.Parse(castlingAvailabilityString);

            // Assert
            Assert.That(castlingAvailability.BlackKingsideCastlingAvailable, Is.EqualTo(blackKingsideCastlingAvailable));
            Assert.That(castlingAvailability.BlackQueensideCastlingAvailable,
                Is.EqualTo(blackQueensideCastlingAvailable));
            Assert.That(castlingAvailability.WhiteKingsideCastlingAvailable, Is.EqualTo(whiteKingsideCastlingAvailable));
            Assert.That(castlingAvailability.WhiteQueenssideCastlingAvailable,
                Is.EqualTo(whiteQueenssideCastlingAvailable));
        }

        private ICastlingAvailabilityParser CastlingAvailabilityParser => new CastlingAvailabilityParser();
    }
}