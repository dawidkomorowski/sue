﻿using NUnit.Framework;
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

    #region Castling test cases

    // White rook move makes white king side castling not available
    [TestCase("rnbqkbnr/ppppppp1/8/7p/7P/8/PPPPPPP1/RNBQKBNR w KQkq - 0 2", "h1h3", "rnbqkbnr/ppppppp1/8/7p/7P/7R/PPPPPPP1/RNBQKBN1 b Qkq - 1 2")]
    // White rook move makes white queen side castling not available
    [TestCase("rnbqkbnr/1ppppppp/8/p7/P7/8/1PPPPPPP/RNBQKBNR w KQkq - 0 2", "a1a3", "rnbqkbnr/1ppppppp/8/p7/P7/R7/1PPPPPPP/1NBQKBNR b Kkq - 1 2")]
    // White king move makes white castling not available
    [TestCase("rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 2", "e1e2", "rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPPKPPP/RNBQ1BNR b kq - 1 2")]
    // White king side castling makes white castling not available
    [TestCase("rnbqk2r/pppp1ppp/3bpn2/8/8/3BPN2/PPPP1PPP/RNBQK2R w KQkq - 4 4", "e1g1", "rnbqk2r/pppp1ppp/3bpn2/8/8/3BPN2/PPPP1PPP/RNBQ1RK1 b kq - 5 4")]
    // White queen side castling makes white castling not available
    [TestCase("r3kbnr/pp2pppp/n1ppb3/q7/Q7/N1PPB3/PP2PPPP/R3KBNR w KQkq - 6 6", "e1c1", "r3kbnr/pp2pppp/n1ppb3/q7/Q7/N1PPB3/PP2PPPP/2KR1BNR b kq - 7 6")]
    // Black rook move makes black king side castling not available
    [TestCase("rnbqkbnr/ppppppp1/8/7p/7P/6P1/PPPPPP2/RNBQKBNR b KQkq - 0 2", "h8h6", "rnbqkbn1/ppppppp1/7r/7p/7P/6P1/PPPPPP2/RNBQKBNR w KQq - 1 3")]
    // Black rook move makes black queen side castling not available
    [TestCase("rnbqkbnr/1ppppppp/8/p7/P7/1P6/2PPPPPP/RNBQKBNR b KQkq - 0 2", "a8a6", "1nbqkbnr/1ppppppp/r7/p7/P7/1P6/2PPPPPP/RNBQKBNR w KQk - 1 3")]
    // Black king move makes black castling not available
    [TestCase("rnbqkbnr/pppp1ppp/8/4p3/4P3/3P4/PPP2PPP/RNBQKBNR b KQkq - 0 2", "e8e7", "rnbq1bnr/ppppkppp/8/4p3/4P3/3P4/PPP2PPP/RNBQKBNR w KQ - 1 3")]
    // Black king side castling makes black castling not available
    [TestCase("rnbqk2r/ppppbppp/4pn2/8/8/3PPN2/PPP1BPPP/RNBQK2R b KQkq - 0 4", "e8g8", "rnbq1rk1/ppppbppp/4pn2/8/8/3PPN2/PPP1BPPP/RNBQK2R w KQ - 1 5")]
    // Black queen side castling makes black castling not available
    [TestCase("r3kbnr/pbqppppp/npp5/8/8/NPPP4/PBQ1PPPP/R3KBNR b KQkq - 0 6", "e8c8", "2kr1bnr/pbqppppp/npp5/8/8/NPPP4/PBQ1PPPP/R3KBNR w KQ - 1 7")]

    #endregion

    #region En passant test cases

    // White pawn creates en passant on the left
    [TestCase("rnbqkbnr/ppp1pppp/8/7P/3p4/8/PPPPPPP1/RNBQKBNR w KQkq - 0 3", "e2e4", "rnbqkbnr/ppp1pppp/8/7P/3pP3/8/PPPP1PP1/RNBQKBNR b KQkq e3 0 3")]
    // White pawn creates en passant on the right
    [TestCase("rnbqkbnr/ppp1pppp/8/7P/3p4/8/PPPPPPP1/RNBQKBNR w KQkq - 0 3", "c2c4", "rnbqkbnr/ppp1pppp/8/7P/2Pp4/8/PP1PPPP1/RNBQKBNR b KQkq c3 0 3")]
    // White pawn captures en passant on the right
    [TestCase("rnbqkbnr/pppp1pp1/8/3Pp2p/8/8/PPP1PPPP/RNBQKBNR w KQkq e6 0 3", "d5e6", "rnbqkbnr/pppp1pp1/4P3/7p/8/8/PPP1PPPP/RNBQKBNR b KQkq - 0 3")]
    // White pawn captures en passant on the left
    [TestCase("rnbqkbnr/pp1pppp1/8/2pP3p/8/8/PPP1PPPP/RNBQKBNR w KQkq c6 0 3", "d5c6", "rnbqkbnr/pp1pppp1/2P5/7p/8/8/PPP1PPPP/RNBQKBNR b KQkq - 0 3")]
    // Black pawn creates en passant on the left
    [TestCase("rnbqkbnr/ppppppp1/8/3P3p/8/8/PPP1PPPP/RNBQKBNR b KQkq - 0 2", "e7e5", "rnbqkbnr/pppp1pp1/8/3Pp2p/8/8/PPP1PPPP/RNBQKBNR w KQkq e6 0 3")]
    // Black pawn creates en passant on the right
    [TestCase("rnbqkbnr/ppppppp1/8/3P3p/8/8/PPP1PPPP/RNBQKBNR b KQkq - 0 2", "c7c5", "rnbqkbnr/pp1pppp1/8/2pP3p/8/8/PPP1PPPP/RNBQKBNR w KQkq c6 0 3")]
    // Black pawn captures en passant on the right
    [TestCase("rnbqkbnr/ppp1pppp/8/7P/3pP3/8/PPPP1PP1/RNBQKBNR b KQkq e3 0 3", "d4e3", "rnbqkbnr/ppp1pppp/8/7P/8/4p3/PPPP1PP1/RNBQKBNR w KQkq - 0 4")]
    // Black pawn captures en passant on the left
    [TestCase("rnbqkbnr/ppp1pppp/8/7P/2Pp4/8/PP1PPPP1/RNBQKBNR b KQkq c3 0 3", "d4c3", "rnbqkbnr/ppp1pppp/8/7P/8/2p5/PP1PPPP1/RNBQKBNR w KQkq - 0 4")]

    #endregion

    #region Move pattern looks like castling, but it is not test cases

    // Move pattern looks like castling, but it is not
    [TestCase("rnbqr3/ppppk2p/3b3n/4ppp1/4PPP1/3B3N/PPPPK2P/RNBQR3 w - - 8 8", "e1g1", "rnbqr3/ppppk2p/3b3n/4ppp1/4PPP1/3B3N/PPPPK2P/RNBQ2R1 b - - 9 8")]
    [TestCase("4rbnr/p2kpppp/n2qb3/1ppp4/1PPP4/N2QB3/P2KPPPP/4RBNR w - - 10 9", "e1c1", "4rbnr/p2kpppp/n2qb3/1ppp4/1PPP4/N2QB3/P2KPPPP/2R2BNR b - - 11 9")]
    [TestCase("rnbqr3/pppppk2/5n1b/5ppp/PPPPPPPP/8/8/RNBQKBNR b KQ - 0 8", "e8g8", "rnbq2r1/pppppk2/5n1b/5ppp/PPPPPPPP/8/8/RNBQKBNR w KQ - 1 9")]
    [TestCase("4rbnr/p2kpppp/n2qb3/1ppp4/PPPPPPPP/8/4K3/RNBQ1BNR b - - 2 9", "e8c8", "2r2bnr/p2kpppp/n2qb3/1ppp4/PPPPPPPP/8/4K3/RNBQ1BNR w - - 3 10")]

    #endregion

    #region Promotion test cases

    // White pawn promotion without capture
    [TestCase("rnbqkbn1/pppppppP/8/8/r6P/8/PPPPPP2/RNBQKBNR w KQq - 1 6", "h7h8q", "rnbqkbnQ/ppppppp1/8/8/r6P/8/PPPPPP2/RNBQKBNR b KQq - 0 6")]
    [TestCase("rnbqkbn1/pppppppP/8/8/r6P/8/PPPPPP2/RNBQKBNR w KQq - 1 6", "h7h8r", "rnbqkbnR/ppppppp1/8/8/r6P/8/PPPPPP2/RNBQKBNR b KQq - 0 6")]
    [TestCase("rnbqkbn1/pppppppP/8/8/r6P/8/PPPPPP2/RNBQKBNR w KQq - 1 6", "h7h8b", "rnbqkbnB/ppppppp1/8/8/r6P/8/PPPPPP2/RNBQKBNR b KQq - 0 6")]
    [TestCase("rnbqkbn1/pppppppP/8/8/r6P/8/PPPPPP2/RNBQKBNR w KQq - 1 6", "h7h8n", "rnbqkbnN/ppppppp1/8/8/r6P/8/PPPPPP2/RNBQKBNR b KQq - 0 6")]
    // White pawn promotion with capture
    [TestCase("rnbqkbn1/pppppppP/8/8/r6P/8/PPPPPP2/RNBQKBNR w KQq - 1 6", "h7g8q", "rnbqkbQ1/ppppppp1/8/8/r6P/8/PPPPPP2/RNBQKBNR b KQq - 0 6")]
    [TestCase("rnbqkbn1/pppppppP/8/8/r6P/8/PPPPPP2/RNBQKBNR w KQq - 1 6", "h7g8r", "rnbqkbR1/ppppppp1/8/8/r6P/8/PPPPPP2/RNBQKBNR b KQq - 0 6")]
    [TestCase("rnbqkbn1/pppppppP/8/8/r6P/8/PPPPPP2/RNBQKBNR w KQq - 1 6", "h7g8b", "rnbqkbB1/ppppppp1/8/8/r6P/8/PPPPPP2/RNBQKBNR b KQq - 0 6")]
    [TestCase("rnbqkbn1/pppppppP/8/8/r6P/8/PPPPPP2/RNBQKBNR w KQq - 1 6", "h7g8n", "rnbqkbN1/ppppppp1/8/8/r6P/8/PPPPPP2/RNBQKBNR b KQq - 0 6")]
    // Black pawn promotion without capture
    [TestCase("rnbqkbnr/p1pppppp/8/7R/8/8/pPPPPPPP/1NBQKBNR b Kkq - 1 5", "a2a1q", "rnbqkbnr/p1pppppp/8/7R/8/8/1PPPPPPP/qNBQKBNR w Kkq - 0 6")]
    [TestCase("rnbqkbnr/p1pppppp/8/7R/8/8/pPPPPPPP/1NBQKBNR b Kkq - 1 5", "a2a1r", "rnbqkbnr/p1pppppp/8/7R/8/8/1PPPPPPP/rNBQKBNR w Kkq - 0 6")]
    [TestCase("rnbqkbnr/p1pppppp/8/7R/8/8/pPPPPPPP/1NBQKBNR b Kkq - 1 5", "a2a1b", "rnbqkbnr/p1pppppp/8/7R/8/8/1PPPPPPP/bNBQKBNR w Kkq - 0 6")]
    [TestCase("rnbqkbnr/p1pppppp/8/7R/8/8/pPPPPPPP/1NBQKBNR b Kkq - 1 5", "a2a1n", "rnbqkbnr/p1pppppp/8/7R/8/8/1PPPPPPP/nNBQKBNR w Kkq - 0 6")]
    // Black pawn promotion with capture
    [TestCase("rnbqkbnr/p1pppppp/8/7R/8/8/pPPPPPPP/1NBQKBNR b Kkq - 1 5", "a2b1q", "rnbqkbnr/p1pppppp/8/7R/8/8/1PPPPPPP/1qBQKBNR w Kkq - 0 6")]
    [TestCase("rnbqkbnr/p1pppppp/8/7R/8/8/pPPPPPPP/1NBQKBNR b Kkq - 1 5", "a2b1r", "rnbqkbnr/p1pppppp/8/7R/8/8/1PPPPPPP/1rBQKBNR w Kkq - 0 6")]
    [TestCase("rnbqkbnr/p1pppppp/8/7R/8/8/pPPPPPPP/1NBQKBNR b Kkq - 1 5", "a2b1b", "rnbqkbnr/p1pppppp/8/7R/8/8/1PPPPPPP/1bBQKBNR w Kkq - 0 6")]
    [TestCase("rnbqkbnr/p1pppppp/8/7R/8/8/pPPPPPPP/1NBQKBNR b Kkq - 1 5", "a2b1n", "rnbqkbnr/p1pppppp/8/7R/8/8/1PPPPPPP/1nBQKBNR w Kkq - 0 6")]

    #endregion

    #region Half move clock and full move number test cases

    // Half move clock is incremented after white knight move
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", "b1c3", "rnbqkbnr/pppppppp/8/8/8/2N5/PPPPPPPP/R1BQKBNR b KQkq - 1 1")]
    // Half move clock is incremented after black knight move
    [TestCase("rnbqkbnr/pppppppp/8/8/8/2N5/PPPPPPPP/R1BQKBNR b KQkq - 1 1", "b8c6", "r1bqkbnr/pppppppp/2n5/8/8/2N5/PPPPPPPP/R1BQKBNR w KQkq - 2 2")]
    // Half move clock is reset after white pawn move
    [TestCase("r1bqkbnr/pppppppp/2n5/8/8/2N5/PPPPPPPP/R1BQKBNR w KQkq - 2 2", "e2e4", "r1bqkbnr/pppppppp/2n5/8/4P3/2N5/PPPP1PPP/R1BQKBNR b KQkq - 0 2")]
    // Half move clock is reset after black pawn move
    [TestCase("r1bqkbnr/pppppppp/2n5/8/8/2N2N2/PPPPPPPP/R1BQKB1R b KQkq - 3 2", "e7e6", "r1bqkbnr/pppp1ppp/2n1p3/8/8/2N2N2/PPPPPPPP/R1BQKB1R w KQkq - 0 3")]
    // Half move clock is reset after white capture
    [TestCase("r1bqkb1r/pppppppp/5n2/3Nn3/8/5N2/PPPPPPPP/R1BQKB1R w KQkq - 6 4", "f3e5", "r1bqkb1r/pppppppp/5n2/3NN3/8/8/PPPPPPPP/R1BQKB1R b KQkq - 0 4")]
    // Half move clock is reset after black capture
    [TestCase("r1bqkb1r/pppppppp/2n2n2/3N4/8/5N2/PPPPPPPP/R1BQKB1R b KQkq - 5 3", "f6d5", "r1bqkb1r/pppppppp/2n5/3n4/8/5N2/PPPPPPPP/R1BQKB1R w KQkq - 0 4")]
    // Full move number is not incremented after white move
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", "e2e4", "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq - 0 1")]
    // Full move number is incremented after black move
    [TestCase("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq - 0 1", "e7e5", "rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 2")]

    #endregion

    #region Chess game test cases

    // Chess game: https://lichess.org/wHn35ZRJ
    [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", "e2e4", "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq - 0 1")]
    [TestCase("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq - 0 1", "e7e6", "rnbqkbnr/pppp1ppp/4p3/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 2")]
    [TestCase("rnbqkbnr/pppp1ppp/4p3/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 2", "g1f3", "rnbqkbnr/pppp1ppp/4p3/8/4P3/5N2/PPPP1PPP/RNBQKB1R b KQkq - 1 2")]
    [TestCase("rnbqkbnr/pppp1ppp/4p3/8/4P3/5N2/PPPP1PPP/RNBQKB1R b KQkq - 1 2", "d7d5", "rnbqkbnr/ppp2ppp/4p3/3p4/4P3/5N2/PPPP1PPP/RNBQKB1R w KQkq - 0 3")]
    [TestCase("rnbqkbnr/ppp2ppp/4p3/3p4/4P3/5N2/PPPP1PPP/RNBQKB1R w KQkq - 0 3", "e4d5", "rnbqkbnr/ppp2ppp/4p3/3P4/8/5N2/PPPP1PPP/RNBQKB1R b KQkq - 0 3")]
    [TestCase("rnbqkbnr/ppp2ppp/4p3/3P4/8/5N2/PPPP1PPP/RNBQKB1R b KQkq - 0 3", "e6d5", "rnbqkbnr/ppp2ppp/8/3p4/8/5N2/PPPP1PPP/RNBQKB1R w KQkq - 0 4")]
    [TestCase("rnbqkbnr/ppp2ppp/8/3p4/8/5N2/PPPP1PPP/RNBQKB1R w KQkq - 0 4", "d2d4", "rnbqkbnr/ppp2ppp/8/3p4/3P4/5N2/PPP2PPP/RNBQKB1R b KQkq - 0 4")]
    [TestCase("rnbqkbnr/ppp2ppp/8/3p4/3P4/5N2/PPP2PPP/RNBQKB1R b KQkq - 0 4", "b8c6", "r1bqkbnr/ppp2ppp/2n5/3p4/3P4/5N2/PPP2PPP/RNBQKB1R w KQkq - 1 5")]
    [TestCase("r1bqkbnr/ppp2ppp/2n5/3p4/3P4/5N2/PPP2PPP/RNBQKB1R w KQkq - 1 5", "f1d3", "r1bqkbnr/ppp2ppp/2n5/3p4/3P4/3B1N2/PPP2PPP/RNBQK2R b KQkq - 2 5")]
    [TestCase("r1bqkbnr/ppp2ppp/2n5/3p4/3P4/3B1N2/PPP2PPP/RNBQK2R b KQkq - 2 5", "c8g4", "r2qkbnr/ppp2ppp/2n5/3p4/3P2b1/3B1N2/PPP2PPP/RNBQK2R w KQkq - 3 6")]
    [TestCase("r2qkbnr/ppp2ppp/2n5/3p4/3P2b1/3B1N2/PPP2PPP/RNBQK2R w KQkq - 3 6", "e1g1", "r2qkbnr/ppp2ppp/2n5/3p4/3P2b1/3B1N2/PPP2PPP/RNBQ1RK1 b kq - 4 6")]
    [TestCase("r2qkbnr/ppp2ppp/2n5/3p4/3P2b1/3B1N2/PPP2PPP/RNBQ1RK1 b kq - 4 6", "g4f3", "r2qkbnr/ppp2ppp/2n5/3p4/3P4/3B1b2/PPP2PPP/RNBQ1RK1 w kq - 0 7")]
    [TestCase("r2qkbnr/ppp2ppp/2n5/3p4/3P4/3B1b2/PPP2PPP/RNBQ1RK1 w kq - 0 7", "d1f3", "r2qkbnr/ppp2ppp/2n5/3p4/3P4/3B1Q2/PPP2PPP/RNB2RK1 b kq - 0 7")]
    [TestCase("r2qkbnr/ppp2ppp/2n5/3p4/3P4/3B1Q2/PPP2PPP/RNB2RK1 b kq - 0 7", "c6d4", "r2qkbnr/ppp2ppp/8/3p4/3n4/3B1Q2/PPP2PPP/RNB2RK1 w kq - 0 8")]
    [TestCase("r2qkbnr/ppp2ppp/8/3p4/3n4/3B1Q2/PPP2PPP/RNB2RK1 w kq - 0 8", "f3e3", "r2qkbnr/ppp2ppp/8/3p4/3n4/3BQ3/PPP2PPP/RNB2RK1 b kq - 1 8")]
    [TestCase("r2qkbnr/ppp2ppp/8/3p4/3n4/3BQ3/PPP2PPP/RNB2RK1 b kq - 1 8", "d4e6", "r2qkbnr/ppp2ppp/4n3/3p4/8/3BQ3/PPP2PPP/RNB2RK1 w kq - 2 9")]
    [TestCase("r2qkbnr/ppp2ppp/4n3/3p4/8/3BQ3/PPP2PPP/RNB2RK1 w kq - 2 9", "f1e1", "r2qkbnr/ppp2ppp/4n3/3p4/8/3BQ3/PPP2PPP/RNB1R1K1 b kq - 3 9")]
    [TestCase("r2qkbnr/ppp2ppp/4n3/3p4/8/3BQ3/PPP2PPP/RNB1R1K1 b kq - 3 9", "f8e7", "r2qk1nr/ppp1bppp/4n3/3p4/8/3BQ3/PPP2PPP/RNB1R1K1 w kq - 4 10")]
    [TestCase("r2qk1nr/ppp1bppp/4n3/3p4/8/3BQ3/PPP2PPP/RNB1R1K1 w kq - 4 10", "f2f4", "r2qk1nr/ppp1bppp/4n3/3p4/5P2/3BQ3/PPP3PP/RNB1R1K1 b kq - 0 10")]
    [TestCase("r2qk1nr/ppp1bppp/4n3/3p4/5P2/3BQ3/PPP3PP/RNB1R1K1 b kq - 0 10", "g8f6", "r2qk2r/ppp1bppp/4nn2/3p4/5P2/3BQ3/PPP3PP/RNB1R1K1 w kq - 1 11")]
    [TestCase("r2qk2r/ppp1bppp/4nn2/3p4/5P2/3BQ3/PPP3PP/RNB1R1K1 w kq - 1 11", "g1h1", "r2qk2r/ppp1bppp/4nn2/3p4/5P2/3BQ3/PPP3PP/RNB1R2K b kq - 2 11")]
    [TestCase("r2qk2r/ppp1bppp/4nn2/3p4/5P2/3BQ3/PPP3PP/RNB1R2K b kq - 2 11", "d5d4", "r2qk2r/ppp1bppp/4nn2/8/3p1P2/3BQ3/PPP3PP/RNB1R2K w kq - 0 12")]
    [TestCase("r2qk2r/ppp1bppp/4nn2/8/3p1P2/3BQ3/PPP3PP/RNB1R2K w kq - 0 12", "e3e2", "r2qk2r/ppp1bppp/4nn2/8/3p1P2/3B4/PPP1Q1PP/RNB1R2K b kq - 1 12")]
    [TestCase("r2qk2r/ppp1bppp/4nn2/8/3p1P2/3B4/PPP1Q1PP/RNB1R2K b kq - 1 12", "e8f8", "r2q1k1r/ppp1bppp/4nn2/8/3p1P2/3B4/PPP1Q1PP/RNB1R2K w - - 2 13")]
    [TestCase("r2q1k1r/ppp1bppp/4nn2/8/3p1P2/3B4/PPP1Q1PP/RNB1R2K w - - 2 13", "f4f5", "r2q1k1r/ppp1bppp/4nn2/5P2/3p4/3B4/PPP1Q1PP/RNB1R2K b - - 0 13")]
    [TestCase("r2q1k1r/ppp1bppp/4nn2/5P2/3p4/3B4/PPP1Q1PP/RNB1R2K b - - 0 13", "e6c5", "r2q1k1r/ppp1bppp/5n2/2n2P2/3p4/3B4/PPP1Q1PP/RNB1R2K w - - 1 14")]
    [TestCase("r2q1k1r/ppp1bppp/5n2/2n2P2/3p4/3B4/PPP1Q1PP/RNB1R2K w - - 1 14", "c1g5", "r2q1k1r/ppp1bppp/5n2/2n2PB1/3p4/3B4/PPP1Q1PP/RN2R2K b - - 2 14")]
    [TestCase("r2q1k1r/ppp1bppp/5n2/2n2PB1/3p4/3B4/PPP1Q1PP/RN2R2K b - - 2 14", "c5d3", "r2q1k1r/ppp1bppp/5n2/5PB1/3p4/3n4/PPP1Q1PP/RN2R2K w - - 0 15")]
    [TestCase("r2q1k1r/ppp1bppp/5n2/5PB1/3p4/3n4/PPP1Q1PP/RN2R2K w - - 0 15", "e2d3", "r2q1k1r/ppp1bppp/5n2/5PB1/3p4/3Q4/PPP3PP/RN2R2K b - - 0 15")]
    [TestCase("r2q1k1r/ppp1bppp/5n2/5PB1/3p4/3Q4/PPP3PP/RN2R2K b - - 0 15", "d8d7", "r4k1r/pppqbppp/5n2/5PB1/3p4/3Q4/PPP3PP/RN2R2K w - - 1 16")]
    [TestCase("r4k1r/pppqbppp/5n2/5PB1/3p4/3Q4/PPP3PP/RN2R2K w - - 1 16", "b1d2", "r4k1r/pppqbppp/5n2/5PB1/3p4/3Q4/PPPN2PP/R3R2K b - - 2 16")]
    [TestCase("r4k1r/pppqbppp/5n2/5PB1/3p4/3Q4/PPPN2PP/R3R2K b - - 2 16", "h7h6", "r4k1r/pppqbpp1/5n1p/5PB1/3p4/3Q4/PPPN2PP/R3R2K w - - 0 17")]
    [TestCase("r4k1r/pppqbpp1/5n1p/5PB1/3p4/3Q4/PPPN2PP/R3R2K w - - 0 17", "g5f4", "r4k1r/pppqbpp1/5n1p/5P2/3p1B2/3Q4/PPPN2PP/R3R2K b - - 1 17")]
    [TestCase("r4k1r/pppqbpp1/5n1p/5P2/3p1B2/3Q4/PPPN2PP/R3R2K b - - 1 17", "c7c5", "r4k1r/pp1qbpp1/5n1p/2p2P2/3p1B2/3Q4/PPPN2PP/R3R2K w - - 0 18")]
    [TestCase("r4k1r/pp1qbpp1/5n1p/2p2P2/3p1B2/3Q4/PPPN2PP/R3R2K w - - 0 18", "d2c4", "r4k1r/pp1qbpp1/5n1p/2p2P2/2Np1B2/3Q4/PPP3PP/R3R2K b - - 1 18")]
    [TestCase("r4k1r/pp1qbpp1/5n1p/2p2P2/2Np1B2/3Q4/PPP3PP/R3R2K b - - 1 18", "f8g8", "r5kr/pp1qbpp1/5n1p/2p2P2/2Np1B2/3Q4/PPP3PP/R3R2K w - - 2 19")]
    [TestCase("r5kr/pp1qbpp1/5n1p/2p2P2/2Np1B2/3Q4/PPP3PP/R3R2K w - - 2 19", "c4e5", "r5kr/pp1qbpp1/5n1p/2p1NP2/3p1B2/3Q4/PPP3PP/R3R2K b - - 3 19")]
    [TestCase("r5kr/pp1qbpp1/5n1p/2p1NP2/3p1B2/3Q4/PPP3PP/R3R2K b - - 3 19", "d7d8", "r2q2kr/pp2bpp1/5n1p/2p1NP2/3p1B2/3Q4/PPP3PP/R3R2K w - - 4 20")]
    [TestCase("r2q2kr/pp2bpp1/5n1p/2p1NP2/3p1B2/3Q4/PPP3PP/R3R2K w - - 4 20", "d3c4", "r2q2kr/pp2bpp1/5n1p/2p1NP2/2Qp1B2/8/PPP3PP/R3R2K b - - 5 20")]
    [TestCase("r2q2kr/pp2bpp1/5n1p/2p1NP2/2Qp1B2/8/PPP3PP/R3R2K b - - 5 20", "d8d5", "r5kr/pp2bpp1/5n1p/2pqNP2/2Qp1B2/8/PPP3PP/R3R2K w - - 6 21")]
    [TestCase("r5kr/pp2bpp1/5n1p/2pqNP2/2Qp1B2/8/PPP3PP/R3R2K w - - 6 21", "c4b5", "r5kr/pp2bpp1/5n1p/1QpqNP2/3p1B2/8/PPP3PP/R3R2K b - - 7 21")]
    [TestCase("r5kr/pp2bpp1/5n1p/1QpqNP2/3p1B2/8/PPP3PP/R3R2K b - - 7 21", "e7d6", "r5kr/pp3pp1/3b1n1p/1QpqNP2/3p1B2/8/PPP3PP/R3R2K w - - 8 22")]
    [TestCase("r5kr/pp3pp1/3b1n1p/1QpqNP2/3p1B2/8/PPP3PP/R3R2K w - - 8 22", "c2c4", "r5kr/pp3pp1/3b1n1p/1QpqNP2/2Pp1B2/8/PP4PP/R3R2K b - c3 0 22")]
    [TestCase("r5kr/pp3pp1/3b1n1p/1QpqNP2/2Pp1B2/8/PP4PP/R3R2K b - c3 0 22", "d6e5", "r5kr/pp3pp1/5n1p/1QpqbP2/2Pp1B2/8/PP4PP/R3R2K w - - 0 23")]
    [TestCase("r5kr/pp3pp1/5n1p/1QpqbP2/2Pp1B2/8/PP4PP/R3R2K w - - 0 23", "c4d5", "r5kr/pp3pp1/5n1p/1QpPbP2/3p1B2/8/PP4PP/R3R2K b - - 0 23")]
    [TestCase("r5kr/pp3pp1/5n1p/1QpPbP2/3p1B2/8/PP4PP/R3R2K b - - 0 23", "e5f4", "r5kr/pp3pp1/5n1p/1QpP1P2/3p1b2/8/PP4PP/R3R2K w - - 0 24")]
    [TestCase("r5kr/pp3pp1/5n1p/1QpP1P2/3p1b2/8/PP4PP/R3R2K w - - 0 24", "b5c5", "r5kr/pp3pp1/5n1p/2QP1P2/3p1b2/8/PP4PP/R3R2K b - - 0 24")]
    [TestCase("r5kr/pp3pp1/5n1p/2QP1P2/3p1b2/8/PP4PP/R3R2K b - - 0 24", "f4e3", "r5kr/pp3pp1/5n1p/2QP1P2/3p4/4b3/PP4PP/R3R2K w - - 1 25")]
    [TestCase("r5kr/pp3pp1/5n1p/2QP1P2/3p4/4b3/PP4PP/R3R2K w - - 1 25", "e1e3", "r5kr/pp3pp1/5n1p/2QP1P2/3p4/4R3/PP4PP/R6K b - - 0 25")]
    [TestCase("r5kr/pp3pp1/5n1p/2QP1P2/3p4/4R3/PP4PP/R6K b - - 0 25", "d4e3", "r5kr/pp3pp1/5n1p/2QP1P2/8/4p3/PP4PP/R6K w - - 0 26")]
    [TestCase("r5kr/pp3pp1/5n1p/2QP1P2/8/4p3/PP4PP/R6K w - - 0 26", "c5e3", "r5kr/pp3pp1/5n1p/3P1P2/8/4Q3/PP4PP/R6K b - - 0 26")]
    [TestCase("r5kr/pp3pp1/5n1p/3P1P2/8/4Q3/PP4PP/R6K b - - 0 26", "f6d5", "r5kr/pp3pp1/7p/3n1P2/8/4Q3/PP4PP/R6K w - - 0 27")]
    [TestCase("r5kr/pp3pp1/7p/3n1P2/8/4Q3/PP4PP/R6K w - - 0 27", "e3b3", "r5kr/pp3pp1/7p/3n1P2/8/1Q6/PP4PP/R6K b - - 1 27")]
    [TestCase("r5kr/pp3pp1/7p/3n1P2/8/1Q6/PP4PP/R6K b - - 1 27", "d5b6", "r5kr/pp3pp1/1n5p/5P2/8/1Q6/PP4PP/R6K w - - 2 28")]
    [TestCase("r5kr/pp3pp1/1n5p/5P2/8/1Q6/PP4PP/R6K w - - 2 28", "a1f1", "r5kr/pp3pp1/1n5p/5P2/8/1Q6/PP4PP/5R1K b - - 3 28")]
    [TestCase("r5kr/pp3pp1/1n5p/5P2/8/1Q6/PP4PP/5R1K b - - 3 28", "a8d8", "3r2kr/pp3pp1/1n5p/5P2/8/1Q6/PP4PP/5R1K w - - 4 29")]
    [TestCase("3r2kr/pp3pp1/1n5p/5P2/8/1Q6/PP4PP/5R1K w - - 4 29", "f5f6", "3r2kr/pp3pp1/1n3P1p/8/8/1Q6/PP4PP/5R1K b - - 0 29")]
    [TestCase("3r2kr/pp3pp1/1n3P1p/8/8/1Q6/PP4PP/5R1K b - - 0 29", "g7g6", "3r2kr/pp3p2/1n3Ppp/8/8/1Q6/PP4PP/5R1K w - - 0 30")]
    [TestCase("3r2kr/pp3p2/1n3Ppp/8/8/1Q6/PP4PP/5R1K w - - 0 30", "f1d1", "3r2kr/pp3p2/1n3Ppp/8/8/1Q6/PP4PP/3R3K b - - 1 30")]
    [TestCase("3r2kr/pp3p2/1n3Ppp/8/8/1Q6/PP4PP/3R3K b - - 1 30", "d8d1", "6kr/pp3p2/1n3Ppp/8/8/1Q6/PP4PP/3r3K w - - 0 31")]
    [TestCase("6kr/pp3p2/1n3Ppp/8/8/1Q6/PP4PP/3r3K w - - 0 31", "b3d1", "6kr/pp3p2/1n3Ppp/8/8/8/PP4PP/3Q3K b - - 0 31")]
    [TestCase("6kr/pp3p2/1n3Ppp/8/8/8/PP4PP/3Q3K b - - 0 31", "g8h7", "7r/pp3p1k/1n3Ppp/8/8/8/PP4PP/3Q3K w - - 1 32")]
    [TestCase("7r/pp3p1k/1n3Ppp/8/8/8/PP4PP/3Q3K w - - 1 32", "d1b3", "7r/pp3p1k/1n3Ppp/8/8/1Q6/PP4PP/7K b - - 2 32")]
    [TestCase("7r/pp3p1k/1n3Ppp/8/8/1Q6/PP4PP/7K b - - 2 32", "h8f8", "5r2/pp3p1k/1n3Ppp/8/8/1Q6/PP4PP/7K w - - 3 33")]
    [TestCase("5r2/pp3p1k/1n3Ppp/8/8/1Q6/PP4PP/7K w - - 3 33", "b3b4", "5r2/pp3p1k/1n3Ppp/8/1Q6/8/PP4PP/7K b - - 4 33")]
    [TestCase("5r2/pp3p1k/1n3Ppp/8/1Q6/8/PP4PP/7K b - - 4 33", "h7g8", "5rk1/pp3p2/1n3Ppp/8/1Q6/8/PP4PP/7K w - - 5 34")]
    [TestCase("5rk1/pp3p2/1n3Ppp/8/1Q6/8/PP4PP/7K w - - 5 34", "h2h4", "5rk1/pp3p2/1n3Ppp/8/1Q5P/8/PP4P1/7K b - - 0 34")]
    [TestCase("5rk1/pp3p2/1n3Ppp/8/1Q5P/8/PP4P1/7K b - - 0 34", "f8c8", "2r3k1/pp3p2/1n3Ppp/8/1Q5P/8/PP4P1/7K w - - 1 35")]
    [TestCase("2r3k1/pp3p2/1n3Ppp/8/1Q5P/8/PP4P1/7K w - - 1 35", "b4e7", "2r3k1/pp2Qp2/1n3Ppp/8/7P/8/PP4P1/7K b - - 2 35")]
    [TestCase("2r3k1/pp2Qp2/1n3Ppp/8/7P/8/PP4P1/7K b - - 2 35", "b6d5", "2r3k1/pp2Qp2/5Ppp/3n4/7P/8/PP4P1/7K w - - 3 36")]
    [TestCase("2r3k1/pp2Qp2/5Ppp/3n4/7P/8/PP4P1/7K w - - 3 36", "e7b7", "2r3k1/pQ3p2/5Ppp/3n4/7P/8/PP4P1/7K b - - 0 36")]
    [TestCase("2r3k1/pQ3p2/5Ppp/3n4/7P/8/PP4P1/7K b - - 0 36", "c8c1", "6k1/pQ3p2/5Ppp/3n4/7P/8/PP4P1/2r4K w - - 1 37")]
    [TestCase("6k1/pQ3p2/5Ppp/3n4/7P/8/PP4P1/2r4K w - - 1 37", "h1h2", "6k1/pQ3p2/5Ppp/3n4/7P/8/PP4PK/2r5 b - - 2 37")]
    [TestCase("6k1/pQ3p2/5Ppp/3n4/7P/8/PP4PK/2r5 b - - 2 37", "d5f6", "6k1/pQ3p2/5npp/8/7P/8/PP4PK/2r5 w - - 0 38")]
    [TestCase("6k1/pQ3p2/5npp/8/7P/8/PP4PK/2r5 w - - 0 38", "b7a7", "6k1/Q4p2/5npp/8/7P/8/PP4PK/2r5 b - - 0 38")]
    [TestCase("6k1/Q4p2/5npp/8/7P/8/PP4PK/2r5 b - - 0 38", "g8g7", "8/Q4pk1/5npp/8/7P/8/PP4PK/2r5 w - - 1 39")]
    [TestCase("8/Q4pk1/5npp/8/7P/8/PP4PK/2r5 w - - 1 39", "a7d4", "8/5pk1/5npp/8/3Q3P/8/PP4PK/2r5 b - - 2 39")]
    [TestCase("8/5pk1/5npp/8/3Q3P/8/PP4PK/2r5 b - - 2 39", "g6g5", "8/5pk1/5n1p/6p1/3Q3P/8/PP4PK/2r5 w - - 0 40")]
    [TestCase("8/5pk1/5n1p/6p1/3Q3P/8/PP4PK/2r5 w - - 0 40", "h4g5", "8/5pk1/5n1p/6P1/3Q4/8/PP4PK/2r5 b - - 0 40")]
    [TestCase("8/5pk1/5n1p/6P1/3Q4/8/PP4PK/2r5 b - - 0 40", "h6g5", "8/5pk1/5n2/6p1/3Q4/8/PP4PK/2r5 w - - 0 41")]
    [TestCase("8/5pk1/5n2/6p1/3Q4/8/PP4PK/2r5 w - - 0 41", "a2a4", "8/5pk1/5n2/6p1/P2Q4/8/1P4PK/2r5 b - - 0 41")]
    [TestCase("8/5pk1/5n2/6p1/P2Q4/8/1P4PK/2r5 b - - 0 41", "g7g6", "8/5p2/5nk1/6p1/P2Q4/8/1P4PK/2r5 w - - 1 42")]
    [TestCase("8/5p2/5nk1/6p1/P2Q4/8/1P4PK/2r5 w - - 1 42", "a4a5", "8/5p2/5nk1/P5p1/3Q4/8/1P4PK/2r5 b - - 0 42")]
    [TestCase("8/5p2/5nk1/P5p1/3Q4/8/1P4PK/2r5 b - - 0 42", "g5g4", "8/5p2/5nk1/P7/3Q2p1/8/1P4PK/2r5 w - - 0 43")]
    [TestCase("8/5p2/5nk1/P7/3Q2p1/8/1P4PK/2r5 w - - 0 43", "a5a6", "8/5p2/P4nk1/8/3Q2p1/8/1P4PK/2r5 b - - 0 43")]
    [TestCase("8/5p2/P4nk1/8/3Q2p1/8/1P4PK/2r5 b - - 0 43", "f6h5", "8/5p2/P5k1/7n/3Q2p1/8/1P4PK/2r5 w - - 1 44")]
    [TestCase("8/5p2/P5k1/7n/3Q2p1/8/1P4PK/2r5 w - - 1 44", "d4g4", "8/5p2/P5k1/7n/6Q1/8/1P4PK/2r5 b - - 0 44")]
    [TestCase("8/5p2/P5k1/7n/6Q1/8/1P4PK/2r5 b - - 0 44", "g6h6", "8/5p2/P6k/7n/6Q1/8/1P4PK/2r5 w - - 1 45")]
    [TestCase("8/5p2/P6k/7n/6Q1/8/1P4PK/2r5 w - - 1 45", "g4f3", "8/5p2/P6k/7n/8/5Q2/1P4PK/2r5 b - - 2 45")]
    [TestCase("8/5p2/P6k/7n/8/5Q2/1P4PK/2r5 b - - 2 45", "c1a1", "8/5p2/P6k/7n/8/5Q2/1P4PK/r7 w - - 3 46")]
    [TestCase("8/5p2/P6k/7n/8/5Q2/1P4PK/r7 w - - 3 46", "f3e3", "8/5p2/P6k/7n/8/4Q3/1P4PK/r7 b - - 4 46")]
    [TestCase("8/5p2/P6k/7n/8/4Q3/1P4PK/r7 b - - 4 46", "h6g6", "8/5p2/P5k1/7n/8/4Q3/1P4PK/r7 w - - 5 47")]
    [TestCase("8/5p2/P5k1/7n/8/4Q3/1P4PK/r7 w - - 5 47", "a6a7", "8/P4p2/6k1/7n/8/4Q3/1P4PK/r7 b - - 0 47")]
    [TestCase("8/P4p2/6k1/7n/8/4Q3/1P4PK/r7 b - - 0 47", "a1a6", "8/P4p2/r5k1/7n/8/4Q3/1P4PK/8 w - - 1 48")]
    [TestCase("8/P4p2/r5k1/7n/8/4Q3/1P4PK/8 w - - 1 48", "e3d3", "8/P4p2/r5k1/7n/8/3Q4/1P4PK/8 b - - 2 48")]

    #endregion

    public void MakeMove_ShouldChangeChessboardStateAccordingToRequestedMove(string fenString, string uciMove, string fenStringAfterMove)
    {
        // Arrange
        var fen = Fen.FromString(fenString);
        var chessboard = Chessboard.FromFen(fen);
        var move = Move.ParseUciMove(uciMove);

        // Act
        chessboard.MakeMove(move);

        // Assert
        var fenAfterMove = chessboard.ToFen();
        Assert.That(fenAfterMove.ToString(), Is.EqualTo(fenStringAfterMove));
    }

    #region White pawn test cases

    // White pawn on D2
    [TestCase("8/8/8/8/8/8/3P4/8 w KQkq - 0 1", "d2d3 d2d4")]
    // White pawn on D2 and black pawn on D3
    [TestCase("8/8/8/8/8/3p4/3P4/8 w KQkq - 0 1", "")]
    // White pawn on D2 and black pawn on D4
    [TestCase("8/8/8/8/3p4/8/3P4/8 w KQkq - 0 1", "d2d3")]
    // White pawn on D2 and black pawn on C3
    [TestCase("8/8/8/8/8/2p5/3P4/8 w KQkq - 0 1", "d2d3 d2d4 d2c3")]
    // White pawn on D2 and black pawn on E3
    [TestCase("8/8/8/8/8/4p3/3P4/8 w KQkq - 0 1", "d2d3 d2d4 d2e3")]
    // White pawn on D2 and black pawn on C3 and E3
    [TestCase("8/8/8/8/8/2p1p3/3P4/8 w KQkq - 0 1", "d2d3 d2d4 d2c3 d2e3")]
    // White pawn on D2 and black pawn on C3, D3 and E3
    [TestCase("8/8/8/8/8/2ppp3/3P4/8 w KQkq - 0 1", "d2c3 d2e3")]
    // White pawn on A2
    [TestCase("8/8/8/8/8/8/P7/8 w KQkq - 0 1", "a2a3 a2a4")]
    // White pawn on A2 and black pawn on B3
    [TestCase("8/8/8/8/8/1p6/P7/8 w KQkq - 0 1", "a2a3 a2a4 a2b3")]
    // White pawn on H2
    [TestCase("8/8/8/8/8/8/7P/8 w KQkq - 0 1", "h2h3 h2h4")]
    // White pawn on H2 and black pawn on G3
    [TestCase("8/8/8/8/8/6p1/7P/8 w KQkq - 0 1", "h2h3 h2h4 h2g3")]
    // White pawn on D4
    [TestCase("8/8/8/8/3P4/8/8/8 w KQkq - 0 1", "d4d5")]
    // White pawn on D4 and black pawn on C5
    [TestCase("8/8/8/2p5/3P4/8/8/8 w KQkq - 0 1", "d4d5 d4c5")]
    // White pawn on D4 and black pawn on E5
    [TestCase("8/8/8/4p3/3P4/8/8/8 w KQkq - 0 1", "d4d5 d4e5")]
    // White pawn on D4 and black pawn on C5 and E5
    [TestCase("8/8/8/2p1p3/3P4/8/8/8 w KQkq - 0 1", "d4d5 d4c5 d4e5")]
    // White pawn on A4
    [TestCase("8/8/8/8/P7/8/8/8 w KQkq - 0 1", "a4a5")]
    // White pawn on A4 and black pawn on B5
    [TestCase("8/8/8/1p6/P7/8/8/8 w KQkq - 0 1", "a4a5 a4b5")]
    // White pawn on H4
    [TestCase("8/8/8/8/7P/8/8/8 w KQkq - 0 1", "h4h5")]
    // White pawn on H4 and black pawn on G5
    [TestCase("8/8/8/6p1/7P/8/8/8 w KQkq - 0 1", "h4h5 h4g5")]
    // White pawn on D7
    [TestCase("8/3P4/8/8/8/8/8/8 w KQkq - 0 1", "d7d8q d7d8r d7d8b d7d8n")]
    // White pawn on D7 and black rook on D8
    [TestCase("3r4/3P4/8/8/8/8/8/8 w KQkq - 0 1", "")]
    // White pawn on D7 and black rook on C8
    [TestCase("2r5/3P4/8/8/8/8/8/8 w KQkq - 0 1", "d7d8q d7d8r d7d8b d7d8n d7c8q d7c8r d7c8b d7c8n")]
    // White pawn on D7 and black rook on E8
    [TestCase("4r3/3P4/8/8/8/8/8/8 w KQkq - 0 1", "d7d8q d7d8r d7d8b d7d8n d7e8q d7e8r d7e8b d7e8n")]
    // White pawn on D7 and black rook on C8 and E8
    [TestCase("2r1r3/3P4/8/8/8/8/8/8 w KQkq - 0 1", "d7d8q d7d8r d7d8b d7d8n d7c8q d7c8r d7c8b d7c8n d7e8q d7e8r d7e8b d7e8n")]
    // White pawn on D7 and black rook on C8, D8 and E8
    [TestCase("2rrr3/3P4/8/8/8/8/8/8 w KQkq - 0 1", "d7c8q d7c8r d7c8b d7c8n d7e8q d7e8r d7e8b d7e8n")]
    // White pawn on A7
    [TestCase("8/P7/8/8/8/8/8/8 w KQkq - 0 1", "a7a8q a7a8r a7a8b a7a8n")]
    // White pawn on A7 and black rook on B8
    [TestCase("1r6/P7/8/8/8/8/8/8 w KQkq - 0 1", "a7a8q a7a8r a7a8b a7a8n a7b8q a7b8r a7b8b a7b8n")]
    // White pawn on H7
    [TestCase("8/7P/8/8/8/8/8/8 w KQkq - 0 1", "h7h8q h7h8r h7h8b h7h8n")]
    // White pawn on H7 and black rook on G8
    [TestCase("6r1/7P/8/8/8/8/8/8 w KQkq - 0 1", "h7h8q h7h8r h7h8b h7h8n h7g8q h7g8r h7g8b h7g8n")]

    #endregion

    #region Black pawn test cases

    // Black pawn on D7
    [TestCase("8/3p4/8/8/8/8/8/8 b KQkq - 0 1", "d7d6 d7d5")]
    // Black pawn on D7 and white pawn on D6
    [TestCase("8/3p4/3P4/8/8/8/8/8 b KQkq - 0 1", "")]
    // Black pawn on D7 and white pawn on D5
    [TestCase("8/3p4/8/3P4/8/8/8/8 b KQkq - 0 1", "d7d6")]
    // Black pawn on D7 and white pawn on C6
    [TestCase("8/3p4/2P5/8/8/8/8/8 b KQkq - 0 1", "d7d6 d7d5 d7c6")]
    // Black pawn on D7 and white pawn on E6
    [TestCase("8/3p4/4P3/8/8/8/8/8 b KQkq - 0 1", "d7d6 d7d5 d7e6")]
    // Black pawn on D7 and white pawn on C6 and E6
    [TestCase("8/3p4/2P1P3/8/8/8/8/8 b KQkq - 0 1", "d7d6 d7d5 d7c6 d7e6")]
    // Black pawn on D7 and white pawn on C6, D6 and E6
    [TestCase("8/3p4/2PPP3/8/8/8/8/8 b KQkq - 0 1", "d7c6 d7e6")]
    // Black pawn on A7
    [TestCase("8/p7/8/8/8/8/8/8 b KQkq - 0 1", "a7a6 a7a5")]
    // Black pawn on A7 and white pawn on B6
    [TestCase("8/p7/1P6/8/8/8/8/8 b KQkq - 0 1", "a7a6 a7a5 a7b6")]
    // Black pawn on H7
    [TestCase("8/7p/8/8/8/8/8/8 b KQkq - 0 1", "h7h6 h7h5")]
    // Black pawn on H7 and white pawn on G6
    [TestCase("8/7p/6P1/8/8/8/8/8 b KQkq - 0 1", "h7h6 h7h5 h7g6")]
    // Black pawn on D5
    [TestCase("8/8/8/3p4/8/8/8/8 b KQkq - 0 1", "d5d4")]
    // Black pawn on D5 and white pawn on C4
    [TestCase("8/8/8/3p4/2P5/8/8/8 b KQkq - 0 1", "d5d4 d5c4")]
    // Black pawn on D5 and white pawn on E4
    [TestCase("8/8/8/3p4/4P3/8/8/8 b KQkq - 0 1", "d5d4 d5e4")]
    // Black pawn on D5 and white pawn on C4 and E4
    [TestCase("8/8/8/3p4/2P1P3/8/8/8 b KQkq - 0 1", "d5d4 d5c4 d5e4")]
    // Black pawn on A5
    [TestCase("8/8/8/p7/8/8/8/8 b KQkq - 0 1", "a5a4")]
    // Black pawn on A5 and white pawn on B4
    [TestCase("8/8/8/p7/1P6/8/8/8 b KQkq - 0 1", "a5a4 a5b4")]
    // Black pawn on H5
    [TestCase("8/8/8/7p/8/8/8/8 b KQkq - 0 1", "h5h4")]
    // Black pawn on H5 and white pawn on G4
    [TestCase("8/8/8/7p/6P1/8/8/8 b KQkq - 0 1", "h5h4 h5g4")]
    // Black pawn on D2
    [TestCase("8/8/8/8/8/8/3p4/8 b KQkq - 0 1", "d2d1q d2d1r d2d1b d2d1n")]
    // Black pawn on D2 and white rook on D1
    [TestCase("8/8/8/8/8/8/3p4/3R4 b KQkq - 0 1", "")]
    // Black pawn on D2 and white rook on C1
    [TestCase("8/8/8/8/8/8/3p4/2R5 b KQkq - 0 1", "d2d1q d2d1r d2d1b d2d1n d2c1q d2c1r d2c1b d2c1n")]
    // Black pawn on D2 and white rook on E1
    [TestCase("8/8/8/8/8/8/3p4/4R3 b KQkq - 0 1", "d2d1q d2d1r d2d1b d2d1n d2e1q d2e1r d2e1b d2e1n")]
    // Black pawn on D2 and white rook on C1 and E1
    [TestCase("8/8/8/8/8/8/3p4/2R1R3 b KQkq - 0 1", "d2d1q d2d1r d2d1b d2d1n d2c1q d2c1r d2c1b d2c1n d2e1q d2e1r d2e1b d2e1n")]
    // Black pawn on D2 and white rook on C1, D1 and E1
    [TestCase("8/8/8/8/8/8/3p4/2RRR3 b KQkq - 0 1", "d2c1q d2c1r d2c1b d2c1n d2e1q d2e1r d2e1b d2e1n")]
    // Black pawn on A2
    [TestCase("8/8/8/8/8/8/p7/8 b KQkq - 0 1", "a2a1q a2a1r a2a1b a2a1n")]
    // Black pawn on A2 and white rook on B1
    [TestCase("8/8/8/8/8/8/p7/1R6 b KQkq - 0 1", "a2a1q a2a1r a2a1b a2a1n a2b1q a2b1r a2b1b a2b1n")]
    // Black pawn on H2
    [TestCase("8/8/8/8/8/8/7p/8 b KQkq - 0 1", "h2h1q h2h1r h2h1b h2h1n")]
    // Black pawn on H2 and white rook on G1
    [TestCase("8/8/8/8/8/8/7p/6R1 b KQkq - 0 1", "h2h1q h2h1r h2h1b h2h1n h2g1q h2g1r h2g1b h2g1n")]

    #endregion

    public void GetMoveCandidates_ShouldReturnMovesThatAreCandidatesForValidMoves(string fenString, string uciMoves)
    {
        // Arrange
        var fen = Fen.FromString(fenString);
        var chessboard = Chessboard.FromFen(fen);
        var expectedMoves = Move.ParseUciMoves(uciMoves);

        // Assume
        Assert.That(expectedMoves, Is.Unique);

        // Act
        var moves = chessboard.GetMoveCandidates();

        // Assert
        Assert.That(moves, Is.Unique);
        Assert.That(moves, Is.EquivalentTo(expectedMoves));
    }
}