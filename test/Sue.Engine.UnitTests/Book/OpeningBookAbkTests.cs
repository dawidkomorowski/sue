using NUnit.Framework;
using Sue.Engine.Book;

namespace Sue.Engine.UnitTests.Book;

[TestFixture]
public class OpeningBookAbkTests
{
    [Test]
    public void MaxDepth_IsCorrect()
    {
        // Arrange
        var openingBookAbk = new OpeningBookAbk();

        // Act
        var actual = BookInspector.ComputeMaxDepth(openingBookAbk);

        // Assert
        Assert.That(actual, Is.EqualTo(OpeningBookAbk.MaxDepth));
    }
}