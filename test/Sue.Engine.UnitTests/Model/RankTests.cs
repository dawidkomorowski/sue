using NUnit.Framework;
using Sue.Engine.Model;

namespace Sue.Engine.UnitTests.Model
{
    [TestFixture]
    public class RankTests
    {
        [TestCase(Rank.One, 0, Rank.One)]
        [TestCase(Rank.One, 1, Rank.Two)]
        [TestCase(Rank.One, 7, Rank.Eight)]
        [TestCase(Rank.Eight, -1, Rank.Seven)]
        [TestCase(Rank.Eight, -7, Rank.One)]
        public void Add_ShouldReturnCorrectRank_GivenRankAndOffsetToAdd(Rank baseRank, int offset, Rank expectedRank)
        {
            // Arrange
            // Act
            var actualFile = baseRank.Add(offset);

            // Assert
            Assert.That(actualFile, Is.EqualTo(expectedRank));
        }
    }
}