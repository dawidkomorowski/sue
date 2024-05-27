using NUnit.Framework;
using Sue.Engine.OldModel.Chessboard;
using Sue.Engine.OldModel.Fen;
using Sue.Engine.UnitTests.Common;

namespace Sue.Engine.UnitTests.OldModel.Chessboard
{
    [TestFixture]
    public class ChessboardTests : CommonTestsBase
    {
        [TestCase(FenString.Empty, FenString.Empty, true)]
        [TestCase(FenString.StartPos, FenString.StartPos, true)]
        [TestCase("8/8/8/8/8/8/8/8 w KQkq - 0 1", "8/8/8/8/8/8/8/8 b KQkq - 0 1", false)]
        [TestCase("8/8/8/8/8/8/8/8 w KQkq - 0 1", "8/8/8/8/8/8/8/8 w Qkq - 0 1", false)]
        [TestCase("8/8/8/8/8/8/8/8 w KQkq - 0 1", "8/8/8/8/8/8/8/8 w Kkq - 0 1", false)]
        [TestCase("8/8/8/8/8/8/8/8 w KQkq - 0 1", "8/8/8/8/8/8/8/8 w KQq - 0 1", false)]
        [TestCase("8/8/8/8/8/8/8/8 w KQkq - 0 1", "8/8/8/8/8/8/8/8 w KQk - 0 1", false)]
        [TestCase("8/8/8/8/8/8/8/8 w KQkq - 0 1", "8/8/8/8/8/8/8/8 w KQk e4 0 1", false)]
        [TestCase("8/8/8/8/8/8/8/8 w KQkq - 0 1", "8/8/8/8/8/8/8/8 w KQk - 12 1", false)]
        [TestCase("8/8/8/8/8/8/8/8 w KQkq - 0 1", "8/8/8/8/8/8/8/8 w KQk - 0 4", false)]
        [TestCase("3q1rk1/p4ppp/2r5/p1bp4/b4B2/P1RQPN2/1P1K1PPP/7R w - - 3 27", "3q1rk1/p4ppp/2r5/p1bp4/b4B2/P1RQPN2/1P1K1PPP/7R w - - 3 27", true)]
        [TestCase("3q1rk1/p4ppp/2r5/p1bp4/b4B2/P1RQPN2/1P1K1PPP/7R w - - 3 27", "3q1rk1/p4ppp/2r5/p1bp4/b4B2/P1R1PN2/1P1K1PPP/7R w - - 3 27", false)]
        [TestCase("3q1rk1/p4ppp/2r5/p1bp4/b4B2/P1RQPN2/1P1K1PPP/7R w - - 3 27", "3p1rk1/p4ppp/2r5/p1bp4/b4B2/P1RQPN2/1P1K1PPP/7R w - - 3 27", false)]
        [TestCase("3q1rk1/p4ppp/2r5/p1bp4/b4B2/P1RQPN2/1P1K1PPP/7R w - - 3 27", "3q1rk1/p4ppp/2r5/p1bp4/b4B2/P1RQPN2/1P1K1PPP/8 w - - 3 27", false)]
        public void ShouldGivenChessboardEqualsToOtherChessboard(string firstFenString, string secondFenString,
            bool expected)
        {
            // Arrange
            var firstChessboard = ChessboardFactory.Create(firstFenString);
            var secondChessboard = ChessboardFactory.Create(secondFenString);

            // Act
            var actual = firstChessboard.EqualsTo(secondChessboard);

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}