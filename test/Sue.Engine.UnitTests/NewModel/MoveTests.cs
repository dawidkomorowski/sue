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

    [TestCase("", File.A, Rank.One, File.A, Rank.One, Promotion.None, true)]
    [TestCase("    ", File.A, Rank.One, File.A, Rank.One, Promotion.None, true)]
    [TestCase("     ", File.A, Rank.One, File.A, Rank.One, Promotion.None, true)]
    [TestCase("x1a2", File.A, Rank.One, File.A, Rank.Two, Promotion.None, true)]
    [TestCase("a0a2", File.A, Rank.One, File.A, Rank.Two, Promotion.None, true)]
    [TestCase("a1y2", File.A, Rank.One, File.A, Rank.Two, Promotion.None, true)]
    [TestCase("a1a9", File.A, Rank.One, File.A, Rank.Two, Promotion.None, true)]
    [TestCase("c7c8z", File.C, Rank.Seven, File.C, Rank.Eight, Promotion.None, true)]
    [TestCase("a1a2", File.A, Rank.One, File.A, Rank.Two, Promotion.None, false)]
    [TestCase("b1b2", File.B, Rank.One, File.B, Rank.Two, Promotion.None, false)]
    [TestCase("a3a4", File.A, Rank.Three, File.A, Rank.Four, Promotion.None, false)]
    [TestCase("a1h8", File.A, Rank.One, File.H, Rank.Eight, Promotion.None, false)]
    [TestCase("h1a8", File.H, Rank.One, File.A, Rank.Eight, Promotion.None, false)]
    [TestCase("c7c8q", File.C, Rank.Seven, File.C, Rank.Eight, Promotion.Queen, false)]
    [TestCase("d2d1r", File.D, Rank.Two, File.D, Rank.One, Promotion.Rook, false)]
    [TestCase("e7e8b", File.E, Rank.Seven, File.E, Rank.Eight, Promotion.Bishop, false)]
    [TestCase("f2f1n", File.F, Rank.Two, File.F, Rank.One, Promotion.Knight, false)]
    [TestCase("F2F1N", File.F, Rank.Two, File.F, Rank.One, Promotion.Knight, false)]
    [TestCase("  f2f1n    ", File.F, Rank.Two, File.F, Rank.One, Promotion.Knight, false)]
    public void ParseUciMove_ShouldCreateMoveIfUciIsValid(string uciMove, File fileFrom, Rank rankFrom, File fileTo, Rank rankTo, object promotion,
        bool expectedException)
    {
        if (expectedException)
        {
            // Arrange
            // Act
            // Assert
            Assert.That(() => Move.ParseUciMove(uciMove), Throws.Exception);
        }
        else
        {
            // Arrange
            // Act
            var move = Move.ParseUciMove(uciMove);

            // Assert
            Assert.That(move.From, Is.EqualTo(new Position(fileFrom, rankFrom)));
            Assert.That(move.To, Is.EqualTo(new Position(fileTo, rankTo)));
            Assert.That(move.Promotion, Is.EqualTo(promotion));
        }
    }

    [TestCase(File.A, Rank.One, File.B, Rank.Two, Promotion.None, "a1b2")]
    [TestCase(File.C, Rank.Three, File.D, Rank.Four, Promotion.None, "c3d4")]
    [TestCase(File.E, Rank.Five, File.F, Rank.Six, Promotion.None, "e5f6")]
    [TestCase(File.G, Rank.Seven, File.H, Rank.Eight, Promotion.None, "g7h8")]
    [TestCase(File.A, Rank.Seven, File.A, Rank.Eight, Promotion.Queen, "a7a8q")]
    [TestCase(File.A, Rank.Seven, File.A, Rank.Eight, Promotion.Rook, "a7a8r")]
    [TestCase(File.A, Rank.Seven, File.A, Rank.Eight, Promotion.Bishop, "a7a8b")]
    [TestCase(File.A, Rank.Seven, File.A, Rank.Eight, Promotion.Knight, "a7a8n")]
    public void ToUci_ShouldReturnUciMove(File fileFrom, Rank rankFrom, File fileTo, Rank rankTo, object promotion, string uciMove)
    {
        // Arrange
        var move = new Move(new Position(fileFrom, rankFrom), new Position(fileTo, rankTo), (Promotion)promotion);

        // Act
        var actual = move.ToUci();

        // Assert
        Assert.That(actual, Is.EqualTo(uciMove));
    }
}