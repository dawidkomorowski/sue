using Sue.Common.Model.Chessboard;
using Sue.Common.Model.Chessboard.Internal;

namespace Sue.Common.Model.Fen.Internal
{
    public class ChessboardFactory : IChessboardFactory
    {
        private readonly IChessPieceFactory _chessPieceFactory;
        private readonly IFenStringParser _fenStringParser;

        public ChessboardFactory(IChessPieceFactory chessPieceFactory, IFenStringParser fenStringParser)
        {
            _chessPieceFactory = chessPieceFactory;
            _fenStringParser = fenStringParser;
        }

        public IChessboard Create(string fenString)
        {
            var chessboard = new ArrayChessboard(_chessPieceFactory);
            _fenStringParser.Parse(fenString, chessboard);
            return chessboard;
        }
    }
}