using NUnit.Framework;
using Sue.Engine.Model;

namespace Sue.Engine.UnitTests.Model;

[TestFixture]
public class ColorTests
{
    [TestCase(Color.White, Color.Black)]
    [TestCase(Color.Black, Color.White)]
    public void Opposite_ShouldReturnOppositeColor(Color initialColor, Color oppositeColor)
    {
        // Arrange
        // Act
        var actual = initialColor.Opposite();

        // Assert
        Assert.That(actual, Is.EqualTo(oppositeColor));
    }
}