using NUnit.Framework;
using Sue.Engine.Model;

namespace Sue.Engine.UnitTests.Model;

[TestFixture]
public class ChessboardTests
{
    [Test]
    public void Constructor_ShouldCreateEmptyChessboardState()
    {
        // Arrange
        // Act
        var chessboard = new Chessboard();

        // Assert
        Assert.That(chessboard.ActiveColor, Is.EqualTo(Color.White));
        Assert.That(chessboard.WhiteKingSideCastlingAvailable, Is.False);
        Assert.That(chessboard.WhiteQueenSideCastlingAvailable, Is.False);
        Assert.That(chessboard.BlackKingSideCastlingAvailable, Is.False);
        Assert.That(chessboard.BlackQueenSideCastlingAvailable, Is.False);
        Assert.That(chessboard.EnPassantTargetPosition, Is.Null);
        Assert.That(chessboard.HalfMoveClock, Is.Zero);
        Assert.That(chessboard.FullMoveNumber, Is.EqualTo(1));

        foreach (var position in Position.All)
        {
            Assert.That(chessboard.GetChessPiece(position), Is.EqualTo(ChessPiece.None));
        }
    }

    [Test]
    public void Properties_CanBeSetAndGet()
    {
        // Arrange
        var chessboard = new Chessboard();

        // Act
        chessboard.ActiveColor = Color.Black;
        chessboard.WhiteKingSideCastlingAvailable = true;
        chessboard.WhiteQueenSideCastlingAvailable = true;
        chessboard.BlackKingSideCastlingAvailable = true;
        chessboard.BlackQueenSideCastlingAvailable = true;
        chessboard.EnPassantTargetPosition = new Position(File.C, Rank.Three);
        chessboard.HalfMoveClock = 2;
        chessboard.FullMoveNumber = 3;

        chessboard.SetChessPiece(new Position(File.A, Rank.One), ChessPiece.WhiteKing);
        chessboard.SetChessPiece(new Position(File.A, Rank.Eight), ChessPiece.WhiteQueen);
        chessboard.SetChessPiece(new Position(File.D, Rank.Four), ChessPiece.WhitePawn);
        chessboard.SetChessPiece(new Position(File.D, Rank.Five), ChessPiece.BlackKnight);
        chessboard.SetChessPiece(new Position(File.H, Rank.One), ChessPiece.BlackQueen);
        chessboard.SetChessPiece(new Position(File.H, Rank.Eight), ChessPiece.BlackKing);

        // Assert
        Assert.That(chessboard.ActiveColor, Is.EqualTo(Color.Black));
        Assert.That(chessboard.WhiteKingSideCastlingAvailable, Is.True);
        Assert.That(chessboard.WhiteQueenSideCastlingAvailable, Is.True);
        Assert.That(chessboard.BlackKingSideCastlingAvailable, Is.True);
        Assert.That(chessboard.BlackQueenSideCastlingAvailable, Is.True);
        Assert.That(chessboard.EnPassantTargetPosition, Is.EqualTo(new Position(File.C, Rank.Three)));
        Assert.That(chessboard.HalfMoveClock, Is.EqualTo(2));
        Assert.That(chessboard.FullMoveNumber, Is.EqualTo(3));

        Assert.That(chessboard.GetChessPiece(new Position(File.A, Rank.One)), Is.EqualTo(ChessPiece.WhiteKing));
        Assert.That(chessboard.GetChessPiece(new Position(File.A, Rank.Eight)), Is.EqualTo(ChessPiece.WhiteQueen));
        Assert.That(chessboard.GetChessPiece(new Position(File.D, Rank.Four)), Is.EqualTo(ChessPiece.WhitePawn));
        Assert.That(chessboard.GetChessPiece(new Position(File.D, Rank.Five)), Is.EqualTo(ChessPiece.BlackKnight));
        Assert.That(chessboard.GetChessPiece(new Position(File.H, Rank.One)), Is.EqualTo(ChessPiece.BlackQueen));
        Assert.That(chessboard.GetChessPiece(new Position(File.H, Rank.Eight)), Is.EqualTo(ChessPiece.BlackKing));
    }

    // Chess piece placement
    [TestCase(Fen.Empty)]
    [TestCase(Fen.StartPos)]
    [TestCase("r1b2r2/1p1nq1b1/1np1p1kp/p7/3PN3/1P2B3/2Q1BPPP/2RR2K1 w - - 4 23")]
    // Color
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w - - 0 1")]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b - - 0 1")]
    // Castling
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w K - 0 1")]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w Q - 0 1")]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w k - 0 1")]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w q - 0 1")]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w Kq - 0 1")]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w Qk - 0 1")]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w Kk - 0 1")]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w Qq - 0 1")]
    // En passant
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq a1 0 1")]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq d4 0 1")]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq h8 0 1")]
    // Half move clock
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 9 1")]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 37 1")]
    // Full move number
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 9")]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 37")]
    // Complex
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b K c4 12 37")]
    public void FromFen_ToFen_ShouldCreateChessboardFromFen_AndShouldCreateFenRepresentingChessBoard_GivenFenString(string fenString)
    {
        // Arrange
        var fen = Fen.FromString(fenString);

        // Act
        var chessboard = Chessboard.FromFen(fen);
        var fen2 = chessboard.ToFen();

        // Assert
        Assert.That(fen2.ToString(), Is.EqualTo(fenString));
    }
}