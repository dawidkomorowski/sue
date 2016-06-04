using NSubstitute;
using NSubstitute.Core.Arguments;
using NUnit.Framework;
using Sue.Common.Model.Chessboard;
using Sue.Common.Model.Chessboard.Internal;
using Sue.Common.Model.Fen;
using Sue.Common.Model.Fen.Internal;

namespace Sue.Common.UnitTests.Model.Fen
{
    [TestFixture]
    public class ChessboardFactoryTests
    {
        [Test]
        public void ShouldReturnNotNull_AndFenStringParserReceived_WhenCreateCalledWithFenstring()
        {
            // Arrange
            var chessPieceFactory = Substitute.For<IChessPieceFactory>();
            var fenStringParser = Substitute.For<IFenStringParser>();
            IChessboardFactory chessboardFactory = new ChessboardFactory(chessPieceFactory, fenStringParser);
            const string fenString = FenString.StartPos;

            // Act
            var chessboard = chessboardFactory.Create(fenString);

            // Assert
            Assert.That(chessboard, Is.Not.Null);
            fenStringParser.Received().Parse(fenString, (ISettableChessboard) chessboard);
        }
    }
}