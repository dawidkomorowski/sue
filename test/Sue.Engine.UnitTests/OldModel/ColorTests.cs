using NUnit.Framework;
using Sue.Engine.OldModel;

namespace Sue.Engine.UnitTests.OldModel
{
    [TestFixture]
    public class ColorTests
    {
        [Test]
        public void ShouldReturnBlack_AsOppositeToWhite()
        {
            // Arrange
            const Color color = Color.White;

            // Act
            var oppositeColor = color.Opposite();

            // Assert
            Assert.That(oppositeColor, Is.EqualTo(Color.Black));
        }

        [Test]
        public void ShouldReturnWhite_AsOppositeToBlack()
        {
            // Arrange
            const Color color = Color.Black;

            // Act
            var oppositeColor = color.Opposite();

            // Assert
            Assert.That(oppositeColor, Is.EqualTo(Color.White));
        }
    }
}