using NUnit.Framework;
using Sue.Engine.Model.Chessboard;
using Sue.Engine.NewModel;

namespace Sue.Engine.UnitTests.NewModel;

[TestFixture]
[TestOf(typeof(Move))]
public class MoveTests
{
    [Test]
    public void Constructor_ShouldCreateMoveWithoutPromotion()
    {
        // Arrange
        var from = new Position(File.A, Rank.One);
        var to = new Position(File.B, Rank.Two);

        // Act
        var actual = new Move(from, to);

        // Assert
        Assert.That(actual.From, Is.EqualTo(from));
        Assert.That(actual.To, Is.EqualTo(to));
        Assert.That(actual.Promotion, Is.EqualTo(Promotion.None));
    }

    [Test]
    public void Constructor_ShouldCreateMoveWithPromotion()
    {
        // Arrange
        var from = new Position(File.A, Rank.One);
        var to = new Position(File.B, Rank.Two);
        var promotion = Promotion.Queen;

        // Act
        var actual = new Move(from, to, promotion);

        // Assert
        Assert.That(actual.From, Is.EqualTo(from));
        Assert.That(actual.To, Is.EqualTo(to));
        Assert.That(actual.Promotion, Is.EqualTo(promotion));
    }
}