using NSubstitute;
using NUnit.Framework;
using Sue.Engine.Model.Chessboard.Internal;
using Sue.Engine.Model.Fen;
using Sue.Engine.Model.Fen.Internal;

namespace Sue.Engine.UnitTests.Model.Fen
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