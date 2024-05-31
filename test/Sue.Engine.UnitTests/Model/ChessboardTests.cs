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
    [TestCase("r5kr/pp3pp1/3b1n1p/1QpqNP2/3p1B2/8/PPP3PP/R3R2K w - - 8 22", "c2c4", "r5kr/pp3pp1/3b1n1p/1QpqNP2/2Pp1B2/8/PP4PP/R3R2K b - c3 0 22")] // TODO En Passant
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
}