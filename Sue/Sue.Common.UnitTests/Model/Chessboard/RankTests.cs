using NUnit.Framework;
using Sue.Common.Model.Chessboard;

namespace Sue.Common.UnitTests.Model.Chessboard
{
    [TestFixture]
    public class RankTests
    {
        [TestCase(Rank.One, 0)]
        [TestCase(Rank.Two, 1)]
        [TestCase(Rank.Three, 2)]
        [TestCase(Rank.Four, 3)]
        [TestCase(Rank.Five, 4)]
        [TestCase(Rank.Six, 5)]
        [TestCase(Rank.Seven, 6)]
        [TestCase(Rank.Eight, 7)]
        public void ShouldReturnCorespondingIntegerIndexValue_GivenSomeRank(Rank rank, int index)
        {
            // Arrange
            // Act
            var actualIndex = rank.Index();

            // Assert
            Assert.That(actualIndex, Is.EqualTo(index));
        }

        [TestCase(Rank.One, 0, Rank.One)]
        [TestCase(Rank.One, 1, Rank.Two)]
        [TestCase(Rank.One, 7, Rank.Eight)]
        [TestCase(Rank.Eight, -1, Rank.Seven)]
        [TestCase(Rank.Eight, -7, Rank.One)]
        public void ShouldReturnCorrectRank_GivenSomeRankAndOffsetToAdd(Rank baseRank, int offset, Rank expectedRank)
        {
            // Arrange
            // Act
            var actualFile = baseRank.Add(offset);

            // Assert
            Assert.That(actualFile, Is.EqualTo(expectedRank));
        }
    }
}