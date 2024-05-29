using NUnit.Framework;
using Sue.Engine.Model;

namespace Sue.Engine.UnitTests.Model;

[TestFixture]
public class FenTests
{
    [Test]
    public void Constructor_ShouldCreateEmptyChessboardState()
    {
        // Arrange
        // Act
        var fen = new Fen();

        // Assert
        Assert.That(fen.ActiveColor, Is.EqualTo(Color.White));
        Assert.That(fen.WhiteKingSideCastlingAvailable, Is.False);
        Assert.That(fen.WhiteQueenSideCastlingAvailable, Is.False);
        Assert.That(fen.BlackKingSideCastlingAvailable, Is.False);
        Assert.That(fen.BlackQueenSideCastlingAvailable, Is.False);
        Assert.That(fen.EnPassantTargetField, Is.Null);
        Assert.That(fen.HalfMoveClock, Is.Zero);
        Assert.That(fen.FullMoveNumber, Is.EqualTo(1));

        foreach (var position in Position.All)
        {
            Assert.That(fen.GetChessPiece(position), Is.EqualTo(ChessPiece.None));
        }
    }

    [Test]
    public void Properties_CanBeSetAndGet()
    {
        // Arrange
        var fen = new Fen();

        // Act
        fen.ActiveColor = Color.Black;
        fen.WhiteKingSideCastlingAvailable = true;
        fen.WhiteQueenSideCastlingAvailable = true;
        fen.BlackKingSideCastlingAvailable = true;
        fen.BlackQueenSideCastlingAvailable = true;
        fen.EnPassantTargetField = new Position(File.C, Rank.Three);
        fen.HalfMoveClock = 2;
        fen.FullMoveNumber = 3;

        fen.SetChessPiece(new Position(File.A, Rank.One), ChessPiece.WhiteKing);
        fen.SetChessPiece(new Position(File.A, Rank.Eight), ChessPiece.WhiteQueen);
        fen.SetChessPiece(new Position(File.D, Rank.Four), ChessPiece.WhitePawn);
        fen.SetChessPiece(new Position(File.D, Rank.Five), ChessPiece.BlackKnight);
        fen.SetChessPiece(new Position(File.H, Rank.One), ChessPiece.BlackQueen);
        fen.SetChessPiece(new Position(File.H, Rank.Eight), ChessPiece.BlackKing);

        // Assert
        Assert.That(fen.ActiveColor, Is.EqualTo(Color.Black));
        Assert.That(fen.WhiteKingSideCastlingAvailable, Is.True);
        Assert.That(fen.WhiteQueenSideCastlingAvailable, Is.True);
        Assert.That(fen.BlackKingSideCastlingAvailable, Is.True);
        Assert.That(fen.BlackQueenSideCastlingAvailable, Is.True);
        Assert.That(fen.EnPassantTargetField, Is.EqualTo(new Position(File.C, Rank.Three)));
        Assert.That(fen.HalfMoveClock, Is.EqualTo(2));
        Assert.That(fen.FullMoveNumber, Is.EqualTo(3));

        Assert.That(fen.GetChessPiece(new Position(File.A, Rank.One)), Is.EqualTo(ChessPiece.WhiteKing));
        Assert.That(fen.GetChessPiece(new Position(File.A, Rank.Eight)), Is.EqualTo(ChessPiece.WhiteQueen));
        Assert.Fail("TODO");
    }
}