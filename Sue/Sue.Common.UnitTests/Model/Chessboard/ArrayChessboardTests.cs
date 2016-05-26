using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Internal;
using NUnit.Framework;
using Sue.Common.Model;
using Sue.Common.Model.Chessboard;
using Sue.Common.Model.Chessboard.Internal;

namespace Sue.Common.UnitTests.Model.Chessboard
{
    [TestFixture]
    public class ArrayChessboardTests
    {
        [Test]
        public void ShouldReturnAllWhiteChessPieces_WhenChessboardInInitialState()
        {
            // Arrange
            IChessPieceFactory chessPieceFactory = new ChessPieceFactory();
            IChessboard chessboard = new ArrayChessboard(chessPieceFactory);

            // Act
            var whiteChessPieces = chessboard.GetChessPieces(Color.White);

            // Assert
            Assert.That(whiteChessPieces.Count(), Is.EqualTo(16));
        }
    }
}
