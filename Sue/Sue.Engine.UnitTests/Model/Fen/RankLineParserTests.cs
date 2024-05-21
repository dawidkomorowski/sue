using System.Linq;
using NUnit.Framework;
using Sue.Engine.Model;
using Sue.Engine.Model.Chessboard;
using Sue.Engine.Model.Chessboard.Internal;
using Sue.Engine.Model.Fen.Internal;

namespace Sue.Engine.UnitTests.Model.Fen
{
    [TestFixture]
    public class RankLineParserTests
    {
        [Test]
        public void ShouldReturnNoChessPieces_GivenRankLine_8_WithRank_One()
        {
            // Arrange
            var rankLineParser = RankLineParser;
            var rankLine = new RankLine("8", Rank.One);

            // Act
            var chessPieces = rankLineParser.Parse(rankLine);

            // Assert
            Assert.That(chessPieces, Is.Empty);
        }

        [Test]
        public void ShouldReturnWhitePawnAtFile_C_AndRank_Four_GivenRankLine_2P5_WithRank_Four()
        {
            // Arrange
            var rankLineParser = RankLineParser;
            var rankLine = new RankLine("2P5", Rank.Four);

            // Act
            var chessPieces = rankLineParser.Parse(rankLine);

            // Assert
            Assert.That(chessPieces.Count(), Is.EqualTo(1));
            var chessPiece = chessPieces.First();
            Assert.That(chessPiece.ChessPiece.Color, Is.EqualTo(Color.White));
            Assert.That(chessPiece.ChessPiece.ChessPieceKind, Is.EqualTo(ChessPieceKind.Pawn));
            Assert.That(chessPiece.File, Is.EqualTo(File.C));
            Assert.That(chessPiece.Rank, Is.EqualTo(Rank.Four));
        }

        [Test]
        public void ShouldReturnWhiteQueenAndBlackKing_GivenRankLine_3Q2k1_WithRank_Seven()
        {
            // Arrange
            var rankLineParser = RankLineParser;
            var rankLine = new RankLine("3Q2k1", Rank.Seven);

            // Act
            var chessPieces = rankLineParser.Parse(rankLine);

            // Assert
            Assert.That(chessPieces.Count(), Is.EqualTo(2));
            var queen = chessPieces.Single(p => p.ChessPiece.ChessPieceKind == ChessPieceKind.Queen);
            Assert.That(queen.ChessPiece.Color, Is.EqualTo(Color.White));
            Assert.That(queen.ChessPiece.ChessPieceKind, Is.EqualTo(ChessPieceKind.Queen));
            Assert.That(queen.File, Is.EqualTo(File.D));
            Assert.That(queen.Rank, Is.EqualTo(Rank.Seven));
            var king = chessPieces.Single(p => p.ChessPiece.ChessPieceKind == ChessPieceKind.King);
            Assert.That(king.ChessPiece.Color, Is.EqualTo(Color.Black));
            Assert.That(king.ChessPiece.ChessPieceKind, Is.EqualTo(ChessPieceKind.King));
            Assert.That(king.File, Is.EqualTo(File.G));
            Assert.That(king.Rank, Is.EqualTo(Rank.Seven));
        }

        [Test]
        public void ShouldReturnEightWhitePawns_GivenRankLine_PPPPPPPP_WithRank_Two()
        {
            // Arrange
            var rankLineParser = RankLineParser;
            var rankLine = new RankLine("PPPPPPPP", Rank.Two);

            // Act
            var chessPieces = rankLineParser.Parse(rankLine);

            // Assert
            Assert.That(chessPieces.Count(), Is.EqualTo(8));
            var pawns =
                chessPieces.Where(
                        p => p.ChessPiece.Color == Color.White && p.ChessPiece.ChessPieceKind == ChessPieceKind.Pawn && p.Rank == Rank.Two)
                    .ToArray();
            Assert.That(pawns.Length, Is.EqualTo(8));
        }

        private static IRankLineParser RankLineParser
        {
            get
            {
                IChessPieceParser chessPieceParser = new ChessPieceParser();
                return new RankLineParser(chessPieceParser);
            }
        }
    }
}