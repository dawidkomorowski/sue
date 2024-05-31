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
        Assert.That(fen.EnPassantTargetPosition, Is.Null);
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
        fen.EnPassantTargetPosition = new Position(File.C, Rank.Three);
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
        Assert.That(fen.EnPassantTargetPosition, Is.EqualTo(new Position(File.C, Rank.Three)));
        Assert.That(fen.HalfMoveClock, Is.EqualTo(2));
        Assert.That(fen.FullMoveNumber, Is.EqualTo(3));

        Assert.That(fen.GetChessPiece(new Position(File.A, Rank.One)), Is.EqualTo(ChessPiece.WhiteKing));
        Assert.That(fen.GetChessPiece(new Position(File.A, Rank.Eight)), Is.EqualTo(ChessPiece.WhiteQueen));
        Assert.That(fen.GetChessPiece(new Position(File.D, Rank.Four)), Is.EqualTo(ChessPiece.WhitePawn));
        Assert.That(fen.GetChessPiece(new Position(File.D, Rank.Five)), Is.EqualTo(ChessPiece.BlackKnight));
        Assert.That(fen.GetChessPiece(new Position(File.H, Rank.One)), Is.EqualTo(ChessPiece.BlackQueen));
        Assert.That(fen.GetChessPiece(new Position(File.H, Rank.Eight)), Is.EqualTo(ChessPiece.BlackKing));
    }

    [TestCase("Xnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1",
        "Invalid FEN string 'Xnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1' at index 0.")]
    [TestCase("9nbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1",
        "Invalid FEN string '9nbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1' at index 0.")]
    [TestCase("rnbqkbnrpppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1",
        "Invalid FEN string 'rnbqkbnrpppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1' at index 8.")]
    [TestCase("rnbqkbn5/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1",
        "Invalid FEN string 'rnbqkbn5/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1' at index 7.")]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR x KQkq - 0 1",
        "Invalid FEN string 'rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR x KQkq - 0 1' at index 44.")]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkX - 0 1",
        "Invalid FEN string 'rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkX - 0 1' at index 49.")]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq c 0 1",
        "Invalid FEN string 'rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq c 0 1' at index 52.")]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 5x 1",
        "Invalid FEN string 'rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 5x 1' at index 54.")]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 x",
        "Invalid FEN string 'rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 x' at index 55.")]
    public void FromString_ShouldThrowException_GivenInvalidFenString(string fenString, string exception)
    {
        // Arrange
        // Act
        // Assert
        Assert.That(() => Fen.FromString(fenString), Throws.ArgumentException.With.Message.EqualTo(exception));
    }

    // Start position
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1",
        Color.White, true, true, true, true, false, File.A, Rank.One, 0, 1)]
    // Color
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w - - 0 1",
        Color.White, false, false, false, false, false, File.A, Rank.One, 0, 1)]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b - - 0 1",
        Color.Black, false, false, false, false, false, File.A, Rank.One, 0, 1)]
    // Castling
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w K - 0 1",
        Color.White, true, false, false, false, false, File.A, Rank.One, 0, 1)]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w Q - 0 1",
        Color.White, false, true, false, false, false, File.A, Rank.One, 0, 1)]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w k - 0 1",
        Color.White, false, false, true, false, false, File.A, Rank.One, 0, 1)]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w q - 0 1",
        Color.White, false, false, false, true, false, File.A, Rank.One, 0, 1)]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w Kq - 0 1",
        Color.White, true, false, false, true, false, File.A, Rank.One, 0, 1)]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w Qk - 0 1",
        Color.White, false, true, true, false, false, File.A, Rank.One, 0, 1)]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w Kk - 0 1",
        Color.White, true, false, true, false, false, File.A, Rank.One, 0, 1)]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w Qq - 0 1",
        Color.White, false, true, false, true, false, File.A, Rank.One, 0, 1)]
    // En passant
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq a1 0 1",
        Color.White, true, true, true, true, true, File.A, Rank.One, 0, 1)]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq d4 0 1",
        Color.White, true, true, true, true, true, File.D, Rank.Four, 0, 1)]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq h8 0 1",
        Color.White, true, true, true, true, true, File.H, Rank.Eight, 0, 1)]
    // Half move clock
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 9 1",
        Color.White, true, true, true, true, false, File.A, Rank.One, 9, 1)]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 37 1",
        Color.White, true, true, true, true, false, File.A, Rank.One, 37, 1)]
    // Full move number
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 9",
        Color.White, true, true, true, true, false, File.A, Rank.One, 0, 9)]
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 37",
        Color.White, true, true, true, true, false, File.A, Rank.One, 0, 37)]
    // Complex
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b K c4 12 37",
        Color.Black, true, false, false, false, true, File.C, Rank.Four, 12, 37)]
    public void FromString_ShouldCreateFenWithCorrect_Color_Castling_EnPassant_HalfMove_FullMove_GivenFenString(string fenString,
        Color color, bool wk, bool wq, bool bk, bool bq, bool enPassantPresent, File enPassantFile, Rank enPassantRank, int halfMove, int fullMove)
    {
        // Arrange
        // Act
        var fen = Fen.FromString(fenString);

        // Assert
        Assert.That(fen.ActiveColor, Is.EqualTo(color));
        Assert.That(fen.WhiteKingSideCastlingAvailable, Is.EqualTo(wk));
        Assert.That(fen.WhiteQueenSideCastlingAvailable, Is.EqualTo(wq));
        Assert.That(fen.BlackKingSideCastlingAvailable, Is.EqualTo(bk));
        Assert.That(fen.BlackQueenSideCastlingAvailable, Is.EqualTo(bq));

        if (enPassantPresent)
        {
            Assert.That(fen.EnPassantTargetPosition, Is.EqualTo(new Position(enPassantFile, enPassantRank)));
        }
        else
        {
            Assert.That(fen.EnPassantTargetPosition, Is.Null);
        }

        Assert.That(fen.HalfMoveClock, Is.EqualTo(halfMove));
        Assert.That(fen.FullMoveNumber, Is.EqualTo(fullMove));
    }

    [Test]
    public void FromString_ShouldCreateFen_GivenFenStringWithInitialPosition()
    {
        // Arrange
        // Act
        var fen = Fen.FromString(Fen.StartPos);

        // Assert
        Assert.That(fen.ActiveColor, Is.EqualTo(Color.White));
        Assert.That(fen.WhiteKingSideCastlingAvailable, Is.True);
        Assert.That(fen.WhiteQueenSideCastlingAvailable, Is.True);
        Assert.That(fen.BlackKingSideCastlingAvailable, Is.True);
        Assert.That(fen.BlackQueenSideCastlingAvailable, Is.True);
        Assert.That(fen.EnPassantTargetPosition, Is.Null);
        Assert.That(fen.HalfMoveClock, Is.EqualTo(0));
        Assert.That(fen.FullMoveNumber, Is.EqualTo(1));

        #region Chess Piece Placement

        Assert.That(fen.GetChessPiece(new Position(File.A, Rank.Eight)), Is.EqualTo(ChessPiece.BlackRook));
        Assert.That(fen.GetChessPiece(new Position(File.B, Rank.Eight)), Is.EqualTo(ChessPiece.BlackKnight));
        Assert.That(fen.GetChessPiece(new Position(File.C, Rank.Eight)), Is.EqualTo(ChessPiece.BlackBishop));
        Assert.That(fen.GetChessPiece(new Position(File.D, Rank.Eight)), Is.EqualTo(ChessPiece.BlackQueen));
        Assert.That(fen.GetChessPiece(new Position(File.E, Rank.Eight)), Is.EqualTo(ChessPiece.BlackKing));
        Assert.That(fen.GetChessPiece(new Position(File.F, Rank.Eight)), Is.EqualTo(ChessPiece.BlackBishop));
        Assert.That(fen.GetChessPiece(new Position(File.G, Rank.Eight)), Is.EqualTo(ChessPiece.BlackKnight));
        Assert.That(fen.GetChessPiece(new Position(File.H, Rank.Eight)), Is.EqualTo(ChessPiece.BlackRook));

        foreach (var file in FileExtensions.Files())
        {
            Assert.That(fen.GetChessPiece(new Position(file, Rank.Seven)), Is.EqualTo(ChessPiece.BlackPawn));
        }

        foreach (var file in FileExtensions.Files())
        {
            for (var r = Rank.Three.Index(); r < Rank.Seven.Index(); r++)
            {
                Assert.That(fen.GetChessPiece(new Position(file, r.ToRank())), Is.EqualTo(ChessPiece.None));
            }
        }

        foreach (var file in FileExtensions.Files())
        {
            Assert.That(fen.GetChessPiece(new Position(file, Rank.Two)), Is.EqualTo(ChessPiece.WhitePawn));
        }

        Assert.That(fen.GetChessPiece(new Position(File.A, Rank.One)), Is.EqualTo(ChessPiece.WhiteRook));
        Assert.That(fen.GetChessPiece(new Position(File.B, Rank.One)), Is.EqualTo(ChessPiece.WhiteKnight));
        Assert.That(fen.GetChessPiece(new Position(File.C, Rank.One)), Is.EqualTo(ChessPiece.WhiteBishop));
        Assert.That(fen.GetChessPiece(new Position(File.D, Rank.One)), Is.EqualTo(ChessPiece.WhiteQueen));
        Assert.That(fen.GetChessPiece(new Position(File.E, Rank.One)), Is.EqualTo(ChessPiece.WhiteKing));
        Assert.That(fen.GetChessPiece(new Position(File.F, Rank.One)), Is.EqualTo(ChessPiece.WhiteBishop));
        Assert.That(fen.GetChessPiece(new Position(File.G, Rank.One)), Is.EqualTo(ChessPiece.WhiteKnight));
        Assert.That(fen.GetChessPiece(new Position(File.H, Rank.One)), Is.EqualTo(ChessPiece.WhiteRook));

        #endregion
    }

    [Test]
    public void FromString_ShouldCreateFen_GivenFenStringWithComplexPosition()
    {
        // Arrange
        const string fenString = "r1b2r2/1p1nq1b1/1np1p1kp/p7/3PN3/1P2B3/2Q1BPPP/2RR2K1 w - - 4 23";

        // Act
        var fen = Fen.FromString(fenString);

        // Assert
        Assert.That(fen.ActiveColor, Is.EqualTo(Color.White));
        Assert.That(fen.WhiteKingSideCastlingAvailable, Is.False);
        Assert.That(fen.WhiteQueenSideCastlingAvailable, Is.False);
        Assert.That(fen.BlackKingSideCastlingAvailable, Is.False);
        Assert.That(fen.BlackQueenSideCastlingAvailable, Is.False);
        Assert.That(fen.EnPassantTargetPosition, Is.Null);
        Assert.That(fen.HalfMoveClock, Is.EqualTo(4));
        Assert.That(fen.FullMoveNumber, Is.EqualTo(23));

        #region Chess Pieces Placement

        Assert.That(fen.GetChessPiece(new Position(File.A, Rank.Eight)), Is.EqualTo(ChessPiece.BlackRook));
        Assert.That(fen.GetChessPiece(new Position(File.B, Rank.Eight)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.C, Rank.Eight)), Is.EqualTo(ChessPiece.BlackBishop));
        Assert.That(fen.GetChessPiece(new Position(File.D, Rank.Eight)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.E, Rank.Eight)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.F, Rank.Eight)), Is.EqualTo(ChessPiece.BlackRook));
        Assert.That(fen.GetChessPiece(new Position(File.G, Rank.Eight)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.H, Rank.Eight)), Is.EqualTo(ChessPiece.None));

        Assert.That(fen.GetChessPiece(new Position(File.A, Rank.Seven)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.B, Rank.Seven)), Is.EqualTo(ChessPiece.BlackPawn));
        Assert.That(fen.GetChessPiece(new Position(File.C, Rank.Seven)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.D, Rank.Seven)), Is.EqualTo(ChessPiece.BlackKnight));
        Assert.That(fen.GetChessPiece(new Position(File.E, Rank.Seven)), Is.EqualTo(ChessPiece.BlackQueen));
        Assert.That(fen.GetChessPiece(new Position(File.F, Rank.Seven)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.G, Rank.Seven)), Is.EqualTo(ChessPiece.BlackBishop));
        Assert.That(fen.GetChessPiece(new Position(File.H, Rank.Seven)), Is.EqualTo(ChessPiece.None));

        Assert.That(fen.GetChessPiece(new Position(File.A, Rank.Six)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.B, Rank.Six)), Is.EqualTo(ChessPiece.BlackKnight));
        Assert.That(fen.GetChessPiece(new Position(File.C, Rank.Six)), Is.EqualTo(ChessPiece.BlackPawn));
        Assert.That(fen.GetChessPiece(new Position(File.D, Rank.Six)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.E, Rank.Six)), Is.EqualTo(ChessPiece.BlackPawn));
        Assert.That(fen.GetChessPiece(new Position(File.F, Rank.Six)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.G, Rank.Six)), Is.EqualTo(ChessPiece.BlackKing));
        Assert.That(fen.GetChessPiece(new Position(File.H, Rank.Six)), Is.EqualTo(ChessPiece.BlackPawn));

        Assert.That(fen.GetChessPiece(new Position(File.A, Rank.Five)), Is.EqualTo(ChessPiece.BlackPawn));
        Assert.That(fen.GetChessPiece(new Position(File.B, Rank.Five)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.C, Rank.Five)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.D, Rank.Five)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.E, Rank.Five)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.F, Rank.Five)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.G, Rank.Five)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.H, Rank.Five)), Is.EqualTo(ChessPiece.None));

        Assert.That(fen.GetChessPiece(new Position(File.A, Rank.Four)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.B, Rank.Four)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.C, Rank.Four)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.D, Rank.Four)), Is.EqualTo(ChessPiece.WhitePawn));
        Assert.That(fen.GetChessPiece(new Position(File.E, Rank.Four)), Is.EqualTo(ChessPiece.WhiteKnight));
        Assert.That(fen.GetChessPiece(new Position(File.F, Rank.Four)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.G, Rank.Four)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.H, Rank.Four)), Is.EqualTo(ChessPiece.None));

        Assert.That(fen.GetChessPiece(new Position(File.A, Rank.Three)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.B, Rank.Three)), Is.EqualTo(ChessPiece.WhitePawn));
        Assert.That(fen.GetChessPiece(new Position(File.C, Rank.Three)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.D, Rank.Three)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.E, Rank.Three)), Is.EqualTo(ChessPiece.WhiteBishop));
        Assert.That(fen.GetChessPiece(new Position(File.F, Rank.Three)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.G, Rank.Three)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.H, Rank.Three)), Is.EqualTo(ChessPiece.None));

        Assert.That(fen.GetChessPiece(new Position(File.A, Rank.Two)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.B, Rank.Two)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.C, Rank.Two)), Is.EqualTo(ChessPiece.WhiteQueen));
        Assert.That(fen.GetChessPiece(new Position(File.D, Rank.Two)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.E, Rank.Two)), Is.EqualTo(ChessPiece.WhiteBishop));
        Assert.That(fen.GetChessPiece(new Position(File.F, Rank.Two)), Is.EqualTo(ChessPiece.WhitePawn));
        Assert.That(fen.GetChessPiece(new Position(File.G, Rank.Two)), Is.EqualTo(ChessPiece.WhitePawn));
        Assert.That(fen.GetChessPiece(new Position(File.H, Rank.Two)), Is.EqualTo(ChessPiece.WhitePawn));

        Assert.That(fen.GetChessPiece(new Position(File.A, Rank.One)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.B, Rank.One)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.C, Rank.One)), Is.EqualTo(ChessPiece.WhiteRook));
        Assert.That(fen.GetChessPiece(new Position(File.D, Rank.One)), Is.EqualTo(ChessPiece.WhiteRook));
        Assert.That(fen.GetChessPiece(new Position(File.E, Rank.One)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.F, Rank.One)), Is.EqualTo(ChessPiece.None));
        Assert.That(fen.GetChessPiece(new Position(File.G, Rank.One)), Is.EqualTo(ChessPiece.WhiteKing));
        Assert.That(fen.GetChessPiece(new Position(File.H, Rank.One)), Is.EqualTo(ChessPiece.None));

        #endregion
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
    public void ToString_ShouldReturnFenString_GivenFen(string fenString)
    {
        // Arrange
        var fen = Fen.FromString(fenString);

        // Act
        var actual = fen.ToString();

        // Assert
        Assert.That(actual, Is.EqualTo(fenString));
    }
}